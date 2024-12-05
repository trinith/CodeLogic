using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using BriefingBuilder.CreationParameters;
using BriefingBuilder.DrawingImplementations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BriefingBuilder.Controls
{
    using APDrawing = ArbitraryPixel.Common.Drawing;

    [Designer(typeof(FormattedTextLabelEditorControlDesigner))]
    public class FormattedTextLabelEditorControl : EditorControlBase<IFormattedTextLabel>
    {
        private ITextObjectBuilder _builder;
        private string _rawText = "";
        private List<ITextObject> _textObjects = new List<ITextObject>();

        #region Properties
        public new string Text
        {
            get { return _rawText; }
            set
            {
                if (value != _rawText)
                {
                    _rawText = value;
                    UpdateTextObjects();
                }
            }
        }

        public string[] TextLines
        {
            get { return _rawText.Split(new char[] { '\n' }); }
            set
            {
                StringBuilder sb = new StringBuilder();
                value.ToList().ForEach(x => sb.AppendLine(x));

                string text = sb.ToString().Replace("\r", "").TrimEnd(new char[] { '\n' });

                this.Text = text;
            }
        }
        #endregion

        #region Constructor(s)
        public FormattedTextLabelEditorControl()
            : base()
        {
        }

        public FormattedTextLabelEditorControl(IDesignerHost host)
            : base(host)
        {
        }
        #endregion

        #region EditorControlBase Implementation
        protected override IEntityCreationParameter[] CreateParameters => new IEntityCreationParameter[]
        {
            new RectangleFCreationParameter(new RectangleF(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height)),
            new StringCreationParameter(this.Text.Replace("\n", "\\n")),
        };

        protected override void UpdateForNewHost(IDesignerHost host)
        {
            _builder = host.Components.GetComponent<ITextObjectBuilder>();

            UpdateTextObjects();
        }
        #endregion

        #region Private Methods
        private void UpdateTextObjects()
        {
            if (_builder != null)
            {
                Rectangle bounds = this.ClientRectangle;
                _textObjects.Clear();
                _textObjects.AddRange(_builder.Build(_rawText, new APDrawing.RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height)));
                this.Invalidate();
            }
        }
        #endregion

        #region Override Methods
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            UpdateTextObjects();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Font defaultFont = this.Font;

            foreach (ITextObject textObject in _textObjects)
            {
                Win32SpriteFont spriteFont = textObject.TextDefinition.Font as Win32SpriteFont;
                Font font = (spriteFont != null) ? spriteFont.WrappedObject : defaultFont;

                TextRenderer.DrawText(
                    g,
                    textObject.TextDefinition.Text,
                    font,
                    new Point((int)textObject.Location.X, (int)textObject.Location.Y),
                    Color.FromArgb(textObject.TextDefinition.Colour.R, textObject.TextDefinition.Colour.G, textObject.TextDefinition.Colour.B)
                );
            }
        }
        #endregion
    }
}
