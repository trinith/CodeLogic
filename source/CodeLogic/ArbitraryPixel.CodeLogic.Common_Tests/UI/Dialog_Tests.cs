using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class Dialog_Tests
    {
        private Dialog _sut;

        private IEngine _mockEngine;
        private ITextObjectBuilder _mockTextBuilder;
        private GameObjectFactory _mockGOF;

        private RectangleF _bounds = new RectangleF(20, 40, 500, 300);
        private string _textFormat = "Test";

        private ILayer _mockHostLayer;
        private ILayer _mockBGLayer;
        private ILayer _mockContentLayer;
        private ILayer _mockAdditionalLayer;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockTextBuilder = Substitute.For<ITextObjectBuilder>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);

            _mockHostLayer = Substitute.For<ILayer>();
            _mockBGLayer = Substitute.For<ILayer>();
            _mockContentLayer = Substitute.For<ILayer>();
            _mockAdditionalLayer = Substitute.For<ILayer>();

            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>()).Returns(_mockHostLayer, _mockBGLayer, _mockContentLayer, _mockAdditionalLayer);

            List<IEntity> childEntities = new List<IEntity>();
            _mockHostLayer.Entities.Returns(childEntities);
            _mockHostLayer.When(x => x.AddEntity(Arg.Any<IEntity>())).Do(x => childEntities.Add(x[0] as IEntity));
        }

        private void Construct()
        {
            _sut = new Dialog(_mockEngine, _bounds, _mockTextBuilder, _textFormat);
        }

        private void ClearLayerCalls()
        {
            _mockHostLayer.ClearReceivedCalls();
            foreach (ILayer mockedLayer in _mockHostLayer.Entities)
                mockedLayer.ClearReceivedCalls();
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_TextBuilder()
        {
            _sut = new Dialog(_mockEngine, _bounds, null, _textFormat);
        }

        [TestMethod]
        public void ConstructShouldCreateGenericLayer()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(_mockEngine.Graphics.GraphicsDevice).Returns(mockSpriteBatch);
            _mockHostLayer.MainSpriteBatch.Returns(mockSpriteBatch);

            Construct();

            _mockGOF.Received(4).CreateGenericLayer(_mockEngine, mockSpriteBatch);
        }

        [TestMethod]
        public void ConstructShouldSetVisibleToFalse()
        {
            Construct();

            Assert.IsFalse(_sut.Visible);
        }

        [TestMethod]
        public void ConstructShouldSetEnabledToFalse()
        {
            Construct();

            Assert.IsFalse(_sut.Enabled);
        }

        [TestMethod]
        public void ConstructShouldAddChildLayersToHostLayer()
        {
            Construct();

            Received.InOrder(
                () =>
                {
                    _mockHostLayer.AddEntity(_mockBGLayer);
                    _mockHostLayer.AddEntity(_mockContentLayer);
                    _mockHostLayer.AddEntity(_mockAdditionalLayer);
                }
            );
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void ClientRectangleShouldReturnExpectedValue()
        {
            Construct();

            RectangleF expected = _bounds;
            expected.Inflate(-CodeLogicEngine.Constants.TextWindowBorderSize.Width, -CodeLogicEngine.Constants.TextWindowBorderSize.Height);
            expected.Inflate(-CodeLogicEngine.Constants.TextWindowPadding.Width, -CodeLogicEngine.Constants.TextWindowPadding.Height);

            Assert.AreEqual<RectangleF>(expected, _sut.ClientRectangle);
        }
        #endregion

        #region Show Tests
        [TestMethod]
        public void ShowShouldClearParentLayer()
        {
            Construct();

            _sut.Show();

            foreach (ILayer mockedLayer in _mockHostLayer.Entities)
                mockedLayer.Received(1).ClearEntities();
        }

        [TestMethod]
        public void ShowShouldCreateBackgroundTexture()
        {
            RectangleF bounds = new RectangleF(Vector2.Zero, new SizeF(700, 500));
            _mockEngine.ScreenManager.World.Returns(new Point(700, 500));

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockTexture);

            Construct();

            _sut.Show();

            _mockGOF.Received(1).CreateTextureEntity(
                _mockEngine,
                bounds,
                ((ILayer)_mockHostLayer.Entities[0]).MainSpriteBatch,
                mockTexture,
                new Color(0, 0, 0, 128)
            );
        }

        [TestMethod]
        public void ShowShouldCreateTextObjectRenderer()
        {
            RectangleF expectedBounds = _bounds;
            expectedBounds.Inflate(-12, -12);

            Construct();

            _sut.Show();

            _mockGOF.Received(1).CreateTextObjectRenderer(_mockEngine.GrfxFactory.RenderTargetFactory, _mockEngine.Graphics.GraphicsDevice, ((ILayer)_mockHostLayer.Entities[0]).MainSpriteBatch, (Rectangle)expectedBounds);
        }

        [TestMethod]
        public void ShowShouldCreateConsoleWindow()
        {
            Construct();

            ITextObjectRenderer mockTextRenderer = Substitute.For<ITextObjectRenderer>();
            _mockGOF.CreateTextObjectRenderer(_mockEngine.GrfxFactory.RenderTargetFactory, _mockEngine.Graphics.GraphicsDevice, ((ILayer)_mockHostLayer.Entities[0]).MainSpriteBatch, Arg.Any<Rectangle>()).Returns(mockTextRenderer);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(_mockEngine, _bounds, ((ILayer)_mockHostLayer.Entities[0]).MainSpriteBatch, _mockTextBuilder, mockTextRenderer).Returns(mockConsoleWindow);

            _sut.Show();

            Received.InOrder(
                () =>
                {
                    mockConsoleWindow.AutoAdvanceOnTap = false;
                    mockConsoleWindow.SetTextFormat(_textFormat);
                    mockConsoleWindow.Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
                    mockConsoleWindow.Disposed += Arg.Any<EventHandler<EventArgs>>();
                    mockConsoleWindow.WindowStateChanged += Arg.Any<EventHandler<StateChangedEventArgs<WindowState>>>();
                    mockConsoleWindow.Padding = CodeLogicEngine.Constants.TextWindowPadding;
                    mockConsoleWindow.BorderSize = CodeLogicEngine.Constants.TextWindowBorderSize;
                }
            );
        }

        [TestMethod]
        public void ShowShouldAddObjectsToParentLayer()
        {
            ITextureEntity mockTextureEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockTextureEntity);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();

            _sut.Show();

            Received.InOrder(
                () =>
                {
                    ((ILayer)_mockHostLayer.Entities[0]).AddEntity(mockTextureEntity);
                    ((ILayer)_mockHostLayer.Entities[0]).AddEntity(mockConsoleWindow);
                }
            );
        }

        [TestMethod]
        public void ShowShouldSetConsoleWindowBorderColour()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.BorderColour = Color.Pink;

            _sut.Show();

            mockConsoleWindow.Received(1).BorderColour = Color.Pink;
        }

        [TestMethod]
        public void ShowShouldSetConsoleWindowBackgroundColour()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.BackgroundColour = Color.Pink;

            _sut.Show();

            mockConsoleWindow.Received(1).BackgroundColour = Color.Pink;
        }

        [TestMethod]
        public void ShowShouldSetVisibleToTrue()
        {
            Construct();

            _sut.Show();

            Assert.IsTrue(_sut.Visible);
        }

        [TestMethod]
        public void ShowShouldSetEnabledToTrue()
        {
            Construct();

            _sut.Show();

            Assert.IsTrue(_sut.Enabled);
        }

        [TestMethod]
        public void ShowShouldAddContentLayersToContentLayer()
        {
            Construct();

            ILayer mockContent = Substitute.For<ILayer>();
            _sut.AddContentLayer(mockContent);

            _sut.Show();

            _mockContentLayer.Received(1).AddEntity(mockContent);
        }

        [TestMethod]
        public void ShowShouldSetContentLayerEnabledToFalse()
        {
            Construct();

            _sut.Show();

            _mockContentLayer.Received(1).Enabled = false;
        }

        [TestMethod]
        public void ShowShouldSetContentLayerVisibleToFalse()
        {
            Construct();

            _sut.Show();

            _mockContentLayer.Received(1).Visible = false;
        }
        #endregion

        #region Console Window Event Tests
        [TestMethod]
        public void ConsoleWindowStateChangedFromOpeningShouldSetContentLayerVisibleToTrue()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Processing);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);
            _sut.Show();

            mockWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockWindow, new StateChangedEventArgs<WindowState>(WindowState.Opening, WindowState.Processing));

            _mockContentLayer.Received(1).Visible = true;
        }

        [TestMethod]
        public void ConsoleWindowStateChangedFromOpeningShouldSetContentLayerEnabledToTrue()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Processing);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);
            _sut.Show();

            mockWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockWindow, new StateChangedEventArgs<WindowState>(WindowState.Opening, WindowState.Processing));

            _mockContentLayer.Received(1).Enabled = true;
        }

        [TestMethod]
        public void ConsoleWindowStateChangedFromNotOpeningShouldNotSetContentLayerVisibleToTrue()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Processing);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);
            _sut.Show();
            _mockContentLayer.ClearReceivedCalls();

            mockWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockWindow, new StateChangedEventArgs<WindowState>(WindowState.Ready, WindowState.Processing));

            _mockContentLayer.Received(0).Visible = Arg.Any<bool>();
        }

        [TestMethod]
        public void ConsoleWindowStateChangedFromNotOpeningShouldNotSetContentLayerEnabledToTrue()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Processing);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);
            _sut.Show();
            _mockContentLayer.ClearReceivedCalls();

            mockWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockWindow, new StateChangedEventArgs<WindowState>(WindowState.Ready, WindowState.Processing));

            _mockContentLayer.Received(0).Enabled = Arg.Any<bool>();
        }

        [TestMethod]
        public void ConsoleWindowTappedWhenStateProcessingShouldCallFlushText()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Processing);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            _sut.Show();

            mockWindow.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockWindow, new ButtonEventArgs(Vector2.Zero));

            mockWindow.Received(1).FlushText();
        }

        [TestMethod]
        public void ConsoleWindowTappedWhenStateNotProcessingShouldNotCallFlushText()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Opening);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            _sut.Show();

            mockWindow.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockWindow, new ButtonEventArgs(Vector2.Zero));

            mockWindow.Received(0).FlushText();
        }

        [TestMethod]
        public void ConsoleWindowTappedOutsideWindowBoundsShouldCallConsoleWindowCloseWindow()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Opening);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            _sut.Show();

            mockWindow.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockWindow, new ButtonEventArgs(_bounds.Location - new Vector2(10)));

            mockWindow.Received(1).CloseWindow();
        }

        [TestMethod]
        public void ConsoleWindowTappedOutsideWindowBoundsShouldSetDialogResultToDialogResultOk()
        {
            Construct();

            var subscriber = Substitute.For<EventHandler<DialogClosedEventArgs>>();
            _sut.DialogClosed += subscriber;

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Opening);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            _sut.Show();

            mockWindow.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockWindow, new ButtonEventArgs(_bounds.Location - new Vector2(10)));
            mockWindow.Disposed += Raise.Event<EventHandler<EventArgs>>(mockWindow, new EventArgs());

            subscriber.Received(1)(_sut, Arg.Is<DialogClosedEventArgs>(x => x.Result == DialogResult.Ok));
        }

        [TestMethod]
        public void ConsoleWindowTappedInsideWindowBoundsShouldNotCallConsoleWindowCloseWindow()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Opening);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            _sut.Show();

            mockWindow.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockWindow, new ButtonEventArgs(_bounds.Location + new Vector2(10)));

            mockWindow.Received(0).CloseWindow();
        }

        [TestMethod]
        public void ConsoleWindowDisposedShouldClearDialogLayerEntities()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);
            _sut.Show();
            ClearLayerCalls();

            mockWindow.Disposed += Raise.Event<EventHandler<EventArgs>>(mockWindow, new EventArgs());

            foreach (ILayer mockedLayer in _mockHostLayer.Entities)
                mockedLayer.Received(1).ClearEntities();
        }

        [TestMethod]
        public void ConsoleWindowDisposedShouldFireDialogClosedEvent()
        {
            Construct();

            var subscriber = Substitute.For<EventHandler<DialogClosedEventArgs>>();
            _sut.DialogClosed += subscriber;

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);
            _sut.Show();
            _mockHostLayer.ClearReceivedCalls();

            mockWindow.Disposed += Raise.Event<EventHandler<EventArgs>>(mockWindow, new EventArgs());

            subscriber.Received(1)(_sut, Arg.Is<DialogClosedEventArgs>(x => x.Result == DialogResult.Cancel));
        }

        [TestMethod]
        public void ConsoleWindowDisposedShouldSetVisibleFalse()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);
            _sut.Show();
            _mockHostLayer.ClearReceivedCalls();

            mockWindow.Disposed += Raise.Event<EventHandler<EventArgs>>(mockWindow, new EventArgs());

            Assert.IsFalse(_sut.Visible);
        }

        [TestMethod]
        public void ConsoleWindowDisposedShouldSetEnabledFalse()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);
            _sut.Show();
            _mockHostLayer.ClearReceivedCalls();

            mockWindow.Disposed += Raise.Event<EventHandler<EventArgs>>(mockWindow, new EventArgs());

            Assert.IsFalse(_sut.Enabled);
        }
        #endregion

        #region Close Tests
        [TestMethod]
        public void CloseShouldCloseConsoleWindow()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();

            _sut.Close();

            mockConsoleWindow.Received(1).CloseWindow();
        }

        [TestMethod]
        public void CloseShouldSetContentLayerVisibleToFalse()
        {
            Construct();

            _sut.Close();

            _mockContentLayer.Received(1).Visible = false;
        }

        [TestMethod]
        public void CloseShouldSetContentLayerEnabledToFalse()
        {
            Construct();

            _sut.Close();

            _mockContentLayer.Received(1).Enabled = false;
        }
        #endregion

        #region IsOpen Tests
        [TestMethod]
        public void IsOpenBeforeShowShouldReturnFalse()
        {
            Construct();

            Assert.IsFalse(_sut.IsOpen);
        }

        [TestMethod]
        public void IsOpenAfterShowShouldReturnTrue()
        {
            Construct();

            _sut.Show();

            Assert.IsTrue(_sut.IsOpen);
        }

        [TestMethod]
        public void IsOpenConsoleWindowDisposesShouldReturnFalse()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            mockConsoleWindow.IsDisposed.Returns(true);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();

            Assert.IsFalse(_sut.IsOpen);
        }
        #endregion

        #region BackgroundColour and BorderColour Tests
        [TestMethod]
        public void BorderColourSetWhenConsoleWindowExistsShouldSetConsoleWindowBorderColour()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();

            _sut.BorderColour = Color.Plum;

            mockConsoleWindow.Received(1).BorderColour = Color.Plum;
        }

        [TestMethod]
        public void BackgroundColourSetWhenConsoleWindowExistsShouldSetConsoleWindowBackgroundColour()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();

            _sut.BackgroundColour = Color.Plum;

            mockConsoleWindow.Received(1).BackgroundColour = Color.Plum;
        }
        #endregion

        #region Update, PreDraw, and Draw Tests
        [TestMethod]
        public void UpdateShouldCallLayerUpdate()
        {
            GameTime expected = new GameTime();
            Construct();

            _sut.Update(expected);

            _mockHostLayer.Received(1).Update(expected);
        }

        [TestMethod]
        public void PreDrawShouldCallLayerPreDraw()
        {
            GameTime expected = new GameTime();
            Construct();

            _sut.PreDraw(expected);

            _mockHostLayer.Received(1).PreDraw(expected);
        }

        [TestMethod]
        public void DrawShouldCallLayerDraw()
        {
            GameTime expected = new GameTime();
            Construct();

            _sut.Draw(expected);

            _mockHostLayer.Received(1).Draw(expected);
        }
        #endregion
    }
}
