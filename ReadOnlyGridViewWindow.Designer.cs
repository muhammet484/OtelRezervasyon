namespace OtelRezervasyon
{
    partial class ReadOnlyGridViewWindow
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
            dataGridView1 = new DataGridView();
            BackButton1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 47);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(776, 391);
            dataGridView1.TabIndex = 0;
            // 
            // BackButton1
            // 
            BackButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackButton1.Location = new Point(694, 12);
            BackButton1.Name = "BackButton1";
            BackButton1.Size = new Size(94, 29);
            BackButton1.TabIndex = 1;
            BackButton1.Text = "Geri";
            BackButton1.UseVisualStyleBackColor = true;
            BackButton1.Click += BackButton1_Click;
            // 
            // ReadOnlyGridViewWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BackButton1);
            Controls.Add(dataGridView1);
            Name = "ReadOnlyGridViewWindow";
            Text = "Form1";
            Load += ReadOnlyGridViewWindow_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public DataGridView dataGridView1;
        private Button BackButton1;
    }
}