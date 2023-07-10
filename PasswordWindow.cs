using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OtelRezervasyon
{
    public partial class PasswordWindow : Form
    {
        public PasswordWindow()
        {
            InitializeComponent();
            WindowManager.PasswordWindow = this;
            WindowManager.SetFormClose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (DebugWindow.DebugMode)
            {
                WindowManager.DebugWindow.Show();
            }
            #region Test the Connection and create database
            using (var connection = SQLHelper.ConnectToServer(SQLHelper.MyServerName))
            {
                //TODO: delete these lines before release. debug purpose code lines:
                //SQLHelper.ExecuteCommand("ALTER DATABASE " + SQLHelper.DatabaseName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE", connection);
                //SQLHelper.ExecuteCommand("DROP DATABASE " + SQLHelper.DatabaseName, connection);
                //----------------------------------------------------

                SQLHelper.CreateDatabaseIfNotExist(connection, SQLHelper.DatabaseName);
            }
            #endregion
            using (var connection = SQLHelper.ConnectToServer(SQLHelper.MyServerName, SQLHelper.DatabaseName))
            {
                #region Create tables if not exist
                var res = SQLHelper.CreateTable("Customers", connection,
                    ("CustomerId", SQLDataType.Char(11)),
                    new Dictionary<string, SQLDataType>()
                {
                    {"CustomerName", SQLDataType.Char(50) },
                    {"CustomerLastName", SQLDataType.Char(50)},
                    {"CustomerDateOfBirth", SQLDataType.Date },
                    {"CustomerMail", SQLDataType.Char(100).Nullable()},
                    {"PhoneNumber", SQLDataType.Char(15)},
                    {"Gender", SQLDataType.Bit},//1 = Erkek(Male), 0 = Kadýn(Female)
                });
                DebugWindow.Print("Table Creation Result = " + res);

                res = SQLHelper.CreateTable("Rooms", connection,
                    ("RoomId", SQLDataType.Int.Identity()),
                    new Dictionary<string, SQLDataType>()
                    {
                        {"RoomType", SQLDataType.Char(20) },
                        {"Price", SQLDataType.Money },
                        {"RoomQualityType", SQLDataType.Char(20) }
                    }
                    );
                DebugWindow.Print("Table Creation Result = " + res);

                res = SQLHelper.CreateTable("Employees", connection,
                    ("EmployeeId", SQLDataType.Int.Identity()),
                    new Dictionary<string, SQLDataType>()
                    {
                        {"FirstName", SQLDataType.Char(50) },
                        {"LastName", SQLDataType.Char(50) },
                        {"BirthDate", SQLDataType.Date},
                        {"PhoneNumber", SQLDataType.Char(15)},
                        {"Gender", SQLDataType.Bit},
                        {"Salary", SQLDataType.Money},
                        {"Seniority", SQLDataType.Char(15)},
                        {"StartDate", SQLDataType.Date},
                        {"IsAccessableToSystem", SQLDataType.Bit},
                        {"SystemPassword", SQLDataType.Varchar(50).Nullable()},
                        {"SystemAccessPermissionsJson", SQLDataType.VarcharMax.Nullable()},
                    }
                );
                DebugWindow.Print("Table Creation Result = " + res);

                res = SQLHelper.CreateTable("Products", connection,
                    ("ProductId", SQLDataType.Int.Identity()),
                    new Dictionary<string, SQLDataType>()
                    {
                        {"ProductName", SQLDataType.Char(50) },
                        {"Price", SQLDataType.Money },
                        {"PoductCategory", SQLDataType.Char(50) }
                    }
                    );
                DebugWindow.Print("Table Creation Result = " + res);

                res = SQLHelper.CreateTable("Orders", connection,
                    ("OrderId", SQLDataType.Int.Identity()),
                    new Dictionary<string, SQLDataType>()
                    {
                        {"ReservationId", SQLDataType.Int },
                        {"ProductId", SQLDataType.Int },
                        {"EmployeeId", SQLDataType.Int },
                        {"Quantity", SQLDataType.Int },
                        {"OrderDateTime", SQLDataType.DateTime }
                    }
                    );
                DebugWindow.Print("Table Creation Result = " + res);

                res = SQLHelper.CreateTable("Reservations", connection,
                    ("ReservationId", SQLDataType.Int.Identity()),
                    new Dictionary<string, SQLDataType>()
                    {
                        {"RoomId", SQLDataType.Int },
                        {"EmployeeId", SQLDataType.Int },
                        {"CheckInDate", SQLDataType.DateTime},
                        {"CheckOutDate", SQLDataType.DateTime },
                        {"TotalPrice", SQLDataType.Money },
                        {"PaymentMethod", SQLDataType.Char(50) },
                    }
                    );
                DebugWindow.Print("Table Creation Result = " + res);
                res = SQLHelper.CreateTable("ReservationCustomers", connection,
                    ("ReservationCustomerId", SQLDataType.Int.Identity()),
                    new Dictionary<string, SQLDataType>()
                    {
                        {"ReservationId", SQLDataType.Int},
                        {"CustomerId", SQLDataType.Int}
                    }
                );
                DebugWindow.Print("Table Creation Result = " + res);


                #endregion
                #region Set foreign keys
                const string _alter = "ALTER TABLE ";
                const string _addFK = "ADD FOREIGN KEY ";
                const string _refTo = "REFERENCES ";

                string cmd = "";
                cmd = _alter + "Reservations " + _addFK + "(RoomId) " + _refTo + "Rooms " + "(RoomId)";
                SQLHelper.ExecuteCommand(cmd, connection);
                cmd = _alter + "Reservations " + _addFK + "(EmployeeId) " + _refTo + "Employees " + "(EmployeeId)";
                SQLHelper.ExecuteCommand(cmd, connection);

                cmd = _alter + "ReservationCustomers " + _addFK + "(ReservationId) " + _refTo + "Reservations " + "(ReservationId)";
                SQLHelper.ExecuteCommand(cmd, connection);
                cmd = _alter + "ReservationCustomers " + _addFK + "(CustomerId) " + _refTo + "Customers " + "(CustomerId)";
                SQLHelper.ExecuteCommand(cmd, connection);

                cmd = _alter + "Orders " + _addFK + "(ReservationId) " + _refTo + "Reservations " + "(ReservationId)";
                SQLHelper.ExecuteCommand(cmd, connection);
                cmd = _alter + "Orders " + _addFK + "(ProductId) " + _refTo + "Products " + "(ProductId)";
                SQLHelper.ExecuteCommand(cmd, connection);
                cmd = _alter + "Orders " + _addFK + "(EmployeeId) " + _refTo + "Employees " + "(EmployeeId)";
                SQLHelper.ExecuteCommand(cmd, connection);
                #endregion
                #region add admin employee for creating super user
                if (!SQLHelper.CheckRowExists(connection, SQLHelper.EmployeeTable.TableName, SQLHelper.EmployeeTable.Columns.FirstName, "admin"))
                {
                    SQLHelper.EmployeeTable.AddRow(connection, "admin", "admin", DateTime.Now.AddYears(-30), "00", true, 0, "admin", DateTime.Now, true, "admin");
                }
                #endregion
            }

            // debug purpose open main menu
            //WindowManager.MainMenu.Show();
            SQLHelper.PrintAllData();
        }

        private void EnterButton_Click(object sender = null, EventArgs e = null)
        {
            string id = IdTextBox.Text;
            string pass = PasswordTextBox.Text;
            DebugWindow.Print("password entered: " + pass);
            string passInData = "";
            using (var connection = SQLHelper.ConnectToServer(SQLHelper.MyServerName, SQLHelper.DatabaseName))
            {
                object? dbPassObj = SQLHelper.GetValueFromTable(connection, SQLHelper.EmployeeTable.TableName, SQLHelper.EmployeeTable.Columns.SystemPassword, SQLHelper.EmployeeTable.Columns.EmployeeId, id);
                if (dbPassObj != null)
                    passInData = (string)dbPassObj;
            }
            if (string.IsNullOrEmpty(passInData))
            {
                WrongPasswordWarningLabel.Text = "Bu kullanýcý Id'si sistemde bulunamadý";
                WrongPasswordWarningLabel.Visible = true;
                return;
            }

            DebugWindow.Print("passInData: " + passInData);
            if (pass != passInData)
            {
                WrongPasswordWarningLabel.Text = "Yanlýþ þifre";
                WrongPasswordWarningLabel.Visible = true;
                return;
            }
            SystemEnter(int.Parse(id));
        }
        void SystemEnter(int EmployeeID)
        {
            DebugWindow.Print("system enter success with employee ID " + EmployeeID);
            SystemManager.EmployeeID = EmployeeID;

            WindowManager.MainMenu.Show();
            Hide();
        }

        private void IdTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //enter only numbers for RoomNo
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                EnterButton_Click(sender);
        }
    }
}