// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Reactive.Disposables.Fluent;
using Ameko.Renderers;
using Ameko.ViewModels.Controls;
using Avalonia;
using Avalonia.Input;
using Holo.Models;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Ameko.Views.Controls;

public partial class TabItemAudioArea : ReactiveUserControl<TabItemViewModel>
{
    public TabItemAudioArea()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.GetObservable(ViewModelProperty)
                .WhereNotNull()
                .Subscribe(vm =>
                {
                    // TODO: Don't do this!!
                    var mc = vm.Workspace.MediaController;
                    var renderer = new OpenAlAudioRenderer(mc);
                    renderer.Initialize();
                    mc.PlaybackStarted += (_, e) =>
                    {
                        // Always play audio target, only play video target if not muted
                        if (e.Target is PlaybackTarget.Audio || !mc.IsMuted)
                            renderer.Play(e.StartTime, e.GoalTime);
                    };
                    mc.PlaybackStopped += (_, _) =>
                    {
                        renderer.Stop();
                    };
                })
                .DisposeWith(disposables);
        });
    }

    private void AudioTarget_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (ViewModel is null)
        {
            e.Handled = true;
            return;
        }
        if (e.Delta.Y > 0)
            ViewModel.Workspace.MediaController.VisualizerPositionMs -= 250; // Quarter second
        if (e.Delta.Y < 0)
            ViewModel.Workspace.MediaController.VisualizerPositionMs += 250;
        e.Handled = true;
    }
}
