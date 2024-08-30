using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using AllTheLists.Models;
using SelectionMode = Microsoft.Maui.Controls.SelectionMode;
using AllTheLists.Pages;
using CommunityToolkit.Mvvm.Input;

namespace AllTheLists.ViewModels
{
    public partial class PhotoGalleryViewModel : ObservableObject
    {
        private ObservableCollection<Photo> _photos;

        private ObservableCollection<object> _selectedPhotos;

        private string[] _guitars = new string[] { "shoe_01.png", "shoe_02.png", "shoe_03.png", "shoe_04.png", "shoe_05.png", "shoe_06.png", "shoe_07.png", "shoe_08.png" };
        
        private SelectionMode _selectionMode = SelectionMode.None;


        public Photo SelectedItem { get; set; }

        public SelectionMode SelectionMode { get => _selectionMode; set => SetProperty(ref _selectionMode, value); }

        public ObservableCollection<Photo> Photos { get => _photos; set => _photos = value; }

        public ObservableCollection<object> SelectedPhotos { get => _selectedPhotos; set => _selectedPhotos = value; }



        public PhotoGalleryViewModel()
        {
            InitData();            
        }

        [RelayCommand]
        async Task Pressed(Photo obj)
        {
            if (_selectionMode != SelectionMode.None)
            { 
                Debug.WriteLine($"Added {obj.ImageSrc}");
                if(_selectedPhotos.Contains(obj))
                    SelectedPhotos.Remove(obj);
                else
                    SelectedPhotos.Add(obj);
            }
            else
            {
                PhotoDetailViewModel vm = new();
                vm.ImageSrc = obj.ImageSrc;
                var mainWindow = Application.Current?.Windows[0];
                await mainWindow.Page.Navigation.PushAsync(new PhotoDetailsPage(vm));
                // Shell.Current.GoToAsync($"photo?src={obj.ImageSrc}");
            }
        }

        
        [RelayCommand]
        void Clear()
        {
            SelectionMode = SelectionMode.None;
            SelectedPhotos.Clear();
        }

        [RelayCommand]
        void LongPress(Photo obj)
        {
            Debug.WriteLine("LongPressed");
            if(_selectionMode == SelectionMode.None)
            {
                SelectionMode = SelectionMode.Multiple;
                SelectedPhotos.Add(obj);
                Debug.WriteLine("SelectionMode.Multiple");
            }
        }

        [RelayCommand]
        async Task GoShare()
        {
            var files = new List<ShareFile>();
            foreach (var obj in SelectedPhotos)
            {
                if (obj is Photo photo)
                {
                    var path = Path.Combine(FileSystem.CacheDirectory, photo.ImageSrc);
                    if (File.Exists(path)) { // it's a cache dir, so delete it
                        File.Delete(path);
                    }
                    var newFile = File.Create(path);
                    using (var stream = await FileSystem.OpenAppPackageFileAsync(photo.ImageSrc))
                    {
                        await stream.CopyToAsync(newFile);    
                    }

                    files.Add(new ShareFile(path));
                    System.Diagnostics.Debug.WriteLine($">>>>>>photo {photo.Id} {photo.ImageSrc} selected in OnShare. Exists {File.Exists(path)} at {path}");
                }
            }

            await Share.Default.RequestAsync(new ShareMultipleFilesRequest
            {
                Title = "Photos",
                Files = files
            });

            SelectedPhotos.Clear();
        }

        

        private void InitData()
        {
            _selectedPhotos = new ObservableCollection<object>();

            Random rand = new Random();
            _photos = new ObservableCollection<Photo>();
            for (int i = 0; i < 30; i++)
            {
                _photos.Add(
                    new Photo
                    {
                        ImageSrc = _guitars[rand.Next(_guitars.Length)],
                        Id = i
                    }
                );
            }

            OnPropertyChanged(nameof(Photos));
        }
    }


}
