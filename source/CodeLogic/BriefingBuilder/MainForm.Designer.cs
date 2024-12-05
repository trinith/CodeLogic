namespace BriefingBuilder
{
    partial class MainForm
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
            this._btnPrev = new System.Windows.Forms.Button();
            this._btnNext = new System.Windows.Forms.Button();
            this._pnlContent = new System.Windows.Forms.Panel();
            this._btnGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _btnPrev
            // 
            this._btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnPrev.Location = new System.Drawing.Point(466, 506);
            this._btnPrev.Name = "_btnPrev";
            this._btnPrev.Size = new System.Drawing.Size(75, 23);
            this._btnPrev.TabIndex = 0;
            this._btnPrev.Text = "<";
            this._btnPrev.UseVisualStyleBackColor = true;
            this._btnPrev.Click += new System.EventHandler(this.Handle_PreviousButtonClick);
            // 
            // _btnNext
            // 
            this._btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnNext.Location = new System.Drawing.Point(547, 506);
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(75, 23);
            this._btnNext.TabIndex = 1;
            this._btnNext.Text = ">";
            this._btnNext.UseVisualStyleBackColor = true;
            this._btnNext.Click += new System.EventHandler(this.Handle_NextButtonClick);
            // 
            // _pnlContent
            // 
            this._pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlContent.Location = new System.Drawing.Point(0, 0);
            this._pnlContent.Name = "_pnlContent";
            this._pnlContent.Size = new System.Drawing.Size(634, 541);
            this._pnlContent.TabIndex = 2;
            // 
            // _btnGenerate
            // 
            this._btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btnGenerate.Location = new System.Drawing.Point(12, 506);
            this._btnGenerate.Name = "_btnGenerate";
            this._btnGenerate.Size = new System.Drawing.Size(75, 23);
            this._btnGenerate.TabIndex = 0;
            this._btnGenerate.Text = "Generate";
            this._btnGenerate.UseVisualStyleBackColor = true;
            this._btnGenerate.Click += new System.EventHandler(this.Handle_GenerateButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(634, 541);
            this.Controls.Add(this._btnGenerate);
            this.Controls.Add(this._btnNext);
            this.Controls.Add(this._btnPrev);
            this.Controls.Add(this._pnlContent);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnPrev;
        private System.Windows.Forms.Button _btnNext;
        private System.Windows.Forms.Panel _pnlContent;
        private System.Windows.Forms.Button _btnGenerate;
    }
}

