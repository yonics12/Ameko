// SPDX-License-Identifier: GPL-3.0-only

using Ameko.ViewModels.Dialogs;
using Avalonia.Input;
using ReactiveUI.Avalonia;

namespace Ameko.Views.Dialogs;

public partial class ChangelogDialog : ReactiveWindow<SelectTrackDialogViewModel>
{
    public ChangelogDialog()
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
    }
}
