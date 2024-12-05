using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_CurrentAttemptIndicator : DesignerHostBase
    {
        private Controls.StaticTextureEditorControl staticTextureEditorControl1;
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;

        public BriefingPage_CurrentAttemptIndicator()
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
            this.staticTextureEditorControl1.Location = new System.Drawing.Point(0, 3);
            this.staticTextureEditorControl1.MaintainAspectRatio = true;
            this.staticTextureEditorControl1.Name = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.Size = new System.Drawing.Size(630, 100);
            this.staticTextureEditorControl1.TabIndex = 0;
            this.staticTextureEditorControl1.Text = "staticTextureEditorControl1";
            this.staticTextureEditorControl1.TextureAsset = "BP4_CurrentAttempt";
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 109);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(630, 400);
            this.formattedTextLabelEditorControl1.TabIndex = 1;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Alignment:Left}{Font:BriefingNormalFont}{C:White}",
        "This is the Current Attempt Indicator. It gives you a visual",
        "reference for which attempt you are on. Since there is a",
        "maximum of 10 attempts, there are 10 spots here.",
        "",
        "An arrow points to the current attempt you are on and, when an",
        "attempt is made, a circle fills in that spot indicating that",
        "it has been used. The colour of the circles also changes",
        "depending on how many attempts you\'ve used:",
        "",
        "  - 3 or less: the circles are {C:Green}green{C:White}.",
        "  - 4 to 6: the circles are {C:Yellow}yellow{C:White}.",
        "  - 7 to 10: the circles are {C:Red}red{C:White}.",
        "    NOTE: On the final attempt, the circles will flash."};
            // 
            // BriefingPage_CurrentAttemptIndicator
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Controls.Add(this.staticTextureEditorControl1);
            this.Name = "BriefingPage_CurrentAttemptIndicator";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
