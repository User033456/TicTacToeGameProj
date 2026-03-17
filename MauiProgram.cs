using CommunityToolkit.Maui;
using Maui.ColorPicker;
using SkiaSharp.Views.Maui.Controls.Hosting;
namespace TicTacToeGameProj
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.UseMauiCommunityToolkit();
            builder.UseMauiApp<App>().UseMauiCommunityToolkit();
            builder.UseMauiApp<App>().UseSkiaSharp();
#if DEBUG


#endif

            return builder.Build();
        }
    }
}
