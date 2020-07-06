namespace PrinterIPLookup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.search = new System.Windows.Forms.Button();
            this.macAddressTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.IPAddressLabel = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.checkBoxCleanCache = new System.Windows.Forms.CheckBox();
            this.txtBoxPortName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxMapPortIP = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(123, 87);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(558, 51);
            this.search.TabIndex = 0;
            this.search.Text = "SEARCH";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // macAddressTextBox
            // 
            this.macAddressTextBox.Location = new System.Drawing.Point(319, 201);
            this.macAddressTextBox.Name = "macAddressTextBox";
            this.macAddressTextBox.Size = new System.Drawing.Size(362, 38);
            this.macAddressTextBox.TabIndex = 1;
            this.macAddressTextBox.Text = "00-15-99-92-CC-EB";
            this.macAddressTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.macAddressTextBox.TextChanged += new System.EventHandler(this.macAddressTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "MAC Address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP Address:";
            // 
            // IPAddressLabel
            // 
            this.IPAddressLabel.AutoSize = true;
            this.IPAddressLabel.Location = new System.Drawing.Point(432, 322);
            this.IPAddressLabel.Name = "IPAddressLabel";
            this.IPAddressLabel.Size = new System.Drawing.Size(0, 32);
            this.IPAddressLabel.TabIndex = 4;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(123, 377);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(154, 49);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(123, 22);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(558, 45);
            this.progressBar.TabIndex = 6;
            // 
            // checkBoxCleanCache
            // 
            this.checkBoxCleanCache.AutoSize = true;
            this.checkBoxCleanCache.Location = new System.Drawing.Point(123, 144);
            this.checkBoxCleanCache.Name = "checkBoxCleanCache";
            this.checkBoxCleanCache.Size = new System.Drawing.Size(217, 36);
            this.checkBoxCleanCache.TabIndex = 7;
            this.checkBoxCleanCache.Text = "Clean Cache";
            this.checkBoxCleanCache.UseVisualStyleBackColor = true;
            // 
            // txtBoxPortName
            // 
            this.txtBoxPortName.Location = new System.Drawing.Point(362, 259);
            this.txtBoxPortName.Name = "txtBoxPortName";
            this.txtBoxPortName.Size = new System.Drawing.Size(319, 38);
            this.txtBoxPortName.TabIndex = 8;
            this.txtBoxPortName.Text = "192.168.0.17";
            this.txtBoxPortName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(116, 263);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 32);
            this.label3.TabIndex = 9;
            this.label3.Text = "Local Port Name:";
            // 
            // checkBoxMapPortIP
            // 
            this.checkBoxMapPortIP.AutoSize = true;
            this.checkBoxMapPortIP.Checked = true;
            this.checkBoxMapPortIP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMapPortIP.Location = new System.Drawing.Point(355, 149);
            this.checkBoxMapPortIP.Name = "checkBoxMapPortIP";
            this.checkBoxMapPortIP.Size = new System.Drawing.Size(307, 36);
            this.checkBoxMapPortIP.TabIndex = 10;
            this.checkBoxMapPortIP.Text = "Map Local Port to IP";
            this.checkBoxMapPortIP.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(319, 377);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(362, 49);
            this.button1.TabIndex = 11;
            this.button1.Text = "Stop";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 454);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxMapPortIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBoxPortName);
            this.Controls.Add(this.checkBoxCleanCache);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.IPAddressLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.macAddressTextBox);
            this.Controls.Add(this.search);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Printer IP Lookup";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button search;
        private System.Windows.Forms.TextBox macAddressTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label IPAddressLabel;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox checkBoxCleanCache;
        private System.Windows.Forms.TextBox txtBoxPortName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxMapPortIP;
        private System.Windows.Forms.Button button1;
    }
}

