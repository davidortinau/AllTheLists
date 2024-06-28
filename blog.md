# All the Lists in .NET MAUI

In every app you will inevitably have a list of things to display and be faced with choosing the best control to use. Here I will muse on how I have approached these decisions.

I surveyed the apps on my phone, and snagged a cross-section of different experiences. For the data I wrote a MockDataService to generate useful but random content. For images I use a combination of [Lorum Picsum](https://picsum.photos), and images I crafted with [ChatGPT](https://chatgpt.com/).

![feature image of various layouts](images/samples_gallery.png)

Jump to each of the samples below:

* Basic list - even rows
* Reviews - uneven rows
* Social check-in - complex layout
* Learning course - expanding rows
* Who's Watching - Flex layout
* Inbox - expanding rows
* Contacts - grouping and search
* Shopping - header and multiple data templates

Before I get into each sample, I want to get out of the way some general thoughts.

**Anything that does everything does nothing well.** In order for a generalized control to be flexible enough to do meet a wide variety of needs, compromises will be made in its implementation. This may lead you to being frustrated when it doesn't meet your expectations. A specialized control that only does what you need it to will best meet the need of that scenario. The other side of that sharp edge though is your knowledge and skill needs to also level up from general to specialized. 

**Flat is faster than fat.** It's true. If speed is important to your scenario, then a layout that avoids a lot of UI and nesting of controls will perform better at scale because it requires fewer measure and layout calls. Avoid measuring at all costs when performance is critical; give your UI explicit size anytime you can.

**UX > UI** I see a lot of apps struggling with list scenarios because they jam a ton of UI into them to get the job done, rather than leaning on good UX principles. Do you really need a whole chat experience in every row of the list, or could you navigate to another page? Perhaps you could use a modal experience or a bottom sheet? Anytime your mobile UI have more than 1 clear call to action, then you're in danger of the UI being less efficient instead of more efficient for your user. Solve problems with UX before UI.

## Overview of .NET MAUI List Controls

In my sample I've used 3 built-in controls, and 2 community controls that all demonstrate different approaches with strengths and weaknesses. .NET MAUI provides `CollectionView`, `ListView`, and `BindableLayout`. From the community I chose `VirtualListView` and `VirtualizedListView`. There are many other options, a few of which I list at the end for you to evaluate yourself.

|               | CollectionView | ListView | BindableLayout | VirtualLayout | VirtualizedLayout |
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

### Performance Tips

If speed of rendering and scroll is of utmost importance to you, then these features are for you.

* **Compiled Bindings** will improve the rendering and updating of your XAML data-bound controls by telling the compiler the type that is being used. On any enclosing XAML element with a BindingContext specify the type with, for example, x:DataType="model:Sample".

* **Binding Modes** - the default binding mode for bindable properties differs from control to control, and property to property. Most are `OneWay` such as `View.Rotation` or `View.Scale`, while properties often used to capture user input are `TwoWay` such as `Entry.Text` and `ListView.IsRefreshing`. In most cases the default will be what you expect and need, but keep in mind you can change these and have other options such as `OneTime` and `OneWayToSource`. [Documentation](https://learn.microsoft.com/dotnet/maui/fundamentals/data-binding/binding-mode?view=net-maui-8.0)

* **ObservableCollection vs List** if your data won't be updating dynamically, and perhaps it's a one-time binding, then use `List`.

* (outdated) **Layout compression** was a run-time optimization in Xamarin.Forms what would remove wrapping layouts from the visual tree. If the layout had no background color or received no user input via gestures, then it could safely be eliminated from the actual UI rendered to the screen. This flattens the UI so the next time it is renderer it can skip those layout calculations. To opt into this, on the layout you wish to strip out add the attached property CompressedLayout.IsHeadless="True". Read me in the archived [Xamarin.Forms documentation](https://learn.microsoft.com/previous-versions/xamarin/xamarin-forms/user-interface/layouts/layout-compression).

## Layout 1: Basic List

This is the most simple and common use of a list. All the rows are exactly the same height and layout. For this need you cannot go wrong between virtualized controls. They all perform this scenario very well, even when displaying 10,000 rows.

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

> You may be wondering why I'm not binding anything above to the ProductListItem. BindingContext automatically propagates in this (and most) cases to the children. Here the provided BindingCOntext is the single Product.