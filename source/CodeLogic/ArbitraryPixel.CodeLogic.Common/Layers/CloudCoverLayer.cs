using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.EntityGenerators;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    using XNA = Microsoft.Xna.Framework.Graphics;

    public interface ICloudCoverLayer : ILayer
    {
        event EventHandler<LightningFlashEventArgs> LightningFlashed;
    }

    public class CloudCoverLayer : LayerBase, ICloudCoverLayer
    {
        private List<ICloudMovementController> _movementControllers = new List<ICloudMovementController>();
        private ICloudFlashController _flashController;

        private Vector2 _topLeft;
        private Vector2 _topRight;

        public CloudCoverLayer(IEngine host, ISpriteBatch mainSpriteBatch, IEntityGenerator<ICloud> cloudGenerator, ICloudControllerFactory cloudControllerFactory, IRandom random, IObjectSearcher objectSearcher)
            : base(host, mainSpriteBatch)
        {
            if (cloudControllerFactory == null)
                throw new ArgumentNullException();

            SizeF _screenSize = (SizeF)this.Host.ScreenManager.World;
            _topLeft = new Vector2(0, _screenSize.Height * 1f / 3f);
            _topRight = new Vector2(_screenSize.Width, _screenSize.Height * 2f / 3f);

            Vector2 dir = _topLeft - _topRight;
            dir.Normalize();

            ICloud[] clouds = cloudGenerator.GenerateEntities(this.Host, 100);

            foreach (var cloud in clouds)
                _movementControllers.Add(cloudControllerFactory.CreateCloudMovementController(cloud, cloudGenerator, dir));

            _flashController = cloudControllerFactory.CreateCloudFlashController(clouds, random, cloudControllerFactory, objectSearcher);
            _flashController.LightningFlashed += Handle_FlashControllerLightningFlashed;
        }

        #region ICloudCoverLayer Implementation
        public event EventHandler<LightningFlashEventArgs> LightningFlashed;
        #endregion

        private void Handle_FlashControllerLightningFlashed(object sender, LightningFlashEventArgs e)
        {
            if (this.LightningFlashed != null)
                this.LightningFlashed(this, e);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _flashController.Update(gameTime);

            foreach (var controller in _movementControllers)
                controller.Update(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            IEffect flashEffect = this.Host.AssetBank.Get<IEffect>("LightningFlash");

            foreach (ICloudMovementController controller in _movementControllers)
            {
                this.MainSpriteBatch.Begin(XNA.SpriteSortMode.Deferred, XNA.BlendState.AlphaBlend, null, null, null, flashEffect, this.Host.ScreenManager.ScaleMatrix);
                flashEffect.SetParameter("xIntensity", controller.Cloud.Intensity);
                controller.Cloud.Draw(gameTime);
                this.MainSpriteBatch.End();
            }
        }
    }
}
