using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public interface IController
    {
        void Reset();
        void Update(GameTime gameTime);
    }
}
