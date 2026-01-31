// SPDX-License-Identifier: MPL-2.0

using System.ComponentModel;
using System.IO.Abstractions.TestingHelpers;
using Holo;
using Holo.Providers;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestingUtils;

public class TestProjectFactory : IProjectProvider
{
    /// <inheritdoc />
#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067

    /// <inheritdoc />
    public Project Current { get; set; } = null!;

    /// <inheritdoc />
    public Project Create(bool isEmpty = false)
    {
        return new Project(
            new MockFileSystem(),
            NullLogger<Project>.Instance,
            new TestWorkspaceFactory(),
            isEmpty
        );
    }

    /// <inheritdoc />
    public Project CreateFromFile(Uri uri)
    {
        return new Project(
            new MockFileSystem(),
            NullLogger<Project>.Instance,
            new TestWorkspaceFactory(),
            uri
        );
    }

    /// <inheritdoc />
    public Project CreateFromDirectory(Uri uri)
    {
        return new Project(
            new MockFileSystem(),
            NullLogger<Project>.Instance,
            new TestWorkspaceFactory(),
            uri
        );
    }
}
