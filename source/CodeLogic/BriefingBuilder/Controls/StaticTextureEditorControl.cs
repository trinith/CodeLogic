using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.UI;
using BriefingBuilder.CreationParameters;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BriefingBuilder.Controls
{
    [Designer(typeof(StaticTextureEditorControlDesigner))]
    public class StaticTextureEditorControl : EditorControlBase<IStaticTexture>
    {
        private ITexture2D _texture = null;
        private string _textureAsset = "";
        private bool _maintainAspectRation = false;

        #region Public Properties
        public string TextureAsset
        {
            get { return _textureAsset; }
            set
            {
                if (value != _textureAsset)
                {
                    _textureAsset = value;
                    GetTextureFromAssetName();
                }
            }
        }

        public bool MaintainAspectRatio
        {
            get { return _maintainAspectRation; }
            set
            {
                if (value != _maintainAspectRation)
                {
                    _maintainAspectRation = value;
                    this.Invalidate();
                }
            }
        }
        #endregion

        #region Constructors
        public StaticTextureEditorControl()
            : base()
        {
            this.DoubleBuffered = true;
            GetTextureFromAssetName();
        }

        public StaticTextureEditorControl(IDesignerHost host)
            : base(host)
        {
            this.DoubleBuffered = true;
            GetTextureFromAssetName();
        }
        #endregion

        #region Private Methods
        private void GetTextureFromAssetName()
        {
            _texture = null;

            if (this.Host != null && !string.IsNullOrEmpty(_textureAsset))
            {
                if (!this.Host.Components.ContainsComponent<IAssetBank>())
                    throw new Exception("Could not look up asset as an IAssetBank component is not present in this control's host.");

                IAssetBank assetBank = this.Host.Components.GetComponent<IAssetBank>();
                if (assetBank.Exists<ITexture2D>(_textureAsset))
                    _texture = assetBank.Get<ITexture2D>(_textureAsset);
            }

            this.Invalidate();
        }

        private RectangleF GetDrawableArea(bool adjustForBounds = false)
        {
            RectangleF drawableArea = this.ClientRectangle;

            if (_maintainAspectRation)
            {
                float aspectRatio = _texture.Width / (float)_texture.Height;
                float boundsAspectRatio = drawableArea.Width / drawableArea.Height;

                if (boundsAspectRatio > aspectRatio)
                    drawableArea.Width = (int)(this.Size.Height * aspectRatio);
                else
                    drawableArea.Height = (int)(this.Size.Width / aspectRatio);

                drawableArea.Location = new PointF(
                    (int)(this.ClientRectangle.Width / 2f - drawableArea.Width / 2f),
                    (int)(this.ClientRectangle.Height / 2f - drawableArea.Height / 2f)
                );

            }

            if (adjustForBounds)
                drawableArea.Location = new PointF(this.Bounds.X + drawableArea.X, this.Bounds.Y + drawableArea.Y);

            return drawableArea;
        }
        #endregion

        #region Public Methods
        public void ResetSizeToTextureDimensions()
        {
            if (_texture == null)
                throw new Exception("A valid texture has not been set, cannot set the size to the texture dimensions.");

            this.Size = new Size(_texture.Width, _texture.Height);
            this.Invalidate();
        }

        public void SetWidthForTextureAspectRatio()
        {
            if (_texture == null)
                throw new Exception("A valid texture has not been set, cannot set the width.");

            double aspectRatio = _texture.Width / (double)_texture.Height;

            this.Size = new Size((int)(aspectRatio * this.Size.Height), this.Size.Height);
            this.Invalidate();
        }

        public void SetHeightForTextureAspectRatio()
        {
            if (_texture == null)
                throw new Exception("A valid texture has not been set, cannot set the width.");

            double aspectRatio = _texture.Width / (double)_texture.Height;

            this.Size = new Size(this.Size.Width, (int)(this.Size.Width / aspectRatio));
            this.Invalidate();
        }
        #endregion

        #region EditorControlBase Implementation
        protected override IEntityCreationParameter[] CreateParameters => new IEntityCreationParameter[]
        {
            new RectangleFCreationParameter(GetDrawableArea(true)),
            (_texture == null)
                ? throw new ArgumentNullException("No texture has been set for this object. Cannot generate code.")
                : new Texture2DCreationParameter(_texture),
        };

        protected override void UpdateForNewHost(IDesignerHost host)
        {
            GetTextureFromAssetName();
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            if (_texture == null)
            {
                g.DrawLine(Pens.Red, this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Right, this.ClientRectangle.Bottom);
                g.DrawLine(Pens.Red, this.ClientRectangle.Right, this.ClientRectangle.Top, this.ClientRectangle.Left, this.ClientRectangle.Bottom);
                g.DrawRectangle(Pens.Red, this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);

                string text = "TextureAsset not found.";
                SizeF textSize = TextRenderer.MeasureText(text, this.Font);
                PointF textLocation = new PointF(this.ClientRectangle.Width / 2f - textSize.Width / 2f, this.ClientRectangle.Height / 2f - textSize.Height / 2f);
                g.FillRectangle(Brushes.Black, textLocation.X, textLocation.Y, textSize.Width, textSize.Height);
                g.DrawRectangle(Pens.Red, textLocation.X, textLocation.Y, textSize.Width, textSize.Height);
                TextRenderer.DrawText(g, text, this.Font, new Point((int)textLocation.X, (int)textLocation.Y), Color.White);
            }
            else
            {
                Bitmap texture = _texture.GetWrappedObject<Bitmap>();
                g.DrawImage(texture, GetDrawableArea());
            }
        }
        #endregion
    }
}
