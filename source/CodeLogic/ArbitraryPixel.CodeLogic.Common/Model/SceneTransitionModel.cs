using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface ISceneTransitionModel
    {
        IRenderTarget2D StartTarget { get; }
        IRenderTarget2D EndTarget { get; }

        IScene StartScene { get; }
        IScene EndScene { get; }

        bool TransitionComplete { get; }

        void Update(GameTime gameTime);
    }

    public class SceneTransitionModel : ISceneTransitionModel
    {
        public SceneTransitionModel(IScene startScene, IScene endScene, IRenderTarget2D startTarget, IRenderTarget2D endTarget)
        {
            this.StartScene = startScene ?? throw new ArgumentNullException();
            this.EndScene = endScene ?? throw new ArgumentNullException();
            this.StartTarget = startTarget ?? throw new ArgumentNullException();
            this.EndTarget = endTarget ?? throw new ArgumentNullException();
        }

        #region ISceneTransitionModel Implementation
        public IRenderTarget2D StartTarget { get; }
        public IRenderTarget2D EndTarget { get; }

        public IScene StartScene { get; }
        public IScene EndScene { get; }

        public bool TransitionComplete { get; protected set; } = false;

        public virtual void Update(GameTime gameTime) { }
        #endregion
    }
}
