using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Text;
using BriefingBuilder.DrawingImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BriefingBuilder.DesignEditors
{
    public partial class FormatStringEditorDialog : Form
    {
        private const string TODO_INSERT_COLOUR = "TODO_INSERT_COLOUR";
        private const string TODO_INSERT_TPC = "TODO_INSERT_TPC";

        private ContextMenu _alignmentPicker = null;
        private ContextMenu _fontPicker = null;

        public string FormattedText { get; set; } = "";

        public FormatStringEditorDialog()
        {
            InitializeComponent();

            BuildAlignmentMenu();
            BuildFontMenu();
        }

        private void BuildAlignmentMenu()
        {
            _alignmentPicker = new ContextMenu();

            List<string> alignmentOptions = Enum.GetNames(typeof(TextLineAlignment)).ToList();
            foreach (string option in alignmentOptions)
                _alignmentPicker.MenuItems.Add(new MenuItem(option, Handle_AlignmentOptionClicked));
        }

        private void BuildFontMenu()
        {
            _fontPicker = new ContextMenu();

            IAssetBank assetBank = EditorDesignerHost.Instance.Components.GetComponent<IAssetBank>();
            ISpriteFont[] fonts = assetBank.GetAllAssets<ISpriteFont>();
            foreach (ISpriteFont font in fonts)
            {
                string assetName = ((Win32SpriteFont)font).SpriteFontAsset;
                _fontPicker.MenuItems.Add(new MenuItem(assetName, Handle_FontOptionClicked));
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            _tbFormatString.Text = this.FormattedText.Replace("\n", Environment.NewLine);
        }

        private void Handle_OkButtonClick(object sender, EventArgs e)
        {
            this.FormattedText = _tbFormatString.Text.Replace("\r", "").TrimEnd();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Handle_AlignmentOptionClicked(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                string insertString = string.Format("{{Alignment:{0}}}", item.Text);
                _tbFormatString.Paste(insertString);
            }
        }

        private void Handle_FontOptionClicked(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                string insertString = string.Format("{{Font:{0}}}", item.Text);
                _tbFormatString.Paste(insertString);
            }
        }

        private void Handle_AlignmentButtonClick(object sender, MouseEventArgs e)
        {
            _alignmentPicker.Show(_btnAlignment, e.Location);
        }

        private void Handle_FontButtonClick(object sender, MouseEventArgs e)
        {
            _fontPicker.Show(_btnFont, e.Location);
        }

        private void Handle_ColourButtonClick(object sender, EventArgs e)
        {
            _tbFormatString.SuspendLayout();

            string insertString = string.Format("{{C:{0}}}", TODO_INSERT_COLOUR);
            int selectionStart = insertString.IndexOf(':') + 1;
            _tbFormatString.Paste(insertString);

            _tbFormatString.SelectionStart += (selectionStart - insertString.Length);
            _tbFormatString.SelectionLength = TODO_INSERT_COLOUR.Length;
            _tbFormatString.Focus();

            _tbFormatString.ResumeLayout();
        }

        private void Handle_TPCButtonClick(object sender, EventArgs e)
        {
            _tbFormatString.SuspendLayout();

            string insertString = string.Format("{{TPC:{0}}}", TODO_INSERT_TPC);
            int selectionStart = insertString.IndexOf(':') + 1;
            _tbFormatString.Paste(insertString);

            _tbFormatString.SelectionStart += (selectionStart - insertString.Length);
            _tbFormatString.SelectionLength = TODO_INSERT_TPC.Length;
            _tbFormatString.Focus();

            _tbFormatString.ResumeLayout();
        }
    }
}
