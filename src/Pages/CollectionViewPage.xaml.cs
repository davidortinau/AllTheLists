using AllTheLists.ViewModels;

namespace AllTheLists;

public partial class CollectionViewPage : ContentPage
{
	public CollectionViewPage()
	{
		InitializeComponent();
		BindingContext = new ProductsViewModel();
	}
}