namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_Chapter1 : DesignerHostBase
    {
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl2;
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;

        public BriefingPage_Chapter1()
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
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(156, 0);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(474, 130);
            this.formattedTextLabelEditorControl1.TabIndex = 6;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "",
        "{Alignment:Centre}{C:255,92,92}{Font:BriefingTitleFont}",
        "Chapter 1",
        "{C:White}{Font:BriefingNormalFont}",
        "Introduction"};
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.BackColor = System.Drawing.Color.Black;
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(150, 130);
            this.staticTextureEditorControl1.TabIndex = 5;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "CX4Logo";
            // 
            // formattedTextLabelEditorControl2
            // 
            this.formattedTextLabelEditorControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl2.Location = new System.Drawing.Point(0, 136);
            this.formattedTextLabelEditorControl2.Name = "formattedTextLabelEditorControl2";
            this.formattedTextLabelEditorControl2.Size = new System.Drawing.Size(630, 246);
            this.formattedTextLabelEditorControl2.TabIndex = 7;
            this.formattedTextLabelEditorControl2.TextLines = new string[] {
        "{Alignment:Left}{C:White}{Font:BriefingNormalFont}",
        "To start things off, we will go over a brief introduction of",
        "your objective, as well as what you\'ll see when you first boot",
        "up the CodeLogic software."};
            // 
            // BriefingPage_Chapter1
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl2);
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Name = "BriefingPage_Chapter1";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
