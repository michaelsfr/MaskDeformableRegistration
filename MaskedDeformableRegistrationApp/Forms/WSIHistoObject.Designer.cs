namespace MaskedDeformableRegistrationApp.Forms
{
    partial class WSIHistoObject
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAdaptSize = new System.Windows.Forms.Button();
            this.buttonRotate = new System.Windows.Forms.Button();
            this.buttonFlip = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAdaptSize
            // 
            this.buttonAdaptSize.Location = new System.Drawing.Point(7, 10);
            this.buttonAdaptSize.Name = "buttonAdaptSize";
            this.buttonAdaptSize.Size = new System.Drawing.Size(27, 23);
            this.buttonAdaptSize.TabIndex = 0;
            this.buttonAdaptSize.Text = "✎";
            this.buttonAdaptSize.UseVisualStyleBackColor = true;
            this.buttonAdaptSize.Click += new System.EventHandler(this.buttonAdaptSize_Click);
            // 
            // buttonRotate
            // 
            this.buttonRotate.Location = new System.Drawing.Point(41, 10);
            this.buttonRotate.Name = "buttonRotate";
            this.buttonRotate.Size = new System.Drawing.Size(42, 23);
            this.buttonRotate.TabIndex = 1;
            this.buttonRotate.Text = "90° ⤵";
            this.buttonRotate.UseVisualStyleBackColor = true;
            this.buttonRotate.Click += new System.EventHandler(this.buttonRotate_Click);
            // 
            // buttonFlip
            // 
            this.buttonFlip.Location = new System.Drawing.Point(90, 10);
            this.buttonFlip.Name = "buttonFlip";
            this.buttonFlip.Size = new System.Drawing.Size(29, 23);
            this.buttonFlip.TabIndex = 2;
            this.buttonFlip.Text = "<>";
            this.buttonFlip.UseVisualStyleBackColor = true;
            this.buttonFlip.Click += new System.EventHandler(this.buttonFlip_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(125, 10);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(26, 23);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "X";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(7, 52);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(143, 104);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(6, 36);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(28, 13);
            this.labelTitle.TabIndex = 5;
            this.labelTitle.Text = "###";
            // 
            // WSIHistoObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonFlip);
            this.Controls.Add(this.buttonRotate);
            this.Controls.Add(this.buttonAdaptSize);
            this.Name = "WSIHistoObject";
            this.Size = new System.Drawing.Size(159, 164);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAdaptSize;
        private System.Windows.Forms.Button buttonRotate;
        private System.Windows.Forms.Button buttonFlip;
        public System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label labelTitle;
    }
}
