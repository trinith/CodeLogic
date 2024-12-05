using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class LogPanelModel_Tests
    {
        private LogPanelModel _sut;

        private ICodeLogicSettings _mockSettings;

        private GameTime _gtOneSecond = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1));
        private GameTime _gtHalfSecond = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5));

        [TestInitialize]
        public void Initialize()
        {
            _mockSettings = Substitute.For<ICodeLogicSettings>();

            _sut = new LogPanelModel(_mockSettings, new SizeF(500, 400));
        }

        #region Property Tests - Defaults
        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_CurrentOffset()
        {
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.CurrentOffset);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_PreviousOffset()
        {
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.PreviousOffset);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_TargetOffset()
        {
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.TargetOffset);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_ProgressTarget()
        {
            Assert.AreEqual<Vector2>(new Vector2(0, 1), _sut.ProgressTarget);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_ProgressValue()
        {
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.ProgressValue);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_ProgressSpeed()
        {
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.ProgressSpeed);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_CurrentMode()
        {
            Assert.AreEqual<LogPanelMode>(LogPanelMode.Closed, _sut.CurrentMode);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_NextMode()
        {
            Assert.AreEqual<LogPanelMode?>(null, _sut.NextMode);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_ClosedSize()
        {
            Assert.AreEqual<SizeF>(SizeF.Empty, _sut.ClosedSize);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_PartialSize()
        {
            Assert.AreEqual<SizeF>(SizeF.Empty, _sut.PartialSize);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefault_FullSize()
        {
            Assert.AreEqual<SizeF>(SizeF.Empty, _sut.FullSize);
        }
        #endregion

        #region Property Tests - SetGet
        [TestMethod]
        public void WorldBoundsShouldReturnConstructorValue()
        {
            Assert.AreEqual<SizeF>(new SizeF(500, 400), _sut.WorldBounds);
        }

        [TestMethod]
        public void SettingCurrentModeShouldUpdateSettings()
        {
            _sut.CurrentMode = LogPanelMode.PartialView;

            _mockSettings.Received(1).LogPanelMode = LogPanelMode.PartialView;
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWhenNextModeNullAndProgressSpeedNonZeroShouldNotModifyProgressValue()
        {
            _sut.ProgressValue = new Vector2(1, 2);
            _sut.NextMode = null;
            _sut.ProgressSpeed = Vector2.UnitX;

            _sut.Update(_gtOneSecond);

            Assert.AreEqual<Vector2>(new Vector2(1, 2), _sut.ProgressValue);
        }

        [TestMethod]
        public void UpdateWhenNextModeSetShouldModifyProgressValueBasedOnProgressSpeedAndElapsedTime()
        {
            _sut.NextMode = LogPanelMode.PartialView;
            _sut.ProgressSpeed = new Vector2(0.5f, 0.6f); // Units per second.

            _sut.Update(_gtHalfSecond);

            Assert.AreEqual<Vector2>(new Vector2(0.25f, 0.3f), _sut.ProgressValue);
        }

        [TestMethod]
        public void UpdateWhenNextModeSetAndProgressValueSurpassesOneShouldClampProgressValueToOne()
        {
            _sut.NextMode = LogPanelMode.PartialView;
            _sut.ProgressSpeed = new Vector2(500, 500);

            _sut.Update(_gtOneSecond);

            Assert.AreEqual<Vector2>(new Vector2(1), _sut.ProgressValue);
        }

        [TestMethod]
        public void UpdateWhenNextModeSetShouldCalculateExpectedCurrentOffset_TestA()
        {
            _sut.PreviousOffset = new Vector2(100, 200);
            _sut.TargetOffset = new Vector2(300, 500);
            _sut.NextMode = LogPanelMode.PartialView;
            _sut.ProgressSpeed = new Vector2(1, 2);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.25)));

            Vector2 expected = new Vector2(
                100 + MathHelper.SmoothStep(0, 1, 0.25f) * (300 - 100),
                200 + MathHelper.SmoothStep(0, 1, 0.5f) * (500 - 200)
            );

            Assert.AreEqual<Vector2>(expected, _sut.CurrentOffset);
        }

        [TestMethod]
        public void UpdateWhenNextModeSetShouldCalculateExpectedCurrentOffset_TestB()
        {
            _sut.PreviousOffset = new Vector2(100, 200);
            _sut.TargetOffset = new Vector2(300, 500);
            _sut.NextMode = LogPanelMode.PartialView;
            _sut.ProgressSpeed = new Vector2(1, 2);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.9)));

            Vector2 expected = new Vector2(
                100 + MathHelper.SmoothStep(0, 1, 0.9f) * (300 - 100),
                200 + MathHelper.SmoothStep(0, 1, 1.8f) * (500 - 200)
            );

            Assert.AreEqual<Vector2>(expected, _sut.CurrentOffset);
        }

        [TestMethod]
        public void UpdateWhenProgressReachesTargetShouldSetCurrentMode()
        {
            _sut.ProgressTarget = new Vector2(1);
            _sut.NextMode = LogPanelMode.PartialView;
            _sut.TargetOffset = new Vector2(100, 200);
            _sut.ProgressSpeed = new Vector2(1);

            _sut.Update(_gtOneSecond);

            Assert.AreEqual<LogPanelMode>(LogPanelMode.PartialView, _sut.CurrentMode);
        }

        [TestMethod]
        public void UpdateWhenProgressReachesTargetShouldSetNextModeNull()
        {
            _sut.ProgressTarget = new Vector2(1);
            _sut.NextMode = LogPanelMode.PartialView;
            _sut.TargetOffset = new Vector2(100, 200);
            _sut.ProgressSpeed = new Vector2(1);

            _sut.Update(_gtOneSecond);

            Assert.AreEqual<LogPanelMode?>(null, _sut.NextMode);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldResetAllPropertiesToExpectedValues()
        {
            _mockSettings.LogPanelMode.Returns(LogPanelMode.PartialView);
            _sut.PartialSize = new SizeF(1000, 123);

            // (Arrange) Set all properties to something they shouldn't be.
            _sut.CurrentOffset = new Vector2(123, 456);
            _sut.PreviousOffset = new Vector2(123, 456);
            _sut.TargetOffset = new Vector2(123, 456);

            _sut.ProgressTarget = new Vector2(123, 456);
            _sut.ProgressValue = new Vector2(123, 456);
            _sut.ProgressSpeed = new Vector2(123, 456);

            _sut.CurrentMode = LogPanelMode.PartialView;
            _sut.NextMode = LogPanelMode.FullView;

            // (Act) Reset the model
            _sut.Reset();

            // (Assert) Ensure all properties got set to the expected values.
            // NOTE: Gonna violate the assert principle here because I just don't feel like writing a billion tests for something so low value.
            Assert.AreEqual<Vector2>(new Vector2(0, 123), _sut.CurrentOffset);
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.PreviousOffset);
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.TargetOffset);

            Assert.AreEqual<Vector2>(new Vector2(0, 1), _sut.ProgressTarget);
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.ProgressValue);
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.ProgressSpeed);

            Assert.AreEqual<LogPanelMode>(LogPanelMode.PartialView, _sut.CurrentMode);
            Assert.AreEqual<LogPanelMode?>(null, _sut.NextMode);
        }

        [TestMethod]
        public void ResetShouldFireModelResetEvent()
        {
            var subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.ModelReset += subscriber;

            _sut.Reset();

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion

        #region SetOffsetForMode Tests
        [TestMethod]
        public void SetOffsetForModeShouldSetExpectedOffset_Closed()
        {
            _sut.ClosedSize = new SizeF(123, 456);
            _sut.ProgressTarget = new Vector2(3, 2);
            _sut.CurrentMode = LogPanelMode.Closed;

            _sut.SetOffsetForMode();

            Assert.AreEqual<Vector2>(new Vector2(369, 912), _sut.CurrentOffset);
        }

        [TestMethod]
        public void SetOffsetForModeShouldSetExpectedOffset_PartialView()
        {
            _sut.PartialSize = new SizeF(123, 456);
            _sut.ProgressTarget = new Vector2(3, 2);
            _sut.CurrentMode = LogPanelMode.PartialView;

            _sut.SetOffsetForMode();

            Assert.AreEqual<Vector2>(new Vector2(369, 912), _sut.CurrentOffset);
        }

        [TestMethod]
        public void SetOffsetForModeShouldSetExpectedOffset_FullView()
        {
            _sut.FullSize = new SizeF(123, 456);
            _sut.ProgressTarget = new Vector2(3, 2);
            _sut.CurrentMode = LogPanelMode.FullView;

            _sut.SetOffsetForMode();

            Assert.AreEqual<Vector2>(new Vector2(369, 912), _sut.CurrentOffset);
        }
        #endregion
    }
}
