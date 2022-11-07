using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HonkaiCharacterBrowser.Messages;
using HonkaiCharacterBrowser.Models;

namespace HonkaiCharacterBrowser.ViewModels;

public class SidePanelViewModel : ObservableObject
{
    public ValkyrieInfo SelectedValkyrie { get; set; }

    public Skill SelectedSkill { get; set; }

    public bool IsSidePanelVisible { get; set; }

    public SidePanelViewModel()
    {
        ShowProtraitCommand = new(
            () => WeakReferenceMessenger
            .Default
            .Send<ShowProtraitMessage>(new(SelectedValkyrie.ProtraitUrl))
        );

        PlayVideoCommand = new(
            () => WeakReferenceMessenger
            .Default
            .Send<PlayVideoMessage>(new(SelectedSkill.VideoUrl))
        );

        HideSidePanelCommand = new(() => IsSidePanelVisible = false);
        ShowSidePanelCommand = new(() => IsSidePanelVisible = true);
    }

    public RelayCommand ShowProtraitCommand { get; }

    public RelayCommand ShowSidePanelCommand { get; }

    public RelayCommand HideSidePanelCommand { get; }

    public RelayCommand PlayVideoCommand { get; }
}
