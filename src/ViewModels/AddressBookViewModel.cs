using System.Collections.ObjectModel;
using System.Diagnostics;
using AllTheLists.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Adapters;

namespace AllTheLists.ViewModels;

public partial class AddressBookViewModel : ObservableObject
{
    
    private List<Contact> _contacts;
    public ObservableCollectionAdapter<Contact> ContactsAdapter { get; private set; }

    [ObservableProperty]
    private List<ContactsGroup> _contactsGroups;

    private List<ContactsGroup> _unfilteredContactsGroups;

    public AddressBookViewModel()
    {
        _contacts = App.GenerateContacts().OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
        // ContactsAdapter = new ObservableCollectionAdapter<Contact>(Contacts);

        ContactsGroups = new List<ContactsGroup>();

        var groupedContacts = _contacts.GroupBy(c => c.LastName[0]).OrderBy(g => g.Key);

        foreach (var group in groupedContacts)
        {
            var contactsGroup = new ContactsGroup(group.Key.ToString(), group.ToList());
            ContactsGroups.Add(contactsGroup);
        }

        _unfilteredContactsGroups = new List<ContactsGroup>(ContactsGroups);
    }

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value)
    {
        Search();
    }

    [RelayCommand]
    void Search()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            // If the search text is empty, show all contacts
            ContactsGroups = _unfilteredContactsGroups;
        }
        else
        {
            // If the search text is not empty, show only contacts that contain the search text
            ContactsGroups = _unfilteredContactsGroups
                .Where(g => g.Any(c => 
                    c.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) 
                    || c.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }
    }
}
