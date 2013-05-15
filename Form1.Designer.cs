namespace UPCOR.W.SetRights
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnRefreshOne = new System.Windows.Forms.Button();
            this.btnInherit = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Ge alla";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRefreshOne
            // 
            this.btnRefreshOne.Location = new System.Drawing.Point(94, 12);
            this.btnRefreshOne.Name = "btnRefreshOne";
            this.btnRefreshOne.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshOne.TabIndex = 1;
            this.btnRefreshOne.Text = "Ge en";
            this.btnRefreshOne.UseVisualStyleBackColor = true;
            this.btnRefreshOne.Click += new System.EventHandler(this.btnRefreshOne_Click);
            // 
            // btnInherit
            // 
            this.btnInherit.Location = new System.Drawing.Point(12, 42);
            this.btnInherit.Name = "btnInherit";
            this.btnInherit.Size = new System.Drawing.Size(75, 23);
            this.btnInherit.TabIndex = 2;
            this.btnInherit.Text = "Ärv alla";
            this.btnInherit.UseVisualStyleBackColor = true;
            this.btnInherit.Click += new System.EventHandler(this.btnInherit_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(209, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(25, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "123";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 345);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnInherit);
            this.Controls.Add(this.btnRefreshOne);
            this.Controls.Add(this.btnRefresh);
            this.Name = "Form1";
            this.Text = "Ge rättigheter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnRefreshOne;
        private System.Windows.Forms.Button btnInherit;
        private System.Windows.Forms.Label lblStatus;
    }
}

