namespace MaskedDeformableRegistrationApp.Forms
{
    partial class RegistrationForm
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
            this.tabControlRegistration = new System.Windows.Forms.TabControl();
            this.tabPageRigid = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonSegmentationParams = new System.Windows.Forms.Button();
            this.buttonEditParameters = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonAffine = new System.Windows.Forms.RadioButton();
            this.radioButtonRigid = new System.Windows.Forms.RadioButton();
            this.radioButtonSimilarity = new System.Windows.Forms.RadioButton();
            this.radioButtonTranslation = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPageNonRigid = new System.Windows.Forms.TabPage();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabControlRegistration.SuspendLayout();
            this.tabPageRigid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlRegistration
            // 
            this.tabControlRegistration.Controls.Add(this.tabPageRigid);
            this.tabControlRegistration.Controls.Add(this.tabPageNonRigid);
            this.tabControlRegistration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlRegistration.Location = new System.Drawing.Point(0, 0);
            this.tabControlRegistration.Name = "tabControlRegistration";
            this.tabControlRegistration.SelectedIndex = 0;
            this.tabControlRegistration.Size = new System.Drawing.Size(800, 450);
            this.tabControlRegistration.TabIndex = 0;
            // 
            // tabPageRigid
            // 
            this.tabPageRigid.Controls.Add(this.splitContainer1);
            this.tabPageRigid.Location = new System.Drawing.Point(4, 22);
            this.tabPageRigid.Name = "tabPageRigid";
            this.tabPageRigid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRigid.Size = new System.Drawing.Size(792, 424);
            this.tabPageRigid.TabIndex = 0;
            this.tabPageRigid.Text = "Rigid registration";
            this.tabPageRigid.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonSegmentationParams);
            this.splitContainer1.Panel1.Controls.Add(this.buttonEditParameters);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(786, 418);
            this.splitContainer1.SplitterDistance = 133;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonSegmentationParams
            // 
            this.buttonSegmentationParams.Location = new System.Drawing.Point(695, 47);
            this.buttonSegmentationParams.Name = "buttonSegmentationParams";
            this.buttonSegmentationParams.Size = new System.Drawing.Size(75, 23);
            this.buttonSegmentationParams.TabIndex = 4;
            this.buttonSegmentationParams.Text = "Seg. parms";
            this.buttonSegmentationParams.UseVisualStyleBackColor = true;
            this.buttonSegmentationParams.Click += new System.EventHandler(this.buttonSegmentationParams_Click);
            // 
            // buttonEditParameters
            // 
            this.buttonEditParameters.Location = new System.Drawing.Point(695, 105);
            this.buttonEditParameters.Name = "buttonEditParameters";
            this.buttonEditParameters.Size = new System.Drawing.Size(75, 23);
            this.buttonEditParameters.TabIndex = 3;
            this.buttonEditParameters.Text = "Edit params";
            this.buttonEditParameters.UseVisualStyleBackColor = true;
            this.buttonEditParameters.Click += new System.EventHandler(this.buttonEditParameters_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(185, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(346, 125);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Masking";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonAffine);
            this.groupBox1.Controls.Add(this.radioButtonRigid);
            this.groupBox1.Controls.Add(this.radioButtonSimilarity);
            this.groupBox1.Controls.Add(this.radioButtonTranslation);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 125);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transformation";
            // 
            // radioButtonAffine
            // 
            this.radioButtonAffine.AutoSize = true;
            this.radioButtonAffine.Location = new System.Drawing.Point(7, 92);
            this.radioButtonAffine.Name = "radioButtonAffine";
            this.radioButtonAffine.Size = new System.Drawing.Size(52, 17);
            this.radioButtonAffine.TabIndex = 3;
            this.radioButtonAffine.TabStop = true;
            this.radioButtonAffine.Text = "Affine";
            this.radioButtonAffine.UseVisualStyleBackColor = true;
            // 
            // radioButtonRigid
            // 
            this.radioButtonRigid.AutoSize = true;
            this.radioButtonRigid.Location = new System.Drawing.Point(7, 68);
            this.radioButtonRigid.Name = "radioButtonRigid";
            this.radioButtonRigid.Size = new System.Drawing.Size(49, 17);
            this.radioButtonRigid.TabIndex = 2;
            this.radioButtonRigid.TabStop = true;
            this.radioButtonRigid.Text = "Rigid";
            this.radioButtonRigid.UseVisualStyleBackColor = true;
            // 
            // radioButtonSimilarity
            // 
            this.radioButtonSimilarity.AutoSize = true;
            this.radioButtonSimilarity.Location = new System.Drawing.Point(7, 44);
            this.radioButtonSimilarity.Name = "radioButtonSimilarity";
            this.radioButtonSimilarity.Size = new System.Drawing.Size(65, 17);
            this.radioButtonSimilarity.TabIndex = 1;
            this.radioButtonSimilarity.TabStop = true;
            this.radioButtonSimilarity.Text = "Similarity";
            this.radioButtonSimilarity.UseVisualStyleBackColor = true;
            // 
            // radioButtonTranslation
            // 
            this.radioButtonTranslation.AutoSize = true;
            this.radioButtonTranslation.Location = new System.Drawing.Point(7, 20);
            this.radioButtonTranslation.Name = "radioButtonTranslation";
            this.radioButtonTranslation.Size = new System.Drawing.Size(77, 17);
            this.radioButtonTranslation.TabIndex = 0;
            this.radioButtonTranslation.TabStop = true;
            this.radioButtonTranslation.Text = "Translation";
            this.radioButtonTranslation.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(695, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPageNonRigid
            // 
            this.tabPageNonRigid.Location = new System.Drawing.Point(4, 22);
            this.tabPageNonRigid.Name = "tabPageNonRigid";
            this.tabPageNonRigid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNonRigid.Size = new System.Drawing.Size(792, 424);
            this.tabPageNonRigid.TabIndex = 1;
            this.tabPageNonRigid.Text = "Non rigid registration";
            this.tabPageNonRigid.UseVisualStyleBackColor = true;
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControlRegistration);
            this.Name = "RegistrationForm";
            this.Text = "RegistrationForm";
            this.Load += new System.EventHandler(this.RegistrationForm_Load);
            this.tabControlRegistration.ResumeLayout(false);
            this.tabPageRigid.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlRegistration;
        private System.Windows.Forms.TabPage tabPageRigid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPageNonRigid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonAffine;
        private System.Windows.Forms.RadioButton radioButtonRigid;
        private System.Windows.Forms.RadioButton radioButtonSimilarity;
        private System.Windows.Forms.RadioButton radioButtonTranslation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonEditParameters;
        private System.Windows.Forms.Button buttonSegmentationParams;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}