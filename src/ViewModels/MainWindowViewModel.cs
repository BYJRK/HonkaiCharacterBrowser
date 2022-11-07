using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HonkaiCharacterBrowser.Helpers;
using HonkaiCharacterBrowser.Models;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HonkaiCharacterBrowser.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<Valkyrie> Valkyries { get; private set; }

    [OnChangedMethod(nameof(ShowSidePanel))]
    public ValkyrieInfo SelectedValkyrie { get; private set; }

    public Valkyrie SelectedItem { get; set; }

    void OnSelectedItemChanged()
    {
        SelectionChanged(SelectedItem);
    }

    public bool IsSidePanelVisible { get; set; }

    private JArray valkyrieData;

    public bool IsProtraitVisible { get; private set; }

    public bool IsGroupView { get; private set; } = true;

    public bool IsVideoPanelVisible { get; set; }

    public MainWindowViewModel()
    {
        LoadedCommand = new(LoadAllAsync);
        HideSidePanelCommand = new(() => IsSidePanelVisible = false);
        SelectionChangedCommand = new(SelectionChanged);
        ToggleListBoxViewCommand = new(() => IsGroupView = !IsGroupView);

        ShowProtraitCommand = new(() => IsProtraitVisible = true);
        HideProtraitCommand = new(() => IsProtraitVisible = false);

        ShowVideoPanelCommand = new(() => IsVideoPanelVisible = true);
        HideVideoPanelCommand = new(() => IsVideoPanelVisible = false);
    }

    #region Relay Commands

    public AsyncRelayCommand<Valkyrie> SelectionChangedCommand { get; }

    public AsyncRelayCommand LoadedCommand { get; }

    public RelayCommand HideSidePanelCommand { get; }

    public RelayCommand ShowProtraitCommand { get; }
    public RelayCommand HideProtraitCommand { get; }

    public RelayCommand ToggleListBoxViewCommand { get; }

    public RelayCommand ShowVideoPanelCommand { get; }
    public RelayCommand HideVideoPanelCommand { get; }

    #endregion

    async Task LoadAllAsync()
    {
        valkyrieData = await WebHelper.GetValkyrieInfosAsync();

        var results = await WebHelper.GetValkyriesAsync();
        Valkyries = new ObservableCollection<Valkyrie>(results);
    }

    async Task SelectionChanged(Valkyrie v)
    {
        if (valkyrieData is null) return;

        var (armor, birthday) = await WebHelper.GetArmorBirthdayAsync(v.Url);

        var info =
            valkyrieData
            .First(v => v["ext"]
            .First(v => v["arrtName"].ToString() == "装甲名")["value"]
            .ToString() == armor)["ext"];

        var skills = new List<Skill>();
        var indexes = "一二三四五";
        foreach (var index in indexes)
        {
            var title = info.FirstOrDefault(v => v["arrtName"].ToString() == $"技能{index}名称")?["value"].ToString();
            if (string.IsNullOrEmpty(title)) continue;

            var desc = info.First(v => v["arrtName"].ToString() == $"技能{index}描述")["value"].ToString();
            var icon = info.First(v => v["arrtName"].ToString() == $"技能{index}图标（选中）")["value"][0]["url"].ToString();
            var prev = info.First(v => v["arrtName"].ToString() == $"技能{index}pc视频封面")["value"][0]["url"].ToString();
            var video = info.First(v => v["arrtName"].ToString() == $"技能{index}视频")["value"][0]["url"].ToString();

            skills.Add(new Skill(
                title,
                icon,
                desc,
                prev,
                video
            ));
        }

        SelectedValkyrie = new ValkyrieInfo(
            v.ChineseName,
            armor,
            birthday,
            info.First(v => v["arrtName"].ToString() == "作战方式")["value"].ToString(),
            info.First(v => v["arrtName"].ToString() == "角色大立绘图")["value"][0]["url"].ToString(),
            skills.ToArray()
        );
    }

    void ShowSidePanel() => IsSidePanelVisible = true;
}