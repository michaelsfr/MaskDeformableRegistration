namespace MaskedDeformableRegistrationApp.Forms
{
    partial class LoadMaskForm
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
            this.buttonChooseMovingMasks = new System.Windows.Forms.Button();
            this.textBoxMovingMasks = new System.Windows.Forms.TextBox();
            this.buttonChooseFixedMask = new System.Windows.Forms.Button();
            this.textBoxFixedMask = new System.Windows.Forms.TextBox();
            this.checkBoxFixedMask = new System.Windows.Forms.CheckBox();
            this.checkBoxMovingMasks = new System.Windows.Forms.CheckBox();
            this.checkBoxCoefficientMaps = new System.Windows.Forms.CheckBox();
            this.buttonChooseCoefficientMaps = new System.Windows.Forms.Button();
            this.textBoxCoefficientMaps = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.openFileDialogFixedMask = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogMovingMasks = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogCoefficientMaps = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // buttonChooseMovingMasks
            // 
            this.buttonChooseMovingMasks.Location = new System.Drawing.Point(214, 89);
            this.buttonChooseMovingMasks.Name = "buttonChooseMovingMasks";
            this.buttonChooseMovingMasks.Size = new System.Drawing.Size(75, 20);
            this.buttonChooseMovingMasks.TabIndex = 11;
            this.buttonChooseMovingMasks.Text = "Choose";
            this.buttonChooseMovingMasks.UseVisualStyleBackColor = true;
            this.buttonChooseMovingMasks.Click += new System.EventHandler(this.buttonChooseMovingMasks_Click);
            // 
            // textBoxMovingMasks
            // 
            this.textBoxMovingMasks.Location = new System.Drawing.Point(30, 89);
            this.textBoxMovingMasks.Name = "textBoxMovingMasks";
            this.textBoxMovingMasks.Size = new System.Drawing.Size(178, 20);
            this.textBoxMovingMasks.TabIndex = 10;
            // 
            // buttonChooseFixedMask
            // 
            this.buttonChooseFixedMask.Location = new System.Drawing.Point(214, 36);
            this.buttonChooseFixedMask.Name = "buttonChooseFixedMask";
            this.buttonChooseFixedMask.Size = new System.Drawing.Size(75, 20);
            this.buttonChooseFixedMask.TabIndex = 9;
            this.buttonChooseFixedMask.Text = "Choose";
            this.buttonChooseFixedMask.UseVisualStyleBackColor = true;
            this.buttonChooseFixedMask.Click += new System.EventHandler(this.buttonChooseFixedMask_Click);
            // 
            // textBoxFixedMask
            // 
            this.textBoxFixedMask.Location = new System.Drawing.Point(30, 36);
            this.textBoxFixedMask.Name = "textBoxFixedMask";
            this.textBoxFixedMask.Size = new System.Drawing.Size(178, 20);
            this.textBoxFixedMask.TabIndex = 8;
            // 
            // checkBoxFixedMask
            // 
            this.checkBoxFixedMask.AutoSize = true;
            this.checkBoxFixedMask.Location = new System.Drawing.Point(13, 13);
            this.checkBoxFixedMask.Name = "checkBoxFixedMask";
            this.checkBoxFixedMask.Size = new System.Drawing.Size(103, 17);
            this.checkBoxFixedMask.TabIndex = 12;
            this.checkBoxFixedMask.Text = "Load fixed mask";
            this.checkBoxFixedMask.UseVisualStyleBackColor = true;
            this.checkBoxFixedMask.CheckedChanged += new System.EventHandler(this.checkBoxFixedMask_CheckedChanged);
            // 
            // checkBoxMovingMasks
            // 
            this.checkBoxMovingMasks.AutoSize = true;
            this.checkBoxMovingMasks.Location = new System.Drawing.Point(13, 67);
            this.checkBoxMovingMasks.Name = "checkBoxMovingMasks";
            this.checkBoxMovingMasks.Size = new System.Drawing.Size(120, 17);
            this.checkBoxMovingMasks.TabIndex = 13;
            this.checkBoxMovingMasks.Text = "Load moving masks";
            this.checkBoxMovingMasks.UseVisualStyleBackColor = true;
            this.checkBoxMovingMasks.CheckedChanged += new System.EventHandler(this.checkBoxMovingMasks_CheckedChanged);
            // 
            // checkBoxCoefficientMaps
            // 
            this.checkBoxCoefficientMaps.AutoSize = true;
            this.checkBoxCoefficientMaps.Location = new System.Drawing.Point(13, 120);
            this.checkBoxCoefficientMaps.Name = "checkBoxCoefficientMaps";
            this.checkBoxCoefficientMaps.Size = new System.Drawing.Size(218, 17);
            this.checkBoxCoefficientMaps.TabIndex = 14;
            this.checkBoxCoefficientMaps.Text = "Load coefficient maps for moving images";
            this.checkBoxCoefficientMaps.UseVisualStyleBackColor = true;
            this.checkBoxCoefficientMaps.CheckedChanged += new System.EventHandler(this.checkBoxCoefficientMaps_CheckedChanged);
            // 
            // e
            // 
            this.buttonChooseCoefficientMaps.Location = new System.Drawing.Point(214, 143);
            this.buttonChooseCoefficientMaps.Name = "e";
            this.buttonChooseCoefficientMaps.Size = new System.Drawing.Size(75, 20);
            this.buttonChooseCoefficientMaps.TabIndex = 16;
            this.buttonChooseCoefficientMaps.Text = "Choose";
            this.buttonChooseCoefficientMaps.UseVisualStyleBackColor = true;
            this.buttonChooseCoefficientMaps.Click += new System.EventHandler(this.buttonChooseCoefficientMaps_Click);
            // 
            // textBoxCoefficientMaps
            // 
            this.textBoxCoefficientMaps.Location = new System.Drawing.Point(30, 143);
            this.textBoxCoefficientMaps.Name = "textBoxCoefficientMaps";
            this.textBoxCoefficientMaps.Size = new System.Drawing.Size(178, 20);
            this.textBoxCoefficientMaps.TabIndex = 15;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(215, 180);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 18;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(133, 180);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // LoadMaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 214);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonChooseCoefficientMaps);
            this.Controls.Add(this.textBoxCoefficientMaps);
            this.Controls.Add(this.checkBoxCoefficientMaps);
            this.Controls.Add(this.checkBoxMovingMasks);
            this.Controls.Add(this.checkBoxFixedMask);
            this.Controls.Add(this.buttonChooseMovingMasks);
            this.Controls.Add(this.textBoxMovingMasks);
            this.Controls.Add(this.buttonChooseFixedMask);
            this.Controls.Add(this.textBoxFixedMask);
            this.Name = "LoadMaskForm";
            this.Text = "LoadMaskForm";
            this.Load += new System.EventHandler(this.LoadMaskForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonChooseMovingMasks;
        private System.Windows.Forms.TextBox textBoxMovingMasks;
        private System.Windows.Forms.Button buttonChooseFixedMask;
        private System.Windows.Forms.TextBox textBoxFixedMask;
        private System.Windows.Forms.CheckBox checkBoxFixedMask;
        private System.Windows.Forms.CheckBox checkBoxMovingMasks;
        private System.Windows.Forms.CheckBox checkBoxCoefficientMaps;
        private System.Windows.Forms.Button buttonChooseCoefficientMaps;
        private System.Windows.Forms.TextBox textBoxCoefficientMaps;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialogFixedMask;
        private System.Windows.Forms.OpenFileDialog openFileDialogMovingMasks;
        private System.Windows.Forms.OpenFileDialog openFileDialogCoefficientMaps;
    }
}