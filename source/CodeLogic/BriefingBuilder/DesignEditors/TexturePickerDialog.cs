using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using BriefingBuilder.DrawingImplementations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BriefingBuilder.DesignEditors
{
    public partial class TexturePickerDialog : Form
    {
        private enum PreviewBackground
        {
            Normal,
            ContrastMagenta,
            ContrastGreen,
            ContrastBlue,
        }

        public string TextureAsset { get; set; }

        public TexturePickerDialog()
        {
            InitializeComponent();

            _lbAvailableTextures.Items.Clear();

            IAssetBank assetBank = EditorDesignerHost.Instance.Components.GetComponent<IAssetBank>();
            List<ITexture2D> textures = assetBank.GetAllAssets<ITexture2D>().ToList();
            textures.Sort(
                (lhs, rhs) =>
                {
                    var lhsTexture = lhs as Win32Texture2D;
                    var rhsTexture = rhs as Win32Texture2D;

                    return lhsTexture.Texture2DAsset.CompareTo(rhsTexture.Texture2DAsset);
                }
            );

            foreach (ITexture2D texture in textures)
                _lbAvailableTextures.Items.Add(((Win32Texture2D)texture).Texture2DAsset);
            _lbAvailableTextures.SelectedItem = _lbAvailableTextures.Items[0];

            _cbPreviewBackground.Items.Clear();
            foreach (string option in Enum.GetNames(typeof(PreviewBackground)))
                _cbPreviewBackground.Items.Add(option);
            _cbPreviewBackground.SelectedItem = _cbPreviewBackground.Items[0];
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            _lbAvailableTextures.SelectedItem = this.TextureAsset;
        }

        private void UpdatePreviewForAsset(string assetName)
        {
            IAssetBank assetBank = EditorDesignerHost.Instance.Components.GetComponent<IAssetBank>();
            ITexture2D texture = assetBank.Get<ITexture2D>(assetName);

            _pbPreview.Image = texture.GetWrappedObject<Bitmap>();
        }

        #region Event Handlers
        private void Handle_ListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            string assetName = _lbAvailableTextures.SelectedItem.ToString();
            UpdatePreviewForAsset(assetName);
        }

        private void Handle_ComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            PreviewBackground option = (PreviewBackground)Enum.Parse(typeof(PreviewBackground), _cbPreviewBackground.SelectedItem.ToString());

            switch (option)
            {
                case PreviewBackground.Normal:
                    _pbPreview.BackColor = SystemColors.Control;
                    break;
                case PreviewBackground.ContrastMagenta:
                    _pbPreview.BackColor = Color.Magenta;
                    break;
                case PreviewBackground.ContrastGreen:
                    _pbPreview.BackColor = Color.MediumSeaGreen;
                    break;
                case PreviewBackground.ContrastBlue:
                    _pbPreview.BackColor = Color.CornflowerBlue;
                    break;
            }
        }

        private void Handle_OkButtonClick(object sender, EventArgs e)
        {
            this.TextureAsset = _lbAvailableTextures.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion
    }
}
