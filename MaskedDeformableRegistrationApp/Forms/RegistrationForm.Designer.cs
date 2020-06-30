﻿namespace MaskedDeformableRegistrationApp.Forms
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
            this.groupBoxMiscRigid = new System.Windows.Forms.GroupBox();
            this.checkBoxMaskRegistration = new System.Windows.Forms.CheckBox();
            this.groupBoxTransformationRigid = new System.Windows.Forms.GroupBox();
            this.radioButtonAffine = new System.Windows.Forms.RadioButton();
            this.radioButtonRigid = new System.Windows.Forms.RadioButton();
            this.radioButtonSimilarity = new System.Windows.Forms.RadioButton();
            this.radioButtonTranslation = new System.Windows.Forms.RadioButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.buttonEvaluation = new System.Windows.Forms.Button();
            this.buttonStartRegistration = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxConsoleRigid = new System.Windows.Forms.TextBox();
            this.progressBarRigid = new System.Windows.Forms.ProgressBar();
            this.tabPageNonRigid = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.buttonEditParamsNonRigid = new System.Windows.Forms.Button();
            this.groupBoxPenalty = new System.Windows.Forms.GroupBox();
            this.radioButtonDistancePreserving = new System.Windows.Forms.RadioButton();
            this.radioButtonBendEnergy = new System.Windows.Forms.RadioButton();
            this.radioButtonTransformRigidity = new System.Windows.Forms.RadioButton();
            this.radioButtonNoPenalties = new System.Windows.Forms.RadioButton();
            this.groupBoxTransformationNonRigid = new System.Windows.Forms.GroupBox();
            this.radioButtonSplineRecursive = new System.Windows.Forms.RadioButton();
            this.radioButtonKernelSpline = new System.Windows.Forms.RadioButton();
            this.radioButtonBsplineDiffusion = new System.Windows.Forms.RadioButton();
            this.radioButtonAdvancedBspline = new System.Windows.Forms.RadioButton();
            this.buttonSegmentationInnerstructures = new System.Windows.Forms.Button();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.buttonEvaluateNonRigidReg = new System.Windows.Forms.Button();
            this.buttonStartNonRigidRegistration = new System.Windows.Forms.Button();
            this.buttonCancelNonRigidReg = new System.Windows.Forms.Button();
            this.textBoxConsoleNonRigid = new System.Windows.Forms.TextBox();
            this.progressBarNonRigid = new System.Windows.Forms.ProgressBar();
            this.buttonSegmentationParams = new System.Windows.Forms.Button();
            this.backgroundWorkerRigid = new System.ComponentModel.BackgroundWorker();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBoxMaskingGeneral = new System.Windows.Forms.GroupBox();
            this.radioButtonFIxedAndMovingMask = new System.Windows.Forms.RadioButton();
            this.radioButtonOnlyMovingMask = new System.Windows.Forms.RadioButton();
            this.radioButtonOnlyFixedMask = new System.Windows.Forms.RadioButton();
            this.radioButtonNoMask = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxRegOrder = new System.Windows.Forms.GroupBox();
            this.radioButtonUsePrevInStack = new System.Windows.Forms.RadioButton();
            this.radioButtonMiddleStack = new System.Windows.Forms.RadioButton();
            this.radioButtonLastInStack = new System.Windows.Forms.RadioButton();
            this.radioButtonFirstFromStack = new System.Windows.Forms.RadioButton();
            this.backgroundWorkerNonRigid = new System.ComponentModel.BackgroundWorker();
            this.tabPageManual = new System.Windows.Forms.TabPage();
            this.tabControlRegistration.SuspendLayout();
            this.tabPageRigid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxMiscRigid.SuspendLayout();
            this.groupBoxTransformationRigid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPageNonRigid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBoxPenalty.SuspendLayout();
            this.groupBoxTransformationNonRigid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBoxMaskingGeneral.SuspendLayout();
            this.groupBoxRegOrder.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlRegistration
            // 
            this.tabControlRegistration.Controls.Add(this.tabPageRigid);
            this.tabControlRegistration.Controls.Add(this.tabPageNonRigid);
            this.tabControlRegistration.Controls.Add(this.tabPageManual);
            this.tabControlRegistration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlRegistration.Location = new System.Drawing.Point(0, 0);
            this.tabControlRegistration.Name = "tabControlRegistration";
            this.tabControlRegistration.SelectedIndex = 0;
            this.tabControlRegistration.Size = new System.Drawing.Size(685, 348);
            this.tabControlRegistration.TabIndex = 0;
            // 
            // tabPageRigid
            // 
            this.tabPageRigid.Controls.Add(this.splitContainer1);
            this.tabPageRigid.Location = new System.Drawing.Point(4, 22);
            this.tabPageRigid.Name = "tabPageRigid";
            this.tabPageRigid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRigid.Size = new System.Drawing.Size(677, 322);
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxMiscRigid);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxTransformationRigid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(671, 316);
            this.splitContainer1.SplitterDistance = 133;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonEditParameters
            // 
            this.buttonEditParameters.Location = new System.Drawing.Point(526, 83);
            this.buttonEditParameters.Name = "buttonEditParameters";
            this.buttonEditParameters.Size = new System.Drawing.Size(128, 41);
            this.buttonEditParameters.TabIndex = 3;
            this.buttonEditParameters.Text = "Edit registration parameters";
            this.buttonEditParameters.UseVisualStyleBackColor = true;
            this.buttonEditParameters.Click += new System.EventHandler(this.buttonEditParameters_Click);
            // 
            // groupBoxMiscRigid
            // 
            this.groupBoxMiscRigid.Controls.Add(this.checkBoxMaskRegistration);
            this.groupBoxMiscRigid.Location = new System.Drawing.Point(185, 3);
            this.groupBoxMiscRigid.Name = "groupBoxMiscRigid";
            this.groupBoxMiscRigid.Size = new System.Drawing.Size(174, 125);
            this.groupBoxMiscRigid.TabIndex = 2;
            this.groupBoxMiscRigid.TabStop = false;
            this.groupBoxMiscRigid.Text = "Misc";
            // 
            // checkBoxMaskRegistration
            // 
            this.checkBoxMaskRegistration.AutoSize = true;
            this.checkBoxMaskRegistration.Location = new System.Drawing.Point(7, 20);
            this.checkBoxMaskRegistration.Name = "checkBoxMaskRegistration";
            this.checkBoxMaskRegistration.Size = new System.Drawing.Size(144, 17);
            this.checkBoxMaskRegistration.TabIndex = 10;
            this.checkBoxMaskRegistration.Text = "Do Registration of masks";
            this.checkBoxMaskRegistration.UseVisualStyleBackColor = true;
            this.checkBoxMaskRegistration.CheckedChanged += new System.EventHandler(this.checkBoxMaskRegistration_CheckedChanged);
            // 
            // groupBoxTransformationRigid
            // 
            this.groupBoxTransformationRigid.Controls.Add(this.radioButtonAffine);
            this.groupBoxTransformationRigid.Controls.Add(this.radioButtonRigid);
            this.groupBoxTransformationRigid.Controls.Add(this.radioButtonSimilarity);
            this.groupBoxTransformationRigid.Controls.Add(this.radioButtonTranslation);
            this.groupBoxTransformationRigid.Location = new System.Drawing.Point(5, 3);
            this.groupBoxTransformationRigid.Name = "groupBoxTransformationRigid";
            this.groupBoxTransformationRigid.Size = new System.Drawing.Size(174, 125);
            this.groupBoxTransformationRigid.TabIndex = 1;
            this.groupBoxTransformationRigid.TabStop = false;
            this.groupBoxTransformationRigid.Text = "Transformation";
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
            this.radioButtonAffine.CheckedChanged += new System.EventHandler(this.radioButtonAffine_CheckedChanged);
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
            this.radioButtonRigid.CheckedChanged += new System.EventHandler(this.radioButtonRigid_CheckedChanged);
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
            this.radioButtonSimilarity.CheckedChanged += new System.EventHandler(this.radioButtonSimilarity_CheckedChanged);
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
            this.radioButtonTranslation.CheckedChanged += new System.EventHandler(this.radioButtonTranslation_CheckedChanged);
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
            this.splitContainer2.Panel2.Controls.Add(this.textBoxConsoleRigid);
            this.splitContainer2.Panel2.Controls.Add(this.progressBarRigid);
            this.splitContainer2.Size = new System.Drawing.Size(671, 179);
            this.splitContainer2.SplitterDistance = 146;
            this.splitContainer2.TabIndex = 2;
            // 
            // buttonEvaluation
            // 
            this.buttonEvaluation.Location = new System.Drawing.Point(13, 141);
            this.buttonEvaluation.Name = "buttonEvaluation";
            this.buttonEvaluation.Size = new System.Drawing.Size(121, 23);
            this.buttonEvaluation.TabIndex = 2;
            this.buttonEvaluation.Text = "Evaluate Registration";
            this.buttonEvaluation.UseVisualStyleBackColor = true;
            this.buttonEvaluation.Click += new System.EventHandler(this.buttonEvaluation_Click);
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
            // textBoxConsoleRigid
            // 
            this.textBoxConsoleRigid.Location = new System.Drawing.Point(20, 45);
            this.textBoxConsoleRigid.Multiline = true;
            this.textBoxConsoleRigid.Name = "textBoxConsoleRigid";
            this.textBoxConsoleRigid.Size = new System.Drawing.Size(505, 119);
            this.textBoxConsoleRigid.TabIndex = 1;
            // 
            // progressBarRigid
            // 
            this.progressBarRigid.Location = new System.Drawing.Point(20, 13);
            this.progressBarRigid.Name = "progressBarRigid";
            this.progressBarRigid.Size = new System.Drawing.Size(505, 23);
            this.progressBarRigid.TabIndex = 0;
            // 
            // tabPageNonRigid
            // 
            this.tabPageNonRigid.Controls.Add(this.splitContainer4);
            this.tabPageNonRigid.Location = new System.Drawing.Point(4, 22);
            this.tabPageNonRigid.Name = "tabPageNonRigid";
            this.tabPageNonRigid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNonRigid.Size = new System.Drawing.Size(677, 322);
            this.tabPageNonRigid.TabIndex = 1;
            this.tabPageNonRigid.Text = "Non rigid registration";
            this.tabPageNonRigid.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.buttonEditParamsNonRigid);
            this.splitContainer4.Panel1.Controls.Add(this.groupBoxPenalty);
            this.splitContainer4.Panel1.Controls.Add(this.groupBoxTransformationNonRigid);
            this.splitContainer4.Panel1.Controls.Add(this.buttonSegmentationInnerstructures);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Size = new System.Drawing.Size(671, 316);
            this.splitContainer4.SplitterDistance = 134;
            this.splitContainer4.TabIndex = 1;
            // 
            // buttonEditParamsNonRigid
            // 
            this.buttonEditParamsNonRigid.Location = new System.Drawing.Point(525, 71);
            this.buttonEditParamsNonRigid.Name = "buttonEditParamsNonRigid";
            this.buttonEditParamsNonRigid.Size = new System.Drawing.Size(128, 41);
            this.buttonEditParamsNonRigid.TabIndex = 3;
            this.buttonEditParamsNonRigid.Text = "Edit registration parameters";
            this.buttonEditParamsNonRigid.UseVisualStyleBackColor = true;
            this.buttonEditParamsNonRigid.Click += new System.EventHandler(this.buttonEditParamsNonRigid_Click);
            // 
            // groupBoxPenalty
            // 
            this.groupBoxPenalty.Controls.Add(this.radioButtonDistancePreserving);
            this.groupBoxPenalty.Controls.Add(this.radioButtonBendEnergy);
            this.groupBoxPenalty.Controls.Add(this.radioButtonTransformRigidity);
            this.groupBoxPenalty.Controls.Add(this.radioButtonNoPenalties);
            this.groupBoxPenalty.Location = new System.Drawing.Point(185, 3);
            this.groupBoxPenalty.Name = "groupBoxPenalty";
            this.groupBoxPenalty.Size = new System.Drawing.Size(159, 125);
            this.groupBoxPenalty.TabIndex = 2;
            this.groupBoxPenalty.TabStop = false;
            this.groupBoxPenalty.Text = "Penalty";
            // 
            // radioButtonDistancePreserving
            // 
            this.radioButtonDistancePreserving.AutoSize = true;
            this.radioButtonDistancePreserving.Location = new System.Drawing.Point(7, 92);
            this.radioButtonDistancePreserving.Name = "radioButtonDistancePreserving";
            this.radioButtonDistancePreserving.Size = new System.Drawing.Size(120, 17);
            this.radioButtonDistancePreserving.TabIndex = 8;
            this.radioButtonDistancePreserving.TabStop = true;
            this.radioButtonDistancePreserving.Text = "Distance Preserving";
            this.radioButtonDistancePreserving.UseVisualStyleBackColor = true;
            this.radioButtonDistancePreserving.CheckedChanged += new System.EventHandler(this.radioButtonDistancePreserving_CheckedChanged);
            // 
            // radioButtonBendEnergy
            // 
            this.radioButtonBendEnergy.AutoSize = true;
            this.radioButtonBendEnergy.Location = new System.Drawing.Point(7, 68);
            this.radioButtonBendEnergy.Name = "radioButtonBendEnergy";
            this.radioButtonBendEnergy.Size = new System.Drawing.Size(100, 17);
            this.radioButtonBendEnergy.TabIndex = 7;
            this.radioButtonBendEnergy.TabStop = true;
            this.radioButtonBendEnergy.Text = "Bending Energy";
            this.radioButtonBendEnergy.UseVisualStyleBackColor = true;
            this.radioButtonBendEnergy.CheckedChanged += new System.EventHandler(this.radioButtonBendEnergy_CheckedChanged);
            // 
            // radioButtonTransformRigidity
            // 
            this.radioButtonTransformRigidity.AutoSize = true;
            this.radioButtonTransformRigidity.Location = new System.Drawing.Point(7, 44);
            this.radioButtonTransformRigidity.Name = "radioButtonTransformRigidity";
            this.radioButtonTransformRigidity.Size = new System.Drawing.Size(109, 17);
            this.radioButtonTransformRigidity.TabIndex = 6;
            this.radioButtonTransformRigidity.TabStop = true;
            this.radioButtonTransformRigidity.Text = "Transform Rigidity";
            this.radioButtonTransformRigidity.UseVisualStyleBackColor = true;
            this.radioButtonTransformRigidity.CheckedChanged += new System.EventHandler(this.radioButtonTransformRigidity_CheckedChanged);
            // 
            // radioButtonNoPenalties
            // 
            this.radioButtonNoPenalties.AutoSize = true;
            this.radioButtonNoPenalties.Location = new System.Drawing.Point(7, 20);
            this.radioButtonNoPenalties.Name = "radioButtonNoPenalties";
            this.radioButtonNoPenalties.Size = new System.Drawing.Size(51, 17);
            this.radioButtonNoPenalties.TabIndex = 5;
            this.radioButtonNoPenalties.TabStop = true;
            this.radioButtonNoPenalties.Text = "None";
            this.radioButtonNoPenalties.UseVisualStyleBackColor = true;
            this.radioButtonNoPenalties.CheckedChanged += new System.EventHandler(this.radioButtonNoPenalties_CheckedChanged);
            // 
            // groupBoxTransformationNonRigid
            // 
            this.groupBoxTransformationNonRigid.Controls.Add(this.radioButtonSplineRecursive);
            this.groupBoxTransformationNonRigid.Controls.Add(this.radioButtonKernelSpline);
            this.groupBoxTransformationNonRigid.Controls.Add(this.radioButtonBsplineDiffusion);
            this.groupBoxTransformationNonRigid.Controls.Add(this.radioButtonAdvancedBspline);
            this.groupBoxTransformationNonRigid.Location = new System.Drawing.Point(5, 3);
            this.groupBoxTransformationNonRigid.Name = "groupBoxTransformationNonRigid";
            this.groupBoxTransformationNonRigid.Size = new System.Drawing.Size(174, 125);
            this.groupBoxTransformationNonRigid.TabIndex = 1;
            this.groupBoxTransformationNonRigid.TabStop = false;
            this.groupBoxTransformationNonRigid.Text = "Transformation";
            // 
            // radioButtonSplineRecursive
            // 
            this.radioButtonSplineRecursive.AutoSize = true;
            this.radioButtonSplineRecursive.Location = new System.Drawing.Point(7, 92);
            this.radioButtonSplineRecursive.Name = "radioButtonSplineRecursive";
            this.radioButtonSplineRecursive.Size = new System.Drawing.Size(105, 17);
            this.radioButtonSplineRecursive.TabIndex = 3;
            this.radioButtonSplineRecursive.Text = "Recursive Spline";
            this.radioButtonSplineRecursive.UseVisualStyleBackColor = true;
            this.radioButtonSplineRecursive.CheckedChanged += new System.EventHandler(this.radioButtonSplineRecursive_CheckedChanged);
            // 
            // radioButtonKernelSpline
            // 
            this.radioButtonKernelSpline.AutoSize = true;
            this.radioButtonKernelSpline.Location = new System.Drawing.Point(7, 68);
            this.radioButtonKernelSpline.Name = "radioButtonKernelSpline";
            this.radioButtonKernelSpline.Size = new System.Drawing.Size(84, 17);
            this.radioButtonKernelSpline.TabIndex = 2;
            this.radioButtonKernelSpline.Text = "SplineKernel";
            this.radioButtonKernelSpline.UseVisualStyleBackColor = true;
            this.radioButtonKernelSpline.CheckedChanged += new System.EventHandler(this.radioButtonKernelSpline_CheckedChanged);
            // 
            // radioButtonBsplineDiffusion
            // 
            this.radioButtonBsplineDiffusion.AutoSize = true;
            this.radioButtonBsplineDiffusion.Location = new System.Drawing.Point(7, 44);
            this.radioButtonBsplineDiffusion.Name = "radioButtonBsplineDiffusion";
            this.radioButtonBsplineDiffusion.Size = new System.Drawing.Size(123, 17);
            this.radioButtonBsplineDiffusion.TabIndex = 1;
            this.radioButtonBsplineDiffusion.Text = "Bspline with diffusion";
            this.radioButtonBsplineDiffusion.UseVisualStyleBackColor = true;
            this.radioButtonBsplineDiffusion.CheckedChanged += new System.EventHandler(this.radioButtonBsplineDiffusion_CheckedChanged);
            // 
            // radioButtonAdvancedBspline
            // 
            this.radioButtonAdvancedBspline.AutoSize = true;
            this.radioButtonAdvancedBspline.Checked = true;
            this.radioButtonAdvancedBspline.Location = new System.Drawing.Point(7, 20);
            this.radioButtonAdvancedBspline.Name = "radioButtonAdvancedBspline";
            this.radioButtonAdvancedBspline.Size = new System.Drawing.Size(135, 17);
            this.radioButtonAdvancedBspline.TabIndex = 0;
            this.radioButtonAdvancedBspline.TabStop = true;
            this.radioButtonAdvancedBspline.Text = "Advanced Bspline (def)";
            this.radioButtonAdvancedBspline.UseVisualStyleBackColor = true;
            this.radioButtonAdvancedBspline.CheckedChanged += new System.EventHandler(this.radioButtonAdvancedBspline_CheckedChanged);
            // 
            // buttonSegmentationInnerstructures
            // 
            this.buttonSegmentationInnerstructures.Location = new System.Drawing.Point(525, 17);
            this.buttonSegmentationInnerstructures.Name = "buttonSegmentationInnerstructures";
            this.buttonSegmentationInnerstructures.Size = new System.Drawing.Size(128, 47);
            this.buttonSegmentationInnerstructures.TabIndex = 4;
            this.buttonSegmentationInnerstructures.Text = "Adjust segmentation parameters";
            this.buttonSegmentationInnerstructures.UseVisualStyleBackColor = true;
            this.buttonSegmentationInnerstructures.Click += new System.EventHandler(this.buttonSegmentationInnerstructures_Click);
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.buttonEvaluateNonRigidReg);
            this.splitContainer5.Panel1.Controls.Add(this.buttonStartNonRigidRegistration);
            this.splitContainer5.Panel1.Controls.Add(this.buttonCancelNonRigidReg);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.textBoxConsoleNonRigid);
            this.splitContainer5.Panel2.Controls.Add(this.progressBarNonRigid);
            this.splitContainer5.Size = new System.Drawing.Size(671, 178);
            this.splitContainer5.SplitterDistance = 146;
            this.splitContainer5.TabIndex = 2;
            // 
            // buttonEvaluateNonRigidReg
            // 
            this.buttonEvaluateNonRigidReg.Location = new System.Drawing.Point(13, 140);
            this.buttonEvaluateNonRigidReg.Name = "buttonEvaluateNonRigidReg";
            this.buttonEvaluateNonRigidReg.Size = new System.Drawing.Size(121, 23);
            this.buttonEvaluateNonRigidReg.TabIndex = 2;
            this.buttonEvaluateNonRigidReg.Text = "Evaluate Registration";
            this.buttonEvaluateNonRigidReg.UseVisualStyleBackColor = true;
            // 
            // buttonStartNonRigidRegistration
            // 
            this.buttonStartNonRigidRegistration.Location = new System.Drawing.Point(13, 13);
            this.buttonStartNonRigidRegistration.Name = "buttonStartNonRigidRegistration";
            this.buttonStartNonRigidRegistration.Size = new System.Drawing.Size(121, 23);
            this.buttonStartNonRigidRegistration.TabIndex = 0;
            this.buttonStartNonRigidRegistration.Text = "Start Registration";
            this.buttonStartNonRigidRegistration.UseVisualStyleBackColor = true;
            this.buttonStartNonRigidRegistration.Click += new System.EventHandler(this.buttonStartNonRigidRegistration_Click);
            // 
            // buttonCancelNonRigidReg
            // 
            this.buttonCancelNonRigidReg.Location = new System.Drawing.Point(13, 42);
            this.buttonCancelNonRigidReg.Name = "buttonCancelNonRigidReg";
            this.buttonCancelNonRigidReg.Size = new System.Drawing.Size(121, 23);
            this.buttonCancelNonRigidReg.TabIndex = 1;
            this.buttonCancelNonRigidReg.Text = "Cancel Registration";
            this.buttonCancelNonRigidReg.UseVisualStyleBackColor = true;
            // 
            // textBoxConsoleNonRigid
            // 
            this.textBoxConsoleNonRigid.Location = new System.Drawing.Point(20, 45);
            this.textBoxConsoleNonRigid.Multiline = true;
            this.textBoxConsoleNonRigid.Name = "textBoxConsoleNonRigid";
            this.textBoxConsoleNonRigid.Size = new System.Drawing.Size(483, 130);
            this.textBoxConsoleNonRigid.TabIndex = 1;
            // 
            // progressBarNonRigid
            // 
            this.progressBarNonRigid.Location = new System.Drawing.Point(20, 13);
            this.progressBarNonRigid.Name = "progressBarNonRigid";
            this.progressBarNonRigid.Size = new System.Drawing.Size(483, 23);
            this.progressBarNonRigid.TabIndex = 0;
            // 
            // buttonSegmentationParams
            // 
            this.buttonSegmentationParams.Location = new System.Drawing.Point(151, 19);
            this.buttonSegmentationParams.Name = "buttonSegmentationParams";
            this.buttonSegmentationParams.Size = new System.Drawing.Size(84, 61);
            this.buttonSegmentationParams.TabIndex = 4;
            this.buttonSegmentationParams.Text = "Adjust segmentation parmas";
            this.buttonSegmentationParams.UseVisualStyleBackColor = true;
            this.buttonSegmentationParams.Click += new System.EventHandler(this.buttonSegmentationParams_Click);
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
            this.splitContainer3.Panel1.Controls.Add(this.groupBoxMaskingGeneral);
            this.splitContainer3.Panel1.Controls.Add(this.label1);
            this.splitContainer3.Panel1.Controls.Add(this.groupBoxRegOrder);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tabControlRegistration);
            this.splitContainer3.Size = new System.Drawing.Size(685, 501);
            this.splitContainer3.SplitterDistance = 149;
            this.splitContainer3.TabIndex = 1;
            // 
            // groupBoxMaskingGeneral
            // 
            this.groupBoxMaskingGeneral.Controls.Add(this.radioButtonFIxedAndMovingMask);
            this.groupBoxMaskingGeneral.Controls.Add(this.radioButtonOnlyMovingMask);
            this.groupBoxMaskingGeneral.Controls.Add(this.radioButtonOnlyFixedMask);
            this.groupBoxMaskingGeneral.Controls.Add(this.buttonSegmentationParams);
            this.groupBoxMaskingGeneral.Controls.Add(this.radioButtonNoMask);
            this.groupBoxMaskingGeneral.Location = new System.Drawing.Point(263, 28);
            this.groupBoxMaskingGeneral.Name = "groupBoxMaskingGeneral";
            this.groupBoxMaskingGeneral.Size = new System.Drawing.Size(248, 113);
            this.groupBoxMaskingGeneral.TabIndex = 3;
            this.groupBoxMaskingGeneral.TabStop = false;
            this.groupBoxMaskingGeneral.Text = "Masking";
            // 
            // radioButtonFIxedAndMovingMask
            // 
            this.radioButtonFIxedAndMovingMask.AutoSize = true;
            this.radioButtonFIxedAndMovingMask.Location = new System.Drawing.Point(6, 90);
            this.radioButtonFIxedAndMovingMask.Name = "radioButtonFIxedAndMovingMask";
            this.radioButtonFIxedAndMovingMask.Size = new System.Drawing.Size(155, 17);
            this.radioButtonFIxedAndMovingMask.TabIndex = 8;
            this.radioButtonFIxedAndMovingMask.TabStop = true;
            this.radioButtonFIxedAndMovingMask.Text = "Use fixed and moving mask";
            this.radioButtonFIxedAndMovingMask.UseVisualStyleBackColor = true;
            // 
            // radioButtonOnlyMovingMask
            // 
            this.radioButtonOnlyMovingMask.AutoSize = true;
            this.radioButtonOnlyMovingMask.Location = new System.Drawing.Point(6, 68);
            this.radioButtonOnlyMovingMask.Name = "radioButtonOnlyMovingMask";
            this.radioButtonOnlyMovingMask.Size = new System.Drawing.Size(109, 17);
            this.radioButtonOnlyMovingMask.TabIndex = 7;
            this.radioButtonOnlyMovingMask.TabStop = true;
            this.radioButtonOnlyMovingMask.Text = "Use moving mask";
            this.radioButtonOnlyMovingMask.UseVisualStyleBackColor = true;
            // 
            // radioButtonOnlyFixedMask
            // 
            this.radioButtonOnlyFixedMask.AutoSize = true;
            this.radioButtonOnlyFixedMask.Location = new System.Drawing.Point(6, 44);
            this.radioButtonOnlyFixedMask.Name = "radioButtonOnlyFixedMask";
            this.radioButtonOnlyFixedMask.Size = new System.Drawing.Size(97, 17);
            this.radioButtonOnlyFixedMask.TabIndex = 6;
            this.radioButtonOnlyFixedMask.TabStop = true;
            this.radioButtonOnlyFixedMask.Text = "Use fixed mask";
            this.radioButtonOnlyFixedMask.UseVisualStyleBackColor = true;
            // 
            // radioButtonNoMask
            // 
            this.radioButtonNoMask.AutoSize = true;
            this.radioButtonNoMask.Location = new System.Drawing.Point(6, 20);
            this.radioButtonNoMask.Name = "radioButtonNoMask";
            this.radioButtonNoMask.Size = new System.Drawing.Size(51, 17);
            this.radioButtonNoMask.TabIndex = 5;
            this.radioButtonNoMask.TabStop = true;
            this.radioButtonNoMask.Text = "None";
            this.radioButtonNoMask.UseVisualStyleBackColor = true;
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
            // groupBoxRegOrder
            // 
            this.groupBoxRegOrder.Controls.Add(this.radioButtonUsePrevInStack);
            this.groupBoxRegOrder.Controls.Add(this.radioButtonMiddleStack);
            this.groupBoxRegOrder.Controls.Add(this.radioButtonLastInStack);
            this.groupBoxRegOrder.Controls.Add(this.radioButtonFirstFromStack);
            this.groupBoxRegOrder.Location = new System.Drawing.Point(12, 28);
            this.groupBoxRegOrder.Name = "groupBoxRegOrder";
            this.groupBoxRegOrder.Size = new System.Drawing.Size(245, 113);
            this.groupBoxRegOrder.TabIndex = 0;
            this.groupBoxRegOrder.TabStop = false;
            this.groupBoxRegOrder.Text = "Registration order";
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
            // radioButtonMiddleStack
            // 
            this.radioButtonMiddleStack.AutoSize = true;
            this.radioButtonMiddleStack.Location = new System.Drawing.Point(7, 68);
            this.radioButtonMiddleStack.Name = "radioButtonMiddleStack";
            this.radioButtonMiddleStack.Size = new System.Drawing.Size(210, 17);
            this.radioButtonMiddleStack.TabIndex = 2;
            this.radioButtonMiddleStack.TabStop = true;
            this.radioButtonMiddleStack.Text = "Use middle image in stack as reference";
            this.radioButtonMiddleStack.UseVisualStyleBackColor = true;
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
            // tabPageManual
            // 
            this.tabPageManual.Location = new System.Drawing.Point(4, 22);
            this.tabPageManual.Name = "tabPageManual";
            this.tabPageManual.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageManual.Size = new System.Drawing.Size(677, 322);
            this.tabPageManual.TabIndex = 2;
            this.tabPageManual.Text = "Manual registration";
            this.tabPageManual.UseVisualStyleBackColor = true;
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 501);
            this.Controls.Add(this.splitContainer3);
            this.Name = "RegistrationForm";
            this.Text = "RegistrationForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RegistrationForm_FormClosed);
            this.Load += new System.EventHandler(this.RegistrationForm_Load);
            this.tabControlRegistration.ResumeLayout(false);
            this.tabPageRigid.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxMiscRigid.ResumeLayout(false);
            this.groupBoxMiscRigid.PerformLayout();
            this.groupBoxTransformationRigid.ResumeLayout(false);
            this.groupBoxTransformationRigid.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabPageNonRigid.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBoxPenalty.ResumeLayout(false);
            this.groupBoxPenalty.PerformLayout();
            this.groupBoxTransformationNonRigid.ResumeLayout(false);
            this.groupBoxTransformationNonRigid.PerformLayout();
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBoxMaskingGeneral.ResumeLayout(false);
            this.groupBoxMaskingGeneral.PerformLayout();
            this.groupBoxRegOrder.ResumeLayout(false);
            this.groupBoxRegOrder.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlRegistration;
        private System.Windows.Forms.TabPage tabPageRigid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPageNonRigid;
        private System.Windows.Forms.Button buttonStartRegistration;
        private System.Windows.Forms.GroupBox groupBoxTransformationRigid;
        private System.Windows.Forms.RadioButton radioButtonAffine;
        private System.Windows.Forms.RadioButton radioButtonRigid;
        private System.Windows.Forms.RadioButton radioButtonSimilarity;
        private System.Windows.Forms.RadioButton radioButtonTranslation;
        private System.Windows.Forms.GroupBox groupBoxMiscRigid;
        private System.Windows.Forms.Button buttonEditParameters;
        private System.Windows.Forms.Button buttonSegmentationParams;
        private System.ComponentModel.BackgroundWorker backgroundWorkerRigid;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxConsoleRigid;
        private System.Windows.Forms.ProgressBar progressBarRigid;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxRegOrder;
        private System.Windows.Forms.RadioButton radioButtonUsePrevInStack;
        private System.Windows.Forms.RadioButton radioButtonMiddleStack;
        private System.Windows.Forms.RadioButton radioButtonLastInStack;
        private System.Windows.Forms.RadioButton radioButtonFirstFromStack;
        private System.Windows.Forms.Button buttonEvaluation;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Button buttonEditParamsNonRigid;
        private System.Windows.Forms.GroupBox groupBoxPenalty;
        private System.Windows.Forms.RadioButton radioButtonDistancePreserving;
        private System.Windows.Forms.RadioButton radioButtonBendEnergy;
        private System.Windows.Forms.RadioButton radioButtonTransformRigidity;
        private System.Windows.Forms.RadioButton radioButtonNoPenalties;
        private System.Windows.Forms.Button buttonSegmentationInnerstructures;
        private System.Windows.Forms.GroupBox groupBoxTransformationNonRigid;
        private System.Windows.Forms.RadioButton radioButtonSplineRecursive;
        private System.Windows.Forms.RadioButton radioButtonKernelSpline;
        private System.Windows.Forms.RadioButton radioButtonBsplineDiffusion;
        private System.Windows.Forms.RadioButton radioButtonAdvancedBspline;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Button buttonEvaluateNonRigidReg;
        private System.Windows.Forms.Button buttonStartNonRigidRegistration;
        private System.Windows.Forms.Button buttonCancelNonRigidReg;
        private System.Windows.Forms.TextBox textBoxConsoleNonRigid;
        private System.Windows.Forms.ProgressBar progressBarNonRigid;
        private System.Windows.Forms.CheckBox checkBoxMaskRegistration;
        private System.Windows.Forms.GroupBox groupBoxMaskingGeneral;
        private System.Windows.Forms.RadioButton radioButtonFIxedAndMovingMask;
        private System.Windows.Forms.RadioButton radioButtonOnlyMovingMask;
        private System.Windows.Forms.RadioButton radioButtonOnlyFixedMask;
        private System.Windows.Forms.RadioButton radioButtonNoMask;
        private System.ComponentModel.BackgroundWorker backgroundWorkerNonRigid;
        private System.Windows.Forms.TabPage tabPageManual;
    }
}