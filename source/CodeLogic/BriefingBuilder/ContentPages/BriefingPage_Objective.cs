namespace BriefingBuilder.ContentPages
{
    public class BriefingPage_Objective : DesignerHostBase
    {
        private Controls.FormattedTextLabelEditorControl formattedTextLabelEditorControl1;

        public BriefingPage_Objective()
            : base()
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
            this.formattedTextLabelEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formattedTextLabelEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.formattedTextLabelEditorControl1.Name = "formattedTextLabelEditorControl1";
            this.formattedTextLabelEditorControl1.Size = new System.Drawing.Size(630, 444);
            this.formattedTextLabelEditorControl1.TabIndex = 0;
            this.formattedTextLabelEditorControl1.TextLines = new string[] {
        "{Font:BriefingNormalFont}{Alignment:Left}{C:White}",
        "Using CodeLogic, you can work your way towards solving a code.",
        "A code is made up of four digits and each digit can have one",
        "of five values. Within CodeLogic, these values are represented",
        "as colours... {C:Red}red{C:White}, {C:Green}green{C:White}, {C:Blue}blue{C:White}" +
            ", {C:Yellow}yellow{C:White}, and {C:Orange}orange{C:White}.",
        "",
        "Using the interface, you will make guesses as to what the code",
        "is and CodeLogic will try it out for you. If you get the right",
        "code you will succeed in your mission; however, you will",
        "likely not get it on your first attempt. When this happens,",
        "CodeLogic will give you some feedback about your guess. You",
        "can use this feedback to guide subsequent guesses.",
        "",
        "We will discuss this process in more detail future in the",
        "briefing but first, lets go over the CodeLogic interface!"};
            // 
            // BriefingPage_Objective
            // 
            this.Controls.Add(this.formattedTextLabelEditorControl1);
            this.Name = "BriefingPage_Objective";
            this.Namespace = "ArbitraryPixel.Common.CodeLogic.BriefingContent";
            this.Size = new System.Drawing.Size(630, 580);
            this.ResumeLayout(false);

        }
    }
}
