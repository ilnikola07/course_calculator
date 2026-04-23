namespace Course_calculator
{
    partial class FormHistory
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
            listBoxHist = new ListBox();
            SuspendLayout();
            // 
            // listBoxHist
            // 
            listBoxHist.Dock = DockStyle.Fill;
            listBoxHist.FormattingEnabled = true;
            listBoxHist.ItemHeight = 15;
            listBoxHist.Location = new Point(0, 0);
            listBoxHist.Name = "listBoxHist";
            listBoxHist.Size = new Size(517, 256);
            listBoxHist.TabIndex = 0;
            // 
            // FormHistory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(517, 256);
            Controls.Add(listBoxHist);
            Name = "FormHistory";
            Text = "История";
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBoxHist;
    }
}