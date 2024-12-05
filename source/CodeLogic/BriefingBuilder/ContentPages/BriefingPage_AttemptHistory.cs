namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_AttemptHistory : DesignerHostBase
    {
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl2;
        private Controls.StaticTextureEditorControl staticTextureEditorControl2;

        public BriefingPage_AttemptHistory()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.staticTextureEditorControl2 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl2 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.SuspendLayout();
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(630, 127);
            this.staticTextureEditorControl1.TabIndex = 0;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "HistoryFull";
            // 
            // staticTextureEditorControl2
            // 
            this.staticTextureEditorControl2.Location = new System.Drawing.Point(0, 253);
            this.staticTextureEditorControl2.MaintainAspectRatio = true;
            this.staticTextureEditorControl2.Name = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.Size = new System.Drawing.Size(630, 36);
            this.staticTextureEditorControl2.TabIndex = 1;
            this.staticTextureEditorControl2.Text = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.TextureAsset = "HistoryZoom";
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 133);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(630, 114);
            this.formattedTextLabelEditorControl1.TabIndex = 2;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Alignment:Left}{Font:BriefingNormalFont}{C:White}",
        "At the bottom of the interface you will find the Attempt",
        "History. This shows you a log of all the attempts you have",
        "made so far, listed in order of most recent to least recent.",
        "Tapping the icon at the top will change the size of the panel",
        "if you want to see less information, or hide it completely."};
            // 
            // formattedTextLabelEditorControl2
            // 
            this.formattedTextLabelEditorControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl2.Location = new System.Drawing.Point(0, 295);
            this.formattedTextLabelEditorControl2.Name = "formattedTextLabelEditorControl2";
            this.formattedTextLabelEditorControl2.Size = new System.Drawing.Size(630, 205);
            this.formattedTextLabelEditorControl2.TabIndex = 3;
            this.formattedTextLabelEditorControl2.TextLines = new string[] {
        "{Alignment:Left}{Font:BriefingNormalFont}{C:White}",
        "Each attempt record shows you the attempt number, the code",
        "that was tested, and the feedback for the attempt. A square",
        "shows that an index was the correct colour in the correct",
        "place, while a triangle shows that an index was the correct",
        "colour but in an incorrect place. An empty spot indicates",
        "that an index was not present in the code.",
        "",
        "We will go over what the feedback means in more detail once we",
        "have finished going over the interface."};
            // 
            // BriefingPage_AttemptHistory
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl2);
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Controls.Add(this.staticTextureEditorControl2);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Name = "BriefingPage_AttemptHistory";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
