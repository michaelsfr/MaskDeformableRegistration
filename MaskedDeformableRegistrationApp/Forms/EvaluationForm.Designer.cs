namespace MaskedDeformableRegistrationApp.Forms
{
    partial class EvaluationForm
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
            this.textBoxFixedPointSet = new System.Windows.Forms.TextBox();
            this.textBoxMovingPointSet = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxMoving = new System.Windows.Forms.ComboBox();
            this.buttonChooseFixedPS = new System.Windows.Forms.Button();
            this.buttonChooseMovPS = new System.Windows.Forms.Button();
            this.buttonCalcDistances = new System.Windows.Forms.Button();
            this.openFileDialogFixed = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogMoving = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxEuclidDist = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBoxDiceJaccard = new System.Windows.Forms.GroupBox();
            this.groupBoxMisc = new System.Windows.Forms.GroupBox();
            this.buttonDiffImage = new System.Windows.Forms.Button();
            this.buttonCheckerBoard = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelAbsDiff = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelMeanDiff = new System.Windows.Forms.Label();
            this.comboBoxMovingImage = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxSegmentationParams = new System.Windows.Forms.ComboBox();
            this.buttonCalcCoef = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxEuclidDist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBoxDiceJaccard.SuspendLayout();
            this.groupBoxMisc.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFixedPointSet
            // 
            this.textBoxFixedPointSet.Location = new System.Drawing.Point(16, 36);
            this.textBoxFixedPointSet.Name = "textBoxFixedPointSet";
            this.textBoxFixedPointSet.Size = new System.Drawing.Size(129, 20);
            this.textBoxFixedPointSet.TabIndex = 0;
            // 
            // textBoxMovingPointSet
            // 
            this.textBoxMovingPointSet.Location = new System.Drawing.Point(16, 116);
            this.textBoxMovingPointSet.Name = "textBoxMovingPointSet";
            this.textBoxMovingPointSet.Size = new System.Drawing.Size(129, 20);
            this.textBoxMovingPointSet.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fixed image point set:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Moving image and corresponding point set";
            // 
            // comboBoxMoving
            // 
            this.comboBoxMoving.FormattingEnabled = true;
            this.comboBoxMoving.Location = new System.Drawing.Point(16, 89);
            this.comboBoxMoving.Name = "comboBoxMoving";
            this.comboBoxMoving.Size = new System.Drawing.Size(129, 21);
            this.comboBoxMoving.TabIndex = 4;
            // 
            // buttonChooseFixedPS
            // 
            this.buttonChooseFixedPS.Location = new System.Drawing.Point(162, 33);
            this.buttonChooseFixedPS.Name = "buttonChooseFixedPS";
            this.buttonChooseFixedPS.Size = new System.Drawing.Size(76, 23);
            this.buttonChooseFixedPS.TabIndex = 5;
            this.buttonChooseFixedPS.Text = "Choose";
            this.buttonChooseFixedPS.UseVisualStyleBackColor = true;
            this.buttonChooseFixedPS.Click += new System.EventHandler(this.buttonChooseFixedPS_Click);
            // 
            // buttonChooseMovPS
            // 
            this.buttonChooseMovPS.Location = new System.Drawing.Point(162, 114);
            this.buttonChooseMovPS.Name = "buttonChooseMovPS";
            this.buttonChooseMovPS.Size = new System.Drawing.Size(76, 23);
            this.buttonChooseMovPS.TabIndex = 6;
            this.buttonChooseMovPS.Text = "Choose";
            this.buttonChooseMovPS.UseVisualStyleBackColor = true;
            this.buttonChooseMovPS.Click += new System.EventHandler(this.buttonChooseMovPS_Click);
            // 
            // buttonCalcDistances
            // 
            this.buttonCalcDistances.Location = new System.Drawing.Point(16, 155);
            this.buttonCalcDistances.Name = "buttonCalcDistances";
            this.buttonCalcDistances.Size = new System.Drawing.Size(222, 23);
            this.buttonCalcDistances.TabIndex = 7;
            this.buttonCalcDistances.Text = "Calculate distances";
            this.buttonCalcDistances.UseVisualStyleBackColor = true;
            this.buttonCalcDistances.Click += new System.EventHandler(this.buttonCalcMetrics_Click);
            // 
            // openFileDialogFixed
            // 
            this.openFileDialogFixed.FileName = "openFileDialog1";
            // 
            // openFileDialogMoving
            // 
            this.openFileDialogMoving.FileName = "openFileDialog1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxEuclidDist);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(737, 264);
            this.splitContainer1.SplitterDistance = 261;
            this.splitContainer1.TabIndex = 8;
            // 
            // groupBoxEuclidDist
            // 
            this.groupBoxEuclidDist.Controls.Add(this.labelMeanDiff);
            this.groupBoxEuclidDist.Controls.Add(this.label5);
            this.groupBoxEuclidDist.Controls.Add(this.labelAbsDiff);
            this.groupBoxEuclidDist.Controls.Add(this.label3);
            this.groupBoxEuclidDist.Controls.Add(this.textBoxFixedPointSet);
            this.groupBoxEuclidDist.Controls.Add(this.label1);
            this.groupBoxEuclidDist.Controls.Add(this.comboBoxMoving);
            this.groupBoxEuclidDist.Controls.Add(this.buttonCalcDistances);
            this.groupBoxEuclidDist.Controls.Add(this.label2);
            this.groupBoxEuclidDist.Controls.Add(this.buttonChooseFixedPS);
            this.groupBoxEuclidDist.Controls.Add(this.buttonChooseMovPS);
            this.groupBoxEuclidDist.Controls.Add(this.textBoxMovingPointSet);
            this.groupBoxEuclidDist.Location = new System.Drawing.Point(3, 3);
            this.groupBoxEuclidDist.Name = "groupBoxEuclidDist";
            this.groupBoxEuclidDist.Size = new System.Drawing.Size(251, 258);
            this.groupBoxEuclidDist.TabIndex = 8;
            this.groupBoxEuclidDist.TabStop = false;
            this.groupBoxEuclidDist.Text = "Euclidean distance";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBoxDiceJaccard);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBoxMisc);
            this.splitContainer2.Size = new System.Drawing.Size(472, 264);
            this.splitContainer2.SplitterDistance = 265;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBoxDiceJaccard
            // 
            this.groupBoxDiceJaccard.Controls.Add(this.buttonCalcCoef);
            this.groupBoxDiceJaccard.Controls.Add(this.comboBoxSegmentationParams);
            this.groupBoxDiceJaccard.Controls.Add(this.label8);
            this.groupBoxDiceJaccard.Controls.Add(this.label7);
            this.groupBoxDiceJaccard.Controls.Add(this.comboBoxMovingImage);
            this.groupBoxDiceJaccard.Location = new System.Drawing.Point(4, 3);
            this.groupBoxDiceJaccard.Name = "groupBoxDiceJaccard";
            this.groupBoxDiceJaccard.Size = new System.Drawing.Size(258, 258);
            this.groupBoxDiceJaccard.TabIndex = 0;
            this.groupBoxDiceJaccard.TabStop = false;
            this.groupBoxDiceJaccard.Text = "Dice-Jaccard-Coefficient";
            // 
            // groupBoxMisc
            // 
            this.groupBoxMisc.Controls.Add(this.buttonCheckerBoard);
            this.groupBoxMisc.Controls.Add(this.buttonDiffImage);
            this.groupBoxMisc.Location = new System.Drawing.Point(3, 3);
            this.groupBoxMisc.Name = "groupBoxMisc";
            this.groupBoxMisc.Size = new System.Drawing.Size(197, 258);
            this.groupBoxMisc.TabIndex = 0;
            this.groupBoxMisc.TabStop = false;
            this.groupBoxMisc.Text = "Misc";
            // 
            // buttonDiffImage
            // 
            this.buttonDiffImage.Location = new System.Drawing.Point(6, 186);
            this.buttonDiffImage.Name = "buttonDiffImage";
            this.buttonDiffImage.Size = new System.Drawing.Size(185, 23);
            this.buttonDiffImage.TabIndex = 0;
            this.buttonDiffImage.Text = "Create Difference-Image";
            this.buttonDiffImage.UseVisualStyleBackColor = true;
            this.buttonDiffImage.Click += new System.EventHandler(this.buttonDiffImage_Click);
            // 
            // buttonCheckerBoard
            // 
            this.buttonCheckerBoard.Location = new System.Drawing.Point(6, 215);
            this.buttonCheckerBoard.Name = "buttonCheckerBoard";
            this.buttonCheckerBoard.Size = new System.Drawing.Size(185, 23);
            this.buttonCheckerBoard.TabIndex = 1;
            this.buttonCheckerBoard.Text = "Create Checkerboard";
            this.buttonCheckerBoard.UseVisualStyleBackColor = true;
            this.buttonCheckerBoard.Click += new System.EventHandler(this.buttonCheckerBoard_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Absolute difference:";
            // 
            // labelAbsDiff
            // 
            this.labelAbsDiff.AutoSize = true;
            this.labelAbsDiff.Location = new System.Drawing.Point(159, 196);
            this.labelAbsDiff.Name = "labelAbsDiff";
            this.labelAbsDiff.Size = new System.Drawing.Size(13, 13);
            this.labelAbsDiff.TabIndex = 9;
            this.labelAbsDiff.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Mean difference:";
            // 
            // labelMeanDiff
            // 
            this.labelMeanDiff.AutoSize = true;
            this.labelMeanDiff.Location = new System.Drawing.Point(159, 215);
            this.labelMeanDiff.Name = "labelMeanDiff";
            this.labelMeanDiff.Size = new System.Drawing.Size(13, 13);
            this.labelMeanDiff.TabIndex = 11;
            this.labelMeanDiff.Text = "0";
            // 
            // comboBoxMovingImage
            // 
            this.comboBoxMovingImage.FormattingEnabled = true;
            this.comboBoxMovingImage.Location = new System.Drawing.Point(16, 36);
            this.comboBoxMovingImage.Name = "comboBoxMovingImage";
            this.comboBoxMovingImage.Size = new System.Drawing.Size(224, 21);
            this.comboBoxMovingImage.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Choose moving image to compare:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Segmentation parameters:";
            // 
            // comboBoxSegmentationParams
            // 
            this.comboBoxSegmentationParams.FormattingEnabled = true;
            this.comboBoxSegmentationParams.Location = new System.Drawing.Point(16, 89);
            this.comboBoxSegmentationParams.Name = "comboBoxSegmentationParams";
            this.comboBoxSegmentationParams.Size = new System.Drawing.Size(224, 21);
            this.comboBoxSegmentationParams.TabIndex = 8;
            // 
            // buttonCalcCoef
            // 
            this.buttonCalcCoef.Location = new System.Drawing.Point(16, 126);
            this.buttonCalcCoef.Name = "buttonCalcCoef";
            this.buttonCalcCoef.Size = new System.Drawing.Size(224, 23);
            this.buttonCalcCoef.TabIndex = 9;
            this.buttonCalcCoef.Text = "Calculate coefficients";
            this.buttonCalcCoef.UseVisualStyleBackColor = true;
            this.buttonCalcCoef.Click += new System.EventHandler(this.buttonCalcCoef_Click);
            // 
            // EvaluationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 264);
            this.Controls.Add(this.splitContainer1);
            this.Name = "EvaluationForm";
            this.Text = "EvaluationForm";
            this.Load += new System.EventHandler(this.EvaluationForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxEuclidDist.ResumeLayout(false);
            this.groupBoxEuclidDist.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBoxDiceJaccard.ResumeLayout(false);
            this.groupBoxDiceJaccard.PerformLayout();
            this.groupBoxMisc.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFixedPointSet;
        private System.Windows.Forms.TextBox textBoxMovingPointSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxMoving;
        private System.Windows.Forms.Button buttonChooseFixedPS;
        private System.Windows.Forms.Button buttonChooseMovPS;
        private System.Windows.Forms.Button buttonCalcDistances;
        private System.Windows.Forms.OpenFileDialog openFileDialogFixed;
        private System.Windows.Forms.OpenFileDialog openFileDialogMoving;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxEuclidDist;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBoxDiceJaccard;
        private System.Windows.Forms.GroupBox groupBoxMisc;
        private System.Windows.Forms.Button buttonCheckerBoard;
        private System.Windows.Forms.Button buttonDiffImage;
        private System.Windows.Forms.Label labelMeanDiff;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelAbsDiff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCalcCoef;
        private System.Windows.Forms.ComboBox comboBoxSegmentationParams;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxMovingImage;
    }
}