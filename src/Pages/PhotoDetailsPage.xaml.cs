using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using AllTheLists.ViewModels;

namespace AllTheLists.Pages
{
    public partial class PhotoDetailsPage : ContentPage
    {
        public PhotoDetailsPage()
        {
            InitializeComponent();
        }

        private readonly PhotoDetailViewModel _viewModel;

        public PhotoDetailsPage(PhotoDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }
    }
}
