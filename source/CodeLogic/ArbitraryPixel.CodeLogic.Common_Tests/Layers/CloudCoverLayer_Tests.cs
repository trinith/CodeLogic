using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.EntityGenerators;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class CloudCoverLayer_Tests
    {
        private CloudCoverLayer _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private IEntityGenerator<ICloud> _mockCloudGen;
        private ICloudControllerFactory _mockControllerFactory;
        private IRandom _mockRandom;
        private IObjectSearcher _mockObjectSearcher;

        private List<ICloudMovementController> _mockControllers;

        public object Host { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockCloudGen = Substitute.For<IEntityGenerator<ICloud>>();
            _mockControllerFactory = Substitute.For<ICloudControllerFactory>();
            _mockRandom = Substitute.For<IRandom>();
            _mockObjectSearcher = Substitute.For<IObjectSearcher>();

            _mockControllers = new List<ICloudMovementController>();
            _mockControllerFactory.CreateCloudMovementController(Arg.Any<ICloud>(), _mockCloudGen, Arg.Any<Vector2>()).Returns(
                x =>
                {
                    ICloud cloud = x[0] as ICloud;
                    ICloudMovementController controller = Substitute.For<ICloudMovementController>();
                    controller.Cloud.Returns(cloud);

                    _mockControllers.Add(controller);

                    return controller;
                }
            );

            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(_mockEngine.Graphics.GraphicsDevice).Returns(_mockSpriteBatch);
        }

        private void Construct()
        {
            _sut = new CloudCoverLayer(_mockEngine, _mockSpriteBatch, _mockCloudGen, _mockControllerFactory, _mockRandom, _mockObjectSearcher);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParamterShouldThrowException_CloudControllerFactory()
        {
            _sut = new CloudCoverLayer(_mockEngine, _mockSpriteBatch, _mockCloudGen, null, _mockRandom, _mockObjectSearcher);
        }

        [TestMethod]
        public void ConstructShouldCallGeneratorGenerateEntitiesWithExpectedParameters()
        {
            Construct();

            _mockCloudGen.Received(1).GenerateEntities(_mockEngine, 100);
        }

        [TestMethod]
        public void ContructShouldCreateMovementControllerForEachGeneratedEntity()
        {
            ICloud[] mockClouds =
                new ICloud[]
                {
                    Substitute.For<ICloud>(),
                    Substitute.For<ICloud>(),
                };
            _mockCloudGen.GenerateEntities(_mockEngine, 100).Returns(mockClouds);

            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));
            SizeF screenSize = (SizeF)_mockEngine.ScreenManager.World;
            Vector2 topLeft = new Vector2(0, screenSize.Height * 1f / 3f);
            Vector2 topRight = new Vector2(screenSize.Width, screenSize.Height * 2f / 3f);

            Vector2 dir = topLeft - topRight;
            dir.Normalize();

            Construct();

            foreach (ICloud cloud in mockClouds)
                _mockControllerFactory.Received(1).CreateCloudMovementController(cloud, _mockCloudGen, dir);
        }

        [TestMethod]
        public void ConstructShouldCreateCloudFlashController()
        {
            ICloud[] mockClouds =
                new ICloud[]
                {
                                Substitute.For<ICloud>(),
                                Substitute.For<ICloud>(),
                };
            _mockCloudGen.GenerateEntities(_mockEngine, 100).Returns(mockClouds);

            Construct();

            _mockControllerFactory.Received(1).CreateCloudFlashController(mockClouds, _mockRandom, _mockControllerFactory, _mockObjectSearcher);
        }

        [TestMethod]
        public void ConstructShouldAttachToLightningFlashControllerLightningFlashedEvent()
        {
            ICloudFlashController mockFlashController = Substitute.For<ICloudFlashController>();
            _mockControllerFactory.CreateCloudFlashController(Arg.Any<ICloud[]>(), _mockRandom, _mockControllerFactory, _mockObjectSearcher).Returns(mockFlashController);

            Construct();

            mockFlashController.Received(1).LightningFlashed += Arg.Any<EventHandler<LightningFlashEventArgs>>();
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldCallUpdateOnCreatedControllers()
        {
            ICloud[] mockClouds =
                new ICloud[]
                {
                    Substitute.For<ICloud>(),
                    Substitute.For<ICloud>(),
                };
            _mockCloudGen.GenerateEntities(_mockEngine, 100).Returns(mockClouds);

            Construct();

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            Received.InOrder(
                () =>
                {
                    foreach (ICloudMovementController controller in _mockControllers)
                        controller.Update(gameTime);
                }
            );
        }

        [TestMethod]
        public void UpdateShouldCallUpdateOnCloudFlashController()
        {
            ICloudFlashController mockCloudFlashController = Substitute.For<ICloudFlashController>();
            _mockControllerFactory.CreateCloudFlashController(Arg.Any<ICloud[]>(), _mockRandom, _mockControllerFactory, _mockObjectSearcher).Returns(mockCloudFlashController);
            Construct();

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            mockCloudFlashController.Received(1).Update(gameTime);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldDrawAllGeneratedClouds()
        {
            Matrix matrix = Matrix.CreateTranslation(-1, 0, 2);
            _mockEngine.ScreenManager.ScaleMatrix.Returns(matrix);

            IEffect mockEffect = Substitute.For<IEffect>();
            _mockEngine.AssetBank.Get<IEffect>("LightningFlash").Returns(mockEffect);

            ICloud[] mockClouds =
                new ICloud[]
                {
                    Substitute.For<ICloud>(),
                    Substitute.For<ICloud>(),
                };
            _mockCloudGen.GenerateEntities(_mockEngine, 100).Returns(mockClouds);

            mockClouds[0].Intensity.Returns(1.23f);
            mockClouds[1].Intensity.Returns(0.86f);

            Construct();

            GameTime gameTime = new GameTime();
            _sut.Draw(gameTime);

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.End(); // This end occurs because the the layer automatically calls it before it moves on to do the custom drawing.

                    for (int i = 0; i < mockClouds.Length; i++)
                    {
                        _mockSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, mockEffect, matrix);
                        mockEffect.SetParameter("xIntensity", mockClouds[i].Intensity);
                        mockClouds[i].Draw(gameTime);
                        _mockSpriteBatch.End();
                    }
                }
            );
        }
        #endregion

        #region FlashController Event Tests
        [TestMethod]
        public void FlashControllerLightningFlashedEventShouldTriggerLightningFlashedEvent()
        {
            ICloud mockCloud = Substitute.For<ICloud>();
            var subscriber = Substitute.For<EventHandler<LightningFlashEventArgs>>();
            ICloudFlashController mockFlashController = Substitute.For<ICloudFlashController>();
            _mockControllerFactory.CreateCloudFlashController(Arg.Any<ICloud[]>(), _mockRandom, _mockControllerFactory, _mockObjectSearcher).Returns(mockFlashController);

            Construct();

            _sut.LightningFlashed += subscriber;

            mockFlashController.LightningFlashed += Raise.Event<EventHandler<LightningFlashEventArgs>>(mockFlashController, new LightningFlashEventArgs(mockCloud));

            subscriber.Received(1)(_sut, Arg.Is<LightningFlashEventArgs>(x => x.Cloud == mockCloud));
        }
        #endregion
    }
}
