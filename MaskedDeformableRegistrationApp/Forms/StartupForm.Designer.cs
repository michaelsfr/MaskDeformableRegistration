namespace MaskedDeformableRegistrationApp.Forms
{
    partial class StartupForm
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
            this.buttonWSIExtraction = new System.Windows.Forms.Button();
            this.buttonExistingStack = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonChoosePath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // buttonWSIExtraction
            // 
            this.buttonWSIExtraction.Location = new System.Drawing.Point(12, 103);
            this.buttonWSIExtraction.Name = "buttonWSIExtraction";
            this.buttonWSIExtraction.Size = new System.Drawing.Size(232, 23);
            this.buttonWSIExtraction.TabIndex = 0;
            this.buttonWSIExtraction.Text = "Extract slices from WSI\'s";
            this.buttonWSIExtraction.UseVisualStyleBackColor = true;
            this.buttonWSIExtraction.Click += new System.EventHandler(this.buttonWSIExtraction_Click);
            // 
            // buttonExistingStack
            // 
            this.buttonExistingStack.Location = new System.Drawing.Point(12, 132);
            this.buttonExistingStack.Name = "buttonExistingStack";
            this.buttonExistingStack.Size = new System.Drawing.Size(232, 23);
            this.buttonExistingStack.TabIndex = 1;
            this.buttonExistingStack.Text = "Registration of existing stack";
            this.buttonExistingStack.UseVisualStyleBackColor = true;
            this.buttonExistingStack.Click += new System.EventHandler(this.buttonExistingStack_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(232, 20);
            this.textBox1.TabIndex = 2;
            // 
            // buttonChoosePath
            // 
            this.buttonChoosePath.Location = new System.Drawing.Point(12, 59);
            this.buttonChoosePath.Name = "buttonChoosePath";
            this.buttonChoosePath.Size = new System.Drawing.Size(232, 23);
            this.buttonChoosePath.TabIndex = 3;
            this.buttonChoosePath.Text = "Choose";
            this.buttonChoosePath.UseVisualStyleBackColor = true;
            this.buttonChoosePath.Click += new System.EventHandler(this.buttonChoosePath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Choose output path:";
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 169);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonChoosePath);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonExistingStack);
            this.Controls.Add(this.buttonWSIExtraction);
            this.Name = "StartupForm";
            this.Text = "StartupForm";
            this.Load += new System.EventHandler(this.StartupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonWSIExtraction;
        private System.Windows.Forms.Button buttonExistingStack;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonChoosePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}