using AllTheLists.ViewModels;

namespace AllTheLists;

public partial class VirtualizedListViewPage : ContentPage
{
	public VirtualizedListViewPage()
	{
		InitializeComponent();
		BindingContext = new ProductsViewModel();
	}
}