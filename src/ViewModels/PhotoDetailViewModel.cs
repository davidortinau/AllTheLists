using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AllTheLists.ViewModels
{
    public partial class PhotoDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _imageSrc;

        public PhotoDetailViewModel()
        {
        }
    }
}
