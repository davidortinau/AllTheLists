using System.Collections.ObjectModel;
using AllTheLists.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AllTheLists.ViewModels;

public partial class ProductDisplaysViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ProductDisplay> _products;

    public ProductDisplaysViewModel()
    {
        Products = new ObservableCollection<ProductDisplay>(App.GenerateProductDisplays());
    }
}
