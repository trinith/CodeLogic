using System;
using System.Collections.Generic;
using System.Linq;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.EntityGenerators;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.EntityGenerators
{
    [TestClass]
    public class CloudCoverGenerator_Tests
    {
        private CloudCoverGenerator _sut;

        private SizeF _screenSize = new SizeF(1000, 800);
        private ITexture2D[] _mockTextures;
        private ISpriteBatch _mockSpriteBatch;
        private IRandom _mockRandom;

        private IEngine _mockEngine;
        private GameObjectFactory _mockGOF;

        private Vector2 _topLeft;
        private Vector2 _topRight;

        [TestInitialize]
        public void Init()
        {
            _topLeft = new Vector2(0, _screenSize.Height * 1f / 3f);
            _topRight = new Vector2(_screenSize.Width, _screenSize.Height * 2f / 3f);

            _mockTextures =
                new ITexture2D[]
                {
                    Substitute.For<ITexture2D>(),
                    Substitute.For<ITexture2D>(),
                };
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockRandom = Substitute.For<IRandom>();

            _mockEngine = Substitute.For<IEngine>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);
        }

        private void Construct()
        {
            _sut = new CloudCoverGenerator(_screenSize, _mockTextures, _mockSpriteBatch, _mockRandom);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_CloudTextures()
        {
            _sut = new CloudCoverGenerator(_screenSize, null, _mockSpriteBatch, _mockRandom);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new CloudCoverGenerator(_screenSize, _mockTextures, null, _mockRandom);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Random()
        {
            _sut = new CloudCoverGenerator(_screenSize, _mockTextures, _mockSpriteBatch, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithEmptyCloudTextureListShouldThrowException()
        {
            ITexture2D[] emptyTextures = new ITexture2D[] { };
            _sut = new CloudCoverGenerator(_screenSize, emptyTextures, _mockSpriteBatch, _mockRandom);
        }
        #endregion

        #region GenerateEntities Tests
        /* Random Usage - Four Calls
         *      [0] - (0, textureCount)          :: Gets a random texture
         *      [1] - (0, screenWidth + 1)       :: Get a random x position
         *      [2] - (0, screenHeight - yStart) :: Get a random yOffset
         *      [3] - (1, 4)                     :: Get a random depth value.
         */

        /* Test Categories
         *      - [X] Generate expected entities
         *      - [X] Generate expected textures :: CreateCloud receives expected textures
         *      - [X] Generate expected positions :: CreateCloud receives expected positions
         *      - [X] Generate expected depth
         *      - [X] Generate expected colour
         *      - [X] Return expected entities in expected order
         */

        [TestMethod]
        public void GenerateEntitiesShouldCreateExpectedCloudsForNumEntities_TestA()
        {
            Construct();

            _sut.GenerateEntities(_mockEngine, 2);

            _mockGOF.Received(2).CreateCloud(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ITexture2D>(), Color.White);
        }

        [TestMethod]
        public void GenerateEntitiesShouldCreateExpectedCloudsForNumEntities_TestB()
        {
            Construct();

            _sut.GenerateEntities(_mockEngine, 3);

            _mockGOF.Received(3).CreateCloud(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ITexture2D>(), Color.White);
        }

        [TestMethod]
        public void GenerateEntitiesShouldCreateCloudWithExpectedTexture_TestA()
        {
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(0);
            Construct();

            _sut.GenerateEntities(_mockEngine, 1);

            _mockGOF.Received(1).CreateCloud(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, _mockTextures[0], Color.White);
        }

        [TestMethod]
        public void GenerateEntitiesShouldCreateCloudWithExpectedTexture_TestB()
        {
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(1);
            Construct();

            _sut.GenerateEntities(_mockEngine, 1);

            _mockGOF.Received(1).CreateCloud(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, _mockTextures[1], Color.White);
        }

        [TestMethod]
        public void CreateEntitiesShouldCreateCloudWithExpectedPosition_TestA()
        {
            Vector2 p0 = _topLeft;
            Vector2 dir = _topRight - _topLeft;
            float length = dir.Length();
            dir.Normalize();

            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(0, 100, 200, 1);

            _mockTextures[0].Width.Returns(50);
            _mockTextures[0].Height.Returns(40);
            Construct();

            _sut.GenerateEntities(_mockEngine, 1);

            float expectedX = 100;
            float expectedY = (p0.Y + ((expectedX - p0.X) / dir.X) * dir.Y) + 200;

            RectangleF expectedBounds = new RectangleF(
                expectedX - 25,
                expectedY - 20,
                50,
                40
            );

            _mockGOF.Received(1).CreateCloud(_mockEngine, expectedBounds, _mockSpriteBatch, _mockTextures[0], Arg.Any<Color>());
        }

        [TestMethod]
        public void CreateEntitiesShouldCreateCloudWithExpectedPosition_TestB()
        {
            Vector2 p0 = _topLeft;
            Vector2 dir = _topRight - _topLeft;
            float length = dir.Length();
            dir.Normalize();

            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(0, 150, 375, 1);

            _mockTextures[0].Width.Returns(50);
            _mockTextures[0].Height.Returns(40);
            Construct();

            _sut.GenerateEntities(_mockEngine, 1);

            float expectedX = 150;
            float expectedY = (p0.Y + ((expectedX - p0.X) / dir.X) * dir.Y) + 375;

            RectangleF expectedBounds = new RectangleF(
                expectedX - 25,
                expectedY - 20,
                50,
                40
            );

            _mockGOF.Received(1).CreateCloud(_mockEngine, expectedBounds, _mockSpriteBatch, _mockTextures[0], Arg.Any<Color>());
        }

        [TestMethod]
        public void GenerateEntitiesShouldCreateCloudWithExpectedDepth()
        {
            Construct();
            ICloud mockEntity = Substitute.For<ICloud>();
            _mockGOF.CreateCloud(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockEntity);
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(0, 100, 150, 2);

            _sut.GenerateEntities(_mockEngine, 1);

            mockEntity.Received(1).Depth = 2;
        }

        [TestMethod]
        public void GenerateEntitiesShouldSetCloudColourWithExpectedValue_Depth1()
        {
            Construct();
            ICloud mockEntity = Substitute.For<ICloud>();
            _mockGOF.CreateCloud(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockEntity);
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(0, 100, 150, 1);

            _sut.GenerateEntities(_mockEngine, 1);

            mockEntity.Received(1).Mask = CloudCoverGenerator.Constants.ClrLow;
        }

        [TestMethod]
        public void GenerateEntitiesShouldSetCloudColourWithExpectedValue_Depth2()
        {
            Construct();
            ICloud mockEntity = Substitute.For<ICloud>();
            _mockGOF.CreateCloud(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockEntity);
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(0, 100, 150, 2);

            _sut.GenerateEntities(_mockEngine, 1);

            mockEntity.Received(1).Mask = CloudCoverGenerator.Constants.ClrMid;
        }

        [TestMethod]
        public void GenerateEntitiesShouldSetCloudColourWithExpectedValue_Depth3()
        {
            Construct();
            ICloud mockEntity = Substitute.For<ICloud>();
            _mockGOF.CreateCloud(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockEntity);
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(0, 100, 150, 3);

            _sut.GenerateEntities(_mockEngine, 1);

            mockEntity.Received(1).Mask = CloudCoverGenerator.Constants.ClrHigh;
        }

        [TestMethod]
        public void GenerateEntitiesShouldReturnEntitiesInExpectedOrder_TestA()
        {
            Construct();
            _mockRandom.Next(1, 4).Returns(2, 1, 3);
            List<ICloud> mockEntities = new List<ICloud>(
                new ICloud[]
                {
                    Substitute.For<ICloud>(),
                    Substitute.For<ICloud>(),
                    Substitute.For<ICloud>(),
                }
            );
            _mockGOF.CreateCloud(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockEntities[0], mockEntities[1], mockEntities[2]);

            var result = _sut.GenerateEntities(_mockEngine, 3);

            Assert.IsTrue(result.SequenceEqual(new ICloud[] { mockEntities[1], mockEntities[0], mockEntities[2] }));
        }

        [TestMethod]
        public void GenerateEntitiesShouldReturnEntitiesInExpectedOrder_TestB()
        {
            Construct();
            _mockRandom.Next(1, 4).Returns(3, 2, 1);
            List<ICloud> mockEntities = new List<ICloud>(
                new ICloud[]
                {
                    Substitute.For<ICloud>(),
                    Substitute.For<ICloud>(),
                    Substitute.For<ICloud>(),
                }
            );
            _mockGOF.CreateCloud(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockEntities[0], mockEntities[1], mockEntities[2]);

            var result = _sut.GenerateEntities(_mockEngine, 3);

            Assert.IsTrue(result.SequenceEqual(new ICloud[] { mockEntities[2], mockEntities[1], mockEntities[0] }));
        }
        #endregion

        #region RepositionEntity Tests
        [TestMethod]
        public void RepositionEntityShouldPlaceEntityInExpectedPosition_TestA()
        {
            // Initial

            ICloud mockEntity = Substitute.For<ICloud>();
            mockEntity.Bounds.Returns(new RectangleF(50, 50, 400, 400));
            mockEntity.Texture.Width.Returns(200);
            mockEntity.Texture.Height.Returns(100);

            Vector2 p0 = _topLeft;
            Vector2 dir = _topRight - _topLeft;
            float length = dir.Length();
            dir.Normalize();

            float expectedX = _screenSize.Width + mockEntity.Texture.Width;
            float expectedY = p0.Y + ((expectedX - p0.X) / dir.X) * dir.Y;

            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(100);
            Construct();

            _sut.RepositionEntity(mockEntity);

            mockEntity.Received(1).Bounds = new RectangleF(expectedX - 200 / 2f, (expectedY + 100) - 100 / 2f, 400, 400);
        }

        [TestMethod]
        public void RepositionEntityShouldPlaceEntityInExpectedPosition_TestB()
        {
            // Change random return

            ICloud mockEntity = Substitute.For<ICloud>();
            mockEntity.Bounds.Returns(new RectangleF(50, 50, 400, 400));
            mockEntity.Texture.Width.Returns(200);
            mockEntity.Texture.Height.Returns(100);

            Vector2 p0 = _topLeft;
            Vector2 dir = _topRight - _topLeft;
            float length = dir.Length();
            dir.Normalize();

            float expectedX = _screenSize.Width + mockEntity.Texture.Width;
            float expectedY = p0.Y + ((expectedX - p0.X) / dir.X) * dir.Y;

            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(200);
            Construct();

            _sut.RepositionEntity(mockEntity);

            mockEntity.Received(1).Bounds = new RectangleF(expectedX - 200 / 2f, (expectedY + 200) - 100 / 2f, 400, 400);
        }

        [TestMethod]
        public void RepositionEntityShouldPlaceEntityInExpectedPosition_TestC()
        {
            // Change texture dimensions

            ICloud mockEntity = Substitute.For<ICloud>();
            mockEntity.Bounds.Returns(new RectangleF(50, 50, 400, 400));
            mockEntity.Texture.Width.Returns(400);
            mockEntity.Texture.Height.Returns(200);

            Vector2 p0 = _topLeft;
            Vector2 dir = _topRight - _topLeft;
            float length = dir.Length();
            dir.Normalize();

            float expectedX = _screenSize.Width + mockEntity.Texture.Width;
            float expectedY = p0.Y + ((expectedX - p0.X) / dir.X) * dir.Y;

            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(100);
            Construct();

            _sut.RepositionEntity(mockEntity);

            mockEntity.Received(1).Bounds = new RectangleF(expectedX - 400 / 2f, (expectedY + 100) - 200 / 2f, 400, 400);
        }
        #endregion
    }
}
