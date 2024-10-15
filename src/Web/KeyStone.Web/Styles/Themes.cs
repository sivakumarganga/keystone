using MudBlazor;

namespace KeyStone.Web.Styles
{
    public class Themes
    {
        public static MudTheme DefaultTheme => GetDefaultTheme();

        private static MudTheme GetDefaultTheme()
        {
            var mudTheme = new MudTheme()
            {
                PaletteLight = new PaletteLight()
                {
                    AppbarBackground = Colors.Shades.White,
                    Background = "#F0F4FC",
                    Primary = "#2161F7",
                    Secondary = "#7D7D7D",
                    Tertiary = "#FFFFFF",
                    TextPrimary = "#000000",
                    TextSecondary = "#7D7D7D",
                    Success = "#0AA007",
                    Black = Colors.Shades.Black,
                    White = Colors.Shades.White
                },

                Typography = new Typography
                {
                    Default = new Default
                    {
                        FontFamily = ["Inter", "Helvetica", "Arial", "sans-serif"],
                    }

                }

            };
            PaletteDark darkPalette = new()
            {
                Primary = mudTheme.PaletteLight.Primary.ColorDarken(0.2f),
                Secondary = mudTheme.PaletteLight.Secondary.ColorDarken(0.2f),
                Background = mudTheme.PaletteLight.Background.ColorDarken(0.5f),
                AppbarBackground = mudTheme.PaletteLight.AppbarBackground.ColorDarken(0.3f),
                Black = Colors.Shades.Black,
                White = Colors.Shades.White
            };
            mudTheme.PaletteDark = darkPalette;
            return mudTheme;
        }
    }
}
