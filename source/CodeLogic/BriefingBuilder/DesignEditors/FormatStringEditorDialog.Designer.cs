namespace BriefingBuilder.DesignEditors
{
    partial class FormatStringEditorDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tbFormatString = new System.Windows.Forms.TextBox();
            this._btnOk = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnFont = new System.Windows.Forms.Button();
            this._btnAlignment = new System.Windows.Forms.Button();
            this._btnColour = new System.Windows.Forms.Button();
            this._btnTPC = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _tbFormatString
            // 
            this._tbFormatString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbFormatString.Location = new System.Drawing.Point(12, 12);
            this._tbFormatString.Multiline = true;
            this._tbFormatString.Name = "_tbFormatString";
            this._tbFormatString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._tbFormatString.Size = new System.Drawing.Size(626, 308);
            this._tbFormatString.TabIndex = 0;
            this._tbFormatString.WordWrap = false;
            // 
            // _btnOk
            // 
            this._btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnOk.Location = new System.Drawing.Point(563, 326);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 1;
            this._btnOk.Text = "Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this.Handle_OkButtonClick);
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(644, 326);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _btnFont
            // 
            this._btnFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnFont.Location = new System.Drawing.Point(644, 70);
            this._btnFont.Name = "_btnFont";
            this._btnFont.Size = new System.Drawing.Size(75, 23);
            this._btnFont.TabIndex = 3;
            this._btnFont.Text = "Font";
            this._btnFont.UseVisualStyleBackColor = true;
            this._btnFont.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Handle_FontButtonClick);
            // 
            // _btnAlignment
            // 
            this._btnAlignment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnAlignment.Location = new System.Drawing.Point(644, 99);
            this._btnAlignment.Name = "_btnAlignment";
            this._btnAlignment.Size = new System.Drawing.Size(75, 23);
            this._btnAlignment.TabIndex = 4;
            this._btnAlignment.Text = "Alignment";
            this._btnAlignment.UseVisualStyleBackColor = true;
            this._btnAlignment.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Handle_AlignmentButtonClick);
            // 
            // _btnColour
            // 
            this._btnColour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnColour.Location = new System.Drawing.Point(644, 12);
            this._btnColour.Name = "_btnColour";
            this._btnColour.Size = new System.Drawing.Size(75, 23);
            this._btnColour.TabIndex = 5;
            this._btnColour.Text = "Colour";
            this._btnColour.UseVisualStyleBackColor = true;
            this._btnColour.Click += new System.EventHandler(this.Handle_ColourButtonClick);
            // 
            // _btnTPC
            // 
            this._btnTPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnTPC.Location = new System.Drawing.Point(644, 41);
            this._btnTPC.Name = "_btnTPC";
            this._btnTPC.Size = new System.Drawing.Size(75, 23);
            this._btnTPC.TabIndex = 6;
            this._btnTPC.Text = "TPC";
            this._btnTPC.UseVisualStyleBackColor = true;
            this._btnTPC.Click += new System.EventHandler(this.Handle_TPCButtonClick);
            // 
            // FormatStringEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(731, 361);
            this.ControlBox = false;
            this.Controls.Add(this._btnTPC);
            this.Controls.Add(this._btnColour);
            this.Controls.Add(this._btnAlignment);
            this.Controls.Add(this._btnFont);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this._tbFormatString);
            this.MinimumSize = new System.Drawing.Size(397, 202);
            this.Name = "FormatStringEditorDialog";
            this.Text = "FormatStringEditorDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _tbFormatString;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Button _btnFont;
        private System.Windows.Forms.Button _btnAlignment;
        private System.Windows.Forms.Button _btnColour;
        private System.Windows.Forms.Button _btnTPC;
    }
}