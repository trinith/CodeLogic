using ArbitraryPixel.Platform2D.UI;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public interface IMenuView : IButton
    {
        IMenuItem ViewOf { get; set; }
    }
}
