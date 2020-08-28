namespace MaskedDeformableRegistrationApp.Forms
{
    partial class LoadStackForm
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
            this.radioButtonLoadStack = new System.Windows.Forms.RadioButton();
            this.radioButtonLoadPNGs = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonChooseStack = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonChoosePNGs = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.openFileDialogStack = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogImages = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // radioButtonLoadStack
            // 
            this.radioButtonLoadStack.AutoSize = true;
            this.radioButtonLoadStack.Location = new System.Drawing.Point(13, 13);
            this.radioButtonLoadStack.Name = "radioButtonLoadStack";
            this.radioButtonLoadStack.Size = new System.Drawing.Size(101, 17);
            this.radioButtonLoadStack.TabIndex = 0;
            this.radioButtonLoadStack.TabStop = true;
            this.radioButtonLoadStack.Text = "Load from stack";
            this.radioButtonLoadStack.UseVisualStyleBackColor = true;
            this.radioButtonLoadStack.CheckedChanged += new System.EventHandler(this.radioButtonLoadStack_CheckedChanged);
            // 
            // radioButtonLoadPNGs
            // 
            this.radioButtonLoadPNGs.AutoSize = true;
            this.radioButtonLoadPNGs.Location = new System.Drawing.Point(13, 66);
            this.radioButtonLoadPNGs.Name = "radioButtonLoadPNGs";
            this.radioButtonLoadPNGs.Size = new System.Drawing.Size(139, 17);
            this.radioButtonLoadPNGs.TabIndex = 1;
            this.radioButtonLoadPNGs.TabStop = true;
            this.radioButtonLoadPNGs.Text = "Load multiple image files";
            this.radioButtonLoadPNGs.UseVisualStyleBackColor = true;
            this.radioButtonLoadPNGs.CheckedChanged += new System.EventHandler(this.radioButtonLoadPNGs_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(31, 37);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(178, 20);
            this.textBox1.TabIndex = 2;
            // 
            // buttonChooseStack
            // 
            this.buttonChooseStack.Location = new System.Drawing.Point(215, 37);
            this.buttonChooseStack.Name = "buttonChooseStack";
            this.buttonChooseStack.Size = new System.Drawing.Size(75, 20);
            this.buttonChooseStack.TabIndex = 3;
            this.buttonChooseStack.Text = "Choose";
            this.buttonChooseStack.UseVisualStyleBackColor = true;
            this.buttonChooseStack.Click += new System.EventHandler(this.buttonChooseStack_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(31, 90);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(178, 20);
            this.textBox2.TabIndex = 4;
            // 
            // buttonChoosePNGs
            // 
            this.buttonChoosePNGs.Location = new System.Drawing.Point(216, 90);
            this.buttonChoosePNGs.Name = "buttonChoosePNGs";
            this.buttonChoosePNGs.Size = new System.Drawing.Size(75, 20);
            this.buttonChoosePNGs.TabIndex = 5;
            this.buttonChoosePNGs.Text = "Choose";
            this.buttonChoosePNGs.UseVisualStyleBackColor = true;
            this.buttonChoosePNGs.Click += new System.EventHandler(this.buttonChoosePNGs_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(134, 126);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(216, 126);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 7;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // openFileDialogStack
            // 
            this.openFileDialogStack.FileName = "openFileDialog";
            // 
            // openFileDialogImages
            // 
            this.openFileDialogImages.FileName = "openFileDialog1";
            // 
            // LoadStackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 163);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonChoosePNGs);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.buttonChooseStack);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.radioButtonLoadPNGs);
            this.Controls.Add(this.radioButtonLoadStack);
            this.Name = "LoadStackForm";
            this.Text = "LoadStackForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoadStackForm_FormClosed);
            this.Load += new System.EventHandler(this.LoadStackForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonLoadStack;
        private System.Windows.Forms.RadioButton radioButtonLoadPNGs;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonChooseStack;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonChoosePNGs;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonProceed;
        private System.Windows.Forms.OpenFileDialog openFileDialogStack;
        private System.Windows.Forms.OpenFileDialog openFileDialogImages;
    }
}