using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using Microsoft.Xna.Framework;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Theme;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class SequenceAttemptRecordView_Draw_Tests
    {
        private SequenceAttemptRecordView _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private IDeviceModel _mockDeviceModel;
        private IThemeManagerCollection _mockThemeCollection;
        private IDeviceTheme _mockDeviceTheme;
        private ISequenceAttemptCollection _mockAttemptCollection;
        private ISequenceAttemptRecord _mockAttempt;
        private ICodeValueColourMap _mockColourMap;
        private ISpriteFont _mockFont;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();

            _mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(_mockThemeCollection);

            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(_mockDeviceTheme = Substitute.For<IDeviceTheme>());

            ITexture2D mockFrameTexture = Substitute.For<ITexture2D>();
            mockFrameTexture.Width.Returns(200);
            mockFrameTexture.Height.Returns(100);
            _mockEngine.AssetBank.Get<ITexture2D>("HistoryAttemptFrame").Returns(mockFrameTexture);

            _mockEngine.AssetBank.Get<ISpriteFont>("HistoryAttemptIndexFont").Returns(_mockFont = Substitute.For<ISpriteFont>());
            _mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(15, 15));

            _mockDeviceModel.Attempts.Returns(_mockAttemptCollection = Substitute.For<ISequenceAttemptCollection>());
            _mockAttemptCollection.Count.Returns(2);
            _mockAttemptCollection[1].Returns(_mockAttempt = Substitute.For<ISequenceAttemptRecord>());
            _mockAttempt.Code.Returns(new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Orange, CodeValue.Red });
            _mockAttempt.Result.Returns(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual });

            _mockDeviceModel.CodeColourMap.Returns(_mockColourMap = Substitute.For<ICodeValueColourMap>());
            _mockColourMap.GetColour(Arg.Any<CodeValue>()).Returns(
                x =>
                {
                    switch ((CodeValue)x[0])
                    {
                        case CodeValue.Red:
                            return Color.Red;
                        case CodeValue.Green:
                            return Color.Green;
                        case CodeValue.Blue:
                            return Color.Blue;
                        case CodeValue.Yellow:
                            return Color.Yellow;
                        case CodeValue.Orange:
                            return Color.Orange;
                        default:
                            return Color.Red;
                    }
                }
            );

            _sut = new SequenceAttemptRecordView(_mockEngine, _mockSpriteBatch, new Vector2(100, 100), _mockDeviceModel, 1);
            _mockEngine.AssetBank.ClearReceivedCalls();
        }

        #region Frame
        [TestMethod]
        public void DrawShouldDrawFrameBackground()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockDeviceTheme.HistoryAttemptBackgroundMask.Returns(Color.Pink);
            _mockEngine.AssetBank.Get<ITexture2D>("HistoryAttemptFrameBackground").Returns(mockTexture);

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("HistoryAttemptFrameBackground");
                    _mockSpriteBatch.Draw(mockTexture, new Vector2(100, 100), Color.Pink);
                }
            );
        }

        [TestMethod]
        public void DrawShouldDrawFrameTexture()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockDeviceTheme.HistoryAttemptBorderMask.Returns(Color.Pink);
            _mockEngine.AssetBank.Get<ITexture2D>("HistoryAttemptFrame").Returns(mockTexture);

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("HistoryAttemptFrame");
                    _mockSpriteBatch.Draw(mockTexture, new Vector2(100, 100), Color.Pink);
                }
            );
        }
        #endregion

        #region Index Text
        [TestMethod]
        public void DrawShouldDrawTargetIndex()
        {
            _mockDeviceTheme.HistoryAttemptIndexFontOffset.Returns(new SizeF(5, 3));
            _mockDeviceTheme.HistoryAttemptBorderMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            SizeF cellSize = new SizeF(30);
            Vector2 expectedLocation = new Vector2(
                100 + 1 + cellSize.Width / 2f - 7.5f + 5,
                100 + 1 + cellSize.Height / 2f - 7.5f + 3f
            );
            _mockSpriteBatch.Received(1).DrawString(_mockFont, "2", expectedLocation, Color.Pink);
        }
        #endregion

        #region Choice Index
        [TestMethod]
        public void DrawShouldDrawIndexChoices()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("HistoryIndexChoice").Returns(mockTexture);

            _sut.Draw(new GameTime());

            float indexIconWidth = 30;
            Vector2 start = new Vector2(100 + 1 + indexIconWidth + 1, 101);
            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Draw(mockTexture, start + new Vector2(0 * (1 + indexIconWidth), 0), Color.Red);
                    _mockSpriteBatch.Draw(mockTexture, start + new Vector2(1 * (1 + indexIconWidth), 0), Color.Green);
                    _mockSpriteBatch.Draw(mockTexture, start + new Vector2(2 * (1 + indexIconWidth), 0), Color.Orange);
                    _mockSpriteBatch.Draw(mockTexture, start + new Vector2(3 * (1 + indexIconWidth), 0), Color.Red);
                }
            );
        }
        #endregion

        #region Result
        [TestMethod]
        public void DrawShouldDrawCompareResult()
        {
            ITexture2D mockEqualTexture = Substitute.For<ITexture2D>();
            ITexture2D mockPartialTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("HistoryEqual").Returns(mockEqualTexture);
            _mockEngine.AssetBank.Get<ITexture2D>("HistoryPartial").Returns(mockPartialTexture);
            _mockDeviceTheme.HistoryAttemptBorderMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            Vector2 start = new Vector2(
                300 - 1 - 30,
                100 + 1
            );

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Draw(mockEqualTexture, start + new Vector2(0, 0), Color.Pink);
                    _mockSpriteBatch.Draw(mockPartialTexture, start + new Vector2(15, 0), Color.Pink);
                    _mockSpriteBatch.Draw(mockPartialTexture, start + new Vector2(0, 15), Color.Pink);
                }
            );
        }
        #endregion
    }
}
