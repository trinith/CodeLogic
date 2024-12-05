namespace BriefingBuilder.ContentPages
{
    public class Dialog_NoFileAccessPermissions : DesignerHostBase
    {
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;

        public Dialog_NoFileAccessPermissions()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.formattedTextLabelEditorControl1 = new BriefingBuilder.Controls.FormattedTextLabelEditorControl();
            this.SuspendLayout();
            // 
            // formattedTextLabelEditorControl1
            // 
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(10, 10);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(730, 180);
            this.formattedTextLabelEditorControl1.TabIndex = 0;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{TPC:0}{Alignment:Centre}{Color:White}",
        "Information",
        "",
        "{Alignment:Left}{Color:128,128,128}",
        "Storage permissions were not granted to CodeLogic so settings and",
        "gameplay statistics can\'t be saved for next time. If this functionality",
        "is desired, exit CodeLogic completely, restart, and accept the",
        "permission request prompt on start up."};
            // 
            // Dialog_NoFileAccessPermissions
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Name = "Dialog_NoFileAccessPermissions";
            this.Size = new System.Drawing.Size(750, 200);
            this.ResumeLayout(false);

        }
    }
}
