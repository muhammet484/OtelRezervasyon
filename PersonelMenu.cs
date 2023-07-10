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
    public partial class PersonelMenu : Form
    {
        public PersonelMenu()
        {
            InitializeComponent();
        }

        private void DataGridView1_TableChanged()
        {
            using (var con = SQLHelper.ConnectToDefaultServer())
            {
                using (var connection = SQLHelper.ConnectToDefaultServer())
                {
                    string deleteCommandText = $"DELETE FROM " + SQLHelper.EmployeeTable.TableName;
                    SQLHelper.ExecuteCommand(deleteCommandText, connection);
                    try
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            var cell = row.Cells;
                            SQLHelper.EmployeeTable.AddRow(connection,
                                cell[SQLHelper.EmployeeTable.Columns.FirstName].Value.ToString(),
                                cell[SQLHelper.EmployeeTable.Columns.LastName].Value.ToString(),
                                Convert.ToDateTime(cell[SQLHelper.EmployeeTable.Columns.BirthDate].Value),
                                cell[SQLHelper.EmployeeTable.Columns.PhoneNumber].Value.ToString(),
                                SQLHelper.CustomerTable.Gender.ConvertGenderToBoolean(cell[SQLHelper.EmployeeTable.Columns.Gender].Value.ToString()) ?? true,
                                Convert.ToDecimal(cell[SQLHelper.EmployeeTable.Columns.Salary].Value),
                                cell[SQLHelper.EmployeeTable.Columns.Seniority].Value.ToString(),
                                Convert.ToDateTime(cell[SQLHelper.EmployeeTable.Columns.BirthDate].Value),
                                Convert.ToBoolean(cell[SQLHelper.EmployeeTable.Columns.IsAccessableToSystem].Value),
                                cell[SQLHelper.EmployeeTable.Columns.SystemPassword].Value.ToString(),
                                cell[SQLHelper.EmployeeTable.Columns.SystemAccessPermissionsJson].Value.ToString()

                                );
                        }
                    }
                    catch (Exception e)
                    {
                        DebugWindow.Print(e.Message);
                    }
                }
            }
        }

        private void Personel_Load(object sender, EventArgs e)
        {

            using (var con = SQLHelper.ConnectToDefaultServer())
            {
                dataGridView1.DataSource = SQLHelper.GetAllTable(con, SQLHelper.EmployeeTable.TableName);
            }
            dataGridView1.RowsAdded += (sender, e) => DataGridView1_TableChanged();
            dataGridView1.RowsRemoved += (sender, e) => DataGridView1_TableChanged();
            dataGridView1.CellEndEdit += (sender, e) => DataGridView1_TableChanged();
        }
        DataTable DataGridViewToDataTable(DataGridView dataGridView)
        {
            DataTable dataTable = new DataTable();

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.HeaderText != SQLHelper.EmployeeTable.Columns.EmployeeId)
                    dataTable.Columns.Add(column.HeaderText, column.ValueType);
                DebugWindow.Print("column.HeaderText: " + column.HeaderText);
                DebugWindow.Print("column.Name: " + column.Name);
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dataRow[i] = row.Cells[i].Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
