using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class CodeInputButton_Tests
    {
        private class CodeInputButtonTestable : CodeInputButton
        {
            public CodeInputButtonTestable(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ICodeInputButtonModel model, int index)
                : base(host, bounds, spriteBatch, model, index)
            {
            }

            public new bool IsPointInBounds(Vector2 p)
            {
                return base.IsPointInBounds(p);
            }
        }

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ICodeInputButtonModel _mockModel;
        private IThemeManagerCollection _mockThemeCollection;
        private ICodeValueColourMap _mockColourMap;
        private IDeviceTheme _mockDeviceTheme;

        private RectangleF _bounds = new RectangleF(100, 200, 300, 400);

        private CodeInputButtonTestable _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockModel = Substitute.For<ICodeInputButtonModel>();

            _mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(_mockThemeCollection);

            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(_mockDeviceTheme = Substitute.For<IDeviceTheme>());

            _mockModel.DeviceModel.InputSequence.Length.Returns(4);
            _mockModel.DeviceModel.CodeColourMap.Returns(_mockColourMap = Substitute.For<ICodeValueColourMap>());

            _sut = new CodeInputButtonTestable(_mockEngine, _bounds, _mockSpriteBatch, _mockModel, 0);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullSpriteBatchShouldThrowException()
        {
            _sut = new CodeInputButtonTestable(_mockEngine, _bounds, null, _mockModel, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorwithNullModelShouldThrowException()
        {
            _sut = new CodeInputButtonTestable(_mockEngine, _bounds, _mockSpriteBatch, null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ConstructorWithInvalidIndexShouldThrowException_LT_Zero()
        {
            _sut = new CodeInputButtonTestable(_mockEngine, _bounds, _mockSpriteBatch, _mockModel, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ConstructorWithInvalidIndexShouldThrowException_GTE_InputSequenceLength()
        {
            _mockModel.DeviceModel.InputSequence.Length.Returns(3);
            _sut = new CodeInputButtonTestable(_mockEngine, _bounds, _mockSpriteBatch, _mockModel, 3);
        }

        [TestMethod]
        public void ConstructorShouldRequestTheme()
        {
            _mockThemeCollection[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void ModelShouldReturnValueFromConstructor()
        {
            Assert.AreSame(_mockModel, _sut.Model);
        }
        #endregion

        #region Update (General) Tests
        [TestMethod]
        public void UpdateWhenOpeningWithScaleValueIncompleteShouldIncrementScaleValue()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Opening);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.05)));

            _mockModel.Received(1).ScaleValue = 1f / 0.125f * 0.05f;
        }

        [TestMethod]
        public void UpdateWhenOpeningWithScaleValueCompleteShouldSetExpectedModelValues()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Opening);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(999)));

            Received.InOrder(
                () =>
                {
                    _mockModel.ScaleValue = 1f;
                    _mockModel.SelectorState = CodeInputButtonSelectorState.Open;
                }
            );
        }

        [TestMethod]
        public void UpdateWhenOpeningWithScaleValueCompleteShouldFireSelectorOpenedEvent()
        {
            EventHandler<EventArgs> subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.SelectorOpened += subscriber;
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Opening);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(999)));

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void UpdateWhenClosingWithScaleValueIncompleteShouldIncrementScaleValue()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closing);
            _mockModel.ScaleValue.Returns(1f);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.05)));

            _mockModel.Received(1).ScaleValue = 1f - 1f / 0.125f * 0.05f;
        }

        [TestMethod]
        public void UpdateWhenClosingWithScaleValueCompleteShouldSetExpectedModelValues()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closing);
            _mockModel.ScaleValue.Returns(1f);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(999)));

            Received.InOrder(
                () =>
                {
                    _mockModel.ScaleValue = 0f;
                    _mockModel.SelectorState = CodeInputButtonSelectorState.Closed;
                }
            );
        }

        [TestMethod]
        public void UpdateWhenClosedWithScaleValueCompleteShouldFireSelectorClosedEvent()
        {
            EventHandler<SelectorClosedEventArgs> subscriber = Substitute.For<EventHandler<SelectorClosedEventArgs>>();
            _sut.SelectorClosed += subscriber;
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closing);
            _mockModel.ScaleValue.Returns(1f);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(999)));

            subscriber.Received(1)(_sut, Arg.Any<SelectorClosedEventArgs>());
        }

        [TestMethod]
        public void UpdateWhenOpeningShouldGetSurfaceState()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Opening);

            _sut.Update(new GameTime());

            _mockEngine.InputManager.Received(2).GetSurfaceState(); // NOTE: One for ButtonBase.Update, anohter for UpdateForInput.
        }

        [TestMethod]
        public void UpdateWhenOpenShouldGetSurfaceState()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);

            _sut.Update(new GameTime());

            _mockEngine.InputManager.Received(2).GetSurfaceState(); // NOTE: One for ButtonBase.Update, anohter for UpdateForInput.
        }

        [TestMethod]
        public void UpdateWhenClosingShouldNotGetSurfaceState()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closing);

            _sut.Update(new GameTime());

            _mockEngine.InputManager.Received(1).GetSurfaceState(); // NOTE: One for ButtonBase.Update, none for UpdateForInput.
        }

        [TestMethod]
        public void UpdateWhenClosedShouldNotGetSurfaceState()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closed);

            _sut.Update(new GameTime());

            _mockEngine.InputManager.Received(1).GetSurfaceState(); // NOTE: One for ButtonBase.Update, none for UpdateForInput.
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldCallColourMapGetColour()
        {
            _mockModel.DeviceModel.InputSequence[0].Returns(CodeValue.Yellow);
            _sut.Draw(new GameTime());

            _mockColourMap.Received(1).GetColour(CodeValue.Yellow);
        }

        [TestMethod]
        public void DrawShouldDrawBackgroundWithExpectedColour()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("HexButtonFill").Returns(mockTexture);
            _mockDeviceTheme.InputButtonBackgroundMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, new RectangleF(175, 300, 150, 200), Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawIconTextureWithExpectedColour()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("HexButtonIcon").Returns(mockTexture);
            _mockColourMap.GetColour(Arg.Any<CodeValue>()).Returns(Color.Pink);
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, new RectangleF(175, 300, 150, 200), Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawBorderTextureWithExpectedColour()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("HexButtonBorder").Returns(mockTexture);
            _mockDeviceTheme.InputButtonBorderMask.Returns(Color.Pink);
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, new RectangleF(175, 300, 150, 200), Color.Pink);
        }

        [TestMethod]
        public void DrawWhenSelectorStateClosedShouldNotDraw_HexButtonSelectorHighlight()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closed);
            string targetTexture = "HexButtonSelectorHighlight";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(mockTexture, Arg.Any<Vector2>(), null, Arg.Any<Color>(), Arg.Any<float>(), Arg.Any<Vector2>(), Arg.Any<Vector2>(), Arg.Any<SpriteEffects>(), Arg.Any<float>());
        }

        [TestMethod]
        public void DrawWhenSelectorStateClosedShouldNotDraw_HexButtonSelectorFill()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closed);
            string targetTexture = "HexButtonSelectorFill";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(mockTexture, Arg.Any<Vector2>(), null, Arg.Any<Color>(), Arg.Any<float>(), Arg.Any<Vector2>(), Arg.Any<Vector2>(), Arg.Any<SpriteEffects>(), Arg.Any<float>());
        }

        [TestMethod]
        public void DrawWhenSelectorStateClosedShouldNotDraw_HexButtonSelectorColours()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closed);
            string targetTexture = "HexButtonSelectorColours";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(mockTexture, Arg.Any<Vector2>(), null, Arg.Any<Color>(), Arg.Any<float>(), Arg.Any<Vector2>(), Arg.Any<Vector2>(), Arg.Any<SpriteEffects>(), Arg.Any<float>());
        }

        [TestMethod]
        public void DrawWhenSelectorStateClosedShouldNotDraw_HexButtonSelectorBorder()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closed);
            string targetTexture = "HexButtonSelectorBorder";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(mockTexture, Arg.Any<Vector2>(), null, Arg.Any<Color>(), Arg.Any<float>(), Arg.Any<Vector2>(), Arg.Any<Vector2>(), Arg.Any<SpriteEffects>(), Arg.Any<float>());
        }

        [TestMethod]
        public void DrawWhenSelectorNotClosedShouldDraw_HexButtonSelectorHighlight()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);
            _mockModel.SelectedHighlightAngle.Returns(0.123f);
            _mockModel.ScaleValue.Returns(456);

            string targetTexture = "HexButtonSelectorHighlight";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);
            _mockDeviceTheme.InputButtonHighlightPreviousSelectionMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds.Centre, null, Color.Pink, 0.123f + (float)Math.PI, _bounds.Centre - _bounds.Location, new Vector2(456), SpriteEffects.None, 0f);
        }

        [TestMethod]
        public void DrawWhenSelectorNotClosedAndGestureAngleNotNullShouldDraw_HexButtonSelectorHighlight()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);
            _mockModel.GestureAngle.Returns(0.789f);
            _mockModel.ScaleValue.Returns(456);

            string targetTexture = "HexButtonSelectorHighlight";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);
            _mockDeviceTheme.InputButtonHighlightCurrentSelectionMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds.Centre, null, Color.Pink, 0.789f + (float)Math.PI, _bounds.Centre - _bounds.Location, new Vector2(456), SpriteEffects.None, 0f);
        }

        [TestMethod]
        public void DrawWhenSelectorNotClosedAndGestureAngleNullShouldNotDraw_HexButtonSelectorHighlight()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);
            _mockModel.GestureAngle.Returns((float?)null);
            _mockModel.ScaleValue.Returns(456);

            string targetTexture = "HexButtonSelectorHighlight";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);
            _mockDeviceTheme.InputButtonHighlightCurrentSelectionMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(mockTexture, _bounds.Centre, null, Color.Pink, 0.789f + (float)Math.PI, _bounds.Centre - _bounds.Location, new Vector2(456), SpriteEffects.None, 0f);
        }

        [TestMethod]
        public void DrawWhenSelectorNotClosedAndGestureAngleNotNullShouldDraw_HexButtonSelectorFill()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);
            _mockModel.ScaleValue.Returns(456);

            string targetTexture = "HexButtonSelectorFill";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);
            _mockDeviceTheme.InputButtonBackgroundMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds.Centre, null, Color.Pink, 0f, _bounds.Centre - _bounds.Location, new Vector2(456), SpriteEffects.None, 0f);
        }

        [TestMethod]
        public void DrawWhenSelectorNotClosedAndGestureAngleNotNullShouldDraw_HexButtonSelectorColours()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);
            _mockModel.ScaleValue.Returns(456);

            string targetTexture = "HexButtonSelectorColours";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);
            _mockDeviceTheme.InputButtonBackgroundMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds.Centre, null, Color.White, 0f, _bounds.Centre - _bounds.Location, new Vector2(456), SpriteEffects.None, 0f);
        }

        [TestMethod]
        public void DrawWhenSelectorNotClosedAndGestureAngleNotNullShouldDraw_HexButtonSelectorBorder()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);
            _mockModel.ScaleValue.Returns(456);

            string targetTexture = "HexButtonSelectorBorder";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>(targetTexture).Returns(mockTexture);
            _mockDeviceTheme.InputButtonBorderMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds.Centre, null, Color.Pink, 0f, _bounds.Centre - _bounds.Location, new Vector2(456), SpriteEffects.None, 0f);
        }
        #endregion

        #region Open/Close Tests
        [TestMethod]
        public void OpenSelectorShouldInformModel()
        {
            _sut.OpenSelector();

            _mockModel.Received(1).OpenSelector();
        }

        [TestMethod]
        public void CloseSelectorShouldInformModel()
        {
            _sut.CloseSelector();

            _mockModel.Received(1).CloseSelector();
        }
        #endregion

        #region IsPointInBounds Tests
        [TestMethod]
        public void IsPointInBoundsWithInBoundsPointShouldReturnTrue_StateClosed()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closed);

            Assert.IsTrue(_sut.IsPointInBounds(_bounds.Centre - _bounds.Size.Centre / 2));
        }

        [TestMethod]
        public void IsPointInBoundsWithInBoundsPointShouldReturnTrue_StateClosing()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closing);

            Assert.IsTrue(_sut.IsPointInBounds(_bounds.Centre - _bounds.Size.Centre / 2));
        }

        [TestMethod]
        public void IsPointInBoundsWithInBoundsPointShouldReturnTrue_StateOpen()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 1000));
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);

            Assert.IsTrue(_sut.IsPointInBounds(new Vector2(999)));
        }

        [TestMethod]
        public void IsPointInBoundsWithInBoundsPointShouldReturnTrue_StateOpening()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 1000));
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Opening);

            Assert.IsTrue(_sut.IsPointInBounds(new Vector2(999)));
        }

        [TestMethod]
        public void IsPointInBoundsWithOutBoundsPointShouldReturnFalse_StateClosed()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closed);

            Assert.IsFalse(_sut.IsPointInBounds(_bounds.Centre - _bounds.Size.Centre / 2f - new Vector2(1)));
        }

        [TestMethod]
        public void IsPointInBoundsWithOutBoundsPointShouldReturnFalse_StateClosing()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closing);

            Assert.IsFalse(_sut.IsPointInBounds(_bounds.Centre - _bounds.Size.Centre / 2f - new Vector2(1)));
        }

        [TestMethod]
        public void IsPointInBoundsWithOutBoundsPointShouldReturnFalse_StateOpen()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 1000));
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);

            Assert.IsFalse(_sut.IsPointInBounds(new Vector2(1000)));
        }

        [TestMethod]
        public void IsPointInBoundsWithOutBoundsPointShouldReturnFalse_StateOpening()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 1000));
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Opening);

            Assert.IsFalse(_sut.IsPointInBounds(new Vector2(1000)));
        }
        #endregion
    }
}
