using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public enum PanSceneTransitionMode
    {
        PanLeft,
        PanRight
    }

    public interface IPanSceneTransitionModel : ISceneTransitionModel
    {
        Vector2 StartAnchor { get; set; }
        Vector2 EndAnchor { get; set; }

        Vector2 TransitionVelocity { get; }
    }

    public class PanSceneTransitionModel : SceneTransitionModel, IPanSceneTransitionModel
    {
        private PanSceneTransitionMode _transitionMode = PanSceneTransitionMode.PanLeft;

        public PanSceneTransitionModel(IScene startScene, IScene endScene, IRenderTarget2D startTarget, IRenderTarget2D endTarget, PanSceneTransitionMode transitionMode, double transitionTime, SizeF screenSize)
            : base(startScene, endScene, startTarget, endTarget)
        {
            _transitionMode = transitionMode;

            int sign = (_transitionMode == PanSceneTransitionMode.PanLeft) ? -1 : 1;
            this.TransitionVelocity = new Vector2((float)(sign * (screenSize.Width / transitionTime)), 0);
            this.StartAnchor = new Vector2(0, 0);
            this.EndAnchor = new Vector2(-sign * screenSize.Width, 0);
        }

        #region IPanSceneTransitionModel Implementation
        public Vector2 StartAnchor { get; set; }
        public Vector2 EndAnchor { get; set; }

        public Vector2 TransitionVelocity { get; private set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.TransitionVelocity != Vector2.Zero)
            {
                this.StartAnchor += this.TransitionVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.EndAnchor += this.TransitionVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                bool transitionFinished = false;
                if (_transitionMode == PanSceneTransitionMode.PanLeft)
                    transitionFinished = (this.EndAnchor.X <= 0);
                else
                    transitionFinished = (this.EndAnchor.X >= 0);

                if (transitionFinished)
                {
                    this.TransitionVelocity = Vector2.Zero;
                    this.EndAnchor = Vector2.Zero;
                    this.TransitionComplete = true;
                }
            }
        }
        #endregion
    }
}
