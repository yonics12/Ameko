// SPDX-License-Identifier: GPL-3.0-only

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

        GotFocus += (_, __) =>
        {
            QueryBox.Focus();
        };

        this.WhenActivated(disposables =>
        {
            QueryBox.Focus();
            Disposable.Create(() => { }).DisposeWith(disposables);
        });
    }

    private void QueryBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        ViewModel?.GenerateSuggestions();
    }
}
