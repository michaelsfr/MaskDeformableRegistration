namespace MaskedDeformableRegistrationApp.Forms
{
    partial class PreSegmentationForm
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
            this.groupBoxExtract = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonCreateStack = new System.Windows.Forms.Button();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.trackBarResolutuion = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSegment = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.hScrollBarMaxContourSize = new System.Windows.Forms.HScrollBar();
            this.hScrollBarMinContourSize = new System.Windows.Forms.HScrollBar();
            this.hScrollBarThreshold = new System.Windows.Forms.HScrollBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonVertAsc = new System.Windows.Forms.RadioButton();
            this.radioButtonVertDesc = new System.Windows.Forms.RadioButton();
            this.radioButtonHorzAsc = new System.Windows.Forms.RadioButton();
            this.radioButtonHorzDesc = new System.Windows.Forms.RadioButton();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxExtract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarResolutuion)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxExtract);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1021, 581);
            this.splitContainer1.SplitterDistance = 144;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBoxExtract
            // 
            this.groupBoxExtract.Controls.Add(this.numericUpDown1);
            this.groupBoxExtract.Controls.Add(this.label5);
            this.groupBoxExtract.Controls.Add(this.label4);
            this.groupBoxExtract.Controls.Add(this.textBox1);
            this.groupBoxExtract.Controls.Add(this.buttonCreateStack);
            this.groupBoxExtract.Controls.Add(this.buttonProceed);
            this.groupBoxExtract.Controls.Add(this.trackBarResolutuion);
            this.groupBoxExtract.Location = new System.Drawing.Point(654, 12);
            this.groupBoxExtract.Name = "groupBoxExtract";
            this.groupBoxExtract.Size = new System.Drawing.Size(355, 126);
            this.groupBoxExtract.TabIndex = 1;
            this.groupBoxExtract.TabStop = false;
            this.groupBoxExtract.Text = "Exraction";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(153, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Resolution";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Stack name";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(194, 20);
            this.textBox1.TabIndex = 0;
            // 
            // buttonCreateStack
            // 
            this.buttonCreateStack.Location = new System.Drawing.Point(236, 53);
            this.buttonCreateStack.Name = "buttonCreateStack";
            this.buttonCreateStack.Size = new System.Drawing.Size(104, 23);
            this.buttonCreateStack.TabIndex = 2;
            this.buttonCreateStack.Text = "Create PNG stack";
            this.buttonCreateStack.UseVisualStyleBackColor = true;
            this.buttonCreateStack.Click += new System.EventHandler(this.buttonCreateStack_Click);
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(236, 81);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(104, 23);
            this.buttonProceed.TabIndex = 1;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // trackBarResolutuion
            // 
            this.trackBarResolutuion.Location = new System.Drawing.Point(16, 67);
            this.trackBarResolutuion.Name = "trackBarResolutuion";
            this.trackBarResolutuion.Size = new System.Drawing.Size(134, 45);
            this.trackBarResolutuion.TabIndex = 0;
            this.trackBarResolutuion.ValueChanged += new System.EventHandler(this.trackBarResolutuion_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSegment);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(635, 126);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Segmentation";
            // 
            // buttonSegment
            // 
            this.buttonSegment.Location = new System.Drawing.Point(505, 84);
            this.buttonSegment.Name = "buttonSegment";
            this.buttonSegment.Size = new System.Drawing.Size(112, 23);
            this.buttonSegment.TabIndex = 2;
            this.buttonSegment.Text = "Segment";
            this.buttonSegment.UseVisualStyleBackColor = true;
            this.buttonSegment.Click += new System.EventHandler(this.buttonSegment_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.hScrollBarMaxContourSize);
            this.groupBox3.Controls.Add(this.hScrollBarMinContourSize);
            this.groupBox3.Controls.Add(this.hScrollBarThreshold);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(245, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(243, 94);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thresholding";
            // 
            // hScrollBarMaxContourSize
            // 
            this.hScrollBarMaxContourSize.Location = new System.Drawing.Point(107, 67);
            this.hScrollBarMaxContourSize.Name = "hScrollBarMaxContourSize";
            this.hScrollBarMaxContourSize.Size = new System.Drawing.Size(120, 17);
            this.hScrollBarMaxContourSize.TabIndex = 5;
            // 
            // hScrollBarMinContourSize
            // 
            this.hScrollBarMinContourSize.Location = new System.Drawing.Point(107, 43);
            this.hScrollBarMinContourSize.Name = "hScrollBarMinContourSize";
            this.hScrollBarMinContourSize.Size = new System.Drawing.Size(120, 17);
            this.hScrollBarMinContourSize.TabIndex = 4;
            // 
            // hScrollBarThreshold
            // 
            this.hScrollBarThreshold.Location = new System.Drawing.Point(107, 19);
            this.hScrollBarThreshold.Name = "hScrollBarThreshold";
            this.hScrollBarThreshold.Size = new System.Drawing.Size(120, 17);
            this.hScrollBarThreshold.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Max contour size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Min contour size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Threshold";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonVertAsc);
            this.groupBox2.Controls.Add(this.radioButtonVertDesc);
            this.groupBox2.Controls.Add(this.radioButtonHorzAsc);
            this.groupBox2.Controls.Add(this.radioButtonHorzDesc);
            this.groupBox2.Controls.Add(this.radioButtonNone);
            this.groupBox2.Location = new System.Drawing.Point(9, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(229, 95);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Serialization";
            // 
            // radioButtonVertAsc
            // 
            this.radioButtonVertAsc.AutoSize = true;
            this.radioButtonVertAsc.Location = new System.Drawing.Point(129, 44);
            this.radioButtonVertAsc.Name = "radioButtonVertAsc";
            this.radioButtonVertAsc.Size = new System.Drawing.Size(84, 17);
            this.radioButtonVertAsc.TabIndex = 4;
            this.radioButtonVertAsc.Text = "Vertical ASC";
            this.radioButtonVertAsc.UseVisualStyleBackColor = true;
            this.radioButtonVertAsc.CheckedChanged += new System.EventHandler(this.radioButtonVertAsc_CheckedChanged);
            // 
            // radioButtonVertDesc
            // 
            this.radioButtonVertDesc.AutoSize = true;
            this.radioButtonVertDesc.Location = new System.Drawing.Point(129, 20);
            this.radioButtonVertDesc.Name = "radioButtonVertDesc";
            this.radioButtonVertDesc.Size = new System.Drawing.Size(85, 17);
            this.radioButtonVertDesc.TabIndex = 3;
            this.radioButtonVertDesc.Text = "Vertical DSC";
            this.radioButtonVertDesc.UseVisualStyleBackColor = true;
            this.radioButtonVertDesc.CheckedChanged += new System.EventHandler(this.radioButtonVertDesc_CheckedChanged);
            // 
            // radioButtonHorzAsc
            // 
            this.radioButtonHorzAsc.AutoSize = true;
            this.radioButtonHorzAsc.Location = new System.Drawing.Point(7, 68);
            this.radioButtonHorzAsc.Name = "radioButtonHorzAsc";
            this.radioButtonHorzAsc.Size = new System.Drawing.Size(98, 17);
            this.radioButtonHorzAsc.TabIndex = 2;
            this.radioButtonHorzAsc.Text = "Horizonzal ASC";
            this.radioButtonHorzAsc.UseVisualStyleBackColor = true;
            this.radioButtonHorzAsc.CheckedChanged += new System.EventHandler(this.radioButtonHorzAsc_CheckedChanged);
            // 
            // radioButtonHorzDesc
            // 
            this.radioButtonHorzDesc.AutoSize = true;
            this.radioButtonHorzDesc.Location = new System.Drawing.Point(7, 44);
            this.radioButtonHorzDesc.Name = "radioButtonHorzDesc";
            this.radioButtonHorzDesc.Size = new System.Drawing.Size(97, 17);
            this.radioButtonHorzDesc.TabIndex = 1;
            this.radioButtonHorzDesc.Text = "Horizontal DSC";
            this.radioButtonHorzDesc.UseVisualStyleBackColor = true;
            this.radioButtonHorzDesc.CheckedChanged += new System.EventHandler(this.radioButtonHorzDesc_CheckedChanged);
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.AutoSize = true;
            this.radioButtonNone.Checked = true;
            this.radioButtonNone.Location = new System.Drawing.Point(7, 20);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(51, 17);
            this.radioButtonNone.TabIndex = 0;
            this.radioButtonNone.TabStop = true;
            this.radioButtonNone.Text = "None";
            this.radioButtonNone.UseVisualStyleBackColor = true;
            this.radioButtonNone.CheckedChanged += new System.EventHandler(this.radioButtonNone_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1021, 433);
            this.panel1.TabIndex = 0;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(157, 81);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(53, 20);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // PreSegmentationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 581);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PreSegmentationForm";
            this.Text = "PreSegmentationForm";
            this.Load += new System.EventHandler(this.PreSegmentationForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PreSegmentationForm_Paint);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxExtract.ResumeLayout(false);
            this.groupBoxExtract.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarResolutuion)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonVertAsc;
        private System.Windows.Forms.RadioButton radioButtonVertDesc;
        private System.Windows.Forms.RadioButton radioButtonHorzAsc;
        private System.Windows.Forms.RadioButton radioButtonHorzDesc;
        private System.Windows.Forms.RadioButton radioButtonNone;
        private System.Windows.Forms.HScrollBar hScrollBarMaxContourSize;
        private System.Windows.Forms.HScrollBar hScrollBarMinContourSize;
        private System.Windows.Forms.HScrollBar hScrollBarThreshold;
        private System.Windows.Forms.Button buttonSegment;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBoxExtract;
        private System.Windows.Forms.Button buttonCreateStack;
        private System.Windows.Forms.Button buttonProceed;
        private System.Windows.Forms.TrackBar trackBarResolutuion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}