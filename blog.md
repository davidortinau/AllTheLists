# All the Lists in .NET MAUI

In every app you will inevitably have a list of things to display and be faced with choosing the best control to use. Here I will muse on how I approach these decisions, and perhaps this will help you when faced with similar scenarios.

First I want to get out of the way some general thoughts.

**Anything that does everything does nothing well.** In order for a generalized control to be flexible enough to do meet a wide variety of needs, compromises will be made in its implementation. This may lead you to being frustrated when it doesn't meet your expectations. A specialized control that only does what you need it to will best meet the need of that scenario. The other side of that sharp edge though is your knowledge and skill needs to also level up from general to specialized. 

**Flat is faster than fat.** It's true. If speed is important to your scenario, then a layout that avoids a lot of UI and nesting of controls will perform better at scale because it requires fewer measure and layout calls. Avoid measuring at all costs when performance is critical; give your UI explicit size anytime you can.

**UX > UI** I see a lot of apps struggling with list scenarios because they jam a ton of UI into them to get the job done, rather than leaning on good UX principles. Do you really need a whole chat experience in every row of the list, or could you navigate to another page? Perhaps you could use a modal experience or a bottom sheet? Anytime your mobile UI have more than 1 clear call to action, then you're in danger of the UI being less efficient instead of more efficient for your user. Solve problems with UX before UI.

## .NET MAUI List Controls

In my sample I've use 3 built-in controls, and 2 community controls that all demonstrate different approaches with strengths and weaknesses. For the built in controls we have `CollectionView`, `ListView`, and `BindableLayout`, and for the community controls I chose `VirtualListView` and `VirtualizedListView`. 

> CompressedLayout


### CollectionView

This is the most modern and feature rich control of the bunch. What sets it apart is the ability to do multiple layouts and infinite scrolling.