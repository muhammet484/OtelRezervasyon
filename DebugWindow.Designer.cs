namespace OtelRezervasyon
{
    partial class DebugWindow
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
            DebugTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // DebugTextBox
            // 
            DebugTextBox.Dock = DockStyle.Fill;
            DebugTextBox.Location = new Point(0, 0);
            DebugTextBox.Name = "DebugTextBox";
            DebugTextBox.Size = new Size(911, 450);
            DebugTextBox.TabIndex = 0;
            DebugTextBox.Text = "";
            // 
            // DebugWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(911, 450);
            Controls.Add(DebugTextBox);
            Name = "DebugWindow";
            Text = "Debug Window";
            Load += DebugWindow_Load;
            ResumeLayout(false);
        }

        #endregion

        public RichTextBox DebugTextBox;
    }
}