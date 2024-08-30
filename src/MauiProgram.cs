using FFImageLoading.Maui;
using Microsoft.Extensions.Logging;
using MPowerKit.VirtualizeListView;
using CommunityToolkit.Maui;
using RatingControlMaui;
using AlohaKit.Layouts.Hosting;
using Effects;
using The49.Maui.BottomSheet;
using AllTheLists.Services;
using Plugin.Maui.DebugOverlay;
// using Soenneker.Blazor.Masonry.Registrars;
using MR.Gestures;

namespace AllTheLists;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()			
			.UseMauiCommunityToolkit()
			.UseVirtualListView()
			.UseMPowerKitListView()
			.UseFFImageLoading()		
			.UseRatingControl()	
			.UseAlohaKitLayouts()
			.UseBottomSheet()
			.UseDebugRibbon(Colors.Blue)
			.ConfigureMRGestures()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("fa_solid.ttf", "FontAwesome");
				fonts.AddFont("fabmdl2.ttf", "Fabric");
				fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentUI");
			})
			.ConfigureEffects(effects =>
			{
                
				effects.Add<ContentInsetAdjustmentBehaviorRoutingEffect, ContentInsetAdjustmentBehaviorPlatformEffect>();
                
			})
			#if IOS || MACCATALYST
			.ConfigureMauiHandlers(handlers =>
			{
				handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
				handlers.AddHandler<Microsoft.Maui.Controls.CarouselView, Microsoft.Maui.Controls.Handlers.Items2.CarouselViewHandler2>();
			})
			#endif
			;

			builder.Services.AddMauiBlazorWebView();
			// builder.Services.AddMasonry();
			builder.Services.AddScoped<IMasonryInterop, MasonryInterop>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
