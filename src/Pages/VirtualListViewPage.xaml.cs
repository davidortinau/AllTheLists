using AllTheLists.ViewModels;

namespace AllTheLists;

public partial class VirtualListViewPage : ContentPage
{
	public VirtualListViewPage()
	{
		InitializeComponent();
		BindingContext = new ProductsViewModel();
	}
}