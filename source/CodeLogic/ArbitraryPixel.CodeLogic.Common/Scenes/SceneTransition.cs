using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class SceneTransition : SceneBase
    {
        public ISceneTransitionModel Model { get; private set; }
        private bool _capturedScenes = false;

        public SceneTransition(IEngine host, ISceneTransitionModel model)
            : base(host)
        {
            this.Model = model ?? throw new ArgumentNullException();
        }

        public TModelType GetModel<TModelType>() where TModelType : ISceneTransitionModel
        {
            return (TModelType)this.Model;
        }

        protected override void OnPreDraw(GameTime gameTime)
        {
            base.OnPreDraw(gameTime);

            if (_capturedScenes == false)
            {
                this.Model.EndScene.Reset();

                this.Model.StartScene.Draw(gameTime, this.Model.StartTarget);
                this.Model.EndScene.Draw(gameTime, this.Model.EndTarget);
                _capturedScenes = true;
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            this.Model.Update(gameTime);

            if (this.Model.TransitionComplete)
            {
                this.NextScene = this.Model.EndScene;
                this.SceneComplete = true;
            }
        }
    }
}
