using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_Overview : DesignerHostBase
    {
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;

        public BriefingPage_Overview()
            : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.staticTextureEditorControl1 = new BriefingBuilder.Controls.StaticTextureEditorControl();
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.SuspendLayout();
            // 
            // staticTextureEditorControl1
            // 
            this.staticTextureEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(630, 185);
            this.staticTextureEditorControl1.TabIndex = 0;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "BP3_Overview";
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 191);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(630, 246);
            this.formattedTextLabelEditorControl1.TabIndex = 1;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Alignment:Left}{C:White}{Font:BriefingNormalFont}",
        "This is an overview of the CodeLogic main screen. Each",
        "element of the interface is identified by the matching number",
        "below.",
        "",
        "1) Current Attempt Indicator",
        "2) Code Input",
        "3) Attempt History",
        "4) Submit Button",
        "5) Menu Button",
        "",
        "Lets go through each of these elements..."};
            // 
            // BriefingPage_Overview
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Name = "BriefingPage_Overview";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
