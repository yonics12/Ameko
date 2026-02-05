// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Ameko.DataModels;
using Ameko.Utilities;
using Holo.Models;
using Holo.Providers;
using ReactiveUI;

namespace Ameko.ViewModels.Dialogs;

public partial class CommandPaletteDialogViewModel : ViewModelBase
{
    private readonly IProjectProvider _projectProvider;

    public string Query { get; set; } = string.Empty;
    public ICommand GoCommand { get; }

    private readonly RangeObservableCollection<CommandPaletteResult> _results = [];

    public AssCS.Utilities.ReadOnlyObservableCollection<CommandPaletteResult> Results { get; }

    public CommandPaletteDialogViewModel(IProjectProvider projectProvider, ITabFactory tabFactory)
    {
        _projectProvider = projectProvider;
        Results = new AssCS.Utilities.ReadOnlyObservableCollection<CommandPaletteResult>(_results);

        GoCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var wsp = _projectProvider.Current.WorkingSpace;
            if (wsp is null)
                return;
        });
    }

    internal void GenerateSuggestions()
    {
        _results.Clear();
        if (string.IsNullOrWhiteSpace(Query))
            return;

        // Search for command
        if (Query[0] is '>') { }

        // Search for file
        var prj = _projectProvider.Current;
        var files = prj.GetAllOfType(ProjectItemType.Document);

        var qFile = new DocumentItem { Name = Query };
        if (prj.SavePath is not null)
        {
            Path.Combine(Path.GetDirectoryName(prj.SavePath.LocalPath) ?? "/", Query);
            qFile.Uri = new Uri(prj.SavePath.LocalPath);
        }

        // Search by name
        var results = FuzzySharp.Process.ExtractTop(qFile, files, f => f.Title);

        // Search by filepath, if applicable
        if (qFile.Uri is not null)
        {
            results = results
                .Concat(
                    FuzzySharp.Process.ExtractTop(
                        qFile,
                        files.Where(f => f.Uri is not null),
                        f => prj.GetRelativePath(f)
                    )
                )
                .DistinctBy(r => r.Value)
                .OrderBy(r => r.Score)
                .Take(10);
        }
        _results.AddRange(
            results.Select(r => new CommandPaletteResult(
                Key: qFile.Uri is not null ? prj.GetRelativePath(r.Value) : string.Empty,
                Name: r.Value.Title
            ))
        );
    }
}
