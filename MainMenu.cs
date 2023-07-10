using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtelRezervasyon
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        record MainMenuReservationRow(int RoomNo, float TotalPayment, int CustomerNumber, DateTime CheckOutDate);

        private void MainMenu_Load(object sender, EventArgs e)
        {
            //debug purpose:
            SQLHelper.FillDatabaseWithSampleData();


            using (var connection = SQLHelper.ConnectToDefaultServer())
            {
                var reservations = SQLHelper.GetDataAsTable(connection, @"
                    SELECT R.RoomID, R.TotalPrice, COUNT(RC.ReservationID) AS 'RowCount',  R.CheckInDate, R.CheckOutDate
                    FROM Reservations R
                    LEFT JOIN ReservationCustomers RC ON R.ReservationID = RC.ReservationID
                    WHERE R.CheckOutDate > GETDATE() -- Sadece bugünden sonraki tarihler için
                    GROUP BY R.RoomID, R.TotalPrice, R.CheckInDate, R.CheckOutDate
                ");
                MainMenuGridView.DataSource = reservations;

                //TODO: delete this:
                DebugWindow.Print("\n" + SQLHelper.DataTableToString(SQLHelper.GetAllTable(connection, "Reservations")));
            }
            MainMenuGridView.Columns[0].Name = MainMenuGridView.Columns[0].HeaderText = "Oda";
            MainMenuGridView.Columns[1].HeaderText = "Toplam Tutar (₺)";
            MainMenuGridView.Columns[1].Name = "Toplam Tutar";
            MainMenuGridView.Columns[2].Name = MainMenuGridView.Columns[2].HeaderText = "Kalan Misafir Sayısı";
            MainMenuGridView.Columns[3].Name = MainMenuGridView.Columns[3].HeaderText = "Giriş Tarihi";
            MainMenuGridView.Columns[3].Name = MainMenuGridView.Columns[4].HeaderText = "Çıkış Tarihi";

            MainMenuGridView.Sort(MainMenuGridView.Columns[4], ListSortDirection.Ascending);

            MainMenuGridView.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

        private void DataGridView1_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            DataGridView? dataGridView = (DataGridView?)sender;
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;

            object? cellValue = null;

            if (dataGridView != null && rowIndex > -1 && columnIndex > -1 && rowIndex < dataGridView.Rows.Count - 1)
            {
                cellValue = MainMenuGridView.Rows[rowIndex].Cells[columnIndex].Value;
                //Text = "Row = " + rowIndex + ", Column = " + columnIndex + ", Value = " + cellValue + ", Rows = " + dataGridView.Rows.Count + ", Columns = " + dataGridView.Columns.Count;
            }
            else
            {
                //Text = "null";
            }
        }

        private void HakkımdaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowManager.ShowAboutWindow();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AddNewReservationButton_Click(object sender, EventArgs e)
        {
            NewReservationPanel newReservation = new NewReservationPanel();
            if (newReservation.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var reservation = newReservation.Reservation;
            var customers = newReservation.Customers.Rows;
            using (var connection = SQLHelper.ConnectToDefaultServer())
            {
                DebugWindow.Print("reservastions >> \n" + SQLHelper.DataTableToString(reservation));

                SQLHelper.ReservationTable.AddRow(connection,
                    Convert.ToInt32(reservation.Rows[0][SQLHelper.ReservationTable.Columns.RoomId]),
                     Convert.ToInt32(reservation.Rows[0][SQLHelper.ReservationTable.Columns.EmployeeId]),
                    Convert.ToDateTime(reservation.Rows[0][SQLHelper.ReservationTable.Columns.CheckInDate]),
                    Convert.ToDateTime(reservation.Rows[0][SQLHelper.ReservationTable.Columns.CheckOutDate]),
                     Convert.ToInt32(reservation.Rows[0][SQLHelper.ReservationTable.Columns.TotalPrice]),
                    (string)reservation.Rows[0][SQLHelper.ReservationTable.Columns.PaymentMethod]
                    );

                DataTable customersInData = SQLHelper.GetAllTable(connection, SQLHelper.CustomerTable.TableName);
                #region Check if data has new customers already in it, then add the customers that is not in data to the "customersInData" table
                for (int i = 0; i < customers.Count; i++)
                {

                    DataRow customer = customers[i];
                    bool ThereIsSameCustomerInData = false;
                    for (int i2 = 0; i2 < customersInData.Rows.Count; i2++)
                    {
                        DataRow _customerInData = customersInData.Rows[i2];
                        var key = SQLHelper.CustomerTable.Columns.CustomerId;
                        if (customer[key] == _customerInData[key])
                        {
                            for (int i3 = 0; i3 < customersInData.Columns.Count; i3++)
                            {
                                customersInData.Rows[i2][i3] = customer[i3];
                            }
                            ThereIsSameCustomerInData = true;
                            break;
                        }
                    }
                    if (!ThereIsSameCustomerInData)
                    {
                        var CustomerRow = customersInData.NewRow();
                        CustomerRow[SQLHelper.CustomerTable.Columns.CustomerName] = customer[SQLHelper.CustomerTable.Columns.CustomerName];
                        CustomerRow[SQLHelper.CustomerTable.Columns.CustomerLastName] = customer[SQLHelper.CustomerTable.Columns.CustomerLastName];
                        CustomerRow[SQLHelper.CustomerTable.Columns.CustomerId] = customer[SQLHelper.CustomerTable.Columns.CustomerId];
                        CustomerRow[SQLHelper.CustomerTable.Columns.CustomerDateOfBirth] = customer[SQLHelper.CustomerTable.Columns.CustomerDateOfBirth];
                        CustomerRow[SQLHelper.CustomerTable.Columns.CustomerMail] = customer[SQLHelper.CustomerTable.Columns.CustomerMail];
                        CustomerRow[SQLHelper.CustomerTable.Columns.PhoneNumber] = customer[SQLHelper.CustomerTable.Columns.PhoneNumber];
                        CustomerRow[SQLHelper.CustomerTable.Columns.Gender] = SQLHelper.CustomerTable.Gender.ConvertGenderToBoolean(customer[SQLHelper.CustomerTable.Columns.Gender].ToString());
                        customersInData.Rows.Add(CustomerRow);
                    }
                }
                #endregion
                #region find duplicates and remove
                for (int i = 0; i < customersInData.Rows.Count; i++)
                {
                    var key = SQLHelper.CustomerTable.Columns.CustomerId;
                    for (int i2 = i+1; i2 < customersInData.Rows.Count; i2++)
                    {
                        if (customersInData.Rows[i][key].ToString() == customersInData.Rows[i2][key].ToString())
                            customersInData.Rows[i].Delete();
                    }
                }
                #endregion
                DebugWindow.Print("customersInData >> \n" + SQLHelper.DataTableToString(customersInData));
                DebugWindow.Print("Old customer data:\n" + SQLHelper.DataTableToString(SQLHelper.GetAllTable(connection, SQLHelper.CustomerTable.TableName)));
                SQLHelper.ReplaceTableData(SQLHelper.CustomerTable.TableName, customersInData, connection);
                DebugWindow.Print("New customer data:\n" + SQLHelper.DataTableToString(SQLHelper.GetAllTable(connection, SQLHelper.CustomerTable.TableName)));

            }
        }

        private void PersonelButtonClick(object sender, EventArgs e)
        {
            WindowManager.PersonelMenu.ShowDialog();
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            var window = new ReadOnlyGridViewWindow();
            window.Text = "Bütün Rezervasyonlar";
            window.dataGridView1.DataSource = SQLHelper.GetAllTable(SQLHelper.ReservationTable.TableName);
            window.ShowDialog();
        }
    }
}
