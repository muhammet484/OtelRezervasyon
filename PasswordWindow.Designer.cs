namespace OtelRezervasyon
{
    partial class PasswordWindow
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
            label2 = new Label();
            IdTextBox = new TextBox();
            label3 = new Label();
            label4 = new Label();
            PasswordTextBox = new TextBox();
            EnterButton = new Button();
            WrongPasswordWarningLabel = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.BorderStyle = BorderStyle.Fixed3D;
            label1.Font = new Font("Lucida Handwriting", 16F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(772, 38);
            label1.TabIndex = 0;
            label1.Text = "Otel Rezervasyon Uygulamasına Hoş Geldiniz!";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Variable Text", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(328, 132);
            label2.Name = "label2";
            label2.Size = new Size(215, 32);
            label2.TabIndex = 1;
            label2.Text = "Lütfen giriş yapınız";
            // 
            // IdTextBox
            // 
            IdTextBox.Anchor = AnchorStyles.None;
            IdTextBox.Location = new Point(418, 187);
            IdTextBox.Name = "IdTextBox";
            IdTextBox.Size = new Size(125, 27);
            IdTextBox.TabIndex = 2;
            IdTextBox.KeyPress += IdTextBox_KeyPress;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Variable Text", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(258, 181);
            label3.Name = "label3";
            label3.Size = new Size(121, 32);
            label3.TabIndex = 3;
            label3.Text = "Çalışan Id:";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Variable Text", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(258, 222);
            label4.Name = "label4";
            label4.Size = new Size(149, 32);
            label4.TabIndex = 5;
            label4.Text = "Çalışan Şifre:";
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Anchor = AnchorStyles.None;
            PasswordTextBox.Location = new Point(418, 228);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PasswordChar = '●';
            PasswordTextBox.Size = new Size(125, 27);
            PasswordTextBox.TabIndex = 4;
            PasswordTextBox.KeyDown += PasswordTextBox_KeyDown;
            // 
            // EnterButton
            // 
            EnterButton.Anchor = AnchorStyles.None;
            EnterButton.Location = new Point(435, 274);
            EnterButton.Name = "EnterButton";
            EnterButton.Size = new Size(94, 29);
            EnterButton.TabIndex = 6;
            EnterButton.Text = "Giriş";
            EnterButton.UseVisualStyleBackColor = true;
            EnterButton.Click += EnterButton_Click;
            // 
            // WrongPasswordWarningLabel
            // 
            WrongPasswordWarningLabel.Anchor = AnchorStyles.None;
            WrongPasswordWarningLabel.AutoSize = true;
            WrongPasswordWarningLabel.BackColor = Color.Transparent;
            WrongPasswordWarningLabel.ForeColor = Color.Coral;
            WrongPasswordWarningLabel.Location = new Point(444, 314);
            WrongPasswordWarningLabel.Name = "WrongPasswordWarningLabel";
            WrongPasswordWarningLabel.Size = new Size(78, 20);
            WrongPasswordWarningLabel.TabIndex = 7;
            WrongPasswordWarningLabel.Text = "Yanlış şifre";
            WrongPasswordWarningLabel.Visible = false;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.None;
            label5.BorderStyle = BorderStyle.Fixed3D;
            label5.Font = new Font("Segoe UI Variable Text", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(247, 120);
            label5.Name = "label5";
            label5.Size = new Size(313, 224);
            label5.TabIndex = 50;
            // 
            // PasswordWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(WrongPasswordWarningLabel);
            Controls.Add(EnterButton);
            Controls.Add(label4);
            Controls.Add(PasswordTextBox);
            Controls.Add(label3);
            Controls.Add(IdTextBox);
            Controls.Add(label2);
            Controls.Add(label5);
            Name = "PasswordWindow";
            Text = "Otel Rezervasyon";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox IdTextBox;
        private Label label3;
        private Label label4;
        private TextBox PasswordTextBox;
        private Button EnterButton;
        private Label WrongPasswordWarningLabel;
        private Label label5;
    }
}