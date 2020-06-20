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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.pictureBoxSegmentation1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxSegmentation2 = new System.Windows.Forms.PictureBox();
            this.buttonSaveParameters = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.comboBoxColorspace = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxChannel = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSegmentation1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSegmentation2)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxChannel);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxColorspace);
            this.splitContainer1.Panel1.Controls.Add(this.buttonPreview);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSaveParameters);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(649, 601);
            this.splitContainer1.SplitterDistance = 247;
            this.splitContainer1.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add(this.pictureBoxOriginal);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(398, 601);
            this.splitContainer2.SplitterDistance = 176;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SizeChanged += new System.EventHandler(this.splitContainer2_SizeChanged);
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
            this.splitContainer3.Panel1.Controls.Add(this.pictureBoxSegmentation1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.pictureBoxSegmentation2);
            this.splitContainer3.Size = new System.Drawing.Size(398, 421);
            this.splitContainer3.SplitterDistance = 214;
            this.splitContainer3.TabIndex = 0;
            this.splitContainer3.SizeChanged += new System.EventHandler(this.splitContainer3_SizeChanged);
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOriginal.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(398, 176);
            this.pictureBoxOriginal.TabIndex = 0;
            this.pictureBoxOriginal.TabStop = false;
            // 
            // pictureBoxSegmentation1
            // 
            this.pictureBoxSegmentation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSegmentation1.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSegmentation1.Name = "pictureBoxSegmentation1";
            this.pictureBoxSegmentation1.Size = new System.Drawing.Size(398, 214);
            this.pictureBoxSegmentation1.TabIndex = 0;
            this.pictureBoxSegmentation1.TabStop = false;
            // 
            // pictureBoxSegmentation2
            // 
            this.pictureBoxSegmentation2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSegmentation2.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSegmentation2.Name = "pictureBoxSegmentation2";
            this.pictureBoxSegmentation2.Size = new System.Drawing.Size(398, 203);
            this.pictureBoxSegmentation2.TabIndex = 0;
            this.pictureBoxSegmentation2.TabStop = false;
            // 
            // buttonSaveParameters
            // 
            this.buttonSaveParameters.Location = new System.Drawing.Point(13, 566);
            this.buttonSaveParameters.Name = "buttonSaveParameters";
            this.buttonSaveParameters.Size = new System.Drawing.Size(220, 23);
            this.buttonSaveParameters.TabIndex = 0;
            this.buttonSaveParameters.Text = "Save parameters";
            this.buttonSaveParameters.UseVisualStyleBackColor = true;
            this.buttonSaveParameters.Click += new System.EventHandler(this.buttonSaveParameters_Click);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Location = new System.Drawing.Point(13, 537);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(220, 23);
            this.buttonPreview.TabIndex = 1;
            this.buttonPreview.Text = "Preview";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // comboBoxColorspace
            // 
            this.comboBoxColorspace.FormattingEnabled = true;
            this.comboBoxColorspace.Location = new System.Drawing.Point(13, 33);
            this.comboBoxColorspace.Name = "comboBoxColorspace";
            this.comboBoxColorspace.Size = new System.Drawing.Size(220, 21);
            this.comboBoxColorspace.TabIndex = 2;
            this.comboBoxColorspace.SelectedIndexChanged += new System.EventHandler(this.comboBoxColorspace_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Color space:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Channel:";
            // 
            // comboBoxChannel
            // 
            this.comboBoxChannel.FormattingEnabled = true;
            this.comboBoxChannel.Location = new System.Drawing.Point(13, 78);
            this.comboBoxChannel.Name = "comboBoxChannel";
            this.comboBoxChannel.Size = new System.Drawing.Size(106, 21);
            this.comboBoxChannel.TabIndex = 5;
            this.comboBoxChannel.SelectedIndexChanged += new System.EventHandler(this.comboBoxChannel_SelectedIndexChanged);
            // 
            // SegmentationParameterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 601);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SegmentationParameterForm";
            this.Text = "SegmentationParameterForm";
            this.Load += new System.EventHandler(this.SegmentationParameterForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
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
    }
}