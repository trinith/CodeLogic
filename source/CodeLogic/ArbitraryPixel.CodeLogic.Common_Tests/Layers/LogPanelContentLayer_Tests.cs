using System;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class LogPanelContentLayer_Tests
    {
        private LogPanelContentLayer _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private IDeviceModel _mockDeviceModel;
        private ILogPanelModel _mockPanelModel;

        private GameObjectFactory _mockGameObjectFactory;
        private Vector2 _scale = new Vector2(1f);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();
            _mockPanelModel = Substitute.For<ILogPanelModel>();

            _mockPanelModel.WorldBounds.Returns(new SizeF(500, 300));

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            Construct();
        }

        private void Construct()
        {
            _sut = new LogPanelContentLayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel, _mockPanelModel);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_DeviceModel()
        {
            _sut = new LogPanelContentLayer(_mockEngine, _mockSpriteBatch, null, _mockPanelModel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_PanelModel()
        {
            _sut = new LogPanelContentLayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel, null);
        }
        #endregion

        #region PreDraw Tests
        // NOTE: For tests involving multiple calls or no calls, just going to test with CreateSequenceAttemptRecordView instead of everything. Nothing is conditional so that's good enough to be effective :)

        [TestMethod]
        public void PreDrawShouldNotCreateSequenceAttemptRecordViewWhenCurrentTrialIsOne()
        {
            _mockDeviceModel.CurrentTrial.Returns(1);

            _sut.PreDraw(new GameTime());

            _mockGameObjectFactory.Received(0).CreateSequenceAttemptRecordView(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<Vector2>(), Arg.Any<IDeviceModel>(), Arg.Any<int>());
        }

        [TestMethod]
        public void PreDrawShouldNotCreateSequenceAttemptRecordViewForTheSameTrial()
        {
            _mockDeviceModel.CurrentTrial.Returns(2);
            _sut.PreDraw(new GameTime());
            _mockGameObjectFactory.ClearReceivedCalls();

            _sut.PreDraw(new GameTime());

            _mockGameObjectFactory.Received(0).CreateSequenceAttemptRecordView(_mockEngine, _mockSpriteBatch, Vector2.Zero, _mockDeviceModel, 0);
        }

        [TestMethod]
        public void PreDrawShouldCreateSequenceAttemptRecordForAllUndrawnTrials()
        {
            _mockDeviceModel.CurrentTrial.Returns(3);

            _sut.PreDraw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateSequenceAttemptRecordView(_mockEngine, _mockSpriteBatch, Vector2.Zero, _mockDeviceModel, 0);
                    _mockGameObjectFactory.CreateSequenceAttemptRecordView(_mockEngine, _mockSpriteBatch, Vector2.Zero, _mockDeviceModel, 1);
                }
            );
        }

        [TestMethod]
        public void PreDrawShouldCreateSequenceAttemptRecordViewForUndrawnTrial()
        {
            _mockDeviceModel.CurrentTrial.Returns(2);

            _sut.PreDraw(new GameTime());

            _mockGameObjectFactory.Received(1).CreateSequenceAttemptRecordView(_mockEngine, _mockSpriteBatch, Vector2.Zero, _mockDeviceModel, 0);
        }

        [TestMethod]
        public void PreDrawShouldAddEventHandlerToAttemptRecordViewDrawBegin()
        {
            ISequenceAttemptRecordView mockView = Substitute.For<ISequenceAttemptRecordView>();
            _mockGameObjectFactory.CreateSequenceAttemptRecordView(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<Vector2>(), Arg.Any<IDeviceModel>(), Arg.Any<int>()).Returns(mockView);
            _mockDeviceModel.CurrentTrial.Returns(2);

            _sut.PreDraw(new GameTime());

            mockView.Received(1).DrawBegin += Arg.Any<EventHandler<ValueEventArgs<GameTime>>>();
        }

        [TestMethod]
        public void PreDrawShouldAddEventHandlerToAttemptRecordViewDrawEnd()
        {
            ISequenceAttemptRecordView mockView = Substitute.For<ISequenceAttemptRecordView>();
            _mockGameObjectFactory.CreateSequenceAttemptRecordView(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<Vector2>(), Arg.Any<IDeviceModel>(), Arg.Any<int>()).Returns(mockView);
            _mockDeviceModel.CurrentTrial.Returns(2);

            _sut.PreDraw(new GameTime());

            mockView.Received(1).DrawEnd += Arg.Any<EventHandler<ValueEventArgs<GameTime>>>();
        }

        [TestMethod]
        public void PreDraw_ViewDrawBeginShouldCallBeginOnSpriteBatch()
        {
            ISequenceAttemptRecordView mockView = Substitute.For<ISequenceAttemptRecordView>();
            _mockGameObjectFactory.CreateSequenceAttemptRecordView(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<Vector2>(), Arg.Any<IDeviceModel>(), Arg.Any<int>()).Returns(mockView);
            _mockDeviceModel.CurrentTrial.Returns(2);
            GameTime gameTime = new GameTime();

            _sut.PreDraw(gameTime);
            mockView.DrawBegin += Raise.Event<EventHandler<ValueEventArgs<GameTime>>>(mockView, new ValueEventArgs<GameTime>(gameTime));

            _mockSpriteBatch.Received(1).Begin();
        }

        [TestMethod]
        public void PreDraw_ViewDrawEndShouldCallEndOnSpriteBatch()
        {
            ISequenceAttemptRecordView mockView = Substitute.For<ISequenceAttemptRecordView>();
            _mockGameObjectFactory.CreateSequenceAttemptRecordView(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<Vector2>(), Arg.Any<IDeviceModel>(), Arg.Any<int>()).Returns(mockView);
            _mockDeviceModel.CurrentTrial.Returns(2);
            GameTime gameTime = new GameTime();

            _sut.PreDraw(gameTime);
            mockView.DrawEnd += Raise.Event<EventHandler<ValueEventArgs<GameTime>>>(mockView, new ValueEventArgs<GameTime>(gameTime));

            _mockSpriteBatch.Received(1).End();
        }

        [TestMethod]
        public void PreDrawShouldCreateRenderTarget()
        {
            ISequenceAttemptRecordView mockView = Substitute.For<ISequenceAttemptRecordView>();
            mockView.Bounds.Returns(new RectangleF(Vector2.Zero, new SizeF(33, 22)));
            _mockGameObjectFactory.CreateSequenceAttemptRecordView(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<Vector2>(), Arg.Any<IDeviceModel>(), Arg.Any<int>()).Returns(mockView);
            _mockDeviceModel.CurrentTrial.Returns(2);

            _sut.PreDraw(new GameTime());

            _mockEngine.GrfxFactory.RenderTargetFactory.Received(1).Create(_mockEngine.Graphics.GraphicsDevice, 33, 22, RenderTargetUsage.DiscardContents);
        }

        [TestMethod]
        public void PreDrawShouldCallSetDataOnRenderTarget()
        {
            ISequenceAttemptRecordView mockView = Substitute.For<ISequenceAttemptRecordView>();
            _mockGameObjectFactory.CreateSequenceAttemptRecordView(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<Vector2>(), Arg.Any<IDeviceModel>(), Arg.Any<int>()).Returns(mockView);
            _mockDeviceModel.CurrentTrial.Returns(2);

            IRenderTarget2D mockRenderTarget = Substitute.For<IRenderTarget2D>();
            _mockEngine.GrfxFactory.RenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<RenderTargetUsage>()).Returns(mockRenderTarget);

            _sut.PreDraw(new GameTime());

            mockRenderTarget.Received(1).SetData<Color>(Color.Transparent);
        }

        [TestMethod]
        public void PreDrawShouldCallDrawOnTempViewWithExpectedParameters()
        {
            ISequenceAttemptRecordView mockView = Substitute.For<ISequenceAttemptRecordView>();
            _mockGameObjectFactory.CreateSequenceAttemptRecordView(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<Vector2>(), Arg.Any<IDeviceModel>(), Arg.Any<int>()).Returns(mockView);
            _mockDeviceModel.CurrentTrial.Returns(2);

            IRenderTarget2D mockRenderTarget = Substitute.For<IRenderTarget2D>();
            _mockEngine.GrfxFactory.RenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<RenderTargetUsage>()).Returns(mockRenderTarget);

            GameTime gameTime = new GameTime();
            _sut.PreDraw(gameTime);

            mockView.Received(1).Draw(gameTime, mockRenderTarget, Color.Transparent);
        }
        #endregion

        #region Draw Tests
        // NOTE: The math for positioning here is involved. Not going to check every single positioning... just going to try to hit the key points.

        [TestMethod]
        public void DrawWithNoTexturesShouldMakeNoCallsToSpriteBatchDraw()
        {
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(Arg.Any<ITexture2D>(), Arg.Any<Vector2>(), Arg.Any<Rectangle?>(), Arg.Any<Color>(), Arg.Any<float>(), Arg.Any<Vector2>(), Arg.Any<Vector2>(), Arg.Any<SpriteEffects>(), Arg.Any<float>());
        }

        [TestMethod]
        public void DrawShouldDrawAllTextures_TestA()
        {
            _sut.EntityTextures.Add(Substitute.For<ITexture2D>());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(Arg.Any<ITexture2D>(), Arg.Any<Vector2>(), Arg.Any<Rectangle?>(), Arg.Any<Color>(), Arg.Any<float>(), Arg.Any<Vector2>(), Arg.Any<Vector2>(), Arg.Any<SpriteEffects>(), Arg.Any<float>());
        }

        [TestMethod]
        public void DrawShouldDrawAllTextures_TestB()
        {
            _sut.EntityTextures.Add(Substitute.For<ITexture2D>());
            _sut.EntityTextures.Add(Substitute.For<ITexture2D>());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(2).Draw(Arg.Any<ITexture2D>(), Arg.Any<Vector2>(), Arg.Any<Rectangle?>(), Arg.Any<Color>(), Arg.Any<float>(), Arg.Any<Vector2>(), Arg.Any<Vector2>(), Arg.Any<SpriteEffects>(), Arg.Any<float>());
        }

        [TestMethod]
        public void DrawShouldDrawTextures_TestA()
        {
            // One texture, should be centered at top of screen area.

            _mockPanelModel.CurrentOffset.Returns(new Vector2(100, 200));
            ITexture2D mockTextureA = Substitute.For<ITexture2D>();
            mockTextureA.Width.Returns(100);
            mockTextureA.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureA);

            SizeF padding = new SizeF(5);
            float borderHeight = 2;
            int colCount = 1;

            SizeF blockSize = new SizeF(
                colCount * mockTextureA.Width + (colCount - 1) * padding.Width,
                mockTextureA.Height
            );
            blockSize *= (SizeF)_scale;

            Vector2 expectedPosition = new Vector2(
                _mockPanelModel.WorldBounds.Width / 2f - blockSize.Width / 2f - _mockPanelModel.CurrentOffset.X,
                _mockPanelModel.WorldBounds.Height - _mockPanelModel.CurrentOffset.Y + padding.Height + borderHeight + ((1 / 3) * (padding.Height + mockTextureA.Height * _scale.Y))
            );
            expectedPosition = expectedPosition.ToPoint().ToVector2();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTextureA, expectedPosition, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        [TestMethod]
        public void DrawShouldDrawTextures_TestB()
        {
            // Two textures, centered at top of screen area -- check first texture

            _mockPanelModel.CurrentOffset.Returns(new Vector2(100, 200));
            ITexture2D mockTextureA = Substitute.For<ITexture2D>();
            mockTextureA.Width.Returns(100);
            mockTextureA.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureA);

            ITexture2D mockTextureB = Substitute.For<ITexture2D>();
            mockTextureB.Width.Returns(100);
            mockTextureB.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureB);

            SizeF padding = new SizeF(5);
            float borderHeight = 2;
            int colCount = 2;

            SizeF blockSize = new SizeF(
                colCount * mockTextureA.Width + (colCount - 1) * padding.Width,
                mockTextureA.Height
            );
            blockSize *= (SizeF)_scale;

            Vector2 expectedPosition = new Vector2(
                _mockPanelModel.WorldBounds.Width / 2f - blockSize.Width / 2f - _mockPanelModel.CurrentOffset.X,
                _mockPanelModel.WorldBounds.Height - _mockPanelModel.CurrentOffset.Y + padding.Height + borderHeight + ((1 / 3) * (padding.Height + mockTextureA.Height * _scale.Y))
            );
            expectedPosition = expectedPosition.ToPoint().ToVector2();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTextureA, expectedPosition, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        [TestMethod]
        public void DrawShouldDrawTextures_TestC()
        {
            // Three textures, centered at top of scren area -- check first texture

            _mockPanelModel.CurrentOffset.Returns(new Vector2(100, 200));
            ITexture2D mockTextureA = Substitute.For<ITexture2D>();
            mockTextureA.Width.Returns(100);
            mockTextureA.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureA);

            ITexture2D mockTextureB = Substitute.For<ITexture2D>();
            mockTextureB.Width.Returns(100);
            mockTextureB.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureB);

            ITexture2D mockTextureC = Substitute.For<ITexture2D>();
            mockTextureC.Width.Returns(100);
            mockTextureC.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureC);

            SizeF padding = new SizeF(5);
            float borderHeight = 2;
            int colCount = 3;

            SizeF blockSize = new SizeF(
                colCount * mockTextureA.Width + (colCount - 1) * padding.Width,
                mockTextureA.Height
            );
            blockSize *= (SizeF)_scale;

            Vector2 expectedPosition = new Vector2(
                _mockPanelModel.WorldBounds.Width / 2f - blockSize.Width / 2f - _mockPanelModel.CurrentOffset.X,
                _mockPanelModel.WorldBounds.Height - _mockPanelModel.CurrentOffset.Y + padding.Height + borderHeight + ((1 / 3) * (padding.Height + mockTextureA.Height * _scale.Y))
            );
            expectedPosition = expectedPosition.ToPoint().ToVector2();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTextureA, expectedPosition, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        [TestMethod]
        public void DrawShouldDrawTextures_TestD()
        {
            // Four textures, first top at top, centered, second row underneath, centered -- check last texture

            _mockPanelModel.CurrentOffset.Returns(new Vector2(100, 200));
            ITexture2D mockTextureA = Substitute.For<ITexture2D>();
            mockTextureA.Width.Returns(100);
            mockTextureA.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureA);

            ITexture2D mockTextureB = Substitute.For<ITexture2D>();
            mockTextureB.Width.Returns(100);
            mockTextureB.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureB);

            ITexture2D mockTextureC = Substitute.For<ITexture2D>();
            mockTextureC.Width.Returns(100);
            mockTextureC.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureC);

            ITexture2D mockTextureD = Substitute.For<ITexture2D>();
            mockTextureD.Width.Returns(100);
            mockTextureD.Height.Returns(50);
            _sut.EntityTextures.Add(mockTextureD);

            SizeF padding = new SizeF(5);
            float borderHeight = 2;
            int colCount = 1;

            SizeF blockSize = new SizeF(
                colCount * mockTextureA.Width + (colCount - 1) * padding.Width,
                mockTextureA.Height
            );
            blockSize *= (SizeF)_scale;

            Vector2 expectedPosition = new Vector2(
                _mockPanelModel.WorldBounds.Width / 2f - blockSize.Width / 2f - _mockPanelModel.CurrentOffset.X,
                _mockPanelModel.WorldBounds.Height - _mockPanelModel.CurrentOffset.Y + padding.Height + borderHeight + ((4 / 3) * (padding.Height + mockTextureA.Height * _scale.Y))
            );
            expectedPosition = expectedPosition.ToPoint().ToVector2();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTextureD, expectedPosition, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
        #endregion

        #region ModelReset Tests
        [TestMethod]
        public void ModelResetShouldClearEntityTextures()
        {
            Construct();

            // Just stuff a texture in the list so we know something is there.
            _sut.EntityTextures.Add(Substitute.For<ITexture2D>());

            // Trigger a reset event.
            _mockPanelModel.ModelReset += Raise.Event<EventHandler<EventArgs>>(_mockPanelModel, new EventArgs());

            // Reset should clear the list.
            Assert.AreEqual<int>(0, _sut.EntityTextures.Count);
        }

        [TestMethod]
        public void ModelResetShouldSetLastIndexDrawnToZero()
        {
            Construct();

            // We'll force a predraw to increment last index drawn.
            _mockDeviceModel.CurrentTrial.Returns(2);
            _mockEngine.GrfxFactory.RenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<RenderTargetUsage>()).Returns(Substitute.For<IRenderTarget2D>());
            _sut.PreDraw(new GameTime());
            Assert.AreNotEqual<int>(0, _sut.LastIndexDrawn, "ARRANGE GUARD - Ensures that the last index drawn is not zero.");

            // Trigger a reset event.
            _mockPanelModel.ModelReset += Raise.Event<EventHandler<EventArgs>>(_mockPanelModel, new EventArgs());

            // Reset should reset LastIndexDrawn.
            Assert.AreEqual<int>(0, _sut.LastIndexDrawn);
        }
        #endregion
    }
}
