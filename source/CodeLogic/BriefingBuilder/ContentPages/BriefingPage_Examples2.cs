namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_Examples2 : DesignerHostBase
    {
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl3;
        private Controls.StaticTextureEditorControl staticTextureEditorControl2;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl2;
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl4;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;

        public BriefingPage_Examples2()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.formattedTextLabelEditorControl3 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl2 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl2 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl4 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.SuspendLayout();
            // 
            // formattedTextLabelEditorControl3
            // 
            this.formattedTextLabelEditorControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl3.Location = new System.Drawing.Point(39, 275);
            this.formattedTextLabelEditorControl3.Name = "formattedTextLabelEditorControl3";
            this.formattedTextLabelEditorControl3.Size = new System.Drawing.Size(591, 115);
            this.formattedTextLabelEditorControl3.TabIndex = 10;
            this.formattedTextLabelEditorControl3.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "In this example, we get two {C:Green}Equal {C:White}results for the blue",
        "and green, which are in the correct spot, then two {C:Orange}Partial",
        "{C:White}results because there is a red and a green in the code,",
        "but in our attempt they are in the wrong spot."};
            // 
            // staticTextureEditorControl2
            // 
            this.staticTextureEditorControl2.Location = new System.Drawing.Point(39, 237);
            this.staticTextureEditorControl2.MaintainAspectRatio = true;
            this.staticTextureEditorControl2.Name = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.Size = new System.Drawing.Size(187, 32);
            this.staticTextureEditorControl2.TabIndex = 9;
            this.staticTextureEditorControl2.Text = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.TextureAsset = "Example_Results4";
            // 
            // formattedTextLabelEditorControl2
            // 
            this.formattedTextLabelEditorControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl2.Location = new System.Drawing.Point(39, 116);
            this.formattedTextLabelEditorControl2.Name = "formattedTextLabelEditorControl2";
            this.formattedTextLabelEditorControl2.Size = new System.Drawing.Size(591, 115);
            this.formattedTextLabelEditorControl2.TabIndex = 8;
            this.formattedTextLabelEditorControl2.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "Here, we get an {C:Green}Equal {C:White}for the blue that\'s in the correct",
        "spot, then a {C:Orange}Partial {C:White}for the blue that\'s in an incorrect",
        "spot. Note that the results order does not go with the",
        "attempt order."};
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(39, 78);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(187, 32);
            this.staticTextureEditorControl1.TabIndex = 7;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "Example_Results3";
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(630, 72);
            this.formattedTextLabelEditorControl1.TabIndex = 6;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "Building on the previous examples, let\'s look at two more that",
        "explain how {C:Green}Equal {C:White}results work."};
            // 
            // formattedTextLabelEditorControl4
            // 
            this.formattedTextLabelEditorControl4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl4.Location = new System.Drawing.Point(0, 396);
            this.formattedTextLabelEditorControl4.Name = "formattedTextLabelEditorControl4";
            this.formattedTextLabelEditorControl4.Size = new System.Drawing.Size(630, 93);
            this.formattedTextLabelEditorControl4.TabIndex = 11;
            this.formattedTextLabelEditorControl4.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "Hopefully that gives you a good idea of how things work, but",
        "there\'s nothing like just diving right and giving things a try",
        "to help you learn!"};
            // 
            // BriefingPage_Examples2
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl4);
            this.Controls.Add(this.formattedTextLabelEditorControl3);
            this.Controls.Add(this.staticTextureEditorControl2);
            this.Controls.Add(this.formattedTextLabelEditorControl2);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Name = "BriefingPage_Examples2";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
