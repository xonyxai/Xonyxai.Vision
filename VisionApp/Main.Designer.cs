namespace VisionApp
{
    partial class Main
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
            ImageBox = new PictureBox();
            groupBox1 = new GroupBox();
            chkEnableInspection = new CheckBox();
            chkCamera = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)ImageBox).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // ImageBox
            // 
            ImageBox.Location = new Point(12, 12);
            ImageBox.Name = "ImageBox";
            ImageBox.Size = new Size(1340, 908);
            ImageBox.SizeMode = PictureBoxSizeMode.AutoSize;
            ImageBox.TabIndex = 0;
            ImageBox.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkEnableInspection);
            groupBox1.Controls.Add(chkCamera);
            groupBox1.Location = new Point(1382, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(241, 116);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Camera";
            // 
            // chkEnableInspection
            // 
            chkEnableInspection.AutoSize = true;
            chkEnableInspection.Location = new Point(28, 72);
            chkEnableInspection.Name = "chkEnableInspection";
            chkEnableInspection.Size = new Size(119, 19);
            chkEnableInspection.TabIndex = 1;
            chkEnableInspection.Text = "Enable Inspection";
            chkEnableInspection.UseVisualStyleBackColor = true;
            chkEnableInspection.CheckedChanged += chkEnableInspection_CheckedChanged;
            // 
            // chkCamera
            // 
            chkCamera.AutoSize = true;
            chkCamera.Location = new Point(28, 38);
            chkCamera.Name = "chkCamera";
            chkCamera.Size = new Size(123, 19);
            chkCamera.TabIndex = 0;
            chkCamera.Text = "Start/Stop Camera";
            chkCamera.UseVisualStyleBackColor = true;
            chkCamera.CheckedChanged += chkCamera_CheckedChanged;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1649, 932);
            Controls.Add(groupBox1);
            Controls.Add(ImageBox);
            Name = "Main";
            Text = "Main";
            Load += Main_Load;
            ((System.ComponentModel.ISupportInitialize)ImageBox).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox ImageBox;
        private GroupBox groupBox1;
        private CheckBox chkCamera;
        private CheckBox chkEnableInspection;
    }
}