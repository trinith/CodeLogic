using ArbitraryPixel.Common.Drawing;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Theme.Device
{
    public class DeviceAgentTheme : CodeLogicTheme, IDeviceTheme
    {
        /*  Palette (Using Complementary)
         *  Index   R   G   B       Description
         *  -----------------------------------
         *  1       0   51  0       Dark Green
         *  2       20  204 20      Bright Green
         *  3       0   128 0       Green                   (( Base ))
         *  4       51  0   26      Dark Purple
         *  5       128 0   86      Magenta
         */

        public override string ThemeID => ThemeType.Agent;
        public override string ObjectID => ThemeObjectType.Device;

        private Color C1DarkGreen { get; } = new Color(0, 51, 0);
        private Color C2BrightGreen { get; } = new Color(20, 204, 20);
        private Color C3Green { get; } = new Color(0, 128, 0);
        private Color C4DarkPurple { get; } = new Color(51, 0, 26);
        private Color C5Magenta { get; } = new Color(128, 0, 86);

        public Color BackgroundImageMask => this.C1DarkGreen;

        public Color NormalColourMask => this.C3Green;
        public Color HighlightColourMask => this.C2BrightGreen;
        public Color BackgroundColourMask { get; } = new Color(32, 32, 32);

        public Color StatusIndicatorBackgroundMask => this.C1DarkGreen;
        public Color StatusIndicatorBorderMask => this.C3Green;
        public Color StatusIndicatorProgressBorderMask => this.C5Magenta;
        public Color StatusIndicatorProgressBackgroundMask => this.C4DarkPurple;
        public Vector2 StatusIndicatorFontOffset { get; } = new Vector2(0, 4);
        public SizeF StatusIndicatorProgressFrameCellSize { get; } = new SizeF(42, 45);
        public SizeF StatusIndicatorProgressFrameCellBorderSize { get; } = new SizeF(1, 0);
        public SizeF StatusIndicatorProgressFrameBorderSize { get; } = new SizeF(2, 2);

        public Color AlarmLowMask { get; } = new Color(0, 255, 0);
        public Color AlarmMediumMask { get; } = Color.Yellow;
        public Color AlarmHighMask { get; } = Color.Red;

        public Color AlarmLowMaskLow { get; } = new Color(0, 64, 0);
        public Color AlarmMediumMaskLow { get; } = new Color(64, 64, 0);
        public Color AlarmHighMaskLow { get; } = new Color(64, 0, 0);

        public double AlarmCriticalBlinkFrequency { get; } = 0.5;

        public Color SubmitButtonForegroundMask => this.HighlightColourMask;
        public Color SubmitButtonBackgroundMask => this.NormalColourMask;
        public Color SubmitButtonHoldOverlayMask => this.HighlightColourMask;

        public Vector2 HistoryAttemptIndexFontOffset { get; } = new Vector2(0, 4);
        public Color HistoryAttemptBackgroundMask => this.C4DarkPurple;
        public Color HistoryAttemptBorderMask => this.C5Magenta;

        public Color SceneChangeBackgroundMask => this.C1DarkGreen;
        public Color SceneChangeBorderNormalMask => this.C3Green;
        public Color SceneChangeBorderHighlightMask => this.C2BrightGreen;
        public Vector2 SceneChangeButtonFontOffset { get; } = new Vector2(-1, 5);
        public Vector2 SceneChangeIconOffset { get; } = new Vector2(-15, 0);

        public Color InputButtonHighlightPreviousSelectionMask { get; } = new Color(96, 96, 96);
        public Color InputButtonHighlightCurrentSelectionMask { get; } = Color.White;
        public Color InputButtonBorderMask => this.C5Magenta;
        public Color InputButtonBackgroundMask => this.C4DarkPurple;

        public Color DeviceMenuButtonBackgroundMask => this.C1DarkGreen;
        public Color DeviceMenuButtonForegroundMask => this.NormalColourMask;
        public Color DeviceMenuButtonForegroundHighlightMask => this.HighlightColourMask;
    }
}
