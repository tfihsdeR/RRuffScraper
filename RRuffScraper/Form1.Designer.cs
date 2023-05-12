namespace OnurScraping
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            btnDownload = new Button();
            lblTotalDocument = new Label();
            lblInforming = new Label();
            label2 = new Label();
            lblDownloadCount = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(60, 39);
            label1.Name = "label1";
            label1.Size = new Size(94, 15);
            label1.TabIndex = 0;
            label1.Text = "Total Document:";
            // 
            // btnDownload
            // 
            btnDownload.Enabled = false;
            btnDownload.Location = new Point(129, 114);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(75, 23);
            btnDownload.TabIndex = 1;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += button1_Click;
            // 
            // lblTotalDocument
            // 
            lblTotalDocument.AutoSize = true;
            lblTotalDocument.Location = new Point(160, 39);
            lblTotalDocument.Name = "lblTotalDocument";
            lblTotalDocument.Size = new Size(59, 15);
            lblTotalDocument.TabIndex = 2;
            lblTotalDocument.Text = "Updating!";
            // 
            // lblInforming
            // 
            lblInforming.AutoSize = true;
            lblInforming.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblInforming.Location = new Point(60, 168);
            lblInforming.Name = "lblInforming";
            lblInforming.Size = new Size(221, 15);
            lblInforming.TabIndex = 3;
            lblInforming.Text = "Please wait. Document page is loading.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(60, 76);
            label2.Name = "label2";
            label2.Size = new Size(173, 15);
            label2.TabIndex = 4;
            label2.Text = "Downloaded document total of";
            // 
            // lblDownloadCount
            // 
            lblDownloadCount.AutoSize = true;
            lblDownloadCount.Location = new Point(239, 76);
            lblDownloadCount.Name = "lblDownloadCount";
            lblDownloadCount.Size = new Size(13, 15);
            lblDownloadCount.TabIndex = 5;
            lblDownloadCount.Text = "0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(342, 241);
            Controls.Add(lblDownloadCount);
            Controls.Add(label2);
            Controls.Add(lblInforming);
            Controls.Add(lblTotalDocument);
            Controls.Add(btnDownload);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnDownload;
        private Label lblTotalDocument;
        private Label lblInforming;
        private Label label2;
        private Label lblDownloadCount;
    }
}