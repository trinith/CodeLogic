using ArbitraryPixel.Platform2D.Text;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public interface IMenuItemView : IMenuView
    {
        TextLineAlignment LineAlignment { get; set; }
        bool IsSelected { get; set; }
    }
}
