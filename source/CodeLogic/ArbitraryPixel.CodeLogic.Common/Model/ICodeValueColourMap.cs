using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface ICodeValueColourMap
    {
        Color GetColour(CodeValue value);
    }
}
