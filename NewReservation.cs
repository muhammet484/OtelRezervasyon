using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtelRezervasyon
{
    public partial class NewReservationPanel : Form
    {
        public DataTable Reservation = new DataTable(), Customers = new DataTable();

        public NewReservationPanel()
        {
            InitializeComponent();
            ((DataGridViewComboBoxColumn)NewCustomerGridView.Columns["Cinsiyet"]).Items.Add(SQLHelper.CustomerTable.Gender.Man);
            ((DataGridViewComboBoxColumn)NewCustomerGridView.Columns["Cinsiyet"]).Items.Add(SQLHelper.CustomerTable.Gender.Woman);

            PaymentMethodComboBox.Items.Add(SQLHelper.ReservationTable.PaymentMethods.Cash);
            PaymentMethodComboBox.Items.Add(SQLHelper.ReservationTable.PaymentMethods.CreditCard);
            PaymentMethodComboBox.Items.Add(SQLHelper.ReservationTable.PaymentMethods.Transfer);
            PaymentMethodComboBox.SelectedItem = PaymentMethodComboBox.Items[0];

            CheckInDateTimePicker.ValueChanged += (sender, e) =>
            {
                CheckOutDateTimePicker.MinDate = CheckInDateTimePicker.Value;
            };

            Reservation.Columns.Add(SQLHelper.ReservationTable.Columns.RoomId);
            Reservation.Columns.Add(SQLHelper.ReservationTable.Columns.EmployeeId);
            Reservation.Columns.Add(SQLHelper.ReservationTable.Columns.CheckInDate);
            Reservation.Columns.Add(SQLHelper.ReservationTable.Columns.CheckOutDate);
            Reservation.Columns.Add(SQLHelper.ReservationTable.Columns.PaymentMethod);
            Reservation.Columns.Add(SQLHelper.ReservationTable.Columns.TotalPrice);

            Customers.Columns.Add(SQLHelper.CustomerTable.Columns.CustomerId);
            Customers.Columns.Add(SQLHelper.CustomerTable.Columns.CustomerName);
            Customers.Columns.Add(SQLHelper.CustomerTable.Columns.CustomerLastName);
            Customers.Columns.Add(SQLHelper.CustomerTable.Columns.CustomerDateOfBirth);
            Customers.Columns.Add(SQLHelper.CustomerTable.Columns.CustomerMail);
            Customers.Columns.Add(SQLHelper.CustomerTable.Columns.PhoneNumber);
            Customers.Columns.Add(SQLHelper.CustomerTable.Columns.Gender);
        }

        private void NewReservationPanel_Load(object sender, EventArgs e)
        {
            CheckInDateTimePicker.Value = CheckInDateTimePicker.MinDate = DateTime.Now;
            CheckOutDateTimePicker.Value = CheckOutDateTimePicker.MinDate = DateTime.Now.AddDays(1);

            #region Set max room number
            using (var con = SQLHelper.ConnectToDefaultServer())
            {
                string query = "SELECT COUNT(*) FROM " + SQLHelper.RoomTable.TableName;

                SqlCommand command = new SqlCommand(query, con);
                int rowCount = (int)command.ExecuteScalar();
                if (rowCount <= 0)
                    DebugWindow.Print("Couldn't get the room amount.");
                else
                    numericRoomID.Maximum = rowCount;
            }
            #endregion

            #region TODO: delete this
            //debug purpose:
            //NewCustomerGridView.Rows.Add("ahmet", "dasdas", "12345678901", "01.01.1999", "dasdsa@dsadsa.dasdsa", "12345678", "Erkek");
            //NewCustomerGridView.Rows.Add("mehmet", "adsdsa", "4567894565", "02.02.1992", "da@adsa.sdsa", "12355578", "Erkek");
            #endregion
        }

        private void ApproveButton_Click(object sender, EventArgs e)
        {

            #region Check if all values are valid

            bool hasNumber(string value) { foreach (char c in value) if (char.IsDigit(c)) return true; return false; }
            bool hasCharacterWhichIsNotNumber(string value) { foreach (char c in value) if (!char.IsDigit(c)) return true; return false; }

            bool hasLetter(string value) { foreach (char c in value) if (char.IsLetter(c)) return true; return false; }

            for (int i = 0; i < NewCustomerGridView.Rows.Count - 1; i++)
            {
                string? name = NewCustomerGridView.Rows[i].Cells[0].Value.ToString();
                if (string.IsNullOrEmpty(name) || hasNumber(name))
                {
                    MessageBox.Show("" + (i + 1) + " numaralı müşteri adı geçersiz.");
                    return;
                }
                string? lastname = NewCustomerGridView.Rows[i].Cells[1].Value.ToString();
                if (string.IsNullOrEmpty(name) || hasNumber(name))
                {
                    MessageBox.Show("" + (i + 1) + " numaralı müşteri soyadı geçersiz.");
                    return;
                }
                string? TcNo = NewCustomerGridView.Rows[i].Cells[2].Value.ToString();
                if (string.IsNullOrEmpty(TcNo) || hasCharacterWhichIsNotNumber(TcNo))
                {
                    MessageBox.Show("" + (i + 1) + " numaralı müşteri için" + " geçersiz tc no");
                    return;
                }
                if (!DateTime.TryParse(NewCustomerGridView.Rows[i].Cells[3].Value.ToString(), out DateTime date))
                {
                    MessageBox.Show("" + (i + 1) + " numaralı müşteri için" + " geçersiz doğum tarihi\n" + NewCustomerGridView.Rows[i].Cells[2].Value.ToString());
                    return;
                }
                string? mail = NewCustomerGridView.Rows[i].Cells[4].Value.ToString();
                if (!string.IsNullOrEmpty(mail) && (!mail.Contains('@') || !mail.Contains('.')))
                {
                    MessageBox.Show("" + (i + 1) + " numaralı müşteri için" + " geçersiz posta adresi");
                    return;
                }
                string? number = NewCustomerGridView.Rows[i].Cells[5].Value.ToString();
                if (!string.IsNullOrEmpty(number) && hasLetter(number))
                {
                    MessageBox.Show("" + (i + 1) + " numaralı müşteri için" + " geçersiz telefon numarası");
                    return;
                }
                string? gender = NewCustomerGridView.Rows[i].Cells[6].Value.ToString();
                if (string.IsNullOrEmpty(gender))
                {
                    MessageBox.Show("" + (i + 1) + " numaralı müşteri için" + " cinsiyet belirtilmemiş");
                    return;
                }
            }
            #endregion
            saveReservation();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        void saveReservation()
        {
            #region Save Reservation
            var reservationRow = Reservation.NewRow();
            int roomID = (int)numericRoomID.Value;
            reservationRow[SQLHelper.ReservationTable.Columns.RoomId] = roomID;
            reservationRow[SQLHelper.ReservationTable.Columns.CheckInDate] = CheckInDateTimePicker.Value;
            reservationRow[SQLHelper.ReservationTable.Columns.CheckOutDate] = CheckOutDateTimePicker.Value;
            reservationRow[SQLHelper.ReservationTable.Columns.PaymentMethod] = PaymentMethodComboBox.SelectedItem.ToString();
            reservationRow[SQLHelper.ReservationTable.Columns.EmployeeId] = SystemManager.EmployeeID;

            int DailyRoomRate = Convert.ToInt32((SQLHelper.ExecuteScalarCommand("SELECT " + SQLHelper.RoomTable.Columns.Price + " FROM " + SQLHelper.RoomTable.TableName + " WHERE " + SQLHelper.RoomTable.Columns.RoomId + "= " + roomID + ";")));
            DebugWindow.Print("newReservation.cs DailyRoom rate:" + DailyRoomRate);
            if (DailyRoomRate == -1)
                DebugWindow.Print("null exception: DailyRoomRate = -1");

            int totalPrice = (CheckOutDateTimePicker.Value - CheckInDateTimePicker.Value).Days * DailyRoomRate;
            reservationRow[SQLHelper.ReservationTable.Columns.TotalPrice] = totalPrice;

            Reservation.Rows.Add(reservationRow);
            #endregion
            #region Save Customers

            for (int i = 0; i < NewCustomerGridView.Rows.Count - 1; i++)
            {
                var CustomerRow = Customers.NewRow();
                CustomerRow[SQLHelper.CustomerTable.Columns.CustomerName] = NewCustomerGridView.Rows[i].Cells["isim"].Value;
                CustomerRow[SQLHelper.CustomerTable.Columns.CustomerLastName] = NewCustomerGridView.Rows[i].Cells["soyisim"].Value;
                CustomerRow[SQLHelper.CustomerTable.Columns.CustomerId] = NewCustomerGridView.Rows[i].Cells["TcNo"].Value;
                CustomerRow[SQLHelper.CustomerTable.Columns.CustomerDateOfBirth] = NewCustomerGridView.Rows[i].Cells["Tarih"].Value;
                CustomerRow[SQLHelper.CustomerTable.Columns.CustomerMail] = NewCustomerGridView.Rows[i].Cells["Mail"].Value;
                CustomerRow[SQLHelper.CustomerTable.Columns.PhoneNumber] = NewCustomerGridView.Rows[i].Cells["Numara"].Value;
                CustomerRow[SQLHelper.CustomerTable.Columns.Gender] = NewCustomerGridView.Rows[i].Cells["Cinsiyet"].Value;
                Customers.Rows.Add(CustomerRow);
            }
            #endregion

        }
    }
}
