using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Graphics;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace BriefingBuilder.DrawingImplementations
{
    using APDrawing = ArbitraryPixel.Common.Drawing;

    public class Win32SpriteFont : WrapperBase<Font>, ISpriteFont
    {
        #region Static Methods
        public static Win32SpriteFont FromFile(string contentPath, string assetName)
        {
            string file = Path.Combine(contentPath, assetName);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(File.ReadAllText(file));
            var fontNameNode = xmlDoc.SelectSingleNode("//XnaContent/Asset/FontName");
            var sizeNode = xmlDoc.SelectSingleNode("//XnaContent/Asset/Size");

            if (fontNameNode == null || sizeNode == null)
                throw new ArgumentException("The specified file does not contain a FontName and/or Size node at the expected XML path.", "file");

            Font internalFont = new Font(fontNameNode.InnerXml, float.Parse(sizeNode.InnerXml));

            return new Win32SpriteFont(internalFont) { SpriteFontAsset = Path.GetFileNameWithoutExtension(assetName) };
        }
        #endregion

        //public Font InternalFont => _internalFont;
        public string SpriteFontAsset { get; set; }

        public Win32SpriteFont(Font internalFont)
            : base(internalFont)
        {
        }

        #region ISpriteFont Implementation
        public int LineSpacing => (int)this.WrappedObject.Size;

        public APDrawing.SizeF MeasureString(string text)
        {
            SizeF textSize = TextRenderer.MeasureText(text, this.WrappedObject);

            return new APDrawing.SizeF(textSize.Width, textSize.Height);
        }
        #endregion
    }
}
