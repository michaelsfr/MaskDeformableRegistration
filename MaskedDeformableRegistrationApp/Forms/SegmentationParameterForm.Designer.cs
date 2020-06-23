namespace MaskedDeformableRegistrationApp.Forms
{
    partial class SegmentationParameterForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxContourSize = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.radioButtonThresholdManually = new System.Windows.Forms.RadioButton();
            this.radioButtonOtsu = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonPreviewColorSpace = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxChannel = new System.Windows.Forms.ComboBox();
            this.comboBoxColorspace = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonSaveParameters = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.pictureBoxColorChannel = new System.Windows.Forms.PictureBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxSegmentation1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxSegmentation2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColorChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSegmentation1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSegmentation2)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.buttonPreview);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSaveParameters);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(856, 510);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxContourSize);
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Location = new System.Drawing.Point(13, 266);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(220, 154);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Misc";
            // 
            // checkBoxContourSize
            // 
            this.checkBoxContourSize.AutoSize = true;
            this.checkBoxContourSize.Location = new System.Drawing.Point(12, 42);
            this.checkBoxContourSize.Name = "checkBoxContourSize";
            this.checkBoxContourSize.Size = new System.Drawing.Size(189, 17);
            this.checkBoxContourSize.TabIndex = 1;
            this.checkBoxContourSize.Text = "Set contour min/max size manually";
            this.checkBoxContourSize.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(129, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Use Kmeas-Clustering";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelThreshold);
            this.groupBox2.Controls.Add(this.trackBar1);
            this.groupBox2.Controls.Add(this.radioButtonThresholdManually);
            this.groupBox2.Controls.Add(this.radioButtonOtsu);
            this.groupBox2.Location = new System.Drawing.Point(13, 140);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 119);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Threshold";
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(164, 67);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(28, 13);
            this.labelThreshold.TabIndex = 3;
            this.labelThreshold.Text = "###";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(30, 68);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(128, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // radioButtonThresholdManually
            // 
            this.radioButtonThresholdManually.AutoSize = true;
            this.radioButtonThresholdManually.Location = new System.Drawing.Point(12, 44);
            this.radioButtonThresholdManually.Name = "radioButtonThresholdManually";
            this.radioButtonThresholdManually.Size = new System.Drawing.Size(131, 17);
            this.radioButtonThresholdManually.TabIndex = 1;
            this.radioButtonThresholdManually.TabStop = true;
            this.radioButtonThresholdManually.Text = "Set threshold manually";
            this.radioButtonThresholdManually.UseVisualStyleBackColor = true;
            this.radioButtonThresholdManually.CheckedChanged += new System.EventHandler(this.radioButtonThresholdManually_CheckedChanged);
            // 
            // radioButtonOtsu
            // 
            this.radioButtonOtsu.AutoSize = true;
            this.radioButtonOtsu.Location = new System.Drawing.Point(12, 20);
            this.radioButtonOtsu.Name = "radioButtonOtsu";
            this.radioButtonOtsu.Size = new System.Drawing.Size(129, 17);
            this.radioButtonOtsu.TabIndex = 0;
            this.radioButtonOtsu.TabStop = true;
            this.radioButtonOtsu.Text = "Use Otsu thresholding";
            this.radioButtonOtsu.UseVisualStyleBackColor = true;
            this.radioButtonOtsu.CheckedChanged += new System.EventHandler(this.radioButtonOtsu_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonPreviewColorSpace);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxChannel);
            this.groupBox1.Controls.Add(this.comboBoxColorspace);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 120);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Color space / channel";
            // 
            // buttonPreviewColorSpace
            // 
            this.buttonPreviewColorSpace.Location = new System.Drawing.Point(104, 83);
            this.buttonPreviewColorSpace.Name = "buttonPreviewColorSpace";
            this.buttonPreviewColorSpace.Size = new System.Drawing.Size(99, 23);
            this.buttonPreviewColorSpace.TabIndex = 6;
            this.buttonPreviewColorSpace.Text = "Preview";
            this.buttonPreviewColorSpace.UseVisualStyleBackColor = true;
            this.buttonPreviewColorSpace.Click += new System.EventHandler(this.buttonPreviewColorSpace_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Convert to space:";
            // 
            // comboBoxChannel
            // 
            this.comboBoxChannel.FormattingEnabled = true;
            this.comboBoxChannel.Location = new System.Drawing.Point(6, 85);
            this.comboBoxChannel.Name = "comboBoxChannel";
            this.comboBoxChannel.Size = new System.Drawing.Size(83, 21);
            this.comboBoxChannel.TabIndex = 5;
            this.comboBoxChannel.SelectedIndexChanged += new System.EventHandler(this.comboBoxChannel_SelectedIndexChanged);
            // 
            // comboBoxColorspace
            // 
            this.comboBoxColorspace.FormattingEnabled = true;
            this.comboBoxColorspace.Location = new System.Drawing.Point(6, 40);
            this.comboBoxColorspace.Name = "comboBoxColorspace";
            this.comboBoxColorspace.Size = new System.Drawing.Size(197, 21);
            this.comboBoxColorspace.TabIndex = 2;
            this.comboBoxColorspace.SelectedIndexChanged += new System.EventHandler(this.comboBoxColorspace_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Channel:";
            // 
            // buttonPreview
            // 
            this.buttonPreview.Location = new System.Drawing.Point(12, 437);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(220, 23);
            this.buttonPreview.TabIndex = 1;
            this.buttonPreview.Text = "Preview";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonSaveParameters
            // 
            this.buttonSaveParameters.Location = new System.Drawing.Point(12, 466);
            this.buttonSaveParameters.Name = "buttonSaveParameters";
            this.buttonSaveParameters.Size = new System.Drawing.Size(220, 23);
            this.buttonSaveParameters.TabIndex = 0;
            this.buttonSaveParameters.Text = "Save parameters";
            this.buttonSaveParameters.UseVisualStyleBackColor = true;
            this.buttonSaveParameters.Click += new System.EventHandler(this.buttonSaveParameters_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(612, 510);
            this.splitContainer2.SplitterDistance = 249;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SizeChanged += new System.EventHandler(this.splitContainer2_SizeChanged);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.pictureBoxOriginal);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.pictureBoxColorChannel);
            this.splitContainer4.Size = new System.Drawing.Size(612, 249);
            this.splitContainer4.SplitterDistance = 306;
            this.splitContainer4.TabIndex = 0;
            this.splitContainer4.SizeChanged += new System.EventHandler(this.splitContainer4_SizeChanged);
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOriginal.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(306, 249);
            this.pictureBoxOriginal.TabIndex = 0;
            this.pictureBoxOriginal.TabStop = false;
            // 
            // pictureBoxColorChannel
            // 
            this.pictureBoxColorChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxColorChannel.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxColorChannel.Name = "pictureBoxColorChannel";
            this.pictureBoxColorChannel.Size = new System.Drawing.Size(302, 249);
            this.pictureBoxColorChannel.TabIndex = 0;
            this.pictureBoxColorChannel.TabStop = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.pictureBoxSegmentation1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.pictureBoxSegmentation2);
            this.splitContainer3.Size = new System.Drawing.Size(612, 257);
            this.splitContainer3.SplitterDistance = 307;
            this.splitContainer3.TabIndex = 0;
            this.splitContainer3.SizeChanged += new System.EventHandler(this.splitContainer3_SizeChanged);
            // 
            // pictureBoxSegmentation1
            // 
            this.pictureBoxSegmentation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSegmentation1.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSegmentation1.Name = "pictureBoxSegmentation1";
            this.pictureBoxSegmentation1.Size = new System.Drawing.Size(307, 257);
            this.pictureBoxSegmentation1.TabIndex = 0;
            this.pictureBoxSegmentation1.TabStop = false;
            // 
            // pictureBoxSegmentation2
            // 
            this.pictureBoxSegmentation2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSegmentation2.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSegmentation2.Name = "pictureBoxSegmentation2";
            this.pictureBoxSegmentation2.Size = new System.Drawing.Size(301, 257);
            this.pictureBoxSegmentation2.TabIndex = 0;
            this.pictureBoxSegmentation2.TabStop = false;
            // 
            // SegmentationParameterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 510);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SegmentationParameterForm";
            this.Text = "SegmentationParameterForm";
            this.Load += new System.EventHandler(this.SegmentationParameterForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColorChannel)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSegmentation1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSegmentation2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonSaveParameters;
        private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.PictureBox pictureBoxSegmentation1;
        private System.Windows.Forms.PictureBox pictureBoxSegmentation2;
        private System.Windows.Forms.ComboBox comboBoxChannel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxColorspace;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.PictureBox pictureBoxColorChannel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonPreviewColorSpace;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.RadioButton radioButtonThresholdManually;
        private System.Windows.Forms.RadioButton radioButtonOtsu;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBoxContourSize;
    }
}