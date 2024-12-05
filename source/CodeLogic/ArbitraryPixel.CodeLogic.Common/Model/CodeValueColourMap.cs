using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public class CodeValueColourMap : ICodeValueColourMap
    {
        private Dictionary<CodeValue, Color> _map = new Dictionary<CodeValue, Color>();

        public CodeValueColourMap()
        {
            PopulateMap();
        }

        private void PopulateMap()
        {
            // Could do reflection here but we'll just take the easy route :D
            _map.Add(CodeValue.Red, Color.Red);
            _map.Add(CodeValue.Green, Color.Green);
            _map.Add(CodeValue.Blue, Color.Blue);
            _map.Add(CodeValue.Yellow, Color.Yellow);
            _map.Add(CodeValue.Orange, Color.Orange);
        }

        public Color GetColour(CodeValue value)
        {
            return _map[value];
        }
    }
}
