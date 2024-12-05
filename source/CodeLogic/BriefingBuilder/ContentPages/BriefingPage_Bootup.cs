namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_Bootup : DesignerHostBase
    {
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;

        public BriefingPage_Bootup()
            : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.SuspendLayout();
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(627, 139);
            this.formattedTextLabelEditorControl1.TabIndex = 0;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Alignment:Left}{C:White}{Font:BriefingNormalFont}",
        "After booting up, the software will make an attempt to",
        "automatically crack the code! Unfortunately, this seldom",
        "works... but hey, it\'s worth a shot!",
        "",
        "If the auto-crack sequence fails, which it certainly will,",
        "tap on the device to begin manually solving the code!"};
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(3, 145);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(624, 351);
            this.staticTextureEditorControl1.TabIndex = 1;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "BP2_BootupImage";
            // 
            // BriefingPage_Bootup
            // 
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Name = "BriefingPage_Bootup";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
