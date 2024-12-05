using BriefingBuilder.Controls;

namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_Cover : DesignerHostBase
    {
        private FormattedTextLabelEditorControl formattedTextLabelEditorControl2;
        private FormattedTextLabelEditorControl formattedTextLabelEditorControl1;
        private StaticTextureEditorControl staticTextureEditorControl1;

        public BriefingPage_Cover()
            : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.formattedTextLabelEditorControl2 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.SuspendLayout();
            // 
            // formattedTextLabelEditorControl2
            // 
            this.formattedTextLabelEditorControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl2.Location = new System.Drawing.Point(0, 136);
            this.formattedTextLabelEditorControl2.Name = "formattedTextLabelEditorControl2";
            this.formattedTextLabelEditorControl2.Size = new System.Drawing.Size(630, 246);
            this.formattedTextLabelEditorControl2.TabIndex = 1;
            this.formattedTextLabelEditorControl2.TextLines = new string[] {
        "{Alignment:Left}{C:White}{Font:BriefingNormalFont}",
        "Welcome to your mission briefing, Agent!",
        "",
        "As a member of the Covert Code Cracking Collective, you will",
        "need to be well versed in the CodeLogic software on your",
        "mobile device. This manual will provide you with that",
        "information and have you well on your way to becoming a top",
        "notch agent!",
        "",
        "You can use the navigation buttons below to make your way",
        "through this manual."};
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.BackColor = System.Drawing.Color.Black;
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(150, 130);
            this.staticTextureEditorControl1.TabIndex = 3;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "CX4Logo";
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(156, 0);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(474, 130);
            this.formattedTextLabelEditorControl1.TabIndex = 4;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "",
        "{Alignment:Centre}{C:255,92,92}{Font:BriefingTitleFont}",
        "Briefing",
        "{C:White}{Font:BriefingNormalFont}",
        "(CX4 Tech Branch)"};
            // 
            // BriefingPage_Cover
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Controls.Add(this.formattedTextLabelEditorControl2);
            this.Name = "BriefingPage_Cover";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
