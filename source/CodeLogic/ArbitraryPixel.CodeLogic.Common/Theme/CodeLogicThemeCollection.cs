using System;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Platform2D.Theme;

namespace ArbitraryPixel.CodeLogic.Common.Theme
{
    public class CodeLogicThemeCollection : ThemeManagerCollection
    {
        public CodeLogicThemeCollection()
        {
            this.RegisterManager(ThemeObjectType.Device, new DeviceThemeManager());
        }
    }
}
