namespace WinFormsApp1
{
    partial class MainForm
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
            EnterBox = new TextBox();
            ResultButton = new Button();
            ResultBox = new TextBox();
            numericUpDown2 = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            SuspendLayout();
            // 
            // EnterBox
            // 
            EnterBox.Location = new Point(27, 83);
            EnterBox.Name = "EnterBox";
            EnterBox.Size = new Size(262, 23);
            EnterBox.TabIndex = 0;
            EnterBox.Text = "Hello world";
            // 
            // ResultButton
            // 
            ResultButton.Location = new Point(261, 116);
            ResultButton.Name = "ResultButton";
            ResultButton.Size = new Size(75, 45);
            ResultButton.TabIndex = 1;
            ResultButton.Text = "Divide";
            ResultButton.UseVisualStyleBackColor = true;
            ResultButton.Click += ResultButton_Click;
            // 
            // ResultBox
            // 
            ResultBox.Location = new Point(27, 134);
            ResultBox.Name = "ResultBox";
            ResultBox.Size = new Size(228, 23);
            ResultBox.TabIndex = 2;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(295, 83);
            numericUpDown2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(41, 23);
            numericUpDown2.TabIndex = 4;
            numericUpDown2.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(371, 243);
            Controls.Add(numericUpDown2);
            Controls.Add(ResultBox);
            Controls.Add(ResultButton);
            Controls.Add(EnterBox);
            Name = "MainForm";
            Text = "Main";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox EnterBox;
        private Button ResultButton;
        private TextBox ResultBox;
        private NumericUpDown numericUpDown2;
    }
}