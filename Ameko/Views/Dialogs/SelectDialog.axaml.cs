// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Ameko.ViewModels.Dialogs;
using Avalonia.Input;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Ameko.Views.Dialogs;

public partial class SelectDialog : ReactiveWindow<SelectDialogViewModel>
{
    public SelectDialog()
    {
        InitializeComponent();

        KeyDown += (_, e) =>
        {
            switch (e.Key)
            {
                case Key.Escape:
                case Key.W
                    when e.KeyModifiers.HasFlag(KeyModifiers.Control)
                        || e.KeyModifiers.HasFlag(KeyModifiers.Meta):
                    Close();
                    break;
            }
        };

        this.WhenActivated(disposables =>
        {
            QueryBox.Focus();
            ViewModel?.SelectCommand.Subscribe(Close);
            Disposable.Create(() => { }).DisposeWith(disposables);
        });
    }
}
