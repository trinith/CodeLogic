using System;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class MenuContentLayerBase_Tests
    {
        #region Passthrough Class
        public class MenuContentLayerBase_Testable : MenuContentLayerBase
        {
            public event EventHandler<EventArgs> HideCalled;
            public event EventHandler<EventArgs> ShowCalled;

            public MenuContentLayerBase_Testable(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds)
                : base(host, mainSpriteBatch, contentBounds)
            {
            }

            protected override void OnHide()
            {
                base.OnHide();

                this.HideCalled?.Invoke(this, new EventArgs());
            }

            protected override void OnShow()
            {
                base.OnShow();

                this.ShowCalled?.Invoke(this, new EventArgs());
            }
        }
        #endregion

        private MenuContentLayerBase_Testable _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;

        private RectangleF _contentBounds = new RectangleF(200, 100, 400, 300);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
        }

        private void Construct()
        {
            _sut = new MenuContentLayerBase_Testable(_mockEngine, _mockSpriteBatch, _contentBounds);
        }

        #region Constructor Tests
        [TestMethod]
        public void ConstructShouldSetContentBoundsToParameterValue()
        {
            Construct();

            Assert.AreEqual<RectangleF>(_contentBounds, _sut.ContentBounds);
        }
        #endregion

        #region Passthrough Tests
        [TestMethod]
        public void HideShouldTriggerOnHide()
        {
            Construct();
            EventHandler<EventArgs> subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.HideCalled += subscriber;

            _sut.Hide();

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void ShowShouldTriggerOnShow()
        {
            Construct();
            EventHandler<EventArgs> subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.ShowCalled += subscriber;

            _sut.Show();

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion
    }
}
