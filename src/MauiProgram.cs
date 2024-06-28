using FFImageLoading.Maui;
using Microsoft.Extensions.Logging;
using MPowerKit.VirtualizeListView;
using CommunityToolkit.Maui;
using RatingControlMaui;
using AlohaKit.Layouts.Hosting;
using Effects;
using The49.Maui.BottomSheet;

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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("fa_solid.ttf", "FontAwesome");
				fonts.AddFont("fabmdl2.ttf", "Fabric");
				fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentUI");
			})
			.ConfigureEffects(effects =>
			{
                #if IOS || MACCATALYST
				effects.Add<ContentInsetAdjustmentBehaviorRoutingEffect, ContentInsetAdjustmentBehaviorPlatformEffect>();
                #endif
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
