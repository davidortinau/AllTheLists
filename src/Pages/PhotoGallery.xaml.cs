using System.Diagnostics;
using AllTheLists.ViewModels;

namespace AllTheLists.Pages;

public partial class PhotoGallery : ContentPage
{
	public PhotoGallery()
	{
		InitializeComponent();
		BindingContext = new PhotoGalleryViewModel();
	}

	uint duration = 100;
        double openY = (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android) ? 20 : 60;
        double lastPanY = 0;
        bool isBackdropTapEnabled = true;

        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Backdrop.Opacity == 0)
            {
                await OpenDrawer();
            }
            else
            {
                await CloseDrawer();
            }
        }

        async void ClearItem_Clicked(System.Object sender, System.EventArgs e)
        {
            
            await CloseDrawer();
            PhotosCV.SelectedItems.Clear();
            
        }

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseDrawer();
            }
        }

        async void PanGestureRecognizer_PanUpdated(System.Object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                lastPanY = e.TotalY;
                Debug.WriteLine($"Running: {e.TotalY}");
                if (e.TotalY > 0)
                {
                    BottomToolbar.TranslationY = openY + e.TotalY;
                }

            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                //Debug.WriteLine($"Completed: {e.TotalY}");
                if (lastPanY < 110)
                {
                    await OpenDrawer();
                }
                else
                {
                    await CloseDrawer();
                }
                isBackdropTapEnabled = true;
            }
        }

        async Task OpenDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(1, length: duration),
                BottomToolbar.TranslateTo(0, openY, length: duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = false;
        }

        async Task CloseDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(0, length: duration),
                BottomToolbar.TranslateTo(0, 260, length: duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = true;
        }

        private void PhotosCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var asdfasdfasdf = PhotosCV.SelectedItems;
            //_vm.SelectedPhotos = new ObservableCollection<Photo>(PhotosCV.SelectedItems.ToList<Photo>());
        }
}
