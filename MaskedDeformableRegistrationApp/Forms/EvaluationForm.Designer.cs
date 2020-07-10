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
            this.buttonCalcMetrics = new System.Windows.Forms.Button();
            this.openFileDialogFixed = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogMoving = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // textBoxFixedPointSet
            // 
            this.textBoxFixedPointSet.Location = new System.Drawing.Point(16, 29);
            this.textBoxFixedPointSet.Name = "textBoxFixedPointSet";
            this.textBoxFixedPointSet.Size = new System.Drawing.Size(129, 20);
            this.textBoxFixedPointSet.TabIndex = 0;
            // 
            // textBoxMovingPointSet
            // 
            this.textBoxMovingPointSet.Location = new System.Drawing.Point(16, 109);
            this.textBoxMovingPointSet.Name = "textBoxMovingPointSet";
            this.textBoxMovingPointSet.Size = new System.Drawing.Size(129, 20);
            this.textBoxMovingPointSet.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fixed image point set:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Moving image and corresponding point set";
            // 
            // comboBoxMoving
            // 
            this.comboBoxMoving.FormattingEnabled = true;
            this.comboBoxMoving.Location = new System.Drawing.Point(16, 82);
            this.comboBoxMoving.Name = "comboBoxMoving";
            this.comboBoxMoving.Size = new System.Drawing.Size(129, 21);
            this.comboBoxMoving.TabIndex = 4;
            // 
            // buttonChooseFixedPS
            // 
            this.buttonChooseFixedPS.Location = new System.Drawing.Point(162, 26);
            this.buttonChooseFixedPS.Name = "buttonChooseFixedPS";
            this.buttonChooseFixedPS.Size = new System.Drawing.Size(76, 23);
            this.buttonChooseFixedPS.TabIndex = 5;
            this.buttonChooseFixedPS.Text = "Choose";
            this.buttonChooseFixedPS.UseVisualStyleBackColor = true;
            this.buttonChooseFixedPS.Click += new System.EventHandler(this.buttonChooseFixedPS_Click);
            // 
            // buttonChooseMovPS
            // 
            this.buttonChooseMovPS.Location = new System.Drawing.Point(162, 107);
            this.buttonChooseMovPS.Name = "buttonChooseMovPS";
            this.buttonChooseMovPS.Size = new System.Drawing.Size(76, 23);
            this.buttonChooseMovPS.TabIndex = 6;
            this.buttonChooseMovPS.Text = "Choose";
            this.buttonChooseMovPS.UseVisualStyleBackColor = true;
            this.buttonChooseMovPS.Click += new System.EventHandler(this.buttonChooseMovPS_Click);
            // 
            // buttonCalcMetrics
            // 
            this.buttonCalcMetrics.Location = new System.Drawing.Point(16, 154);
            this.buttonCalcMetrics.Name = "buttonCalcMetrics";
            this.buttonCalcMetrics.Size = new System.Drawing.Size(75, 23);
            this.buttonCalcMetrics.TabIndex = 7;
            this.buttonCalcMetrics.Text = "Calc metrics";
            this.buttonCalcMetrics.UseVisualStyleBackColor = true;
            this.buttonCalcMetrics.Click += new System.EventHandler(this.buttonCalcMetrics_Click);
            // 
            // openFileDialogFixed
            // 
            this.openFileDialogFixed.FileName = "openFileDialog1";
            // 
            // openFileDialogMoving
            // 
            this.openFileDialogMoving.FileName = "openFileDialog1";
            // 
            // EvaluationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonCalcMetrics);
            this.Controls.Add(this.buttonChooseMovPS);
            this.Controls.Add(this.buttonChooseFixedPS);
            this.Controls.Add(this.comboBoxMoving);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMovingPointSet);
            this.Controls.Add(this.textBoxFixedPointSet);
            this.Name = "EvaluationForm";
            this.Text = "EvaluationForm";
            this.Load += new System.EventHandler(this.EvaluationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFixedPointSet;
        private System.Windows.Forms.TextBox textBoxMovingPointSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxMoving;
        private System.Windows.Forms.Button buttonChooseFixedPS;
        private System.Windows.Forms.Button buttonChooseMovPS;
        private System.Windows.Forms.Button buttonCalcMetrics;
        private System.Windows.Forms.OpenFileDialog openFileDialogFixed;
        private System.Windows.Forms.OpenFileDialog openFileDialogMoving;
    }
}