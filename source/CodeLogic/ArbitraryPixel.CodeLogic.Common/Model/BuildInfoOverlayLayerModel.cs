using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface IBuildInfoOverlayLayerModel
    {
        Vector2 TextAnchor { get; set; }
        List<ITextObject> TextObjects { get; }
        SizeF TextSize { get; }
    }

    public class BuildInfoOverlayLayerModel : IBuildInfoOverlayLayerModel
    {
        private Vector2 _textAnchor = Vector2.Zero;

        public List<ITextObject> TextObjects { get; } = new List<ITextObject>();

        public Vector2 TextAnchor
        {
            get { return _textAnchor; }
            set
            {
                Vector2 offset = value - _textAnchor;

                _textAnchor = value;

                foreach (ITextObject textObject in this.TextObjects)
                    textObject.Location += offset;
            }
        }

        public SizeF TextSize
        {
            get
            {
                SizeF blockSize = SizeF.Empty;
                foreach (ITextObject textObject in this.TextObjects)
                {
                    SizeF textSize = textObject.TextDefinition.Font.MeasureString(textObject.TextDefinition.Text);
                    blockSize.Width = Math.Max(blockSize.Width, textSize.Width);
                    blockSize.Height += textSize.Height;
                }

                return blockSize;
            }
        }

        public BuildInfoOverlayLayerModel(IBuildInfoStore buildInfoStore, ITextObjectBuilder textBuilder)
        {
            if (buildInfoStore == null || textBuilder == null)
                throw new ArgumentNullException();

            StringBuilder overlayText = new StringBuilder();
            overlayText.Append("{Alignment:Centre}{C:255, 255, 255, 64}");
            overlayText.AppendLine(buildInfoStore.AssemblyTitle + $" ({buildInfoStore.Platform})");
            overlayText.AppendLine(buildInfoStore.Version + " - " + buildInfoStore.Date);
            overlayText.AppendLine("DO NOT DISTRIBUTE");

            this.TextObjects.AddRange(textBuilder.Build(overlayText.ToString(), new RectangleF(Vector2.Zero, new SizeF(1, 1))));

            // We centred on a 1 pixel boundary, so now find out how wide the text is and shift all objects over.
            // This puts our text in alignment with the default value of TextAnchor (0, 0).
            Vector2 offset = new Vector2(this.TextSize.Width / 2f, 0);
            foreach (ITextObject textObject in this.TextObjects)
                textObject.Location += offset;
        }
    }
}
