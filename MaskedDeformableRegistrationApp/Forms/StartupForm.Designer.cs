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
            this.SuspendLayout();
            // 
            // buttonWSIExtraction
            // 
            this.buttonWSIExtraction.Location = new System.Drawing.Point(12, 12);
            this.buttonWSIExtraction.Name = "buttonWSIExtraction";
            this.buttonWSIExtraction.Size = new System.Drawing.Size(232, 23);
            this.buttonWSIExtraction.TabIndex = 0;
            this.buttonWSIExtraction.Text = "Extract slices from WSI\'s";
            this.buttonWSIExtraction.UseVisualStyleBackColor = true;
            this.buttonWSIExtraction.Click += new System.EventHandler(this.buttonWSIExtraction_Click);
            // 
            // buttonExistingStack
            // 
            this.buttonExistingStack.Location = new System.Drawing.Point(12, 41);
            this.buttonExistingStack.Name = "buttonExistingStack";
            this.buttonExistingStack.Size = new System.Drawing.Size(232, 23);
            this.buttonExistingStack.TabIndex = 1;
            this.buttonExistingStack.Text = "Registration of existing stack";
            this.buttonExistingStack.UseVisualStyleBackColor = true;
            this.buttonExistingStack.Click += new System.EventHandler(this.buttonExistingStack_Click);
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 76);
            this.Controls.Add(this.buttonExistingStack);
            this.Controls.Add(this.buttonWSIExtraction);
            this.Name = "StartupForm";
            this.Text = "StartupForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonWSIExtraction;
        private System.Windows.Forms.Button buttonExistingStack;
    }
}