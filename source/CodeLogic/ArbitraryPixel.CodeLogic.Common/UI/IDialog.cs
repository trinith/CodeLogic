using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public enum DialogResult
    {
        Ok,
        Cancel,
    }

    public class DialogClosedEventArgs : EventArgs
    {
        public DialogResult Result { get; private set; }
        public DialogClosedEventArgs(DialogResult result)
        {
            this.Result = result;
        }
    }

    public interface IDialog : IGameEntity
    {
        Color BackgroundColour { get; set; }
        Color BorderColour { get; set; }
        RectangleF ClientRectangle { get; }

        bool IsOpen { get; }
        event EventHandler<DialogClosedEventArgs> DialogClosed;

        void Show();
        void Close();

        void AddContentLayer(ILayer layer);
    }
}
