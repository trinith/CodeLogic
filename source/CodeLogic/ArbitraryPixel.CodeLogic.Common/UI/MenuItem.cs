using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class MenuItem : IMenuItem
    {
        private List<IMenuItem> _childItems = new List<IMenuItem>();

        public float Height { get; }
        public string Text { get; }
        public IMenuItem Parent { get; set; } = null;
        public IMenuItem[] Items { get { return _childItems.ToArray(); } }

        public MenuItem(string text, float height)
        {
            this.Text = text;
            this.Height = height;
        }

        public IMenuItem CreateChild(string title, float height)
        {
            IMenuItem newItem = new MenuItem(title, height);
            newItem.Parent = this;
            _childItems.Add(newItem);

            return newItem;
        }
    }
}
