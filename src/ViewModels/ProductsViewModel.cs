using System.Collections.ObjectModel;
using System.Diagnostics;
using AllTheLists.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Adapters;

namespace AllTheLists.ViewModels;

public partial class ProductsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Product> _products;
    public ObservableCollectionAdapter<Product> ProductsAdapter { get; private set; }

    public ProductsViewModel()
    {
        Products = new ObservableCollection<Product>(App.GenerateProducts());
        ProductsAdapter = new ObservableCollectionAdapter<Product>(Products);
    }

    [RelayCommand]
    void ItemTapped(Product product)
    {
        Debug.WriteLine($"Tapped: {product.ImageUrl}");
        OnPropertyChanged(nameof(product));
    }
    
    

}
