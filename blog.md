# All the Lists in .NET MAUI

In every app you will inevitably have a list of things to display and be faced with choosing the best control to use. Here I will muse on how I approach these decisions, and perhaps this will help you when faced with similar scenarios.

First I want to get out of the way some general thoughts.

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


## Layout 1: 

This is the most modern and feature rich control of the bunch. What sets it apart is the ability to do multiple layouts and infinite scrolling.