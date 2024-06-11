using AllTheLists.ViewModels;

namespace AllTheLists.Pages;

public partial class ShoppingPage : ContentPage
{
	public ShoppingPage()
	{
		InitializeComponent();
		BindingContext = new ProductDisplaysViewModel();
	}
}