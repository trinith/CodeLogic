using System;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public interface IMenuItem
    {
        float Height { get; }
        string Text { get; }
        IMenuItem Parent { get; set; }
        IMenuItem[] Items { get; }

        IMenuItem CreateChild(string title, float height);
    }
}
