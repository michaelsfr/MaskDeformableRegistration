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
            this.buttonTransformFixed = new System.Windows.Forms.Button();
            this.labelMax = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelStdDev = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelMeanDiff = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBoxDiceJaccard = new System.Windows.Forms.GroupBox();
            this.labelUnionOverlap = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.labelMeanOverlap = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.labelfalseNegPos = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelJacc = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelDice = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonCalcCoef = new System.Windows.Forms.Button();
            this.comboBoxSegmentationParams = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxMovingImage = new System.Windows.Forms.ComboBox();
            this.groupBoxMisc = new System.Windows.Forms.GroupBox();
            this.numericUpDownCheckerSize = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonCheckerBoard = new System.Windows.Forms.Button();
            this.buttonDiffImage = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCheckerSize)).BeginInit();
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
            this.comboBoxMoving.Size = new System.Drawing.Size(222, 21);
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
            this.groupBoxEuclidDist.Controls.Add(this.buttonTransformFixed);
            this.groupBoxEuclidDist.Controls.Add(this.labelMax);
            this.groupBoxEuclidDist.Controls.Add(this.label9);
            this.groupBoxEuclidDist.Controls.Add(this.labelStdDev);
            this.groupBoxEuclidDist.Controls.Add(this.label5);
            this.groupBoxEuclidDist.Controls.Add(this.labelMeanDiff);
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
            // buttonTransformFixed
            // 
            this.buttonTransformFixed.Location = new System.Drawing.Point(163, 9);
            this.buttonTransformFixed.Name = "buttonTransformFixed";
            this.buttonTransformFixed.Size = new System.Drawing.Size(75, 23);
            this.buttonTransformFixed.TabIndex = 14;
            this.buttonTransformFixed.Text = "Transf. fix";
            this.buttonTransformFixed.UseVisualStyleBackColor = true;
            this.buttonTransformFixed.Click += new System.EventHandler(this.buttonTransformFixed_Click);
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.Location = new System.Drawing.Point(189, 228);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(13, 13);
            this.labelMax.TabIndex = 13;
            this.labelMax.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 228);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Max TRE:";
            // 
            // labelStdDev
            // 
            this.labelStdDev.AutoSize = true;
            this.labelStdDev.Location = new System.Drawing.Point(189, 209);
            this.labelStdDev.Name = "labelStdDev";
            this.labelStdDev.Size = new System.Drawing.Size(13, 13);
            this.labelStdDev.TabIndex = 11;
            this.labelStdDev.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 209);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "StdDev TRE:";
            // 
            // labelMeanDiff
            // 
            this.labelMeanDiff.AutoSize = true;
            this.labelMeanDiff.Location = new System.Drawing.Point(189, 190);
            this.labelMeanDiff.Name = "labelMeanDiff";
            this.labelMeanDiff.Size = new System.Drawing.Size(13, 13);
            this.labelMeanDiff.TabIndex = 9;
            this.labelMeanDiff.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Mean TRE:";
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
            this.groupBoxDiceJaccard.Controls.Add(this.labelUnionOverlap);
            this.groupBoxDiceJaccard.Controls.Add(this.label16);
            this.groupBoxDiceJaccard.Controls.Add(this.labelMeanOverlap);
            this.groupBoxDiceJaccard.Controls.Add(this.label14);
            this.groupBoxDiceJaccard.Controls.Add(this.labelfalseNegPos);
            this.groupBoxDiceJaccard.Controls.Add(this.label12);
            this.groupBoxDiceJaccard.Controls.Add(this.labelJacc);
            this.groupBoxDiceJaccard.Controls.Add(this.label10);
            this.groupBoxDiceJaccard.Controls.Add(this.labelDice);
            this.groupBoxDiceJaccard.Controls.Add(this.label4);
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
            // labelUnionOverlap
            // 
            this.labelUnionOverlap.AutoSize = true;
            this.labelUnionOverlap.Location = new System.Drawing.Point(194, 236);
            this.labelUnionOverlap.Name = "labelUnionOverlap";
            this.labelUnionOverlap.Size = new System.Drawing.Size(13, 13);
            this.labelUnionOverlap.TabIndex = 19;
            this.labelUnionOverlap.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 235);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(73, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "Union overlap";
            // 
            // labelMeanOverlap
            // 
            this.labelMeanOverlap.AutoSize = true;
            this.labelMeanOverlap.Location = new System.Drawing.Point(194, 216);
            this.labelMeanOverlap.Name = "labelMeanOverlap";
            this.labelMeanOverlap.Size = new System.Drawing.Size(13, 13);
            this.labelMeanOverlap.TabIndex = 17;
            this.labelMeanOverlap.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 215);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "Mean overlap:";
            // 
            // labelfalseNegPos
            // 
            this.labelfalseNegPos.AutoSize = true;
            this.labelfalseNegPos.Location = new System.Drawing.Point(194, 197);
            this.labelfalseNegPos.Name = "labelfalseNegPos";
            this.labelfalseNegPos.Size = new System.Drawing.Size(13, 13);
            this.labelfalseNegPos.TabIndex = 15;
            this.labelfalseNegPos.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 196);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(129, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "False negative / positive: ";
            // 
            // labelJacc
            // 
            this.labelJacc.AutoSize = true;
            this.labelJacc.Location = new System.Drawing.Point(194, 178);
            this.labelJacc.Name = "labelJacc";
            this.labelJacc.Size = new System.Drawing.Size(13, 13);
            this.labelJacc.TabIndex = 13;
            this.labelJacc.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 177);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Jaccard coefficient:";
            // 
            // labelDice
            // 
            this.labelDice.AutoSize = true;
            this.labelDice.Location = new System.Drawing.Point(194, 159);
            this.labelDice.Name = "labelDice";
            this.labelDice.Size = new System.Drawing.Size(13, 13);
            this.labelDice.TabIndex = 11;
            this.labelDice.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Dice coefficient:";
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
            // comboBoxSegmentationParams
            // 
            this.comboBoxSegmentationParams.FormattingEnabled = true;
            this.comboBoxSegmentationParams.Location = new System.Drawing.Point(16, 89);
            this.comboBoxSegmentationParams.Name = "comboBoxSegmentationParams";
            this.comboBoxSegmentationParams.Size = new System.Drawing.Size(224, 21);
            this.comboBoxSegmentationParams.TabIndex = 8;
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Choose moving image to compare:";
            // 
            // comboBoxMovingImage
            // 
            this.comboBoxMovingImage.FormattingEnabled = true;
            this.comboBoxMovingImage.Location = new System.Drawing.Point(16, 36);
            this.comboBoxMovingImage.Name = "comboBoxMovingImage";
            this.comboBoxMovingImage.Size = new System.Drawing.Size(224, 21);
            this.comboBoxMovingImage.TabIndex = 5;
            // 
            // groupBoxMisc
            // 
            this.groupBoxMisc.Controls.Add(this.numericUpDownCheckerSize);
            this.groupBoxMisc.Controls.Add(this.label6);
            this.groupBoxMisc.Controls.Add(this.buttonCheckerBoard);
            this.groupBoxMisc.Controls.Add(this.buttonDiffImage);
            this.groupBoxMisc.Location = new System.Drawing.Point(3, 3);
            this.groupBoxMisc.Name = "groupBoxMisc";
            this.groupBoxMisc.Size = new System.Drawing.Size(197, 258);
            this.groupBoxMisc.TabIndex = 0;
            this.groupBoxMisc.TabStop = false;
            this.groupBoxMisc.Text = "Misc";
            // 
            // numericUpDownCheckerSize
            // 
            this.numericUpDownCheckerSize.Location = new System.Drawing.Point(13, 100);
            this.numericUpDownCheckerSize.Name = "numericUpDownCheckerSize";
            this.numericUpDownCheckerSize.Size = new System.Drawing.Size(95, 20);
            this.numericUpDownCheckerSize.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Checkerboard size:";
            // 
            // buttonCheckerBoard
            // 
            this.buttonCheckerBoard.Location = new System.Drawing.Point(10, 126);
            this.buttonCheckerBoard.Name = "buttonCheckerBoard";
            this.buttonCheckerBoard.Size = new System.Drawing.Size(181, 23);
            this.buttonCheckerBoard.TabIndex = 1;
            this.buttonCheckerBoard.Text = "Create Checkerboard";
            this.buttonCheckerBoard.UseVisualStyleBackColor = true;
            this.buttonCheckerBoard.Click += new System.EventHandler(this.buttonCheckerBoard_Click);
            // 
            // buttonDiffImage
            // 
            this.buttonDiffImage.Location = new System.Drawing.Point(6, 19);
            this.buttonDiffImage.Name = "buttonDiffImage";
            this.buttonDiffImage.Size = new System.Drawing.Size(185, 23);
            this.buttonDiffImage.TabIndex = 0;
            this.buttonDiffImage.Text = "Create Difference-Image";
            this.buttonDiffImage.UseVisualStyleBackColor = true;
            this.buttonDiffImage.Click += new System.EventHandler(this.buttonDiffImage_Click);
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
            this.groupBoxMisc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCheckerSize)).EndInit();
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
        private System.Windows.Forms.Label labelStdDev;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelMeanDiff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCalcCoef;
        private System.Windows.Forms.ComboBox comboBoxSegmentationParams;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxMovingImage;
        private System.Windows.Forms.Label labelUnionOverlap;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label labelMeanOverlap;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelfalseNegPos;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelJacc;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelDice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownCheckerSize;
        private System.Windows.Forms.Button buttonTransformFixed;
    }
}