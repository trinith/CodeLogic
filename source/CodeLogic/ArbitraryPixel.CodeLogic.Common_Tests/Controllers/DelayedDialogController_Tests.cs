using System;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Controllers
{
    [TestClass]
    public class DelayedDialogController_Tests
    {
        private DelayedDialogController _sut;
        private IEngine _mockEngine;
        private IDialog _mockDialog;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockDialog = Substitute.For<IDialog>();

            _mockDialog.Bounds.Returns(new RectangleF(10, 20, 30, 40));
        }

        private void Construct(double delayTime = 2)
        {
            _sut = new DelayedDialogController(_mockEngine, _mockDialog, delayTime);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Dialog()
        {
            _sut = new DelayedDialogController(_mockEngine, null, 2);
        }

        [TestMethod]
        public void ConstructShouldSetBoundsToDialogBounds()
        {
            Construct();

            Assert.AreEqual<RectangleF>(new RectangleF(10, 20, 30, 40), _sut.Bounds);
        }

        [TestMethod]
        public void ConstructShouldAttachEventHandlerToDialogDialogClosedEvent()
        {
            Construct();

            _mockDialog.Received(1).DialogClosed += Arg.Any<EventHandler<DialogClosedEventArgs>>();
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateBeforeDelayTimeExpiresShouldNotCallDialogShow()
        {
            Construct();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1.9)));

            _mockDialog.Received(0).Show();
        }

        [TestMethod]
        public void UpdateAfterDelayTimeExpiresShouldCallDialogShow_TestA()
        {
            Construct();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            _mockDialog.Received(1).Show();
        }

        [TestMethod]
        public void UpdateAfterDelayTimeExpiresShouldCallDialogShow_TestB()
        {
            Construct(3);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3)));

            _mockDialog.Received(1).Show();
        }

        [TestMethod]
        public void UpdateAfterDelayTimeExpiresAndDialogShownShouldNotCallDialogShow()
        {
            Construct();
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));
            _mockDialog.ClearReceivedCalls();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(10)));

            _mockDialog.Received(0).Show();
        }

        [TestMethod]
        public void UpdateShouldCallDialogUpdate()
        {
            Construct();
            GameTime gameTime = new GameTime();

            _sut.Update(gameTime);

            _mockDialog.Received(1).Update(gameTime);
        }
        #endregion

        #region PreDraw Tests
        [TestMethod]
        public void PreDrawShouldCallDialogPreDraw()
        {
            Construct();
            GameTime gameTime = new GameTime();

            _sut.PreDraw(gameTime);

            _mockDialog.Received(1).PreDraw(gameTime);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldCallDialogDraw()
        {
            Construct();
            GameTime gameTime = new GameTime();

            _sut.Draw(gameTime);

            _mockDialog.Received(1).Draw(gameTime);
        }
        #endregion

        #region DialogClosed Event Tests
        [TestMethod]
        public void DialogClosedShouldUnsubscribeToDialogClosedEvent()
        {
            Construct();

            _mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(_mockDialog, new DialogClosedEventArgs(DialogResult.Cancel));

            _mockDialog.Received(1).DialogClosed -= Arg.Any<EventHandler<DialogClosedEventArgs>>();
        }

        [TestMethod]
        public void DialogClosedShouldCallDialogDispose()
        {
            Construct();

            _mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(_mockDialog, new DialogClosedEventArgs(DialogResult.Cancel));

            _mockDialog.Received(1).Dispose();
        }

        [TestMethod]
        public void DialogClosedShouldCallDisposeOnSelf()
        {
            var eventHandler = Substitute.For<EventHandler<EventArgs>>();
            Construct();
            _sut.Disposed += eventHandler;

            _mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(_mockDialog, new DialogClosedEventArgs(DialogResult.Cancel));

            // If we get the disposed event, we know dispose was called.
            eventHandler.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion
    }
}
