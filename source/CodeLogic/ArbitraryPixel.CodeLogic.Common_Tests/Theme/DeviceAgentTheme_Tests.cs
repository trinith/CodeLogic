using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.CodeLogic.Common.Theme;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Common.Drawing;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Theme
{
    [TestClass]
    public class DeviceAgentTheme_Tests : UnitTestBase<DeviceAgentTheme>
    {
        protected override DeviceAgentTheme OnCreateSUT()
        {
            return new DeviceAgentTheme();
        }

        [TestMethod]
        public void ThemeIDShouldBeExpectedValue()
        {
            Assert.AreEqual<string>(ThemeType.Agent, _sut.ThemeID);
        }

        [TestMethod]
        public void ObjectIDShouldBeExpectedValue()
        {
            Assert.AreEqual<string>(ThemeObjectType.Device, _sut.ObjectID);
        }

        [TestMethod]
        public void BackgroundShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(new Color(0, 51, 0, 255), _sut.BackgroundImageMask);
        }

        [TestMethod]
        public void TextNormalShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(new Color(0, 128, 0, 255), _sut.NormalColourMask);
        }

        [TestMethod]
        public void TextHighlightShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(new Color(20, 204, 20, 255), _sut.HighlightColourMask);
        }

        [TestMethod]
        public void TextBackgroundShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(new Color(32, 32, 32, 255), _sut.BackgroundColourMask);
        }

        [TestMethod]
        public void AlarmLowMaskShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(new Color(0, 255, 0, 255), _sut.AlarmLowMask);
        }

        [TestMethod]
        public void AlarmMediumMaskShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(Color.Yellow, _sut.AlarmMediumMask);
        }

        [TestMethod]
        public void AlarmHighMaskShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(Color.Red, _sut.AlarmHighMask);
        }

        [TestMethod]
        public void AlarmLowMaskLowShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(new Color(0, 64, 0, 255), _sut.AlarmLowMaskLow);
        }

        [TestMethod]
        public void AlarmMediumMaskLowShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(new Color(64, 64, 0), _sut.AlarmMediumMaskLow);
        }

        [TestMethod]
        public void AlarmHighMaskLowShouldBeExpectedColour()
        {
            Assert.AreEqual<Color>(new Color(64, 0, 0), _sut.AlarmHighMaskLow);
        }

        [TestMethod]
        public void AlarmCriticalBlinkFrequencyShouldBeExpectedValue()
        {
            Assert.AreEqual<double>(0.5, _sut.AlarmCriticalBlinkFrequency);
        }

        [TestMethod]
        public void SubmitButtonForegroundMaskShouldBeExpectedValue()
        {
            Assert.AreEqual<Color>(new Color(20, 204, 20, 255), _sut.SubmitButtonForegroundMask);
        }

        [TestMethod]
        public void SubmitButtonHoldOverlayMaskShouldBeExpectedValue()
        {
            Assert.AreEqual<Color>(new Color(20, 204, 20, 255), _sut.SubmitButtonHoldOverlayMask);
        }

        [TestMethod]
        public void SubmitButtonBackgroundMaskShouldBeExpectedValue()
        {
            Assert.AreEqual<Color>(new Color(0, 128, 0, 255), _sut.SubmitButtonBackgroundMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_StatusIndicatorFontOffset()
        {
            Assert.AreEqual<Vector2>(new Vector2(0, 4), _sut.StatusIndicatorFontOffset);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_StatusIndicatorBackgroundMask()
        {
            Assert.AreEqual<Color>(new Color(0, 51, 0), _sut.StatusIndicatorBackgroundMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_StatusIndicatorBorderMask()
        {
            Assert.AreEqual<Color>(new Color(0, 128, 0), _sut.StatusIndicatorBorderMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_StatusIndicatorProgressBorderMask()
        {
            Assert.AreEqual<Color>(new Color(128, 0, 86), _sut.StatusIndicatorProgressBorderMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_StatusIndicatorProgressBackgroundMask()
        {
            Assert.AreEqual<Color>(new Color(51, 0, 26), _sut.StatusIndicatorProgressBackgroundMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_StatusIndicatorProgressFrameCellSize()
        {
            Assert.AreEqual<SizeF>(new SizeF(42, 45), _sut.StatusIndicatorProgressFrameCellSize);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_StatusIndicatorProgressFrameCellBorderSize()
        {
            Assert.AreEqual<SizeF>(new SizeF(1, 0), _sut.StatusIndicatorProgressFrameCellBorderSize);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_StatusIndicatorProgressFrameBorderSize()
        {
            Assert.AreEqual<SizeF>(new SizeF(2, 2), _sut.StatusIndicatorProgressFrameBorderSize);
        }


        [TestMethod]
        public void PropertyIsExpectedValue_HistoryAttemptIndexForOffset()
        {
            Assert.AreEqual<Vector2>(new Vector2(0, 4), _sut.HistoryAttemptIndexFontOffset);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_HistoryAttemptBackgroundMask()
        {
            Assert.AreEqual<Color>(new Color(51, 0, 26), _sut.HistoryAttemptBackgroundMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_HistoryAttemptBorderMask()
        {
            Assert.AreEqual<Color>(new Color(128, 0, 86), _sut.HistoryAttemptBorderMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_SceneChangeBackgroundMask()
        {
            Assert.AreEqual<Color>(new Color(0, 51, 0), _sut.SceneChangeBackgroundMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_SceneChangeBorderNormalMask()
        {
            Assert.AreEqual<Color>(new Color(0, 128, 0), _sut.SceneChangeBorderNormalMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_SceneChangeBorderHighlightMask()
        {
            Assert.AreEqual<Color>(new Color(20, 204, 20), _sut.SceneChangeBorderHighlightMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_SceneChangeButtonFontOffset()
        {
            Assert.AreEqual<Vector2>(new Vector2(-1, 5), _sut.SceneChangeButtonFontOffset);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_SceneChangeIconOffset()
        {
            Assert.AreEqual<Vector2>(new Vector2(-15, 0), _sut.SceneChangeIconOffset);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_InputButtonHighlightPreviousSelectionMask()
        {
            Assert.AreEqual<Color>(new Color(96, 96, 96), _sut.InputButtonHighlightPreviousSelectionMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_InputButtonHighlightCurrentSelectionMask()
        {
            Assert.AreEqual<Color>(Color.White, _sut.InputButtonHighlightCurrentSelectionMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_InputButtonBorderMask()
        {
            Assert.AreEqual<Color>(new Color(128, 0, 86), _sut.InputButtonBorderMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_InputButtonBackgroundMask()
        {
            Assert.AreEqual<Color>(new Color(51, 0, 26), _sut.InputButtonBackgroundMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_DeviceMenuButtonBackgroundMask()
        {
            Assert.AreEqual<Color>(new Color(0, 51, 0), _sut.DeviceMenuButtonBackgroundMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_DeviceMenuButtonForegroundMask()
        {
            Assert.AreEqual<Color>(new Color(0, 128, 0), _sut.DeviceMenuButtonForegroundMask);
        }

        [TestMethod]
        public void PropertyIsExpectedValue_DeviceMenuButtonForegroundHighlightMask()
        {
            Assert.AreEqual<Color>(new Color(20, 204, 20), _sut.DeviceMenuButtonForegroundHighlightMask);
        }
    }
}
