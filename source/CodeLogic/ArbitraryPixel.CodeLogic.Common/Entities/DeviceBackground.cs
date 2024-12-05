using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface IDeviceBackground : IGameEntity
    {
        Color Colour { get; set; }
    }

    public class DeviceBackground : GameEntityBase, IDeviceBackground
    {
        private ISpriteBatch _spriteBatch;

        public Color Colour { get; set; } = Color.White;

        public DeviceBackground(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            _spriteBatch.Draw(this.Host.AssetBank.Get<ITexture2D>("DeviceBackground"), this.Bounds, this.Colour);
        }
    }
}
