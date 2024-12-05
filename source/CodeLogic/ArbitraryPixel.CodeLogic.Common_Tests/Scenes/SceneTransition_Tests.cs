using System;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class SceneTransition_Tests
    {
        #region Private Types and Such
        public interface IFakeTransitionModel : ISceneTransitionModel
        {
        }
        #endregion

        private SceneTransition _sut;
        private IEngine _mockEngine;
        private ISceneTransitionModel _mockModel;

        private IScene _mockStartScene, _mockEndScene;
        private IRenderTarget2D _mockStartTarget, _mockEndTarget;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockModel = Substitute.For<ISceneTransitionModel>();

            _mockStartScene = Substitute.For<IScene>();
            _mockEndScene = Substitute.For<IScene>();
            _mockStartTarget = Substitute.For<IRenderTarget2D>();
            _mockEndTarget = Substitute.For<IRenderTarget2D>();

            _mockModel.StartScene.Returns(_mockStartScene);
            _mockModel.EndScene.Returns(_mockEndScene);
            _mockModel.StartTarget.Returns(_mockStartTarget);
            _mockModel.EndTarget.Returns(_mockEndTarget);
        }

        private void Construct()
        {
            _sut = new SceneTransition(_mockEngine, _mockModel);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Model()
        {
            _sut = new SceneTransition(_mockEngine, null);
        }
        #endregion

        #region Model/GetModel Tests
        [TestMethod]
        public void ModelShouldReturnConstructorParameter()
        {
            Construct();

            Assert.AreSame(_mockModel, _sut.Model);
        }

        [TestMethod]
        public void GetModelWithValidTypeShouldReturnModel_TestA()
        {
            Construct();

            Assert.AreSame(_mockModel, _sut.GetModel<ISceneTransitionModel>());
        }

        [TestMethod]
        public void GetModelWithValidTypeShouldReturnModel_TestB()
        {
            _mockModel = Substitute.For<IFakeTransitionModel>();
            Construct();

            Assert.AreSame(_mockModel, _sut.GetModel<IFakeTransitionModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetModelWithInvalidTypeShouldThrowExpectedException()
        {
            Construct();

            var model = _sut.GetModel<IFakeTransitionModel>();
        }
        #endregion

        #region PreDraw Tests
        [TestMethod]
        public void PreDrawFirstTimeShouldCallDrawWithTargetOnStartScene()
        {
            Construct();

            _mockModel.StartTarget.Returns(_mockStartTarget);
            GameTime expectedGT = new GameTime();
            _sut.PreDraw(expectedGT);

            _mockModel.StartScene.Received(1).Draw(expectedGT, _mockStartTarget);
        }

        [TestMethod]
        public void PreDrawFirstTimeShouldCallDrawWithTargetOnEndScene()
        {
            Construct();

            _mockModel.EndTarget.Returns(_mockEndTarget);
            GameTime expectedGT = new GameTime();
            _sut.PreDraw(expectedGT);

            _mockModel.EndScene.Received(1).Draw(expectedGT, _mockEndTarget);
        }

        [TestMethod]
        public void PreDrawSecondTimeShouldNotCallDrawWithTargetOnStartScene()
        {
            Construct();

            _mockModel.StartTarget.Returns(_mockStartTarget);
            GameTime expectedGT = new GameTime();
            _sut.PreDraw(expectedGT);
            _mockModel.StartScene.ClearReceivedCalls();
            _sut.PreDraw(expectedGT);

            _mockModel.StartScene.Received(0).Draw(expectedGT, _mockStartTarget);
        }

        [TestMethod]
        public void PreDrawSecondTimeShouldNotCallDrawWithTargetOnEndScene()
        {
            Construct();

            _mockModel.EndTarget.Returns(_mockEndTarget);
            GameTime expectedGT = new GameTime();
            _sut.PreDraw(expectedGT);
            _mockModel.EndScene.ClearReceivedCalls();
            _sut.PreDraw(expectedGT);

            _mockModel.EndScene.Received(0).Draw(expectedGT, _mockEndTarget);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldUpdateModel()
        {
            Construct();

            GameTime expectedGT = new GameTime();
            _sut.Update(expectedGT);

            _mockModel.Received(1).Update(expectedGT);
        }

        [TestMethod]
        public void UpdateWhenModelNotCompleteShouldLeaveNextSceneNull()
        {
            Construct();

            _sut.Update(new GameTime());

            Assert.IsNull(_sut.NextScene);
        }

        [TestMethod]
        public void UpdateWhenModelNotCompleteShouldLeaveSceneCompleteFalse()
        {
            Construct();

            _sut.Update(new GameTime());

            Assert.IsFalse(_sut.SceneComplete);
        }

        [TestMethod]
        public void UpdateWhenModelCompleteShouldSetNextSceneToEndScene()
        {
            IScene mockEndScene = Substitute.For<IScene>();

            Construct();

            _mockModel.EndScene.Returns(mockEndScene);
            _mockModel.TransitionComplete.Returns(true);
            _sut.Update(new GameTime());

            Assert.AreSame(mockEndScene, _sut.NextScene);
        }

        [TestMethod]
        public void UpdateWhenModelCompleteShouldSetSceneCompleteTrue()
        {
            Construct();

            _mockModel.TransitionComplete.Returns(true);
            _sut.Update(new GameTime());

            Assert.IsTrue(_sut.SceneComplete);
        }
        #endregion
    }
}
