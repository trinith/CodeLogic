using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public enum FadeSceneTransitionMode
    {
        Out,
        In,
        OutIn
    }

    public interface IFadeSceneTransitionModel : ISceneTransitionModel
    {
        Color Background { get; set; }

        float CurrentOpacity { get; }
        float OpacityTarget { get; }
        float OpacityVelocity { get; }
        FadeSceneTransitionMode Mode { get; }

        float StartSceneOpacity { get; }
        float EndSceneOpacity { get; }
    }

    public class FadeSceneTransitionModel : SceneTransitionModel, IFadeSceneTransitionModel
    {
        public FadeSceneTransitionModel(IScene startScene, IScene endScene, IRenderTarget2D startTarget, IRenderTarget2D endTarget, FadeSceneTransitionMode mode, float transitionTime)
            : base(startScene, endScene, startTarget, endTarget)
        {
            float opacityTravelAmount = 1;

            switch (mode)
            {
                case FadeSceneTransitionMode.Out:
                    this.CurrentOpacity = -1;
                    this.OpacityTarget = 0;
                    opacityTravelAmount = 1;
                    break;
                case FadeSceneTransitionMode.In:
                    this.CurrentOpacity = 0;
                    this.OpacityTarget = 1;
                    opacityTravelAmount = 1;
                    break;
                case FadeSceneTransitionMode.OutIn:
                    this.CurrentOpacity = -1;
                    this.OpacityTarget = 1;
                    opacityTravelAmount = 2;
                    break;
            }

            this.OpacityVelocity = opacityTravelAmount / transitionTime;
        }

        #region IFadeSceneTransitionModel Implementation
        public Color Background { get; set; } = Color.Black;
        public float CurrentOpacity { get; private set; } = -1;
        public float OpacityTarget { get; private set; } = 1;
        public float OpacityVelocity { get; private set; } = 0;
        public FadeSceneTransitionMode Mode { get; private set; }

        public float StartSceneOpacity
        {
            get
            {
                return (this.CurrentOpacity <= 0f) ? Math.Abs(this.CurrentOpacity) : 0f;
            }
        }

        public float EndSceneOpacity
        {
            get
            {
                return (this.CurrentOpacity > 0f) ? Math.Abs(this.CurrentOpacity) : 0f;
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.OpacityVelocity != 0)
            {
                this.CurrentOpacity += this.OpacityVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this.CurrentOpacity >= this.OpacityTarget)
                {
                    this.CurrentOpacity = this.OpacityTarget;
                    this.OpacityVelocity = 0f;
                    this.TransitionComplete = true;
                }
            }
        }
    }
}
