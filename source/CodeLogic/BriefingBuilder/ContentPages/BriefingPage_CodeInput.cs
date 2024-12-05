namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_CodeInput : DesignerHostBase
    {
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;
        private Controls.StaticTextureEditorControl staticTextureEditorControl2;

        public BriefingPage_CodeInput()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.staticTextureEditorControl2 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.SuspendLayout();
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(3, 0);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(624, 81);
            this.staticTextureEditorControl1.TabIndex = 0;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "CodeInput";
            // 
            // staticTextureEditorControl2
            // 
            this.staticTextureEditorControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staticTextureEditorControl2.Location = new System.Drawing.Point(3, 270);
            this.staticTextureEditorControl2.MaintainAspectRatio = true;
            this.staticTextureEditorControl2.Name = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.Size = new System.Drawing.Size(624, 127);
            this.staticTextureEditorControl2.TabIndex = 1;
            this.staticTextureEditorControl2.Text = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.TextureAsset = "CodeInput_Example";
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 87);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(630, 177);
            this.formattedTextLabelEditorControl1.TabIndex = 2;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Alignment:Left}{Font:BriefingNormalFont}{C:White}",
        "Using this part of CodeLogic allows you to change the code you",
        "want to input. To do so, simply tap on the desired code index",
        "and the selection wheel pops out. Next, tap the colour you",
        "want to change to and the code index will be updated.",
        "",
        "Alternatively, you can tap on the index and drag in the",
        "direction of the colour you want. When you release, the",
        "highlighted colour will be selected."};
            // 
            // BriefingPage_CodeInput
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Controls.Add(this.staticTextureEditorControl2);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Name = "BriefingPage_CodeInput";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
