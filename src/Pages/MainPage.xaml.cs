using AllTheLists.Models;
using AllTheLists.Pages;

namespace AllTheLists;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void ListView_ItemSelected(object sender, EventArgs e)
	{
		// var selectedItem = (sender as ListView).SelectedItem;
		// if (selectedItem is Sample sampleModel)
		// {
		var tappedView = (sender as View);
		Sample itemData = (Sample)tappedView.BindingContext;
		if (itemData.Page != null)
		{
			await Navigation.PushAsync((ContentPage)Activator.CreateInstance(itemData.Page));
		}
		// }
	}

	
}