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
		var selectedItem = (sender as ListView).SelectedItem;
		if (selectedItem is Sample sampleModel)
		{
			await Navigation.PushAsync((Page)Activator.CreateInstance(sampleModel.Page));
		}
	}

	
}