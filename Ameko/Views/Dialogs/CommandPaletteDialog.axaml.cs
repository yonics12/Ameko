// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Ameko.ViewModels.Dialogs;
using Avalonia.Controls;
using Avalonia.Input;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Ameko.Views.Dialogs;

public partial class CommandPaletteDialog : ReactiveWindow<CommandPaletteDialogViewModel>
{
    public CommandPaletteDialog()
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

        GotFocus += (_, _) =>
        {
            QueryBox.Text ??= string.Empty;

            QueryBox.Focus();
            QueryBox.SelectionStart = 0;
            QueryBox.SelectionEnd = QueryBox.Text.Length;
        };

        this.WhenActivated(disposables =>
        {
            QueryBox.Text ??= string.Empty;

            QueryBox.Focus();
            QueryBox.SelectionStart = 0;
            QueryBox.SelectionEnd = QueryBox.Text.Length;

            ViewModel?.GoCommand.Subscribe(Close);
            Disposable.Create(() => { }).DisposeWith(disposables);
        });
    }

    private void QueryBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        ViewModel?.GenerateSuggestions();
    }
}
