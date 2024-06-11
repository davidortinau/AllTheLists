using System.Collections.ObjectModel;
using System.Diagnostics;
using AllTheLists.Models;
using AllTheLists.Models.Learning;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Adapters;

namespace AllTheLists.ViewModels;

public partial class LearningUnitsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Unit> _items;
    public ObservableCollectionAdapter<Unit> ItemsAdapter { get; private set; }

    public LearningUnitsViewModel()
    {
        Items = new ObservableCollection<Unit>(App.GenerateUnits());
        ItemsAdapter = new ObservableCollectionAdapter<Unit>(Items);
    }

    

}
