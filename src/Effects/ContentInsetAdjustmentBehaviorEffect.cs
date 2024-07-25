using System.ComponentModel;
using Microsoft.Maui.Controls.Platform;
#if IOS || MACCATALYST
using UIKit;
#endif

#if ANDROID
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Platform;
#endif

namespace Effects;

public static class ContentInset
{
    public static readonly BindableProperty InsetProperty =
        BindableProperty.CreateAttached("Inset", typeof(Thickness), typeof(ContentInset), default(Thickness), propertyChanged: OnInsetChanged);

    private static void OnInsetChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as VisualElement;
            if (control == null)
                return;

            Thickness? thickness = (Thickness)newValue;

            var attachedEffect = control.Effects.FirstOrDefault (e => e is ContentInset);
            if (thickness != Thickness.Zero && attachedEffect == null)
                control.Effects.Add (new ContentInsetAdjustmentBehaviorPlatformEffect ());
            else if (thickness == Thickness.Zero && attachedEffect != null)
                control.Effects.Remove (attachedEffect);
    }

    public static Thickness GetInset (BindableObject view)
        {
            return (Thickness)view.GetValue (InsetProperty);
        }

        public static void SetInset (BindableObject view, Thickness thickness)
        {
            view.SetValue (InsetProperty, thickness);
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
            
            var inset = (Thickness)Element.GetValue(ContentInset.InsetProperty);
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
                var inset = (Thickness)Element.GetValue(ContentInset.InsetProperty);
                scroll.ContentInset = new UIEdgeInsets((nfloat)inset.Top, (nfloat)inset.Left, (nfloat)inset.Bottom, (nfloat)inset.Right);

            }
        }
        catch (Exception ex)
        {

        }
    }
}
#endif
#if ANDROID
public class ContentInsetAdjustmentBehaviorPlatformEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        
        if (Container is not null && Container.Context is not null)
        {
            if (Container is RecyclerView recyclerView)
            {
                if (recyclerView != null)
                {
                    var inset = (Thickness)Element.GetValue(ContentInset.InsetProperty);
                    var context = recyclerView.Context;
                    var left = (int)context.ToPixels(inset.Left);
                    var top = (int)context.ToPixels(inset.Top);
                    var right = (int)context.ToPixels(inset.Right);
                    var bottom = (int)context.ToPixels(inset.Bottom);

                    recyclerView.SetPadding(left, top, right, bottom);
                    recyclerView.SetClipToPadding(false);
                }
            }
        }
    }

    protected override void OnDetached()
    {
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);        
    }
}
#endif