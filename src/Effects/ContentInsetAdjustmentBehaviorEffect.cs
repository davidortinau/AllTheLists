using System.ComponentModel;
using Microsoft.Maui.Controls.Platform;
#if IOS || MACCATALYST
using UIKit;
#endif

namespace Effects;

public static class ContentInsetAdjustmentBehavior
{
    public static readonly BindableProperty ContentInsetProperty =
        BindableProperty.CreateAttached("ContentInset", typeof(Thickness), typeof(ContentInsetAdjustmentBehavior), new Thickness(0));

    public static Thickness GetContentInset(BindableObject view)
    {
        return (Thickness)view.GetValue(ContentInsetProperty);
    }

    public static void SetContentInset (BindableObject view, bool value)
    {
        view.SetValue(ContentInsetProperty, value);
    }
}

public class ContentInsetAdjustmentBehaviorRoutingEffect : RoutingEffect
{
    public ContentInsetAdjustmentBehaviorRoutingEffect()
    { }
}

#if IOS || MACCATALYST
public class ContentInsetAdjustmentBehaviorPlatformEffect : PlatformEffect
{

    protected override void OnAttached()
    {
        try
        {
            var scroll = Control.Subviews[0] as UICollectionView;
            scroll.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
            
            var inset = (Thickness)Element.GetValue(ContentInsetAdjustmentBehavior.ContentInsetProperty);
            scroll.ContentInset = new UIEdgeInsets((nfloat)inset.Top, (nfloat)inset.Left, (nfloat)inset.Bottom, (nfloat)inset.Right);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
        }
    }

    protected override void OnDetached()
    {
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        try
        {
            if (args.PropertyName == "ContentInset")
            {
                var scroll = Control.Subviews[0] as UICollectionView;
                var inset = (Thickness)Element.GetValue(ContentInsetAdjustmentBehavior.ContentInsetProperty);
                scroll.ContentInset = new UIEdgeInsets((nfloat)inset.Top, (nfloat)inset.Left, (nfloat)inset.Bottom, (nfloat)inset.Right);

            }
        }
        catch (Exception ex)
        {

        }
    }
}
#endif
// #if ANDROID
// public class ContentInsetAdjustmentBehaviorPlatformEffect : PlatformEffect
// {
//     protected override void OnAttached()
//     {
//         // try
//         // {
//         //     var scroll = Control.Subviews[0] as UICollectionView;
//         //     scroll.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
            
//         //     var inset = (Thickness)Element.GetValue(ContentInsetAdjustmentBehavior.ContentInsetProperty);
//         //     scroll.ContentInset = new UIEdgeInsets((nfloat)inset.Top, (nfloat)inset.Left, (nfloat)inset.Bottom, (nfloat)inset.Right);
//         // }
//         // catch (Exception ex)
//         // {
//         //     Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
//         // }
//     }

//     protected override void OnDetached()
//     {
//     }

//     protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
//     {
//         base.OnElementPropertyChanged(args);

        
//     }
// }
// #endif