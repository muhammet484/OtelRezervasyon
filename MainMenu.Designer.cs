namespace OtelRezervasyon
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            statusStrip1 = new StatusStrip();
            toolStripSplitButton1 = new ToolStripDropDownButton();
            dasdsaToolStripMenuItem = new ToolStripMenuItem();
            dsadsaToolStripMenuItem = new ToolStripMenuItem();
            MainMenuGridView = new DataGridView();
            ExitButton = new Button();
            AddNewReservationButton = new Button();
            HistoryButton = new Button();
            PersonelButton = new Button();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainMenuGridView).BeginInit();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Dock = DockStyle.Top;
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripSplitButton1 });
            statusStrip1.Location = new Point(0, 0);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 26);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            statusStrip1.Visible = false;
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripSplitButton1.DropDownItems.AddRange(new ToolStripItem[] { dasdsaToolStripMenuItem, dsadsaToolStripMenuItem });
            toolStripSplitButton1.Image = (Image)resources.GetObject("toolStripSplitButton1.Image");
            toolStripSplitButton1.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new Size(60, 24);
            toolStripSplitButton1.Text = "Menü";
            // 
            // dasdsaToolStripMenuItem
            // 
            dasdsaToolStripMenuItem.Name = "dasdsaToolStripMenuItem";
            dasdsaToolStripMenuItem.Size = new Size(159, 26);
            dasdsaToolStripMenuItem.Text = "Ayarlar";
            // 
            // dsadsaToolStripMenuItem
            // 
            dsadsaToolStripMenuItem.Name = "dsadsaToolStripMenuItem";
            dsadsaToolStripMenuItem.Size = new Size(159, 26);
            dsadsaToolStripMenuItem.Text = "Hakkımda";
            dsadsaToolStripMenuItem.Click += HakkımdaToolStripMenuItem_Click;
            // 
            // MainMenuGridView
            // 
            MainMenuGridView.AllowUserToAddRows = false;
            MainMenuGridView.AllowUserToDeleteRows = false;
            MainMenuGridView.AllowUserToOrderColumns = true;
            MainMenuGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MainMenuGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            MainMenuGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            MainMenuGridView.BackgroundColor = SystemColors.Control;
            MainMenuGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MainMenuGridView.GridColor = SystemColors.Control;
            MainMenuGridView.Location = new Point(12, 47);
            MainMenuGridView.Margin = new Padding(55);
            MainMenuGridView.Name = "MainMenuGridView";
            MainMenuGridView.ReadOnly = true;
            MainMenuGridView.RowHeadersWidth = 51;
            MainMenuGridView.RowTemplate.Height = 29;
            MainMenuGridView.Size = new Size(776, 349);
            MainMenuGridView.TabIndex = 1;
            // 
            // ExitButton
            // 
            ExitButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ExitButton.Location = new Point(694, 12);
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new Size(94, 30);
            ExitButton.TabIndex = 2;
            ExitButton.Text = "Çıkış";
            ExitButton.UseVisualStyleBackColor = true;
            ExitButton.Click += ExitButton_Click;
            // 
            // AddNewReservationButton
            // 
            AddNewReservationButton.Location = new Point(12, 12);
            AddNewReservationButton.Name = "AddNewReservationButton";
            AddNewReservationButton.Size = new Size(133, 29);
            AddNewReservationButton.TabIndex = 3;
            AddNewReservationButton.Text = "Yeni Rezervasyon";
            AddNewReservationButton.UseVisualStyleBackColor = true;
            AddNewReservationButton.Click += AddNewReservationButton_Click;
            // 
            // HistoryButton
            // 
            HistoryButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            HistoryButton.Location = new Point(634, 409);
            HistoryButton.Name = "HistoryButton";
            HistoryButton.Size = new Size(154, 29);
            HistoryButton.TabIndex = 4;
            HistoryButton.Tag = "";
            HistoryButton.Text = "Tüm Rezervasyonlar";
            HistoryButton.UseVisualStyleBackColor = true;
            HistoryButton.Click += HistoryButton_Click;
            // 
            // PersonelButton
            // 
            PersonelButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PersonelButton.Location = new Point(12, 409);
            PersonelButton.Name = "PersonelButton";
            PersonelButton.Size = new Size(94, 29);
            PersonelButton.TabIndex = 5;
            PersonelButton.Tag = "";
            PersonelButton.Text = "Personel";
            PersonelButton.UseVisualStyleBackColor = true;
            PersonelButton.Click += PersonelButtonClick;
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(PersonelButton);
            Controls.Add(HistoryButton);
            Controls.Add(AddNewReservationButton);
            Controls.Add(ExitButton);
            Controls.Add(MainMenuGridView);
            Controls.Add(statusStrip1);
            Name = "MainMenu";
            Text = "Aktif Rezervasyonlar";
            Load += MainMenu_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MainMenuGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripDropDownButton toolStripSplitButton1;
        private ToolStripMenuItem dasdsaToolStripMenuItem;
        private ToolStripMenuItem dsadsaToolStripMenuItem;
        private DataGridView MainMenuGridView;
        private Button ExitButton;
        private Button AddNewReservationButton;
        private Button HistoryButton;
        private Button PersonelButton;
    }
}