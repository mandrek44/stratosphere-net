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
            this.buttonBacktrackGradient = new System.Windows.Forms.Button();
            this.buttonNewton = new System.Windows.Forms.Button();
            this.buttonBanana = new System.Windows.Forms.Button();
            this.buttonXSquared = new System.Windows.Forms.Button();
            this.buttonRegression = new System.Windows.Forms.Button();
            this.buttonNewtonBactrack = new System.Windows.Forms.Button();
            this.buttonLogistic = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonBacktrackGradient
            // 
            this.buttonBacktrackGradient.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonBacktrackGradient.Location = new System.Drawing.Point(54, 146);
            this.buttonBacktrackGradient.Name = "buttonBacktrackGradient";
            this.buttonBacktrackGradient.Size = new System.Drawing.Size(126, 23);
            this.buttonBacktrackGradient.TabIndex = 9;
            this.buttonBacktrackGradient.Text = "Steepest Decent (B)";
            this.buttonBacktrackGradient.UseVisualStyleBackColor = true;
            this.buttonBacktrackGradient.Click += new System.EventHandler(this.buttonBacktrackGradient_Click);
            // 
            // buttonNewton
            // 
            this.buttonNewton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonNewton.Location = new System.Drawing.Point(54, 117);
            this.buttonNewton.Name = "buttonNewton";
            this.buttonNewton.Size = new System.Drawing.Size(126, 23);
            this.buttonNewton.TabIndex = 7;
            this.buttonNewton.Text = "Newton\'s Method";
            this.buttonNewton.UseVisualStyleBackColor = true;
            this.buttonNewton.Click += new System.EventHandler(this.buttonNewton_Click);
            // 
            // buttonBanana
            // 
            this.buttonBanana.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonBanana.Location = new System.Drawing.Point(54, 204);
            this.buttonBanana.Name = "buttonBanana";
            this.buttonBanana.Size = new System.Drawing.Size(126, 23);
            this.buttonBanana.TabIndex = 8;
            this.buttonBanana.Text = "Quasi Newton";
            this.buttonBanana.UseVisualStyleBackColor = true;
            this.buttonBanana.Click += new System.EventHandler(this.buttonBanana_Click);
            // 
            // buttonXSquared
            // 
            this.buttonXSquared.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonXSquared.Location = new System.Drawing.Point(54, 59);
            this.buttonXSquared.Name = "buttonXSquared";
            this.buttonXSquared.Size = new System.Drawing.Size(126, 23);
            this.buttonXSquared.TabIndex = 5;
            this.buttonXSquared.Text = "x^2";
            this.buttonXSquared.UseVisualStyleBackColor = true;
            this.buttonXSquared.Click += new System.EventHandler(this.buttonXSquared_Click);
            // 
            // buttonRegression
            // 
            this.buttonRegression.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonRegression.Location = new System.Drawing.Point(54, 88);
            this.buttonRegression.Name = "buttonRegression";
            this.buttonRegression.Size = new System.Drawing.Size(126, 23);
            this.buttonRegression.TabIndex = 6;
            this.buttonRegression.Text = "Regression";
            this.buttonRegression.UseVisualStyleBackColor = true;
            this.buttonRegression.Click += new System.EventHandler(this.buttonRegression_Click);
            // 
            // buttonNewtonBactrack
            // 
            this.buttonNewtonBactrack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonNewtonBactrack.Location = new System.Drawing.Point(54, 175);
            this.buttonNewtonBactrack.Name = "buttonNewtonBactrack";
            this.buttonNewtonBactrack.Size = new System.Drawing.Size(126, 23);
            this.buttonNewtonBactrack.TabIndex = 10;
            this.buttonNewtonBactrack.Text = "Newton\'s Method (B)";
            this.buttonNewtonBactrack.UseVisualStyleBackColor = true;
            this.buttonNewtonBactrack.Click += new System.EventHandler(this.buttonNewtonBactrack_Click);
            // 
            // buttonLogistic
            // 
            this.buttonLogistic.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonLogistic.Location = new System.Drawing.Point(54, 233);
            this.buttonLogistic.Name = "buttonLogistic";
            this.buttonLogistic.Size = new System.Drawing.Size(126, 23);
            this.buttonLogistic.TabIndex = 11;
            this.buttonLogistic.Text = "Logistic Regression";
            this.buttonLogistic.UseVisualStyleBackColor = true;
            this.buttonLogistic.Click += new System.EventHandler(this.buttonLogistic_Click);
            // 
            // ExerciseSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 305);
            this.Controls.Add(this.buttonLogistic);
            this.Controls.Add(this.buttonNewtonBactrack);
            this.Controls.Add(this.buttonBacktrackGradient);
            this.Controls.Add(this.buttonNewton);
            this.Controls.Add(this.buttonBanana);
            this.Controls.Add(this.buttonXSquared);
            this.Controls.Add(this.buttonRegression);
            this.Name = "ExerciseSelector";
            this.Text = "Select Exercise";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonBacktrackGradient;
        private System.Windows.Forms.Button buttonRegression;
        private System.Windows.Forms.Button buttonXSquared;
        private System.Windows.Forms.Button buttonBanana;
        private System.Windows.Forms.Button buttonNewton;
        private System.Windows.Forms.Button buttonNewtonBactrack;
        private System.Windows.Forms.Button buttonLogistic;
    }
}