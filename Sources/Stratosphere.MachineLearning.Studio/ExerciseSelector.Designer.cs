namespace Stratosphere.MachineLearning.Studio
{
    partial class ExerciseSelector
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
            this.buttonRegression = new System.Windows.Forms.Button();
            this.buttonXSquared = new System.Windows.Forms.Button();
            this.buttonBanana = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRegression
            // 
            this.buttonRegression.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonRegression.Location = new System.Drawing.Point(53, 70);
            this.buttonRegression.Name = "buttonRegression";
            this.buttonRegression.Size = new System.Drawing.Size(126, 23);
            this.buttonRegression.TabIndex = 1;
            this.buttonRegression.Text = "Regression";
            this.buttonRegression.UseVisualStyleBackColor = true;
            this.buttonRegression.Click += new System.EventHandler(this.buttonRegression_Click);
            // 
            // buttonXSquared
            // 
            this.buttonXSquared.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonXSquared.Location = new System.Drawing.Point(53, 41);
            this.buttonXSquared.Name = "buttonXSquared";
            this.buttonXSquared.Size = new System.Drawing.Size(126, 23);
            this.buttonXSquared.TabIndex = 0;
            this.buttonXSquared.Text = "x^2";
            this.buttonXSquared.UseVisualStyleBackColor = true;
            this.buttonXSquared.Click += new System.EventHandler(this.buttonXSquared_Click);
            // 
            // buttonBanana
            // 
            this.buttonBanana.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonBanana.Location = new System.Drawing.Point(53, 99);
            this.buttonBanana.Name = "buttonBanana";
            this.buttonBanana.Size = new System.Drawing.Size(126, 23);
            this.buttonBanana.TabIndex = 2;
            this.buttonBanana.Text = "Banana Optimization";
            this.buttonBanana.UseVisualStyleBackColor = true;
            this.buttonBanana.Click += new System.EventHandler(this.buttonBanana_Click);
            // 
            // ExerciseSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 163);
            this.Controls.Add(this.buttonBanana);
            this.Controls.Add(this.buttonXSquared);
            this.Controls.Add(this.buttonRegression);
            this.Name = "ExerciseSelector";
            this.Text = "Select Exercise";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRegression;
        private System.Windows.Forms.Button buttonXSquared;
        private System.Windows.Forms.Button buttonBanana;
    }
}