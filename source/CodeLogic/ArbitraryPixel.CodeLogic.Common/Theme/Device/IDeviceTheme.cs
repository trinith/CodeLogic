using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Theme.Device
{
    public interface IDeviceTheme : ITheme
    {
        Color BackgroundImageMask { get; }

        Color BackgroundColourMask { get; }
        Color NormalColourMask { get; }
        Color HighlightColourMask { get; }

        Color StatusIndicatorBackgroundMask { get; }
        Color StatusIndicatorBorderMask { get; }
        Color StatusIndicatorProgressBorderMask { get; }
        Color StatusIndicatorProgressBackgroundMask { get; }
        Vector2 StatusIndicatorFontOffset { get; }
        SizeF StatusIndicatorProgressFrameCellSize { get; }
        SizeF StatusIndicatorProgressFrameCellBorderSize { get; }
        SizeF StatusIndicatorProgressFrameBorderSize { get; }

        Color AlarmLowMask { get; }
        Color AlarmMediumMask { get; }
        Color AlarmHighMask { get; }

        Color AlarmLowMaskLow { get; }
        Color AlarmMediumMaskLow { get; }
        Color AlarmHighMaskLow { get; }

        double AlarmCriticalBlinkFrequency { get; }

        Color SubmitButtonForegroundMask { get; }
        Color SubmitButtonBackgroundMask { get; }
        Color SubmitButtonHoldOverlayMask { get; }

        Vector2 HistoryAttemptIndexFontOffset { get; }
        Color HistoryAttemptBackgroundMask { get; }
        Color HistoryAttemptBorderMask { get; }

        Color SceneChangeBackgroundMask { get; }
        Color SceneChangeBorderNormalMask { get; }
        Color SceneChangeBorderHighlightMask { get; }
        Vector2 SceneChangeButtonFontOffset { get; }
        Vector2 SceneChangeIconOffset { get; }

        Color InputButtonHighlightPreviousSelectionMask { get; }
        Color InputButtonHighlightCurrentSelectionMask { get; }
        Color InputButtonBorderMask { get; }
        Color InputButtonBackgroundMask { get; }

        Color DeviceMenuButtonBackgroundMask { get; }
        Color DeviceMenuButtonForegroundMask { get; }
        Color DeviceMenuButtonForegroundHighlightMask { get; }
    }
}
