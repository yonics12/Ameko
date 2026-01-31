// SPDX-License-Identifier: MPL-2.0

const std = @import("std");
const c = @import("c.zig").c;
const ffms = @import("ffms.zig");
const libass = @import("libass.zig");
const frames = @import("frames.zig");
const common = @import("common.zig");
const logger = @import("logger.zig");
const context = @import("context.zig");
const viz = @import("visualization.zig");

/// Pre-initialize video buffers
pub fn InitVideo(g_ctx: *context.GlobalContext, num_buffers: usize, max_cache_mb: c_int, width: usize, height: usize, pitch: usize) ffms.FfmsError!void {
    var ctx = &g_ctx.*.buffers;
    ctx.frame_buffers = .empty;
    ctx.max_size = @as(i64, max_cache_mb) * std.math.pow(i64, 1024, 2);

    // Pre-allocate buffers
    var i: usize = 0;
    while (i < num_buffers) : (i += 1) {
        const frame = try AllocateVideoFrame(width, height, pitch);
        try ctx.frame_buffers.append(common.allocator, frame);
    }
}

/// Initialize audio buffer
pub fn InitAudio(g_ctx: *context.GlobalContext) ffms.FfmsError!void {
    var ctx = &g_ctx.*.buffers;
    const ffms_ctx = &g_ctx.*.ffms;

    const total_samples: usize = @intCast(ffms_ctx.sample_count * ffms_ctx.channel_count);
    ctx.audio_buffer = try common.allocator.alloc(i16, total_samples);
    ctx.audio_frame = try common.allocator.create(frames.AudioFrame);
    ctx.audio_frame.?.* = .{
        .data = ctx.audio_buffer.?.ptr,
        .length = @intCast(ctx.audio_buffer.?.len),
        .channel_count = ffms_ctx.channel_count,
        .sample_count = ffms_ctx.sample_count,
        .sample_rate = ffms_ctx.sample_rate,
        .duration_ms = @divFloor((ffms_ctx.sample_count * 1000), ffms_ctx.sample_rate),
        .valid = 0,
    };
}

/// Initialize the visualization buffer
pub fn InitVisualization(g_ctx: *context.GlobalContext) void {
    const ctx = &g_ctx.*.buffers;

    ctx.viz_buffers = .empty;
}

/// Get the audio buffer
pub fn GetAudio(g_ctx: *context.GlobalContext, progress_cb: common.ProgressCallback) ffms.FfmsError!*frames.AudioFrame {
    const ctx = &g_ctx.*.buffers;
    const ffms_ctx = &g_ctx.*.ffms;

    if (ctx.audio_frame == null) {
        return ffms.FfmsError.NoAudioTracks;
    }
    if (ctx.audio_frame.?.*.valid == 0) {
        try ffms.GetAudio(
            g_ctx,
            @ptrCast(ctx.audio_buffer.?.ptr),
            0,
            ffms_ctx.sample_count,
            progress_cb,
        );
        ctx.audio_frame.?.*.valid = 1;
    }

    return ctx.audio_frame.?;
}

// Free the audio buffers
// Public to facilitate switching audio tracks
pub fn FreeAudioBuffer(g_ctx: *context.GlobalContext) void {
    var ctx = &g_ctx.*.buffers;

    // free audio buffer
    if (ctx.audio_buffer) |audio_buffer| {
        common.allocator.free(audio_buffer);
        ctx.audio_buffer = null;
    }

    // destroy audio_frame
    if (ctx.audio_frame) |audio_frame| {
        common.allocator.destroy(audio_frame);
        ctx.audio_frame = null;
    }
}

/// Free the buffers
pub fn Deinit(g_ctx: *context.GlobalContext) void {
    var ctx = &g_ctx.*.buffers;

    // free video frame buffers
    for (ctx.frame_buffers.items) |buffer| {
        // free video pixel buffer
        const v_total: usize = @intCast(buffer.*.video_frame.*.height * buffer.*.video_frame.*.pitch);
        if (v_total != 0) {
            common.allocator.free(buffer.*.video_frame.*.data[0..v_total]);
        }

        // free subtitle pixel buffer
        const s_total: usize = @intCast(buffer.*.subtitle_frame.*.height * buffer.*.subtitle_frame.*.pitch);
        if (s_total != 0) {
            common.allocator.free(buffer.*.subtitle_frame.*.data[0..s_total]);
        }

        // destroy [build destroy]
        common.allocator.destroy(buffer.*.video_frame);
        common.allocator.destroy(buffer.*.subtitle_frame);
        common.allocator.destroy(buffer);
    }

    ctx.frame_buffers.deinit(common.allocator);

    FreeAudioBuffer(g_ctx);

    // free visualization buffers
    if (ctx.viz_buffers) |viz_buffers| {
        for (viz_buffers.items) |buffer| {
            const total: usize = buffer.*.capacity;
            if (total != 0) {
                common.allocator.free(buffer.*.data[0..total]);
            }
            common.allocator.destroy(buffer);
        }

        ctx.viz_buffers.?.deinit(common.allocator);
    }
}

pub fn ProcVizualizationFrame(
    g_ctx: *context.GlobalContext,
    width: c_int,
    height: c_int,
    pixel_ms: f64,
    amplitude_scale: f64,
    start_time: f64,
    video_time: f64,
    audio_time: f64,
    event_bounds: [*]i64,
    event_bounds_len: usize,
) !*frames.Bitmap {
    const ctx = &g_ctx.*.buffers;
    const result: *frames.Bitmap = try GetOrCreateVisualizationFrame(ctx, width, height);

    viz.RenderWaveform(
        g_ctx,
        result,
        pixel_ms,
        amplitude_scale,
        start_time,
        video_time,
        audio_time,
        event_bounds,
        event_bounds_len,
    );
    return result;
}

fn GetOrCreateVisualizationFrame(ctx: *context.BuffersContext, width: c_int, height: c_int) !*frames.Bitmap {
    var buffers = &ctx.viz_buffers.?;

    // Mutex, just in case™
    ctx.viz_mutex.lock();
    defer ctx.viz_mutex.unlock();

    // Check if there's room to add a new buffer
    if (buffers.items.len < ctx.max_viz_buffers) {
        const new_buffer = try AllocateVisualizationFrame(@intCast(width), @intCast(height));
        try buffers.insert(common.allocator, 0, new_buffer);
        return new_buffer;
    }

    // Try to find a usable buffer, starting at the end
    var idx = buffers.items.len;
    while (idx > 0) {
        idx -= 1;
        const buffer = buffers.items[idx];
        if (buffer.refcount > 0) {
            continue;
        } else if (buffer.width == width and buffer.height == height) {
            // Correct size, re-use the buffer
            if (idx != 0) {
                _ = buffers.swapRemove(idx);
                try buffers.insert(common.allocator, 0, buffer);
            }
            return buffer;
        } else {
            // Wrong size, remove & create one with the right size
            _ = buffers.swapRemove(idx);
            FreeVisualizationFrame(buffer);
            const new_buffer = try AllocateVisualizationFrame(@intCast(width), @intCast(height));
            try buffers.insert(common.allocator, 0, new_buffer);
            return new_buffer;
        }
    }

    // No room and no free buffers, make a new one but this scenario is Not Good!!
    logger.Warn("Visualization buffer pool exhausted and no free buffers!");
    const new_buffer = try AllocateVisualizationFrame(@intCast(width), @intCast(height));
    try buffers.insert(common.allocator, 0, new_buffer);
    return new_buffer;
}

/// Get a frame
pub fn ProcVideoFrame(g_ctx: *context.GlobalContext, frame_number: c_int, timestamp: c_longlong, raw: c_int) ffms.FfmsError!*frames.FrameGroup {
    var ctx = &g_ctx.*.buffers;
    var buffers = &ctx.frame_buffers;

    _ = raw; // For sub-less frame
    var result: ?*frames.FrameGroup = null;

    // See if the frame is in the list of cached buffers already
    for (buffers.items, 0..) |buffer, idx| {
        if (buffer.*.video_frame.*.valid == 1 and buffer.*.video_frame.*.frame_number == frame_number) {
            result = buffer;

            // Move buffer to the front of the list
            if (idx != 0) {
                _ = buffers.swapRemove(idx);
                try buffers.insert(common.allocator, 0, buffer);
            }

            // Check if we need to (re)render the subtitles
            if (!libass.VerifyHash(g_ctx, result.?.*.subtitle_frame)) {
                try libass.GetFrame(g_ctx, timestamp, result.?.*.subtitle_frame);
            }

            // TODO: Render to the buffer
            return result.?;
        }
    }

    // The frame was not cached, so we need to find an invalidated buffer
    // (or create a new one if there's space in the cache)

    // If we're at the size limit, make space
    if (ctx.total_size >= ctx.max_size) {
        const last = buffers.swapRemove(buffers.items.len - 1);
        _ = InvalidateVideoFrame(last);
        try buffers.insert(common.allocator, 0, last);
    }

    // Find an invalidated buffer
    for (buffers.items) |buffer| {
        if (buffer.*.video_frame.*.valid == 0) {
            result = buffer;
            break;
        }
    }

    // Allocate a new buffer
    if (result == null) {
        // Clone geometry from the first buffer
        const reference = buffers.items[0];
        result = try AllocateVideoFrame(
            @intCast(reference.*.video_frame.*.width),
            @intCast(reference.*.video_frame.*.height),
            @intCast(reference.*.video_frame.*.pitch),
        );
        try buffers.insert(common.allocator, 0, result.?);
        ctx.total_size += (result.?.*.video_frame.*.height * result.?.*.video_frame.*.pitch) * 2; // add size
    }

    try ffms.GetFrame(g_ctx, frame_number, result.?.*.video_frame);
    try libass.GetFrame(g_ctx, timestamp, result.?.*.subtitle_frame);

    result.?.*.video_frame.*.valid = 1;
    result.?.*.subtitle_frame.valid = 1;
    return result.?;
}

/// Mark a frame as invalid so it can be reused
pub fn InvalidateVideoFrame(frame: *frames.FrameGroup) c_int {
    frame.video_frame.*.valid = 0;
    frame.subtitle_frame.*.valid = 0;
    return 0;
}

/// Allocate a new frame buffer
fn AllocateVideoFrame(width: usize, height: usize, pitch: usize) !*frames.FrameGroup {
    const total_bytes = height * pitch;
    const v_pixel_buffer = try common.allocator.alloc(u8, total_bytes);
    const s_pixel_buffer = try common.allocator.alloc(u8, total_bytes);
    const v_frame = try common.allocator.create(frames.VideoFrame);
    const s_frame = try common.allocator.create(frames.SubtitleFrame);
    const group = try common.allocator.create(frames.FrameGroup);

    v_frame.* = .{
        .frame_number = -1,
        .timestamp = 0,
        .width = @intCast(width),
        .height = @intCast(height),
        .pitch = @intCast(pitch),
        .flipped = 0,
        .data = v_pixel_buffer.ptr,
        .valid = 0,
    };

    s_frame.* = .{
        .hash = 0,
        .timestamp = 0,
        .width = @intCast(width),
        .height = @intCast(height),
        .pitch = @intCast(pitch),
        .flipped = 0,
        .data = s_pixel_buffer.ptr,
        .valid = 0,
    };

    group.* = .{
        .video_frame = v_frame,
        .subtitle_frame = s_frame,
    };

    return group;
}

/// Allocate a visualization frame
fn AllocateVisualizationFrame(width: usize, height: usize) !*frames.Bitmap {
    const pitch = try std.math.mul(usize, width, 4); // BGRA
    const total_bytes = try std.math.mul(usize, height, pitch);

    const frame = try common.allocator.create(frames.Bitmap);
    const pixel_buffer = try common.allocator.alloc(u8, total_bytes);

    frame.* = .{
        .width = @intCast(width),
        .height = @intCast(height),
        .pitch = @intCast(pitch),
        .capacity = total_bytes,
        .data = pixel_buffer.ptr,
        .refcount = 0,
    };

    return frame;
}

/// Free the viz frame
fn FreeVisualizationFrame(frame: *frames.Bitmap) void {
    std.debug.assert(frame.refcount <= 0);

    const total = frame.capacity;
    if (total != 0) {
        common.allocator.free(frame.*.data[0..total]);
    }
    common.allocator.destroy(frame);
}
