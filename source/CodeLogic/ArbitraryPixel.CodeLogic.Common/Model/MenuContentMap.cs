using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.UI;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface IMenuContentMap
    {
        void Clear();
        void RegisterContentLayer(IMenuItem menuItem, IMenuContentLayer contentLayer);
        bool HasMappedLayer(IMenuItem menuItem);
        IMenuContentLayer GetLayer(IMenuItem menuItem);
    }

    public class MenuContentMap : IMenuContentMap
    {
        private Dictionary<IMenuItem, IMenuContentLayer> _contentMap = new Dictionary<IMenuItem, IMenuContentLayer>();

        public MenuContentMap()
        {
        }

        public void Clear()
        {
            _contentMap.Clear();
        }

        public void RegisterContentLayer(IMenuItem menuItem, IMenuContentLayer contentLayer)
        {
            _contentMap.Add(menuItem, contentLayer);
        }

        public bool HasMappedLayer(IMenuItem menuItem)
        {
            return _contentMap.ContainsKey(menuItem);
        }

        public IMenuContentLayer GetLayer(IMenuItem menuItem)
        {
            return _contentMap[menuItem];
        }
    }
}
