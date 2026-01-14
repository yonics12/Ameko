// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Ameko.ViewModels.Dialogs;
using Avalonia.Input;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Ameko.Views.Dialogs;

public partial class PasteOverDialog : ReactiveWindow<PasteOverDialogViewModel>
{
    public PasteOverDialog()
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
            ViewModel?.ConfirmCommand.Subscribe(Close);

            Disposable.Create(() => { }).DisposeWith(disposables);
        });
    }
}
