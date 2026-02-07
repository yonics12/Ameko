// SPDX-License-Identifier: MPL-2.0

using Microsoft.Extensions.Logging;

namespace Holo.Models;

public record LogEntry(LogLevel Level, DateTimeOffset Timestamp, string Message);
