using ArbitraryPixel.Platform2D.Theme;

namespace ArbitraryPixel.CodeLogic.Common.Theme.Device
{
    public class DeviceThemeManager : ThemeManagerBase
    {
        public DeviceThemeManager()
        {
            this.CurrentThemeID = ThemeType.Agent;

            this.RegisterTheme(new DeviceAgentTheme());

            // Set the default.
            this.DefaultTheme = this[ThemeType.Agent];
        }
    }
}
