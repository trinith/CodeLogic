namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_DeviceMenu : DesignerHostBase
    {
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;
        private Controls.StaticTextureEditorControl staticTextureEditorControl2;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl2;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl3;
        private Controls.StaticTextureEditorControl staticTextureEditorControl3;

        public BriefingPage_DeviceMenu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.staticTextureEditorControl2 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.staticTextureEditorControl3 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl2 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.formattedTextLabelEditorControl3 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.SuspendLayout();
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(36, 100);
            this.staticTextureEditorControl1.TabIndex = 0;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "MenuButton";
            // 
            // staticTextureEditorControl2
            // 
            this.staticTextureEditorControl2.Location = new System.Drawing.Point(0, 373);
            this.staticTextureEditorControl2.MaintainAspectRatio = true;
            this.staticTextureEditorControl2.Name = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.Size = new System.Drawing.Size(36, 100);
            this.staticTextureEditorControl2.TabIndex = 1;
            this.staticTextureEditorControl2.Text = "staticTextureEditorControl2";
            this.staticTextureEditorControl2.TextureAsset = "ReturnButton";
            // 
            // staticTextureEditorControl3
            // 
            this.staticTextureEditorControl3.Location = new System.Drawing.Point(0, 106);
            this.staticTextureEditorControl3.MaintainAspectRatio = true;
            this.staticTextureEditorControl3.Name = "staticTextureEditorControl3";
            this.staticTextureEditorControl3.Size = new System.Drawing.Size(630, 185);
            this.staticTextureEditorControl3.TabIndex = 2;
            this.staticTextureEditorControl3.Text = "staticTextureEditorControl3";
            this.staticTextureEditorControl3.TextureAsset = "MenuScreen";
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(42, 26);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(588, 50);
            this.formattedTextLabelEditorControl1.TabIndex = 3;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "Tapping this button on the interface will take you to the",
        "menu screen."};
            // 
            // formattedTextLabelEditorControl2
            // 
            this.formattedTextLabelEditorControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl2.Location = new System.Drawing.Point(0, 297);
            this.formattedTextLabelEditorControl2.Name = "formattedTextLabelEditorControl2";
            this.formattedTextLabelEditorControl2.Size = new System.Drawing.Size(630, 70);
            this.formattedTextLabelEditorControl2.TabIndex = 4;
            this.formattedTextLabelEditorControl2.TextLines = new string[] {
        "{Alignment:Left}{Font:BriefingNormalFont}{C:White}",
        "Here, you can tap the left button to abort the mission and",
        "request extraction. The right button will allow you to change",
        "the volume of the interface."};
            // 
            // formattedTextLabelEditorControl3
            // 
            this.formattedTextLabelEditorControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl3.Location = new System.Drawing.Point(42, 390);
            this.formattedTextLabelEditorControl3.Name = "formattedTextLabelEditorControl3";
            this.formattedTextLabelEditorControl3.Size = new System.Drawing.Size(588, 67);
            this.formattedTextLabelEditorControl3.TabIndex = 5;
            this.formattedTextLabelEditorControl3.TextLines = new string[] {
        "{Alignment:Left}{Font:BriefingNormalFont}{C:White}",
        "This button, when tapped, will return you to the main",
        "screen where you can continue attempting to solve the",
        "code."};
            // 
            // BriefingPage_DeviceMenu
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl3);
            this.Controls.Add(this.formattedTextLabelEditorControl2);
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Controls.Add(this.staticTextureEditorControl3);
            this.Controls.Add(this.staticTextureEditorControl2);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Name = "BriefingPage_DeviceMenu";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
