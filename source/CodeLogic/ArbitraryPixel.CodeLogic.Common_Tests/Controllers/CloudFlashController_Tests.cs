using System;
using System.Collections.Generic;
using System.Linq;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Controllers
{
    [TestClass]
    public class CloudFlashController_Tests
    {
        private CloudFlashController _sut;

        private ICloud[] _mockClouds;
        private IRandom _mockRandom;
        private ICloudControllerFactory _mockControllerFactory;
        private IObjectSearcher _mockObjectSearcher;

        [TestInitialize]
        public void Initialize()
        {
            _mockClouds = new ICloud[]
                {
                    Substitute.For<ICloud>(),
                    Substitute.For<ICloud>(),
                };

            _mockRandom = Substitute.For<IRandom>();
            _mockControllerFactory = Substitute.For<ICloudControllerFactory>();
            _mockObjectSearcher = Substitute.For<IObjectSearcher>();

            // Default case for most tests, just return the target object passed. This bypasses the actual searcher that is given. Write separate tests for this.
            _mockObjectSearcher.GetMatchingObjects<ICloud>(_mockClouds, _mockClouds[0], Arg.Any<ObjectSearchComparer<ICloud>>()).Returns(
                x =>
                {
                    return new ICloud[] { x[1] as ICloud };
                }
            );
        }

        private void Construct()
        {
            _sut = new CloudFlashController(_mockClouds, _mockRandom, _mockControllerFactory, _mockObjectSearcher);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Clouds()
        {
            _sut = new CloudFlashController(null, _mockRandom, _mockControllerFactory, _mockObjectSearcher);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Random()
        {
            _sut = new CloudFlashController(_mockClouds, null, _mockControllerFactory, _mockObjectSearcher);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_CloudControllerFactory()
        {
            _sut = new CloudFlashController(_mockClouds, _mockRandom, null, _mockObjectSearcher);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_ObjectSearcher()
        {
            _sut = new CloudFlashController(_mockClouds, _mockRandom, _mockControllerFactory, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithEmptyCloudArrayShouldThrowExpectedException()
        {
            ICloud[] mockClouds = new ICloud[] { };
            _sut = new CloudFlashController(mockClouds, _mockRandom, _mockControllerFactory, _mockObjectSearcher);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateFirstTimeShouldRequestExpectedRandomRangeForFirstFlash()
        {
            Construct();

            _sut.Update(new GameTime());

            _mockRandom.Received(1).Next(2, 3);
        }

        [TestMethod]
        public void UpdateAfterFlashExpiresShouldRequestExpectedRandomRangeForSubsequentFlash()
        {
            Construct();

            _sut.Update(new GameTime());
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            _mockRandom.Received(1).Next(10, 15);
        }

        [TestMethod]
        public void UpdateSecondTimeAfterTimeExpiresShouldCreateIntensityController()
        {
            _mockRandom.Next(2, 3).Returns(2);
            _mockRandom.Next(0, 2).Returns(0);
            _mockRandom.Next(0, 10).Returns(4);
            Construct();
            _sut.Update(new GameTime());
            _mockRandom.ClearReceivedCalls();

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockControllerFactory.CreateCloudIntensityAnimation(4).Returns(mockAnimation);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            Received.InOrder(
                () =>
                {
                    // Create random number between 0 and 2 for target
                    // Create random number between 0 and 10 for animation
                    // Create cloud intensity animation
                    // Create cloud intensity controller

                    _mockRandom.Received(1).Next(0, 2);
                    _mockRandom.Received(1).Next(0, 10);
                    _mockControllerFactory.CreateCloudIntensityAnimation(4);
                    _mockControllerFactory.CreateCloudIntensityController(_mockClouds[0], mockAnimation);
                }
            );
        }

        [TestMethod]
        public void UpdateSecondTimeAfterTimeExpiresShouldTriggerLightningFlashedEvent()
        {
            _mockRandom.Next(0, 2).Returns(0);
            Construct();
            _sut.Update(new GameTime());
            var subscriber = Substitute.For<EventHandler<LightningFlashEventArgs>>();
            _sut.LightningFlashed += subscriber;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            subscriber.Received(1)(_sut, Arg.Is<LightningFlashEventArgs>(x => x.Cloud == _mockClouds[0]));
        }

        [TestMethod]
        public void UpdateAfterCreatingFlashShouldUpdateIntensityController()
        {
            _mockRandom.Next(2, 3).Returns(2);
            _mockRandom.Next(0, 2).Returns(0);
            _mockRandom.Next(0, 10).Returns(4);
            Construct();
            _sut.Update(new GameTime());
            _mockRandom.ClearReceivedCalls();

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockControllerFactory.CreateCloudIntensityAnimation(4).Returns(mockAnimation);

            ICloudIntensityController mockIntensityController = Substitute.For<ICloudIntensityController>();
            _mockControllerFactory.CreateCloudIntensityController(_mockClouds[0], mockAnimation).Returns(mockIntensityController);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            mockIntensityController.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateOnIntensityControllerThatIsNotCompleteShouldUpdateIntensityController()
        {
            _mockRandom.Next(2, 3).Returns(2);
            _mockRandom.Next(0, 2).Returns(0);
            _mockRandom.Next(0, 10).Returns(4);
            Construct();
            _sut.Update(new GameTime());
            _mockRandom.ClearReceivedCalls();

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockControllerFactory.CreateCloudIntensityAnimation(4).Returns(mockAnimation);

            ICloudIntensityController mockIntensityController = Substitute.For<ICloudIntensityController>();
            mockIntensityController.IsComplete.Returns(false);
            _mockControllerFactory.CreateCloudIntensityController(_mockClouds[0], mockAnimation).Returns(mockIntensityController);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            _sut.Update(new GameTime());
            _sut.Update(new GameTime());

            mockIntensityController.Received(2).Update(Arg.Any<GameTime>());
        }

        [TestMethod]
        public void UpdateOnIntensityControllerThatIsCompleteShouldRemoveItFromFutureUpdates()
        {
            _mockRandom.Next(2, 3).Returns(2);
            _mockRandom.Next(10, 15).Returns(10);
            _mockRandom.Next(0, 2).Returns(0);
            Construct();
            _sut.Update(new GameTime());
            _mockRandom.ClearReceivedCalls();

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockControllerFactory.CreateCloudIntensityAnimation(_mockRandom).Returns(mockAnimation);

            ICloudIntensityController mockIntensityController = Substitute.For<ICloudIntensityController>();
            mockIntensityController.IsComplete.Returns(true);
            _mockControllerFactory.CreateCloudIntensityController(_mockClouds[0], mockAnimation).Returns(mockIntensityController);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            _sut.Update(new GameTime());
            mockIntensityController.ClearReceivedCalls();
            _sut.Update(new GameTime());

            mockIntensityController.Received(0).Update(Arg.Any<GameTime>());
        }

        [TestMethod]
        public void UpdateWhenCreatingIntensityControllerOnCloudThatAlreadyHasControllerShouldNotCreateAnotherController()
        {
            _mockRandom.Next(2, 3).Returns(2);
            _mockRandom.Next(0, 2).Returns(0);
            Construct();
            _sut.Update(new GameTime());
            _mockRandom.ClearReceivedCalls();

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockControllerFactory.CreateCloudIntensityAnimation(_mockRandom).Returns(mockAnimation);

            ICloudIntensityController mockIntensityController = Substitute.For<ICloudIntensityController>();
            mockIntensityController.IsComplete.Returns(false);
            _mockControllerFactory.CreateCloudIntensityController(_mockClouds[0], mockAnimation).Returns(mockIntensityController);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));
            _mockControllerFactory.ClearReceivedCalls();
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            _mockControllerFactory.Received(0).CreateCloudIntensityController(Arg.Any<ICloud>(), Arg.Any<IValueAnimation<float>>());
        }
        #endregion

        #region ObjectSearcher Comparer Tests
        [TestMethod]
        public void SearchComparerShouldAlwaysReturnTarget()
        {
            // Expecting _mockClouds[0] (target)

            List<ICloud> foundObjects = new List<ICloud>();
            _mockObjectSearcher = Substitute.For<IObjectSearcher>();
            _mockObjectSearcher.When (x => x.GetMatchingObjects<ICloud>(Arg.Any<ICloud[]>(), Arg.Any<ICloud>(), Arg.Any<ObjectSearchComparer<ICloud>>())).Do(
                x =>
                {
                    ICloud[] clouds = x[0] as ICloud[];
                    ICloud target = x[1] as ICloud;
                    ObjectSearchComparer<ICloud> testMethod = x[2] as ObjectSearchComparer<ICloud>;
                    foreach (ICloud cloud in clouds)
                    {
                        if (testMethod(target, cloud))
                            foundObjects.Add(cloud);
                    }
                }
            );

            Construct();
            _sut.Update(new GameTime());

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            Assert.IsTrue(foundObjects.Contains(_mockClouds[0]));
        }

        [TestMethod]
        public void SearchComparerWithObjectWithinRangeShouldReturnExpectedObjects()
        {
            // Expecting _mockClouds[0] and _mockClouds[1]
            _mockClouds[0].Bounds.Returns(new RectangleF(0, 0, 200, 200));
            _mockClouds[1].Bounds.Returns(new RectangleF(100, 100, 200, 200));

            List<ICloud> foundObjects = new List<ICloud>();
            _mockObjectSearcher = Substitute.For<IObjectSearcher>();
            _mockObjectSearcher.When(x => x.GetMatchingObjects<ICloud>(Arg.Any<ICloud[]>(), Arg.Any<ICloud>(), Arg.Any<ObjectSearchComparer<ICloud>>())).Do(
                x =>
                {
                    ICloud[] clouds = x[0] as ICloud[];
                    ICloud target = x[1] as ICloud;
                    ObjectSearchComparer<ICloud> testMethod = x[2] as ObjectSearchComparer<ICloud>;
                    foreach (ICloud cloud in clouds)
                    {
                        if (testMethod(target, cloud))
                            foundObjects.Add(cloud);
                    }
                }
            );

            Construct();
            _sut.Update(new GameTime());

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            Assert.IsTrue(foundObjects.SequenceEqual<ICloud>(new ICloud[] { _mockClouds[0], _mockClouds[1] }));
        }

        [TestMethod]
        public void SearchComparerWithObjectOutOfRangeShouldReturnExpectedObjects()
        {
            // Expecting _mockClouds[0]
            _mockClouds[0].Bounds.Returns(new RectangleF(0, 0, 200, 200));
            _mockClouds[1].Bounds.Returns(new RectangleF(400, 400, 200, 200));

            List<ICloud> foundObjects = new List<ICloud>();
            _mockObjectSearcher = Substitute.For<IObjectSearcher>();
            _mockObjectSearcher.When(x => x.GetMatchingObjects<ICloud>(Arg.Any<ICloud[]>(), Arg.Any<ICloud>(), Arg.Any<ObjectSearchComparer<ICloud>>())).Do(
                x =>
                {
                    ICloud[] clouds = x[0] as ICloud[];
                    ICloud target = x[1] as ICloud;
                    ObjectSearchComparer<ICloud> testMethod = x[2] as ObjectSearchComparer<ICloud>;
                    foreach (ICloud cloud in clouds)
                    {
                        if (testMethod(target, cloud))
                            foundObjects.Add(cloud);
                    }
                }
            );

            Construct();
            _sut.Update(new GameTime());

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            Assert.IsTrue(foundObjects.SequenceEqual<ICloud>(new ICloud[] { _mockClouds[0] }));
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldClearFlashTimer()
        {
            // Update to set next flash time, reset, next update should create a flash timer.

            Construct();
            _sut.Update(new GameTime());

            _sut.Reset();
            _sut.Update(new GameTime());

            _mockRandom.Received(2).Next(2, 3);
        }

        [TestMethod]
        public void ResetShouldClearExistingIntensityControllers()
        {
            // Update to set next flash time, update to create an intensity controller, reset, next update should not call update on intensity controller
            _mockRandom.Next(2, 3).Returns(2);
            _mockRandom.Next(0, 2).Returns(0);
            Construct();
            _sut.Update(new GameTime());
            _mockRandom.ClearReceivedCalls();

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockControllerFactory.CreateCloudIntensityAnimation(_mockRandom).Returns(mockAnimation);

            ICloudIntensityController mockIntensityController = Substitute.For<ICloudIntensityController>();
            mockIntensityController.IsComplete.Returns(false);
            _mockControllerFactory.CreateCloudIntensityController(_mockClouds[0], mockAnimation).Returns(mockIntensityController);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            _sut.Reset();
            _sut.Update(new GameTime());

            mockIntensityController.Received(0).Update(Arg.Any<GameTime>());
        }
        #endregion

        #region IsFlashing Tests
        [TestMethod]
        public void IsFlashingForCloudWithoutIntensityControllerShouldReturnFalse()
        {
            Construct();

            Assert.IsFalse(_sut.IsFlashing(_mockClouds[0]));
        }

        [TestMethod]
        public void IsFlashingForCloudWithIntensityControllerShouldReturnTrue()
        {
            _mockRandom.Next(2, 3).Returns(2);
            _mockRandom.Next(0, 2).Returns(0);
            Construct();
            _sut.Update(new GameTime());
            _mockRandom.ClearReceivedCalls();

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockControllerFactory.CreateCloudIntensityAnimation(_mockRandom).Returns(mockAnimation);

            ICloudIntensityController mockIntensityController = Substitute.For<ICloudIntensityController>();
            mockIntensityController.IsComplete.Returns(false);
            _mockControllerFactory.CreateCloudIntensityController(_mockClouds[0], mockAnimation).Returns(mockIntensityController);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            Assert.IsTrue(_sut.IsFlashing(_mockClouds[0]));
        }
        #endregion
    }
}
