namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_Examples1 : DesignerHostBase
    {
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl2;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl3;
        private Controls.StaticTextureEditorControl staticTextureEditorControl2;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;

        public BriefingPage_Examples1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl2 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl3 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl2 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.SuspendLayout();
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(630, 107);
            this.formattedTextLabelEditorControl1.TabIndex = 1;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "Using our example code of ({C:Red}Red{C:White}, {C:Blue}Blue{C:White}, {C:Blue}Bl" +
            "ue{C:White}, {C:Green}Green{C:White}), lets look",
        "at some sample guesses and the results they generate.",
        "",
        "To start, lets explore how the {C:Orange}Partial {C:White}results work."};
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(39, 113);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(187, 32);
            this.staticTextureEditorControl1.TabIndex = 2;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "Example_Results1";
            // 
            // formattedTextLabelEditorControl2
            // 
            this.formattedTextLabelEditorControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl2.Location = new System.Drawing.Point(39, 151);
            this.formattedTextLabelEditorControl2.Name = "formattedTextLabelEditorControl2";
            this.formattedTextLabelEditorControl2.Size = new System.Drawing.Size(591, 91);
            this.formattedTextLabelEditorControl2.TabIndex = 3;
            this.formattedTextLabelEditorControl2.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "In this example, we get one {C:Orange}Partial {C:White}match because we have",
        "a green in the code, but in our attempt it\'s in the wrong",
        "place."};
            // 
            // formattedTextLabelEditorControl3
            // 
            this.formattedTextLabelEditorControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl3.Location = new System.Drawing.Point(39, 286);
            this.formattedTextLabelEditorControl3.Name = "formattedTextLabelEditorControl3";
            this.formattedTextLabelEditorControl3.Size = new System.Drawing.Size(591, 91);
            this.formattedTextLabelEditorControl3.TabIndex = 5;
            this.formattedTextLabelEditorControl3.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "Again, we get one {C:Orange}Partial {C:White}because we have a green in the",
        "wrong place. Since the code only has one green and the",
        "attempt has two, we get a {C:Red}Not Equal {C:White}for the second green."};
            // 
            // staticTextureEditorControl2
            // 
            this.staticTextureEditorControl2.Location = new System.Drawing.Point(39, 248);
            this.staticTextureEditorControl2.MaintainAspectRatio = true;
            this.staticTextureEditorControl2.Name = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.Size = new System.Drawing.Size(187, 32);
            this.staticTextureEditorControl2.TabIndex = 4;
            this.staticTextureEditorControl2.Text = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.TextureAsset = "Example_Results2";
            // 
            // BriefingPage_Examples1
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl3);
            this.Controls.Add(this.staticTextureEditorControl2);
            this.Controls.Add(this.formattedTextLabelEditorControl2);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Name = "BriefingPage_Examples1";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
