namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_Chapter2 : DesignerHostBase
    {
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl2;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;

        public BriefingPage_Chapter2()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.formattedTextLabelEditorControl2 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.SuspendLayout();
            // 
            // formattedTextLabelEditorControl2
            // 
            this.formattedTextLabelEditorControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl2.Location = new System.Drawing.Point(0, 136);
            this.formattedTextLabelEditorControl2.Name = "formattedTextLabelEditorControl2";
            this.formattedTextLabelEditorControl2.Size = new System.Drawing.Size(630, 246);
            this.formattedTextLabelEditorControl2.TabIndex = 10;
            this.formattedTextLabelEditorControl2.TextLines = new string[] {
        "{Alignment:Left}{C:White}{Font:BriefingNormalFont}",
        "In this chapter, we will cover the interface for CodeLogic,",
        "going over all of the elements that you will see on your",
        "screen, what they do, and how to use them."};
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(156, 0);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(474, 130);
            this.formattedTextLabelEditorControl1.TabIndex = 9;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "",
        "{Alignment:Centre}{C:255,92,92}{Font:BriefingTitleFont}",
        "Chapter 2",
        "{C:White}{Font:BriefingNormalFont}",
        "Interface Overview"};
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.BackColor = System.Drawing.Color.Black;
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(150, 130);
            this.staticTextureEditorControl1.TabIndex = 8;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "CX4Logo";
            // 
            // BriefingPage_Chapter2
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl2);
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Name = "BriefingPage_Chapter2";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
