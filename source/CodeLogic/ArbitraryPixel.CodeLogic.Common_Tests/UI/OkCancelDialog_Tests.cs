using System;
using System.Collections.Generic;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
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

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class OkCancelDialog_Tests
    {
        private OkCancelDialog _sut;

        private IEngine _mockEngine;
        private ITextObjectBuilder _mockTextBuilder;
        private GameObjectFactory _mockGOF;

        private RectangleF _bounds = new RectangleF(20, 40, 500, 300);
        private string _textFormat = "Test";

        private ILayer _mockHostLayer;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockTextBuilder = Substitute.For<ITextObjectBuilder>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);

            _mockHostLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>()).Returns(_mockHostLayer, Substitute.For<ILayer>());

            List<IEntity> childEntities = new List<IEntity>();
            _mockHostLayer.Entities.Returns(childEntities);
            _mockHostLayer.When(x => x.AddEntity(Arg.Any<IEntity>())).Do(x => childEntities.Add(x[0] as IEntity));
        }

        private void Construct()
        {
            _sut = new OkCancelDialog(_mockEngine, _bounds, _mockTextBuilder, _textFormat);
        }

        #region Show Tests
        [TestMethod]
        public void ShowShouldCreateCancelButton()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);
            mockConsoleWindow.ClientRectangle.Returns(_bounds);

            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(
                mockCancelButton,
                Substitute.For<ISimpleButton>()
            );

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            Construct();

            _sut.Show();

            Received.InOrder(
                () =>
                {
                    _mockGOF.CreateSimpleButton(_mockEngine, new RectangleF(_bounds.Right - 200, _bounds.Bottom - 75, 200, 75), ((ILayer)_mockHostLayer.Entities[2]).MainSpriteBatch, mockFont);
                    mockCancelButton.Text = "Cancel";
                    mockCancelButton.Visible = false;
                    mockCancelButton.Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
                }
            );
        }

        [TestMethod]
        public void ShowShouldCreateOkButton()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);
            mockConsoleWindow.ClientRectangle.Returns(_bounds);

            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(
                Substitute.For<ISimpleButton>(),
                mockOkButton
            );

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            Construct();

            _sut.Show();

            Received.InOrder(
                () =>
                {
                    _mockGOF.CreateSimpleButton(_mockEngine, new RectangleF(_bounds.Right - 200 - 10 - 200, _bounds.Bottom - 75, 200, 75), ((ILayer)_mockHostLayer.Entities[2]).MainSpriteBatch, mockFont);
                    mockOkButton.Text = "Ok";
                    mockOkButton.Visible = false;
                    mockOkButton.Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
                }
            );
        }

        [TestMethod]
        public void ShowShouldAddObjectsToParentLayer()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(
                mockCancelButton,
                mockOkButton
            );

            Construct();

            _sut.Show();

            Received.InOrder(
                () =>
                {
                    ((ILayer)_mockHostLayer.Entities[0]).AddEntity(mockCancelButton);
                    ((ILayer)_mockHostLayer.Entities[0]).AddEntity(mockOkButton);
                }
            );
        }
        #endregion

        #region Console Window Event Tests
        [TestMethod]
        public void ConsoleWindowStateChangedFromOpeningShouldSetOkButtonVisibleTrue()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            _sut.Show();
            mockCancelButton.ClearReceivedCalls();
            mockOkButton.ClearReceivedCalls();

            mockWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockWindow, new StateChangedEventArgs<WindowState>(WindowState.Opening, WindowState.Ready));

            mockOkButton.Received(1).Visible = true;
        }

        [TestMethod]
        public void ConsoleWindowStateChangedFromOpeningShouldSetCancelButtonVisibleTrue()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            _sut.Show();
            mockCancelButton.ClearReceivedCalls();
            mockOkButton.ClearReceivedCalls();

            mockWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockWindow, new StateChangedEventArgs<WindowState>(WindowState.Opening, WindowState.Ready));

            mockCancelButton.Received(1).Visible = true;
        }

        [TestMethod]
        public void ConsoleWindowStateChangedFromNotOpeningShouldNotSetOkButtonVisible()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            _sut.Show();
            mockCancelButton.ClearReceivedCalls();
            mockOkButton.ClearReceivedCalls();

            mockWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockWindow, new StateChangedEventArgs<WindowState>(WindowState.Processing, WindowState.Ready));

            mockOkButton.Received(0).Visible = Arg.Any<bool>();
        }

        [TestMethod]
        public void ConsoleWindowStateChangedFromNotOpeningShouldNotSetCancelButtonVisible()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            _sut.Show();
            mockCancelButton.ClearReceivedCalls();
            mockOkButton.ClearReceivedCalls();

            mockWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockWindow, new StateChangedEventArgs<WindowState>(WindowState.Processing, WindowState.Ready));

            mockCancelButton.Received(0).Visible = Arg.Any<bool>();
        }

        [TestMethod]
        public void ConsoleWindowTappedOutsideBoundsShouldNotCallConsoleWindowCloseWindow()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Opening);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            _sut.Show();

            mockWindow.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockWindow, new ButtonEventArgs(_bounds.Location - new Vector2(10)));

            mockWindow.Received(0).CloseWindow();
        }

        [TestMethod]
        public void ConsoleWindowTappedInsideBoundsShouldNotCallConsoleWindowCloseWindow()
        {
            Construct();

            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            mockWindow.WindowState.Returns(WindowState.Opening);
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            _sut.Show();

            mockWindow.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockWindow, new ButtonEventArgs(_bounds.Location + new Vector2(10)));

            mockWindow.Received(0).CloseWindow();
        }
        #endregion

        #region Ok Button Tapped Tests
        [TestMethod]
        public void OkButtonTappedShouldCloseConsoleWindow()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            mockOkButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));

            mockConsoleWindow.Received(1).CloseWindow();
        }

        [TestMethod]
        public void OkButtonTappedShouldHideOkButton()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            mockOkButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));

            mockOkButton.Received(1).Visible = false;
        }

        [TestMethod]
        public void OkButtonTappedShouldHideCancelButton()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            mockOkButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));

            mockCancelButton.Received(1).Visible = false;
        }

        [TestMethod]
        public void OkButtonTappedShouldGenerateDialogClosedEventWithDialogResultOk()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            EventHandler<DialogClosedEventArgs> subscriber = Substitute.For<EventHandler<DialogClosedEventArgs>>();
            _sut.DialogClosed += subscriber;

            mockOkButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));
            mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>(mockConsoleWindow, new EventArgs());

            subscriber.Received(1)(_sut, Arg.Is<DialogClosedEventArgs>(x => x.Result == DialogResult.Ok));
        }

        [TestMethod]
        public void OkButtonTappedShouldPlaySound()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            mockOkButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));

            mockSound.Received(1).Play();
        }
        #endregion

        #region Cancel Button Tapped Tests
        [TestMethod]
        public void CancelButtonTappedShouldCloseConsoleWindow()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            mockCancelButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));

            mockConsoleWindow.Received(1).CloseWindow();
        }

        [TestMethod]
        public void CancelButtonTappedShouldHideOkButton()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            mockCancelButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));

            mockOkButton.Received(1).Visible = false;
        }

        [TestMethod]
        public void CancelButtonTappedShouldHideCancelButton()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            mockCancelButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));

            mockCancelButton.Received(1).Visible = false;
        }

        [TestMethod]
        public void CancelButtonTappedShouldGenerateDialogClosedEventWithDialogResultCancel()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            EventHandler<DialogClosedEventArgs> subscriber = Substitute.For<EventHandler<DialogClosedEventArgs>>();
            _sut.DialogClosed += subscriber;

            mockCancelButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));
            mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>(mockConsoleWindow, new EventArgs());

            subscriber.Received(1)(_sut, Arg.Is<DialogClosedEventArgs>(x => x.Result == DialogResult.Cancel));
        }

        [TestMethod]
        public void CancelButtonTappedShouldPlaySound()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            mockCancelButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockOkButton, new ButtonEventArgs(Vector2.Zero));

            mockSound.Received(1).Play();
        }
        #endregion

        #region Close Tests
        [TestMethod]
        public void CloseShouldHideOkButton()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            _sut.Close();

            mockOkButton.Received(1).Visible = false;
        }

        [TestMethod]
        public void CloseShouldHideCancelButton()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            _sut.Close();

            mockCancelButton.Received(1).Visible = false;
        }

        [TestMethod]
        public void CloseShouldGenerateDialogClosedEventWithDialogResultCancel()
        {
            ISimpleButton mockCancelButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockOkButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockCancelButton, mockOkButton);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGOF.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            Construct();
            _sut.Show();
            mockOkButton.ClearReceivedCalls();
            mockCancelButton.ClearReceivedCalls();

            EventHandler<DialogClosedEventArgs> subscriber = Substitute.For<EventHandler<DialogClosedEventArgs>>();
            _sut.DialogClosed += subscriber;

            _sut.Close();
            mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>(mockConsoleWindow, new EventArgs());

            subscriber.Received(1)(_sut, Arg.Is<DialogClosedEventArgs>(x => x.Result == DialogResult.Cancel));
        }
        #endregion
    }
}
