using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.EntityGenerators
{
    public class CloudCoverGenerator : IEntityGenerator<ICloud>
    {
        public static class Constants
        {
            public static readonly Color ClrLow = new Color(64, 64, 64, 128);
            public static readonly Color ClrMid = new Color(92, 92, 92, 128);
            public static readonly Color ClrHigh = new Color(150, 150, 150, 150);
        }

        private Vector2 _topLeft;
        private Vector2 _topRight;

        private SizeF _screenSize;
        private ITexture2D[] _cloudTextures;
        private ISpriteBatch _spriteBatch;
        private IRandom _random;

        public CloudCoverGenerator(SizeF screenSize, ITexture2D[] cloudTextures, ISpriteBatch spriteBatch, IRandom random)
        {
            _screenSize = screenSize;
            _cloudTextures = cloudTextures ?? throw new ArgumentNullException();
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _random = random ?? throw new ArgumentNullException();

            if (_cloudTextures.Length <= 0)
                throw new ArgumentException("The list of cloud textures must contain at least one item.");

            _topLeft = new Vector2(0, _screenSize.Height * 1f / 3f);
            _topRight = new Vector2(_screenSize.Width, _screenSize.Height * 2f / 3f);
        }

        private Color GetDepthMask(int layerNum)
        {
            switch (layerNum)
            {
                case 1:
                    return Constants.ClrLow;
                case 2:
                    return Constants.ClrMid;
                default:
                case 3:
                    return Constants.ClrHigh;
            }
        }

        private float CalculateYPosition(Vector2 p0, Vector2 dir, float x)
        {
            // On the line between topLeft and topRight, where does y intersect for a given x value?
            // p = p0 + tD
            // [x y] = [x0 y0] + t[Dx Dy]
            // x = x0 + tDx => t = (x - x0) / Dx
            // y = y0 + tDy
            // ---> Solve for y using t
            // y = y0 + ((x - x0) / Dx)Dy

            // Calculate an upper y value bound
            float y = p0.Y + ((x - p0.X) / dir.X) * dir.Y;
            y = y + _random.Next(0, (int)(_screenSize.Height - _topLeft.Y));

            return y;
        }

        #region IGameEntityGenerator Implementation
        public ICloud[] GenerateEntities(IEngine host, int numEntities)
        {
            List<ICloud> entities = new List<ICloud>();

            Vector2 p0 = _topLeft;
            Vector2 dir = _topRight - _topLeft;
            float length = dir.Length();
            dir.Normalize();

            for (int i = 0; i < numEntities; i++)
            {
                ITexture2D texture = _cloudTextures[_random.Next(0, _cloudTextures.Length)];

                float x = (float)_random.Next(0, (int)_screenSize.Width + 1);
                float y = CalculateYPosition(p0, dir, x);
                Vector2 pos = new Vector2(x - texture.Width / 2f, y - texture.Height / 2f);

                ICloud newCloud = GameObjectFactory.Instance.CreateCloud(host, new RectangleF(pos, new SizeF(texture.Width, texture.Height)), _spriteBatch, texture, Color.White);
                newCloud.Depth = _random.Next(1, 4);
                newCloud.Mask = GetDepthMask(newCloud.Depth);

                entities.Add(newCloud);
            }

            entities.Sort((lhs, rhs) => lhs.Depth.CompareTo(rhs.Depth));

            return entities.ToArray();
        }

        public void RepositionEntity(ICloud entity)
        {
            Vector2 p0 = _topLeft;
            Vector2 dir = _topRight - _topLeft;
            float length = dir.Length();
            dir.Normalize();

            float x = _screenSize.Width + entity.Texture.Width;
            float y = CalculateYPosition(p0, dir, x);
            Vector2 pos = new Vector2(x - entity.Texture.Width / 2f, y - entity.Texture.Height / 2f);

            entity.Bounds = new RectangleF(pos, entity.Bounds.Size);
        }
        #endregion
    }
}
