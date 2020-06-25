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
            this.buttonEditParameters = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonRegMasks = new System.Windows.Forms.RadioButton();
            this.radioButtonUsefixMov = new System.Windows.Forms.RadioButton();
            this.radioButtonUseMoving = new System.Windows.Forms.RadioButton();
            this.radioButtonUseFixed = new System.Windows.Forms.RadioButton();
            this.radioButtonNoMasks = new System.Windows.Forms.RadioButton();
            this.buttonSegmentationParams = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonAffine = new System.Windows.Forms.RadioButton();
            this.radioButtonRigid = new System.Windows.Forms.RadioButton();
            this.radioButtonSimilarity = new System.Windows.Forms.RadioButton();
            this.radioButtonTranslation = new System.Windows.Forms.RadioButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.buttonStartRegistration = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabPageNonRigid = new System.Windows.Forms.TabPage();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonFirstFromStack = new System.Windows.Forms.RadioButton();
            this.radioButtonLastInStack = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButtonUsePrevInStack = new System.Windows.Forms.RadioButton();
            this.buttonEvaluation = new System.Windows.Forms.Button();
            this.tabControlRegistration.SuspendLayout();
            this.tabPageRigid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.tabControlRegistration.Size = new System.Drawing.Size(800, 356);
            this.tabControlRegistration.TabIndex = 0;
            // 
            // tabPageRigid
            // 
            this.tabPageRigid.Controls.Add(this.splitContainer1);
            this.tabPageRigid.Location = new System.Drawing.Point(4, 22);
            this.tabPageRigid.Name = "tabPageRigid";
            this.tabPageRigid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRigid.Size = new System.Drawing.Size(792, 330);
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
            this.splitContainer1.Panel1.Controls.Add(this.buttonEditParameters);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(786, 324);
            this.splitContainer1.SplitterDistance = 131;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonEditParameters
            // 
            this.buttonEditParameters.Location = new System.Drawing.Point(640, 89);
            this.buttonEditParameters.Name = "buttonEditParameters";
            this.buttonEditParameters.Size = new System.Drawing.Size(128, 23);
            this.buttonEditParameters.TabIndex = 3;
            this.buttonEditParameters.Text = "Edit params";
            this.buttonEditParameters.UseVisualStyleBackColor = true;
            this.buttonEditParameters.Click += new System.EventHandler(this.buttonEditParameters_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonRegMasks);
            this.groupBox2.Controls.Add(this.radioButtonUsefixMov);
            this.groupBox2.Controls.Add(this.radioButtonUseMoving);
            this.groupBox2.Controls.Add(this.radioButtonUseFixed);
            this.groupBox2.Controls.Add(this.radioButtonNoMasks);
            this.groupBox2.Controls.Add(this.buttonSegmentationParams);
            this.groupBox2.Location = new System.Drawing.Point(185, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(283, 125);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Masking";
            // 
            // radioButtonRegMasks
            // 
            this.radioButtonRegMasks.AutoSize = true;
            this.radioButtonRegMasks.Location = new System.Drawing.Point(151, 20);
            this.radioButtonRegMasks.Name = "radioButtonRegMasks";
            this.radioButtonRegMasks.Size = new System.Drawing.Size(126, 17);
            this.radioButtonRegMasks.TabIndex = 9;
            this.radioButtonRegMasks.TabStop = true;
            this.radioButtonRegMasks.Text = "Registration of masks";
            this.radioButtonRegMasks.UseVisualStyleBackColor = true;
            // 
            // radioButtonUsefixMov
            // 
            this.radioButtonUsefixMov.AutoSize = true;
            this.radioButtonUsefixMov.Location = new System.Drawing.Point(7, 92);
            this.radioButtonUsefixMov.Name = "radioButtonUsefixMov";
            this.radioButtonUsefixMov.Size = new System.Drawing.Size(127, 17);
            this.radioButtonUsefixMov.TabIndex = 8;
            this.radioButtonUsefixMov.TabStop = true;
            this.radioButtonUsefixMov.Text = "Use fixed and moving";
            this.radioButtonUsefixMov.UseVisualStyleBackColor = true;
            // 
            // radioButtonUseMoving
            // 
            this.radioButtonUseMoving.AutoSize = true;
            this.radioButtonUseMoving.Location = new System.Drawing.Point(7, 68);
            this.radioButtonUseMoving.Name = "radioButtonUseMoving";
            this.radioButtonUseMoving.Size = new System.Drawing.Size(109, 17);
            this.radioButtonUseMoving.TabIndex = 7;
            this.radioButtonUseMoving.TabStop = true;
            this.radioButtonUseMoving.Text = "Use moving mask";
            this.radioButtonUseMoving.UseVisualStyleBackColor = true;
            // 
            // radioButtonUseFixed
            // 
            this.radioButtonUseFixed.AutoSize = true;
            this.radioButtonUseFixed.Location = new System.Drawing.Point(7, 44);
            this.radioButtonUseFixed.Name = "radioButtonUseFixed";
            this.radioButtonUseFixed.Size = new System.Drawing.Size(97, 17);
            this.radioButtonUseFixed.TabIndex = 6;
            this.radioButtonUseFixed.TabStop = true;
            this.radioButtonUseFixed.Text = "Use fixed mask";
            this.radioButtonUseFixed.UseVisualStyleBackColor = true;
            // 
            // radioButtonNoMasks
            // 
            this.radioButtonNoMasks.AutoSize = true;
            this.radioButtonNoMasks.Location = new System.Drawing.Point(7, 20);
            this.radioButtonNoMasks.Name = "radioButtonNoMasks";
            this.radioButtonNoMasks.Size = new System.Drawing.Size(51, 17);
            this.radioButtonNoMasks.TabIndex = 5;
            this.radioButtonNoMasks.TabStop = true;
            this.radioButtonNoMasks.Text = "None";
            this.radioButtonNoMasks.UseVisualStyleBackColor = true;
            // 
            // buttonSegmentationParams
            // 
            this.buttonSegmentationParams.Location = new System.Drawing.Point(151, 86);
            this.buttonSegmentationParams.Name = "buttonSegmentationParams";
            this.buttonSegmentationParams.Size = new System.Drawing.Size(126, 23);
            this.buttonSegmentationParams.TabIndex = 4;
            this.buttonSegmentationParams.Text = "Seg. parms";
            this.buttonSegmentationParams.UseVisualStyleBackColor = true;
            this.buttonSegmentationParams.Click += new System.EventHandler(this.buttonSegmentationParams_Click);
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
            this.radioButtonSimilarity.Text = "Similarity";
            this.radioButtonSimilarity.UseVisualStyleBackColor = true;
            // 
            // radioButtonTranslation
            // 
            this.radioButtonTranslation.AutoSize = true;
            this.radioButtonTranslation.Checked = true;
            this.radioButtonTranslation.Location = new System.Drawing.Point(7, 20);
            this.radioButtonTranslation.Name = "radioButtonTranslation";
            this.radioButtonTranslation.Size = new System.Drawing.Size(77, 17);
            this.radioButtonTranslation.TabIndex = 0;
            this.radioButtonTranslation.TabStop = true;
            this.radioButtonTranslation.Text = "Translation";
            this.radioButtonTranslation.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.buttonEvaluation);
            this.splitContainer2.Panel1.Controls.Add(this.buttonStartRegistration);
            this.splitContainer2.Panel1.Controls.Add(this.buttonCancel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textBox1);
            this.splitContainer2.Panel2.Controls.Add(this.progressBar1);
            this.splitContainer2.Size = new System.Drawing.Size(786, 189);
            this.splitContainer2.SplitterDistance = 148;
            this.splitContainer2.TabIndex = 2;
            // 
            // buttonStartRegistration
            // 
            this.buttonStartRegistration.Location = new System.Drawing.Point(13, 13);
            this.buttonStartRegistration.Name = "buttonStartRegistration";
            this.buttonStartRegistration.Size = new System.Drawing.Size(121, 23);
            this.buttonStartRegistration.TabIndex = 0;
            this.buttonStartRegistration.Text = "Start Registration";
            this.buttonStartRegistration.UseVisualStyleBackColor = true;
            this.buttonStartRegistration.Click += new System.EventHandler(this.buttonStartRegistration_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(13, 42);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(121, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel Registration";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(20, 45);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(596, 130);
            this.textBox1.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(20, 13);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(596, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // tabPageNonRigid
            // 
            this.tabPageNonRigid.Location = new System.Drawing.Point(4, 22);
            this.tabPageNonRigid.Name = "tabPageNonRigid";
            this.tabPageNonRigid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNonRigid.Size = new System.Drawing.Size(792, 330);
            this.tabPageNonRigid.TabIndex = 1;
            this.tabPageNonRigid.Text = "Non rigid registration";
            this.tabPageNonRigid.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.label1);
            this.splitContainer3.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tabControlRegistration);
            this.splitContainer3.Size = new System.Drawing.Size(800, 507);
            this.splitContainer3.SplitterDistance = 147;
            this.splitContainer3.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "General registration settings";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonUsePrevInStack);
            this.groupBox3.Controls.Add(this.radioButton3);
            this.groupBox3.Controls.Add(this.radioButtonLastInStack);
            this.groupBox3.Controls.Add(this.radioButtonFirstFromStack);
            this.groupBox3.Location = new System.Drawing.Point(12, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(245, 113);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Registration order";
            // 
            // radioButtonFirstFromStack
            // 
            this.radioButtonFirstFromStack.AutoSize = true;
            this.radioButtonFirstFromStack.Location = new System.Drawing.Point(8, 20);
            this.radioButtonFirstFromStack.Name = "radioButtonFirstFromStack";
            this.radioButtonFirstFromStack.Size = new System.Drawing.Size(196, 17);
            this.radioButtonFirstFromStack.TabIndex = 0;
            this.radioButtonFirstFromStack.TabStop = true;
            this.radioButtonFirstFromStack.Text = "Use first image in stack as reference";
            this.radioButtonFirstFromStack.UseVisualStyleBackColor = true;
            // 
            // radioButtonLastInStack
            // 
            this.radioButtonLastInStack.AutoSize = true;
            this.radioButtonLastInStack.Location = new System.Drawing.Point(7, 44);
            this.radioButtonLastInStack.Name = "radioButtonLastInStack";
            this.radioButtonLastInStack.Size = new System.Drawing.Size(196, 17);
            this.radioButtonLastInStack.TabIndex = 1;
            this.radioButtonLastInStack.TabStop = true;
            this.radioButtonLastInStack.Text = "Use last image in stack as reference";
            this.radioButtonLastInStack.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(7, 68);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(210, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Use middle image in stack as reference";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButtonUsePrevInStack
            // 
            this.radioButtonUsePrevInStack.AutoSize = true;
            this.radioButtonUsePrevInStack.Location = new System.Drawing.Point(8, 90);
            this.radioButtonUsePrevInStack.Name = "radioButtonUsePrevInStack";
            this.radioButtonUsePrevInStack.Size = new System.Drawing.Size(220, 17);
            this.radioButtonUsePrevInStack.TabIndex = 3;
            this.radioButtonUsePrevInStack.TabStop = true;
            this.radioButtonUsePrevInStack.Text = "Use previous image in stack as reference";
            this.radioButtonUsePrevInStack.UseVisualStyleBackColor = true;
            // 
            // buttonEvaluation
            // 
            this.buttonEvaluation.Location = new System.Drawing.Point(13, 151);
            this.buttonEvaluation.Name = "buttonEvaluation";
            this.buttonEvaluation.Size = new System.Drawing.Size(121, 23);
            this.buttonEvaluation.TabIndex = 2;
            this.buttonEvaluation.Text = "Evaluate Registration";
            this.buttonEvaluation.UseVisualStyleBackColor = true;
            this.buttonEvaluation.Click += new System.EventHandler(this.buttonEvaluation_Click);
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 507);
            this.Controls.Add(this.splitContainer3);
            this.Name = "RegistrationForm";
            this.Text = "RegistrationForm";
            this.Load += new System.EventHandler(this.RegistrationForm_Load);
            this.tabControlRegistration.ResumeLayout(false);
            this.tabPageRigid.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlRegistration;
        private System.Windows.Forms.TabPage tabPageRigid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPageNonRigid;
        private System.Windows.Forms.Button buttonStartRegistration;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonAffine;
        private System.Windows.Forms.RadioButton radioButtonRigid;
        private System.Windows.Forms.RadioButton radioButtonSimilarity;
        private System.Windows.Forms.RadioButton radioButtonTranslation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonEditParameters;
        private System.Windows.Forms.Button buttonSegmentationParams;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RadioButton radioButtonUseFixed;
        private System.Windows.Forms.RadioButton radioButtonNoMasks;
        private System.Windows.Forms.RadioButton radioButtonRegMasks;
        private System.Windows.Forms.RadioButton radioButtonUsefixMov;
        private System.Windows.Forms.RadioButton radioButtonUseMoving;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonUsePrevInStack;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButtonLastInStack;
        private System.Windows.Forms.RadioButton radioButtonFirstFromStack;
        private System.Windows.Forms.Button buttonEvaluation;
    }
}