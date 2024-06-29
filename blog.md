# All the Lists in .NET MAUI

In every app you will inevitably have a list of things to display and be faced with choosing the best control to use. Here I will muse on how I have approached these decisions focusing on mobile applications. 

I surveyed the apps on my phone, and snagged a cross-section of different experiences. For the data I wrote a `MockDataService` to generate useful yet random content. For images I use a combination of [Lorum Picsum](https://picsum.photos), and images I crafted with [ChatGPT](https://chatgpt.com/).

I think the results are pretty nice, although I warn they are not production polished and feature complete.

![feature image of various layouts](images/samples_gallery.png)

Jump to each of the samples below:

* [Basic list](#layout-1-basic-list) - even rows
* [Reviews](#layout-2-reviews-uneven-rows) - uneven rows
* [Social check-in](#layout-3-social-check-in-uneven-rows-complex-layout) - complex layout
* [Learning course](#layout-4-learning-course-expand-and-contract) - expanding rows
* [Who's Watching](#layout-5-whos-watching-flex-layout) - Flex layout
* [Mailboxes](#layout-6-mailboxes-expand-and-contract) - expanding rows
* [Contacts](#layout-7-contacts-grouping-search) - grouping and search
* [Shopping](#layout-8-shopping-header-data-template-selector-infinite-scroll) - header, multiple data templates, infinite scroll

Before I get into each sample, I want to get out of the way some general thoughts.

**Anything that does everything does nothing well.** In order for a generalized control to be flexible enough to do meet a wide variety of needs, compromises will be made in its implementation. This may lead you to being frustrated when it doesn't meet your expectations. A specialized control that only does what you need it to will best meet the need of that scenario. The other side of that sharp edge though is your knowledge and skill needs to also level up from general to specialized. 

**Flat is faster than fat.** It's true. If speed is important to your scenario, then a layout that avoids a lot of UI and nesting of controls will perform better at scale because it requires fewer measure and layout calls. Avoid measuring at all costs when performance is critical; give your UI explicit size anytime you can.

**UX > UI** I see a lot of apps struggling with list scenarios because they jam a ton of UI into them to get the job done, rather than leaning on good UX principles. Do you really need a whole chat experience in every row of the list, or could you navigate to another page? Perhaps you could use a modal experience or a bottom sheet? Anytime your mobile UI have more than 1 clear call to action, then you're in danger of the UI being less efficient instead of more efficient for your user. Solve problems with UX before UI.

## Overview of .NET MAUI List Controls

In my sample I've used 3 built-in controls, and 2 community controls that all demonstrate different approaches with strengths and weaknesses. .NET MAUI provides `CollectionView`, `ListView`, and `BindableLayout`. From the community I chose [`VirtualListView`](https://www.nuget.org/packages/Redth.Maui.VirtualListView) and [`VirtualizeListView`](https://www.nuget.org/packages/MPowerKit.VirtualizeListView). There are many other options, a few of which I list at the end for you to evaluate yourself.

|               | CollectionView | ListView | BindableLayout | VirtualLayout | VirtualizeLayout |
|---------------|----------------|----------|----------------|---------------|-------------------|
| **Virtualized**   | Yes            | Yes      | No             | Yes           | Yes               |
| **Pull-to-Refresh** | Yes - with RefreshView | Yes | Yes - with RefreshView |                 |                   |
| **Layout - Vertical** | Yes          | Yes      | Yes            |               |                   |
| **Layout - Horizontal** | Yes        | No       | Yes            |               |                   |
| **Layout - Grid** | Yes              | No       | Yes            |               |                   |
| **Layout - Custom** | Yes            | No       | Yes            |               |                   |
| **Behavior**      | Platform specific | Platform specific | Cross-platform | Platform specific | Cross-platform |
| **Grouped Data**  | Yes              | Yes      | No             | Yes           |                   |
| **Header / Footer** | Yes            | Yes      | No             | Yes           |                   |
| **Context Menu Items** | Yes - with SwipeView | Yes | Yes - with SwipeView |               |                   |
| **Predefined Templates** | No         | Yes      | No             | No            | No                |
| **Single Selection** | Yes | Yes | No | Yes | Yes |
| **Multiple Selection** | Yes | No | No | No | No |
| **Edit mode** | No | Yes | No | No | No |

I will mostly focus on `CollectionView` over `ListView` unless there is a compelling reason to prefer the latter.

### Additional Performance Notes

If speed of rendering and scroll is of utmost importance to you, then these notes are for you.

* **Layout Lifecycle** - understanding the [layout measure and arrange process](https://learn.microsoft.com/dotnet/maui/user-interface/layouts/custom?view=net-maui-8.0#layout-process) is essential when you're trying to diagnose and improve the rendering performance of a complex UI. In general, if you know the size of something, then provide it.

* **Compiled Bindings** will improve the rendering and updating of your XAML data-bound controls by telling the compiler the type that is being used. On any enclosing XAML element with a BindingContext specify the type with, for example, x:DataType="model:Sample".

* **Binding Modes** - the default binding mode for bindable properties differs from control to control, and property to property. Most are `OneWay` such as `View.Rotation` or `View.Scale`, while properties often used to capture user input are `TwoWay` such as `Entry.Text` and `ListView.IsRefreshing`. In most cases the default will be what you expect and need, but keep in mind you can change these and have other options such as `OneTime` and `OneWayToSource`. [Documentation](https://learn.microsoft.com/dotnet/maui/fundamentals/data-binding/binding-mode?view=net-maui-8.0)

* **ObservableCollection vs List** if your data won't be updating dynamically, and perhaps it's a `OneTime` binding, then use `List`.

* **Images** - make sure your images are appropriately sized for their use on screen. Scaling down images at runtime can be a massive demand on resources, quickly sending you into memory and crash issues. Raster images render faster than vector images in almost every situation. AND if you're loading images from a remote source, be sure you're not blocking the UI loading them. Use a control like FFImage to show a placeholder image and lazy load the remote image. Also be aware you can customize the [image caching policy](https://learn.microsoft.com/dotnet/maui/user-interface/controls/image?view=net-maui-8.0#image-caching) in .NET MAUI.

* **Release vs Debug** - when evaluating performance, you must be using a release build. There are just so many things going on in a debug build that slow the app down that it's not at all useful to judge. Produce a release build and measure that. And know your options for AOT (Ahead of Time) compilation. .NET 9 has a preview Native AOT for iOS, however it's extremely strict and most libraries are not compatible. We did a lot of work in .NET MAUI itself to make it compatible. Android has partial (startup tracing) and full AOT to choose from. 

* **Test on Device** - be sure to review release builds on device. If you know the target device and OS version of your users, then ideally test on that. I've used my iPhone 15 Pro, and a Pixel 5. I'm 99.9999% of cases, iOS isn't going to be the performance concern. 

* **Layout compression (obsolete)** was a run-time optimization in Xamarin.Forms what would remove wrapping layouts from the visual tree. If the layout had no background color or received no user input via gestures, then it could safely be eliminated from the actual UI rendered to the screen. This was useful in Xamarin.Forms where nearly all views (renderers) were wrapped in views. Later in Xamarin.Forms a set of updated renderers was introduced aptly named "fast renderers" which removed those wrapping views. In .NET MAUI this redundancy was eliminated, and **Layout Compression** was not implemented. The API remains, but should be deprecated and you should treat it so.

## Layout 1: Basic List

This is the most simple and common use of a list, so there's not much to say about it. All the rows are exactly the same height and layout. For this need you cannot go wrong between the virtualized controls. They all perform this scenario very well, even when displaying 10,000 rows.

```xml
<CollectionView ItemsSource="{Binding Products}">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <v:ProductListItem />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

```xml
<ListView ItemsSource="{Binding Products}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <v:ProductListItem />
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

You may be wondering why I'm not binding anything above to the ProductListItem. BindingContext automatically propagates in this (and most) cases to the children. Here the provided BindingCOntext is the single Product.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:ffimageloading="clr-namespace:FFImageLoading.Maui;assembly=FFImageLoading.Maui"
             xmlns:m="clr-namespace:AllTheLists.Models"
             xmlns:vm="clr-namespace:AllTheLists.ViewModels"
             x:DataType="m:Product"
             x:Class="AllTheLists.Views.ProductListItem">
    <Grid Padding="16" ColumnDefinitions="80,*,40" ColumnSpacing="16">
        <ffimageloading:CachedImage 
            Source="{Binding ImageUrl}" 
            HeightRequest="80" 
            WidthRequest="80" 
            LoadingPlaceholder="https://via.placeholder.com/80" 
            ErrorPlaceholder="error.png">
        </ffimageloading:CachedImage>
        <VerticalStackLayout Grid.Column="1" Padding="10">
            <Label Text="{Binding Name}" FontSize="16" />
            <Label Text="{Binding Price, StringFormat='Price: {0:C}'}" FontSize="14" />
            <Label Text="{Binding Description}" FontSize="12" LineBreakMode="TailTruncation" />
        </VerticalStackLayout>      
        <CheckBox Grid.Column="2" VerticalOptions="Center" />  
    </Grid>
</ContentView>
```

In addition to samples for `ListView` and `CollectionView` I checked out `VirtualListView` by Redth and `VirtualizedListView` by MPowerKit. The latter is a completely cross-platform virtualized control, which is an interesting approach. If consistency across platforms is your goal, then that might be a great option for you. 

## Layout 2: Reviews [Uneven rows]

While the template in this example is not very complex, it does have a variable length string that wraps in a `Label`. This is something that was problematic in early releases of .NET MAUI where the text would be clipped or flow offscreen. By default the `ItemSizingStrategy` is to measure only the first item and assume all the rest of the items are the same size. This is much more performant for obvious reasons.

To accomodate the variable sizing I need to use a strategy that measures all items, or rather each item individually. In practice this performs well, and scrolls very smoothly.

```xml
<CollectionView ItemsSource="{Binding Reviews}" ItemSizingStrategy="MeasureAllItems">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <v:ReviewListItem />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

```xml
<Grid ColumnDefinitions="40,*" 
    RowDefinitions="Auto,Auto"
    ColumnSpacing="8"
    Margin="16">
    <Image 
        Source="{Binding StatusImage}"
        Grid.Column="0" 
        Grid.RowSpan="2" 
        HeightRequest="20"
        WidthRequest="20"
        VerticalOptions="Start" 
        HorizontalOptions="Center"/>

    <VerticalStackLayout Grid.Column="1" Spacing="8">
        <Label 
            Text="{Binding Author}"
            FontSize="18"
            FontAttributes="Bold" />
        <Label Text="{Binding Comment}" MaxLines="5" Margin="0,0,0,8" />
        <Label Text="{Binding Car}" TextColor="Gray"/>
        <Label Text="{Binding ChargerType}" TextColor="Gray"/>
    </VerticalStackLayout>

    <Label Text="{Binding CreatedAt, StringFormat='{0:MM/dd/yyyy}'}" 
        Grid.Row="0" 
        Grid.Column="1" 
        FontSize="10"
        TextColor="Gray"
        HorizontalOptions="End" 
        VerticalOptions="Start" />

    <BoxView 
        HeightRequest="1" 
        BackgroundColor="LightGray"
        VerticalOptions="End" 
        Grid.Column="1"
        TranslationY="16" />
</Grid>
```

## Layout 3: Social Check-in [Uneven rows, Complex Layout]

For this sample I took inspiration from [Untapped](https://untappd.com), a social beer enthusiast app. The Activity feed shows the beer check-ins of your friends including a rating and an optional photo. When the photo is present the template is a bit taller, so I again need to handle uneven rows. 

In this scenario, `CollectionView` has a clear advantage over `ListView` because I'm able to specify a spacing between the items by calling up the `LinearItemsLayout`.

```xml
<CollectionView 
    ItemSizingStrategy="MeasureAllItems"
    ItemsSource="{Binding CheckIns}">
    <CollectionView.ItemsLayout>
        <LinearItemsLayout Orientation="Vertical" ItemSpacing="10" />
        </CollectionView.ItemsLayout>
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <v:CheckInListItem />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

To accomodate the different looks I could have opted for a [DataTemplateSelector](https://learn.microsoft.com/dotnet/maui/fundamentals/datatemplate?view=net-maui-8.0#create-a-datatemplateselector), but I chose instead to add a `HasImage` read-only property to the model in order to show/hide the `Image` control as well as adjust the Y position of the content.

```csharp
public class Product
{
    ///...

    public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);
}
```

```xml
<Border 
    Grid.Row="1"
    TranslationY="{Binding Product.HasImage, Converter={StaticResource BoolToIntConverter}}"
```

I had not previously used the `BoolToObjectConverter` from the [.NET MAUI Community Toolkit](https://learn.microsoft.com/dotnet/communitytoolkit/maui/converters/bool-to-object-converter). What a tasty discovery! 

```xml
<mct:BoolToObjectConverter 
    x:Key="BoolToIntConverter" 
    TrueObject="-60" 
    FalseObject="0"/>
```

Also great for flip-flopping colors.

```xml
<mct:BoolToObjectConverter 
    x:Key="BoolToColorBrushConverter" 
    TrueObject="#FFFFFF" 
    FalseObject="#000000"/>
```

References:
* [.NET MAUI Community Toolkit](https://learn.microsoft.com/dotnet/communitytoolkit/maui/)
* [FontAwesome Icons](https://fontawesome.com)
* [Lorem Picsum Photos](https://picsum.photos)
* [Rating Control](https://www.nuget.org/packages/pankaj.util.RatingControl)

## Layout 4: Learning Course [Expand and Contract]

Those of you who know me are aware I enjoy language learning. One of the apps I use has a nice UI that presents courses in units and lessons. Tapping a unit expands to display the different lessons with chapters in a table of contents, roadmap fashion. 

Originally I tried this with `CollectionView` and `ListView`, but this confirmed a bug in .NET MAUI on iOS where resizing at runtime doesn't trigger the rest of the list control to resize as you would expect. As of version 8.0.60 this works great on Android. 

As I evaluated the content to be displayed, I recognized I don't have a LOT of data. On each page of the app I have usually 4 units each with a variable number of chapters and lessons that never exceeds 10. 

For these reasons I chose to use [`BindableLayout`](https://learn.microsoft.com/dotnet/maui/user-interface/layouts/bindablelayout?view=net-maui-8.0). In fact, this sample uses 3 nested `BindableLayout`. 😲 Did this become a problem? Nope.

`BindableLayout` is a bit of an odd duck, and perhaps in retrospect it should have been a standalone control like the others. Instead it's an attached property that you can add to any other layout. So rather than starting with the control and specifying a layout like with `CollectionView`, you start with the layout you prefer and tag on the items source and data template. Simple enough.

```xml
<ScrollView>
    <VerticalStackLayout Spacing="10" 
        BindableLayout.ItemsSource="{Binding Items}">
        <BindableLayout.ItemTemplate>
            <DataTemplate>
                <v:LearningUnitListItem />
            </DataTemplate>
        </BindableLayout.ItemTemplate>
    </VerticalStackLayout>
</ScrollView>
```

The `LearningUnitListItem` displays the primary box, and a hidden list that is a loop over the chapters and lessons.

To expand and contract the list of chapters and lessons I'm simply using a click handler and toggling the visibility of the `VerticalStackLayout` that contains that content.

References:

* [BindableLayout](https://learn.microsoft.com/dotnet/maui/user-interface/layouts/bindablelayout?view=net-maui-8.0)


## Layout 5: Who's Watching [Flex layout]

Inspired by Netflix and Disney+ and "insert other streaming service" I made a "Who's Watching" sample. This one is very simple. It's a `FlexLayout` with `BindableLayout`.

```xml
<FlexLayout 
    Direction="Row" 
    JustifyContent="Center" 
    Wrap="Wrap"
    BindableLayout.ItemsSource="{Binding WhoIsWatching}" 
    VerticalOptions="Center">
    <BindableLayout.ItemTemplate>
        <DataTemplate x:DataType="m:Contact">
            <VerticalStackLayout 
                HorizontalOptions="Center" 
                Spacing="8" 
                FlexLayout.Basis="40%" 
                FlexLayout.AlignSelf="Start">
                <Image 
                    Source="{Binding ProfilePicture}" 
                    WidthRequest="80" 
                    HeightRequest="80" 
                    Aspect="AspectFill"
                    BackgroundColor="Transparent">                        <Image.Clip>
                        <EllipseGeometry Center="40, 40" RadiusX="40" RadiusY="40" />
                    </Image.Clip>
                </Image>
                <Label 
                    Text="{Binding FirstName}" 
                    HorizontalOptions="Center" />
            </VerticalStackLayout>
        </DataTemplate>
    </BindableLayout.ItemTemplate>
</FlexLayout>
```

References:

* [BindableLayout](https://learn.microsoft.com/dotnet/maui/user-interface/layouts/bindablelayout?view=net-maui-8.0)
* [FlexLayout](https://learn.microsoft.com/dotnet/maui/user-interface/layouts/flexlayout?view=net-maui-8.0)

## Layout 6: Mailboxes [Expand and Contract]

To reproduce the Mailboxes UI I chose `BindableLayout` and `Expander` from the .NET MAUI Community Toolkit. While a user could end up with a lot of mail accounts that would then benefit from some virtualization, it seems reasonable to start here and grow up into a `CollectionView` when necessary.

Since I've covered use of `BindableLayout` already, I'll focus now on the `Expander`. The control has 2 main parts, the header and the content. The header is always visible, and the content is was is shown/hidden based on the user interaction. 

In order to toggle the chevron indicator for open/closed, I started with 2 `Label` controls to display the font icons, and used a relative source binding to watch the `IsExpanded` property of the parent control. Since I'm within the control, I can reference it this way rather than by name. I refactored this to a single `Label` and used the magnificent `BoolToObjectConverter`. How did I ever code without that?!

```xml
<mct:Expander>
    <mct:Expander.Header>
        <Grid ColumnDefinitions="*,100,50" RowDefinitions="40">
            <Label 
                Text="Ortinau" 
                Grid.Column="0" 
                FontSize="Subtitle" 
                VerticalOptions="Center" />
            <Label Text="38386" Grid.Column="1" 
                Style="{StaticResource SecondaryLabel}"
                HorizontalOptions="End" HorizontalTextAlignment="End" 
                IsVisible="{Binding Path=IsExpanded, Source={RelativeSource AncestorType={x:Type mct:Expander}}, Converter={StaticResource InvertedBoolConverter}}" />
            <Label 
                Text="{Binding Path=IsExpanded, Source={RelativeSource AncestorType={x:Type mct:Expander}},Converter={StaticResource BoolToChevronConverter}}" 
                FontSize="14" 
                FontFamily="FluentUI" 
                Style="{StaticResource SecondaryLabel}" 
                TextColor="{StaticResource ActionColor}"
                Grid.Column="2" 
                VerticalOptions="Center"                            
                HorizontalOptions="Center" />    
        </Grid>                    
    </mct:Expander.Header>

    <mct:Expander.Content>
        <Border>
            <VerticalStackLayout>
                <BindableLayout.ItemsSource>
                    ...
                </BindableLayout.ItemsSource>
                <BindableLayout.ItemTemplate>
                    <DataTemplate>
                        <Grid 
                            ColumnDefinitions="60,*,100,50" 
                            RowDefinitions="40,1">
                            <Image 
                                Aspect="Center" 
                                HorizontalOptions="Center" 
                                VerticalOptions="Center">
                                <Image.Source>
                                    <FontImageSource 
                                        Glyph="{Binding Icon}" 
                                        FontFamily="FluentUI" 
                                        Size="18" 
                                        Color="{StaticResource ActionColor}" />
                                </Image.Source>
                            </Image> 
                            <Label 
                                Text="{Binding Name}" 
                                Grid.Column="1" 
                                FontSize="14" 
                                VerticalOptions="Center" />
                            <Label 
                                Text="{Binding UnreadCount}" 
                                Grid.Column="2" 
                                Style="{StaticResource SecondaryLabel}"
                                HorizontalOptions="End" 
                                HorizontalTextAlignment="End" />
                            <Label 
                                Text="{x:Static f:FluentUI.chevron_right_12_regular}" 
                                Grid.Column="3" 
                                Style="{StaticResource SecondaryLabel}"
                                VerticalOptions="Center"
                                FontSize="14" 
                                FontFamily="FluentUI" 
                                HorizontalOptions="Center" />

                            <BoxView 
                                Grid.ColumnSpan="4" 
                                Grid.Row="1" 
                                Margin="16,0,0,0" 
                                HeightRequest="1" 
                                Color="{AppThemeBinding Light=#f3f3f4, Dark=#333333}" />
                        </Grid>
                    </DataTemplate>
                </BindableLayout.ItemTemplate>
            </VerticalStackLayout>
        </Border>
    </mct:Expander.Content>
</mct:Expander>
```

References:

* [BoolToObjectConverter](https://learn.microsoft.com/dotnet/communitytoolkit/maui/converters/bool-to-object-converter)
* [BindableLayout](https://learn.microsoft.com/dotnet/maui/user-interface/layouts/bindablelayout?view=net-maui-8.0)
* [Expander](https://learn.microsoft.com/dotnet/communitytoolkit/maui/views/expander)
* [Relative bindings](https://learn.microsoft.com/dotnet/maui/fundamentals/data-binding/relative-bindings?view=net-maui-8.0)

## Layout 7: Contacts [Grouping, Search]

Getting back into a sample with need for virtualization, grouping, and search I reproduced a Contacts list. 

### Header

My contact needed to appear at the top of the list and scroll away before the rest of the content. For that I added a header to the `ListView`. Notice it doesn't NOT take a `DataTemplate` since there can be only one of these and there's no need to instantiate it lazily.

```xml
<ListView.Header>
    <HorizontalStackLayout Spacing="16" Padding="16">
        <Border StrokeShape="RoundRectangle 40"
            StrokeThickness="0">
            <Image Source="avatar_01.png" 
            WidthRequest="80" 
            HeightRequest="80" 
            Aspect="AspectFill" 
            VerticalOptions="Center" 
            />
        </Border>
        <Label Text="David Ortinau" 
            FontSize="20" 
            FontAttributes="Bold" 
            VerticalOptions="Center" />
    </HorizontalStackLayout>        
</ListView.Header>
```

### Grouping

Preparing your data sources to be grouped and searchable is the first step. In my approach I get all my contacts in an ordered flat list, group them by the first initial of the last name, and then add them to a list of grouped contacts. The final piece is setting that aside to a new list that is unfiltered on which I can perform searches.

```csharp
_contacts = MockDataService.GenerateContacts().OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
        
ContactsGroups = new List<ContactsGroup>();

var groupedContacts = _contacts.GroupBy(c => c.LastName[0]).OrderBy(g => g.Key);

foreach (var group in groupedContacts)
{
    var contactsGroup = new ContactsGroup(group.Key.ToString(), group.ToList());
    ContactsGroups.Add(contactsGroup);
}

_unfilteredContactsGroups = new List<ContactsGroup>(ContactsGroups);
```

To display the grouped list I went with `ListView` primarily because this scenario is one of the fundamental scenarios it was made for. To group I set `IsGroupingEnabled="True"` and provide a template for the group header.

```xml
<ListView.GroupHeaderTemplate>
    <DataTemplate>
        <ViewCell>
            <Label Text="{Binding GroupName}" 
            FontSize="18" 
            FontAttributes="Bold"
            Padding="12,0,0,0"
            VerticalOptions="Center"
            Background="Transparent" />
        </ViewCell>
    </DataTemplate>
</ListView.GroupHeaderTemplate>
```

And just like that I have the basic grouped list.

### Search

.NET MAUI provides a `SearchBar` control, so I added that above the `ListView` on the page. As the user types, the `SearchCommand` is executed. The `Text` property does default to a `TwoWay` binding, so I didn't need to specify that, but I wasn't sure until [reading the documentation](https://learn.microsoft.com/dotnet/maui/fundamentals/data-binding/binding-mode?view=net-maui-8.0#two-way-bindings) about binding modes for writing this post. ;)

```xml
<SearchBar 
    x:Name="SearchBar" 
    Placeholder="Search" 
    Text="{Binding SearchText, Mode=TwoWay}"
    SearchCommand="{Binding SearchCommand}" 
    VerticalOptions="Start" 
    BackgroundColor="{AppThemeBinding Light=White, Dark=Black}"
    />
```

The search command filters down the unfiltered list and repopulates the `ContactsGroups` that is bound to the `ListView`.

```csharp
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
                c.FirstName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) 
                || c.LastName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase)))
            .ToList();
    }
}
```

BUT I was having a problem because I would type and the list would filter, but I was also getting results I didn't expect. Why?!

I explained my situation to Copilot and it explained (as I suspected) that I was only searching on the group and not the contacts within the group as I expected. Copilot provided the solution. 

```csharp
ContactsGroups = _unfilteredContactsGroups
    .Select(g => new ContactsGroup(g.GroupName, g.Where(c =>
        c.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
        || c.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList()))
    .Where(g => g.Any())
    .ToList();
```

References:

* [ListView](https://learn.microsoft.com/dotnet/maui/user-interface/controls/listview?view=net-maui-8.0)
* [ListView grouping](https://learn.microsoft.com/dotnet/maui/user-interface/controls/listview?view=net-maui-8.0#display-grouped-data)
* [ListView header](https://learn.microsoft.com/dotnet/maui/user-interface/controls/listview?view=net-maui-8.0#headers-and-footers)
* [RelayCommand](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/generators/relaycommand)
* [SearchBar](https://learn.microsoft.com/dotnet/maui/user-interface/controls/searchbar?view=net-maui-8.0)

## Layout 8: Shopping [Header, Data template selector, infinite scroll]

Inspired by the Adidas app, I had a bit of fun making this one. In addition to a header, and making product images with ChatGPT, the display pattern is unique. You begin thinking it's going to be a grid layout with 2 columns, but then after 4 rows you hit a product that spans both columns. Ok, so 4 and then 1, right? Wrong. From there on out it's 2 and 1. 🤯

Because I need to load data in batches as the user reaches the end of the list, I chose `CollectionView` which has this feature built-in.

### Filter Header

So the header is simple: a horizontal scrolling set of buttons to filter the list. 

```xml
<CollectionView.Header>
    <v:FilterView />
</CollectionView.Header>
```

`FilterView.xaml`

```xml
<Grid ColumnDefinitions="Auto,*" ColumnSpacing="16" Margin="16,16,-16,16">
    <Image 
        HeightRequest="24" 
        WidthRequest="24" 
        Aspect="Center"
        Background="Transparent">
        <Image.Source>
            <FontImageSource FontFamily="FontAwesome" 
                Glyph="{x:Static f:FontAwesome.Filter}" 
                Size="14" 
                Color="{AppThemeBinding Light={StaticResource Gray900}, Dark={StaticResource Gray300}}"/>
        </Image.Source>
        </Image>
    <ScrollView Orientation="Horizontal" Grid.Column="1" HorizontalScrollBarVisibility="Never">
        <HorizontalStackLayout Spacing="8">
            <Button Text="705" Style="{StaticResource FilterButtonStyle}" />
            <Button Text="SAMBA" Style="{StaticResource FilterButtonStyle}" />
            <Button Text="GAZELLE" Style="{StaticResource FilterButtonStyle}" />
            <Button Text="ULTRABOOST" Style="{StaticResource FilterButtonStyle}" />
            <Button Text="ADIZERO" Style="{StaticResource FilterButtonStyle}" />
            <Button Text="FORUM" Style="{StaticResource FilterButtonStyle}" />
            <Button Text="SUPERSTAR" Style="{StaticResource FilterButtonStyle}" />
            <Button Text="CAMPUS" Style="{StaticResource FilterButtonStyle}" />
            <Button Text="LITE RACER" Style="{StaticResource FilterButtonStyle}" />
            <Button Text="2000S" Style="{StaticResource FilterButtonStyle}" />                
        </HorizontalStackLayout>
    </ScrollView>
</Grid>
```

Of course in a real app the buttons would be sourced from some collection and I would use a `BindableLayout` for them.

### Funky Layout Pattern

How could I achieve this layout pattern? I chose to massage the data to represent how it would be displayed. That's what a ViewModel is for anyway. With more help from Copilot I told it the pattern I needed to achieve and watched the code flow! I KNOW KUNG FU!!!

```csharp
_productDisplays = new List<ProductDisplay>();
for (int i = 0; i < count; i++)
{
    if (i < 4)
    {
        _productDisplays.Add(new ProductDisplay
        {
            Products = GenerateProducts().GetRange(i * 2, 2)
        });
    }
    else if (i % 3 == 1)
    {
        _productDisplays.Add(new ProductDisplay
        {
            Products = GenerateProducts().GetRange(i * 2 - 1, 1)
        });
    }
    else
    {
        _productDisplays.Add(new ProductDisplay
        {
            Products = GenerateProducts().GetRange(i * 2 - 2, 2)
        });
    }
```

Seeing `GenerateProducts()` repeated may look like it's regenerating data over and over, but I'm actually returning the cached data set once it's populated. It doesn't read well, I admit.

Now that I have the data representing the pattern I need of 4:1:2:1:2:1:2:1 etc. I can move on to the data template. 

The `CollectionView` implements a linear items layout by default and that's just fine. Using a data template selector I can have 2 templates based on how many items I need to display: Mono and Duo.

```csharp
public class ShopTemplateSelector : DataTemplateSelector
{
    public DataTemplate MonoTemplate { get; set; }
    public DataTemplate DuoTemplate { get; set; }
    public DataTemplate LoadingMoreTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        ProductDisplay productDisplay = (ProductDisplay)item;
        if(productDisplay.IsLoading)
        {
            return LoadingMoreTemplate;
        }

        return ((ProductDisplay)item).Products.Count < 2 ? MonoTemplate : DuoTemplate;
    }
}
```

The `DuoTemplate` is the more interesting one was it just displays 2 `MonoTemplate`s side by side. 

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:v="clr-namespace:AllTheLists.Views"
             xmlns:m="clr-namespace:AllTheLists.Models"
             x:DataType="m:ProductDisplay"
             x:Class="AllTheLists.Views.DuoProductListItem">
    <Grid ColumnDefinitions="*,*" ColumnSpacing="4">
        <v:MonoProductListItem Grid.Column="0" BindingContext="{Binding Products[0]}" />
        <v:MonoProductListItem Grid.Column="1" BindingContext="{Binding Products[1]}" />
    </Grid>
</ContentView>
```

And just like that, I have the display I need and I don't feel like it's overly complex.

### Infinite Scrolling

As the user reaches near the end of the list, I need to start fetching more data and display an indicator to the user that this is happening. The indicator is meant to appear at the bottom of the list.

The `CollectionView` has properties to help with the first part. `RemainingItemsThreshold` tells the control when that many items remain to be displayed, then call the event `RemainingItemsThresholdReached` and execute the command `RemainingItemsThresholdReachedCommand`. In my case, I use both of the latter, but you may only need the command. More on why I do this below.

```xml
RemainingItemsThreshold="4"
RemainingItemsThresholdReached="CollectionView_RemainingItemsThresholdReached"
RemainingItemsThresholdReachedCommand="{Binding OnThresholdReachedCommand}"
```

The `OnThresholdReachedCommand` fetches more data and appends it to the end of the `ObservableCollection`.

```csharp
[RelayCommand]
async Task OnThresholdReached()
{
    if(IsLoadingMore) return;

    IsLoadingMore = true;
    
    VisibleProducts.Add(new ProductDisplay { IsLoading = true });
    
    await Task.Delay(4000); // fake a server call delay, allows the loading template to show
    
    VisibleProducts.Remove(VisibleProducts.Last());
    
    var newProducts = Products.Skip(VisibleProducts.Count).Take(16);
    foreach (var product in newProducts)
    {
        VisibleProducts.Add(product);
    }

    await Task.Delay(200); // tiny delay for a ui refresh
    IsLoadingMore = false;
}
```

The attentive reader will have noticed some code in the data template selector in from the previous section, which connects now with the command above. As soon as the call is made to get more data, create a blank `ProductDisplay` object which has one job, to tell the user `IsLoading=true`. In the data template selector, I opt to display this special template and add it to the bottom of the list.

```csharp
if(productDisplay.IsLoading)
{
    return LoadingMoreTemplate;
}
```

As soon as my data arrives, I remove the last item from the collection and resume adding real data to be displayed.

The `IsLoadingMore` boolean protects from calling this method while it's already in progress. Maybe there's a better way to do this, but old habits...

To wrap this up, why am I also handling the event with `CollectionView_RemainingItemsThresholdReached`? It's to work around a bug on one of the platforms where the command is not being executed.

```csharp
private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
{
    ((ProductDisplaysViewModel)BindingContext).ThresholdReachedCommand.Execute(null);
}
```

## Conclusion

In conclusion, when choosing the right control for your app scenario, you have options! Consider your specific requirements and the level of customization you need for your list or layout. Prefer `CollectionView` over `ListView`, and don't ignore `BindableLayout`!

As I was writing this, I kept seeing more things to add and try such as editing and ordering a list. I suppose that's what tomorrow is for. 

I hope this was a fun read and you have learned a thing or two. Maybe you have a better way to do something, or you hate how I did it. Code can be a very personal thing to. Whatever your reaction, be energized to go make something amazing to share with the world. 