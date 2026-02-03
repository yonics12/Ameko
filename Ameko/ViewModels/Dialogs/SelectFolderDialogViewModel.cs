// SPDX-License-Identifier: GPL-3.0-only

using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Ameko.DataModels;
using Ameko.Messages;
using ReactiveUI;

namespace Ameko.ViewModels.Dialogs;

public class SelectFolderDialogViewModel : ViewModelBase
{
    public List<FolderInformation> Folders { get; }

    public FolderInformation SelectedFolder
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ReactiveCommand<Unit, SelectFolderMessage> SelectFolderCommand { get; }

    public SelectFolderDialogViewModel(List<FolderInformation> folders)
    {
        Folders = folders;
        SelectedFolder = Folders.First();

        SelectFolderCommand = ReactiveCommand.Create(() =>
            new SelectFolderMessage(SelectedFolder.Id)
        );
    }
}
