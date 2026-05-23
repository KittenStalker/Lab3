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
            NumericUpDown = new NumericUpDown();
            ErrorLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown).BeginInit();
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
            // NumericUpDown
            // 
            NumericUpDown.Location = new Point(295, 83);
            NumericUpDown.Name = "NumericUpDown";
            NumericUpDown.Size = new Size(41, 23);
            NumericUpDown.TabIndex = 4;
            NumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // ErrorLabel
            // 
            ErrorLabel.AutoSize = true;
            ErrorLabel.Location = new Point(27, 42);
            ErrorLabel.Name = "ErrorLabel";
            ErrorLabel.Size = new Size(75, 15);
            ErrorLabel.TabIndex = 5;
            ErrorLabel.Text = "Нет ошибок";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(371, 243);
            Controls.Add(ErrorLabel);
            Controls.Add(NumericUpDown);
            Controls.Add(ResultBox);
            Controls.Add(ResultButton);
            Controls.Add(EnterBox);
            Name = "MainForm";
            Text = "Main";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)NumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox EnterBox;
        private Button ResultButton;
        private TextBox ResultBox;
        private NumericUpDown NumericUpDown;
        private Label ErrorLabel;
    }
}