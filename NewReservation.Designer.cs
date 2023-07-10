namespace OtelRezervasyon
{
    partial class NewReservationPanel
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
            NewCustomerGridView = new DataGridView();
            İsim = new DataGridViewTextBoxColumn();
            Soyad = new DataGridViewTextBoxColumn();
            numericRoomID = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            CheckInDateTimePicker = new DateTimePicker();
            CheckOutDateTimePicker = new DateTimePicker();
            PaymentMethodComboBox = new ComboBox();
            CancelButton = new Button();
            ApproveButton = new Button();
            isim = new DataGridViewTextBoxColumn();
            soyisim = new DataGridViewTextBoxColumn();
            TcNo = new DataGridViewTextBoxColumn();
            Tarih = new DataGridViewTextBoxColumn();
            Mail = new DataGridViewTextBoxColumn();
            Numara = new DataGridViewTextBoxColumn();
            Cinsiyet = new DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)NewCustomerGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericRoomID).BeginInit();
            SuspendLayout();
            // 
            // NewCustomerGridView
            // 
            NewCustomerGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            NewCustomerGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            NewCustomerGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            NewCustomerGridView.Columns.AddRange(new DataGridViewColumn[] { isim, soyisim, TcNo, Tarih, Mail, Numara, Cinsiyet });
            NewCustomerGridView.Location = new Point(12, 250);
            NewCustomerGridView.Name = "NewCustomerGridView";
            NewCustomerGridView.RowHeadersWidth = 51;
            NewCustomerGridView.RowTemplate.Height = 29;
            NewCustomerGridView.Size = new Size(776, 188);
            NewCustomerGridView.TabIndex = 0;
            // 
            // İsim
            // 
            İsim.HeaderText = "İsim";
            İsim.MinimumWidth = 6;
            İsim.Name = "İsim";
            İsim.Width = 125;
            // 
            // Soyad
            // 
            Soyad.HeaderText = "Soyad";
            Soyad.MinimumWidth = 6;
            Soyad.Name = "Soyad";
            Soyad.Width = 125;
            // 
            // numericRoomID
            // 
            numericRoomID.Location = new Point(184, 27);
            numericRoomID.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericRoomID.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericRoomID.Name = "numericRoomID";
            numericRoomID.Size = new Size(150, 27);
            numericRoomID.TabIndex = 1;
            numericRoomID.ThousandsSeparator = true;
            numericRoomID.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Variable Text", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 23);
            label1.Name = "label1";
            label1.Size = new Size(89, 27);
            label1.TabIndex = 2;
            label1.Text = "Oda No:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Variable Text", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 220);
            label2.Name = "label2";
            label2.Size = new Size(115, 27);
            label2.TabIndex = 3;
            label2.Text = "Müşteriler:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Variable Text", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(12, 68);
            label3.Name = "label3";
            label3.Size = new Size(166, 27);
            label3.TabIndex = 4;
            label3.Text = "Başlangıç Tarihi:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Variable Text", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(12, 110);
            label4.Name = "label4";
            label4.Size = new Size(120, 27);
            label4.TabIndex = 5;
            label4.Text = "Çıkış Tarihi:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Variable Text", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(12, 150);
            label5.Name = "label5";
            label5.Size = new Size(167, 27);
            label5.TabIndex = 6;
            label5.Text = "Ödeme Yöntemi:";
            // 
            // CheckInDateTimePicker
            // 
            CheckInDateTimePicker.Location = new Point(184, 69);
            CheckInDateTimePicker.Name = "CheckInDateTimePicker";
            CheckInDateTimePicker.Size = new Size(250, 27);
            CheckInDateTimePicker.TabIndex = 7;
            // 
            // CheckOutDateTimePicker
            // 
            CheckOutDateTimePicker.Location = new Point(184, 111);
            CheckOutDateTimePicker.Name = "CheckOutDateTimePicker";
            CheckOutDateTimePicker.Size = new Size(250, 27);
            CheckOutDateTimePicker.TabIndex = 8;
            // 
            // PaymentMethodComboBox
            // 
            PaymentMethodComboBox.FormattingEnabled = true;
            PaymentMethodComboBox.Location = new Point(184, 153);
            PaymentMethodComboBox.Name = "PaymentMethodComboBox";
            PaymentMethodComboBox.Size = new Size(151, 28);
            PaymentMethodComboBox.TabIndex = 9;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(658, 47);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(130, 29);
            CancelButton.TabIndex = 10;
            CancelButton.Text = "İptal";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // ApproveButton
            // 
            ApproveButton.Location = new Point(658, 12);
            ApproveButton.Name = "ApproveButton";
            ApproveButton.Size = new Size(130, 29);
            ApproveButton.TabIndex = 11;
            ApproveButton.Text = "Onayla";
            ApproveButton.UseVisualStyleBackColor = true;
            ApproveButton.Click += ApproveButton_Click;
            // 
            // isim
            // 
            isim.HeaderText = "Ad";
            isim.MinimumWidth = 6;
            isim.Name = "isim";
            // 
            // soyisim
            // 
            soyisim.HeaderText = "Soyad";
            soyisim.MinimumWidth = 6;
            soyisim.Name = "soyisim";
            // 
            // TcNo
            // 
            TcNo.HeaderText = "TcNo";
            TcNo.MaxInputLength = 11;
            TcNo.MinimumWidth = 6;
            TcNo.Name = "TcNo";
            // 
            // Tarih
            // 
            Tarih.HeaderText = "Doğum Tarihi";
            Tarih.MinimumWidth = 6;
            Tarih.Name = "Tarih";
            // 
            // Mail
            // 
            Mail.HeaderText = "E-Posta";
            Mail.MinimumWidth = 6;
            Mail.Name = "Mail";
            // 
            // Numara
            // 
            Numara.HeaderText = "Numara";
            Numara.MinimumWidth = 6;
            Numara.Name = "Numara";
            // 
            // Cinsiyet
            // 
            Cinsiyet.HeaderText = "Cinsiyet";
            Cinsiyet.MinimumWidth = 6;
            Cinsiyet.Name = "Cinsiyet";
            Cinsiyet.Resizable = DataGridViewTriState.True;
            Cinsiyet.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // NewReservationPanel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ApproveButton);
            Controls.Add(CancelButton);
            Controls.Add(PaymentMethodComboBox);
            Controls.Add(CheckOutDateTimePicker);
            Controls.Add(CheckInDateTimePicker);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numericRoomID);
            Controls.Add(NewCustomerGridView);
            Name = "NewReservationPanel";
            Text = "Yeni Rezervasyon";
            Load += NewReservationPanel_Load;
            ((System.ComponentModel.ISupportInitialize)NewCustomerGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericRoomID).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView NewCustomerGridView;
        private DataGridViewTextBoxColumn İsim;
        private DataGridViewTextBoxColumn Soyad;
        private NumericUpDown numericRoomID;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private DateTimePicker CheckInDateTimePicker;
        private DateTimePicker CheckOutDateTimePicker;
        private ComboBox PaymentMethodComboBox;
        private Button CancelButton;
        private Button ApproveButton;
        private DataGridViewTextBoxColumn isim;
        private DataGridViewTextBoxColumn soyisim;
        private DataGridViewTextBoxColumn TcNo;
        private DataGridViewTextBoxColumn Tarih;
        private DataGridViewTextBoxColumn Mail;
        private DataGridViewTextBoxColumn Numara;
        private DataGridViewComboBoxColumn Cinsiyet;
    }
}