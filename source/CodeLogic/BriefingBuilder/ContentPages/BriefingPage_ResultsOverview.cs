namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_ResultsOverview : DesignerHostBase
    {
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl2;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl3;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl4;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl5;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl6;
        private Controls.StaticTextureEditorControl staticTextureEditorControl2;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl7;
        private Controls.StaticTextureEditorControl staticTextureEditorControl3;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl8;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;

        public BriefingPage_ResultsOverview()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl2 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl3 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl4 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl5 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl6 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl2 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl7 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl3 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl8 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.SuspendLayout();
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(630, 54);
            this.formattedTextLabelEditorControl1.TabIndex = 1;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "The first thing we need to understand is how the results work.",
        "CodeLogic will give you three kinds of feedback on your guess."};
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(37, 74);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(26, 26);
            this.staticTextureEditorControl1.TabIndex = 2;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "Results_Equal";
            // 
            // formattedTextLabelEditorControl2
            // 
            this.formattedTextLabelEditorControl2.Location = new System.Drawing.Point(179, 79);
            this.formattedTextLabelEditorControl2.Name = "formattedTextLabelEditorControl2";
            this.formattedTextLabelEditorControl2.Size = new System.Drawing.Size(451, 67);
            this.formattedTextLabelEditorControl2.TabIndex = 3;
            this.formattedTextLabelEditorControl2.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "One of the digits in the attempt was the",
        "correct colour and was in the correct place."};
            // 
            // formattedTextLabelEditorControl3
            // 
            this.formattedTextLabelEditorControl3.Location = new System.Drawing.Point(69, 79);
            this.formattedTextLabelEditorControl3.Name = "formattedTextLabelEditorControl3";
            this.formattedTextLabelEditorControl3.Size = new System.Drawing.Size(104, 26);
            this.formattedTextLabelEditorControl3.TabIndex = 4;
            this.formattedTextLabelEditorControl3.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:Green}",
        "Equal"};
            // 
            // formattedTextLabelEditorControl4
            // 
            this.formattedTextLabelEditorControl4.Location = new System.Drawing.Point(69, 152);
            this.formattedTextLabelEditorControl4.Name = "formattedTextLabelEditorControl4";
            this.formattedTextLabelEditorControl4.Size = new System.Drawing.Size(104, 26);
            this.formattedTextLabelEditorControl4.TabIndex = 5;
            this.formattedTextLabelEditorControl4.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:Orange}",
        "Partial"};
            // 
            // formattedTextLabelEditorControl5
            // 
            this.formattedTextLabelEditorControl5.Location = new System.Drawing.Point(69, 248);
            this.formattedTextLabelEditorControl5.Name = "formattedTextLabelEditorControl5";
            this.formattedTextLabelEditorControl5.Size = new System.Drawing.Size(104, 26);
            this.formattedTextLabelEditorControl5.TabIndex = 6;
            this.formattedTextLabelEditorControl5.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:Red}",
        "Not Equal"};
            // 
            // formattedTextLabelEditorControl6
            // 
            this.formattedTextLabelEditorControl6.Location = new System.Drawing.Point(179, 152);
            this.formattedTextLabelEditorControl6.Name = "formattedTextLabelEditorControl6";
            this.formattedTextLabelEditorControl6.Size = new System.Drawing.Size(451, 90);
            this.formattedTextLabelEditorControl6.TabIndex = 7;
            this.formattedTextLabelEditorControl6.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "One of the digits in the attempt was the",
        "correct colour, but it was not in the",
        "correct place."};
            // 
            // staticTextureEditorControl2
            // 
            this.staticTextureEditorControl2.Location = new System.Drawing.Point(37, 147);
            this.staticTextureEditorControl2.MaintainAspectRatio = true;
            this.staticTextureEditorControl2.Name = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.Size = new System.Drawing.Size(26, 26);
            this.staticTextureEditorControl2.TabIndex = 8;
            this.staticTextureEditorControl2.Text = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.TextureAsset = "Results_Partial";
            // 
            // formattedTextLabelEditorControl7
            // 
            this.formattedTextLabelEditorControl7.Location = new System.Drawing.Point(179, 248);
            this.formattedTextLabelEditorControl7.Name = "formattedTextLabelEditorControl7";
            this.formattedTextLabelEditorControl7.Size = new System.Drawing.Size(451, 68);
            this.formattedTextLabelEditorControl7.TabIndex = 9;
            this.formattedTextLabelEditorControl7.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "One of the digits in the attempt was not a",
        "colour present in the code."};
            // 
            // staticTextureEditorControl3
            // 
            this.staticTextureEditorControl3.Location = new System.Drawing.Point(37, 243);
            this.staticTextureEditorControl3.MaintainAspectRatio = true;
            this.staticTextureEditorControl3.Name = "staticTextureEditorControl3";
            this.staticTextureEditorControl3.Size = new System.Drawing.Size(26, 26);
            this.staticTextureEditorControl3.TabIndex = 10;
            this.staticTextureEditorControl3.Text = "staticTextureEditorControl3";
            this.staticTextureEditorControl3.TextureAsset = "Results_NotEqual";
            // 
            // formattedTextLabelEditorControl8
            // 
            this.formattedTextLabelEditorControl8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl8.Location = new System.Drawing.Point(-3, 322);
            this.formattedTextLabelEditorControl8.Name = "formattedTextLabelEditorControl8";
            this.formattedTextLabelEditorControl8.Size = new System.Drawing.Size(630, 147);
            this.formattedTextLabelEditorControl8.TabIndex = 11;
            this.formattedTextLabelEditorControl8.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "You will always get four pieces of feedback, one for each",
        "digit in your attempt. However, they will always be listed in",
        "the order of {C:Green}Equal {C:White}-> {C:Orange}Partial {C:White}-> {C:Red}Not " +
            "Equal{C:White}.",
        "",
        "This is important to remember. The order of the results does",
        "not correspond to the order in your attempt."};
            // 
            // BriefingPage_ResultsOverview
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl8);
            this.Controls.Add(this.staticTextureEditorControl3);
            this.Controls.Add(this.formattedTextLabelEditorControl7);
            this.Controls.Add(this.staticTextureEditorControl2);
            this.Controls.Add(this.formattedTextLabelEditorControl6);
            this.Controls.Add(this.formattedTextLabelEditorControl5);
            this.Controls.Add(this.formattedTextLabelEditorControl4);
            this.Controls.Add(this.formattedTextLabelEditorControl3);
            this.Controls.Add(this.formattedTextLabelEditorControl2);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Name = "BriefingPage_ResultsOverview";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
