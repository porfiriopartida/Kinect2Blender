namespace HelloKinect
{
    partial class MainForm
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
            this.labelHello = new System.Windows.Forms.Label();
            this.btnSkeleton = new System.Windows.Forms.Button();
            this.txtSkeletonData = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHello
            // 
            this.labelHello.AutoSize = true;
            this.labelHello.Location = new System.Drawing.Point(93, 17);
            this.labelHello.Name = "labelHello";
            this.labelHello.Size = new System.Drawing.Size(0, 13);
            this.labelHello.TabIndex = 1;
            // 
            // btnSkeleton
            // 
            this.btnSkeleton.Location = new System.Drawing.Point(3, 3);
            this.btnSkeleton.Name = "btnSkeleton";
            this.btnSkeleton.Size = new System.Drawing.Size(126, 23);
            this.btnSkeleton.TabIndex = 2;
            this.btnSkeleton.Text = "Start Kinect";
            this.btnSkeleton.UseVisualStyleBackColor = true;
            this.btnSkeleton.Click += new System.EventHandler(this.btnStartKinect);
            // 
            // txtSkeletonData
            // 
            this.txtSkeletonData.CausesValidation = false;
            this.txtSkeletonData.HideSelection = false;
            this.txtSkeletonData.Location = new System.Drawing.Point(135, 3);
            this.txtSkeletonData.Multiline = true;
            this.txtSkeletonData.Name = "txtSkeletonData";
            this.txtSkeletonData.ReadOnly = true;
            this.txtSkeletonData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSkeletonData.Size = new System.Drawing.Size(681, 582);
            this.txtSkeletonData.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnSkeleton);
            this.flowLayoutPanel1.Controls.Add(this.txtSkeletonData);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(826, 598);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(857, 622);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.labelHello);
            this.Name = "MainForm";
            this.Text = "Informal Kinect Test";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelHello;
        private System.Windows.Forms.Button btnSkeleton;
        private System.Windows.Forms.TextBox txtSkeletonData;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}

