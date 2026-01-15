// SPDX-License-Identifier: MPL-2.0

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AssCS;

/// <summary>
/// Fields in an <see cref="Event"/>
/// </summary>
[Flags]
public enum EventField
{
    None = 0,
    Comment = 1,
    Layer = 2,
    StartTime = 4,
    EndTime = 8,
    Style = 16,
    Actor = 32,
    MarginLeft = 64,
    MarginRight = 128,
    MarginVertical = 256,
    Effect = 512,
    Text = 1024,
    All =
        Comment
        | Layer
        | StartTime
        | EndTime
        | Style
        | Actor
        | MarginLeft
        | MarginRight
        | MarginVertical
        | Effect
        | Text,
}
