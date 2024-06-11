using FFImageLoading.Maui;
using Microsoft.Extensions.Logging;
using MPowerKit.VirtualizeListView;
using CommunityToolkit.Maui;
using RatingControlMaui;

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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("fa_solid.ttf", "FontAwesome");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
