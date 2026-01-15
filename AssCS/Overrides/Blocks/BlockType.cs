// SPDX-License-Identifier: MPL-2.0

namespace AssCS.Overrides.Blocks;

/// <summary>
/// Type of Block
/// </summary>
public enum BlockType
{
    /// <summary>
    /// Plain text block
    /// </summary>
    Plain,

    /// <summary>
    /// Comment block
    /// </summary>
    Comment,

    /// <summary>
    /// Override tags block
    /// </summary>
    Override,

    /// <summary>
    /// Drawing block
    /// </summary>
    Drawing,
}
