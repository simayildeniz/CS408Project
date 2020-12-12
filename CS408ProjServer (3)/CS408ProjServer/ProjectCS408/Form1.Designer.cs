namespace ProjectCS408
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
            this.portLabel = new System.Windows.Forms.Label();
            this.portInput = new System.Windows.Forms.TextBox();
            this.myLogOutput = new System.Windows.Forms.RichTextBox();
            this.listenButton = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDirectory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(16, 352);
            this.portLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(127, 25);
            this.portLabel.TabIndex = 0;
            this.portLabel.Text = "Port Number:";
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(162, 352);
            this.portInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(249, 29);
            this.portInput.TabIndex = 1;
            // 
            // myLogOutput
            // 
            this.myLogOutput.Location = new System.Drawing.Point(102, 458);
            this.myLogOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.myLogOutput.Name = "myLogOutput";
            this.myLogOutput.Size = new System.Drawing.Size(469, 276);
            this.myLogOutput.TabIndex = 2;
            this.myLogOutput.Text = "";
            // 
            // listenButton
            // 
            this.listenButton.Enabled = false;
            this.listenButton.Location = new System.Drawing.Point(458, 352);
            this.listenButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listenButton.Name = "listenButton";
            this.listenButton.Size = new System.Drawing.Size(114, 44);
            this.listenButton.TabIndex = 3;
            this.listenButton.Text = "Listen";
            this.listenButton.UseVisualStyleBackColor = true;
            this.listenButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(162, 136);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(301, 38);
            this.buttonBrowse.TabIndex = 4;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(643, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "In order to choose predetermined folder for file to be loaded, click Browse:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 256);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Chosen directory:";
            // 
            // textBoxDirectory
            // 
            this.textBoxDirectory.Enabled = false;
            this.textBoxDirectory.Location = new System.Drawing.Point(216, 252);
            this.textBoxDirectory.Name = "textBoxDirectory";
            this.textBoxDirectory.Size = new System.Drawing.Size(355, 29);
            this.textBoxDirectory.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 778);
            this.Controls.Add(this.textBoxDirectory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.listenButton);
            this.Controls.Add(this.myLogOutput);
            this.Controls.Add(this.portInput);
            this.Controls.Add(this.portLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox portInput;
        private System.Windows.Forms.RichTextBox myLogOutput;
        private System.Windows.Forms.Button listenButton;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDirectory;
    }
}