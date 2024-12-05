namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_SubmitButton : DesignerHostBase
    {
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl2;

        public BriefingPage_SubmitButton()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl2 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.SuspendLayout();
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(630, 69);
            this.formattedTextLabelEditorControl1.TabIndex = 0;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Alignment:Left}{Font:BriefingNormalFont}{C:White}",
        "Once you feel you have a code that you want to try, you can",
        "use the Submit Button on the right-hand side of the",
        "interface. Tap and hold the button to submit the code."};
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(0, 75);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(630, 200);
            this.staticTextureEditorControl1.TabIndex = 1;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "SubmitButton";
            // 
            // formattedTextLabelEditorControl2
            // 
            this.formattedTextLabelEditorControl2.Location = new System.Drawing.Point(0, 281);
            this.formattedTextLabelEditorControl2.Name = "formattedTextLabelEditorControl2";
            this.formattedTextLabelEditorControl2.Size = new System.Drawing.Size(630, 216);
            this.formattedTextLabelEditorControl2.TabIndex = 2;
            this.formattedTextLabelEditorControl2.TextLines = new string[] {
        "{Alignment:Left}{Font:BriefingNormalFont}{C:White}",
        "As you hold the button, its graphic will change to indicate",
        "how far along it is. When submitting completes, a window will",
        "appear describing the submit process. If you want to speed",
        "things up, simply tap the window and it will finish, then tap",
        "again to close the window.",
        "",
        "Releasing the button before the animation completes will not",
        "submit the sequence, giving you an opportunity to change your",
        "guess."};
            // 
            // BriefingPage_SubmitButton
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl2);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Name = "BriefingPage_SubmitButton";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
