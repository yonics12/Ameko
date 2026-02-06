// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Ameko.DataModels;
using Ameko.Messages;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using Holo.Models;
using Holo.Providers;
using ReactiveUI;

namespace Ameko.ViewModels.Dialogs;

public class CommandPaletteDialogViewModel : ViewModelBase
{
    private readonly IProjectProvider _projectProvider;

    public string Query { get; set; } = string.Empty;
    public ReactiveCommand<Unit, EmptyMessage> GoCommand { get; }

    private readonly RangeObservableCollection<CommandPaletteResult> _results = [];

    public AssCS.Utilities.ReadOnlyObservableCollection<CommandPaletteResult> Results { get; }

    public CommandPaletteResult? SelectedResult
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public CommandPaletteDialogViewModel(
        IProjectProvider projectProvider,
        ICommand openDocumentCommand
    )
    {
        _projectProvider = projectProvider;
        Results = new AssCS.Utilities.ReadOnlyObservableCollection<CommandPaletteResult>(_results);

        GoCommand = ReactiveCommand.Create(() =>
        {
            var wsp = _projectProvider.Current.WorkingSpace;
            if (SelectedResult is null || wsp is null)
                return new EmptyMessage();

            // Open Document
            if (SelectedResult is DocumentCommandPaletteResult dr)
            {
                openDocumentCommand.Execute(dr.Id);
                return new EmptyMessage();
            }

            // Execute Command
            return new EmptyMessage();
        });
    }

    internal void GenerateSuggestions()
    {
        _results.Clear();
        if (string.IsNullOrWhiteSpace(Query))
            return;

        // Search for command
        if (Query[0] is '>')
        {
            SelectedResult = _results.FirstOrDefault();
            return;
        }

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
        var results = FuzzySharp.Process.ExtractTop(
            qFile,
            files,
            f => f.Title,
            ScorerCache.Get<DefaultRatioScorer>()
        );

        _results.AddRange(
            results.Select(r => new DocumentCommandPaletteResult(
                Key: qFile.Uri is not null ? prj.GetRelativePath(r.Value) : string.Empty,
                Name: r.Value.Title,
                Id: r.Value.Id
            ))
        );

        SelectedResult = _results.FirstOrDefault();
    }
}
