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
    }

    

}
