using System.Collections.ObjectModel;
using System.Diagnostics;
using AllTheLists.Models;
using AllTheLists.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Adapters;
using Contact = AllTheLists.Models.Contact;

namespace AllTheLists.ViewModels;

public partial class StreamingServiceViewModel : ObservableObject
{
    [ObservableProperty]
    private List<Contact> _contacts;

    [ObservableProperty]
    private List<string> _featured;
    
    public StreamingServiceViewModel()
    {
        Contacts = MockDataService.GenerateContacts().Where(c => c.ProfilePicture != string.Empty).Take(7).ToList();
        Featured = MockDataService.GenerateFeaturedMovies();
    }

    

}
