using AllTheLists.Pages;

namespace AllTheLists;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCollectionView_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new CollectionViewPage());
	}

	private void OnListView_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new ListViewPage());
	}

	private void OnVirtualListView_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new VirtualListViewPage());
	}

	private void OnVirtualizedListView_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new VirtualizedListViewPage());
	}

	private void OnReviews_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new ReviewsPage());
	}

	private void OnCheckIns_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new CheckInsPage());
	}

	private void OnLearning_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new LearningPage());
	}

	private void OnShopping_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new ShoppingPage());
	}
}