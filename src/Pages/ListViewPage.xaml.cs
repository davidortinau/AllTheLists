using AllTheLists.ViewModels;

namespace AllTheLists;

public partial class ListViewPage : ContentPage
{
	public ListViewPage()
	{
		InitializeComponent();
		BindingContext = new ProductsViewModel();
	}
}