// SPDX-License-Identifier: MPL-2.0

namespace AssCS.History;

/// <summary>
/// Type of change made
/// </summary>
public enum ChangeType
{
    /// <summary>
    /// One or more <see cref="Event"/>s were added
    /// </summary>
    AddEvent,

    /// <summary>
    /// One or more <see cref="Event"/>s were removed
    /// </summary>
    RemoveEvent,

    /// <summary>
    /// One or more <see cref="Event"/>s had their <see cref="Event.Text"/> modified
    /// </summary>
    ModifyEventText,

    /// <summary>
    /// One or more <see cref="Event"/>s had their metadata modified
    /// </summary>
    ModifyEventMeta,

    /// <summary>
    /// A complex change relating to <see cref="Event"/>s
    /// </summary>
    /// <remarks>Compound additions, removals, and modifications</remarks>
    ComplexEvent,

    /// <summary>
    /// One or more <see cref="Style"/>s were added
    /// </summary>
    AddStyle,

    /// <summary>
    /// One or more <see cref="Style"/>s were removed
    /// </summary>
    RemoveStyle,

    /// <summary>
    /// One or more <see cref="Style"/>s were modified
    /// </summary>
    ModifyStyle,

    /// <summary>
    /// A complex change relating to <see cref="Style"/>s
    /// </summary>
    /// <remarks>Compound additions, removals, and modifications</remarks>
    ComplexStyle,

    /// <summary>
    /// Initial state of the document
    /// </summary>
    Initial,

    /// <summary>
    /// An undo/redo operation
    /// </summary>
    TimeMachine,
}
