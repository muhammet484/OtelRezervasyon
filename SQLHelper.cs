
using OtelRezervasyon;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OtelRezervasyon
{
    public static class SQLHelper
    {
        public static string MyServerName = "MUHAMMET";

        public static string DatabaseName = "OtelRezervasyonDB";

        public static bool FillDBWithSampleData = true;

        /// <summary> Connects to default server and databese. Then returns the connection. </summary>
        public static SqlConnection ConnectToDefaultServer()
        {
            return ConnectToServer(MyServerName, DatabaseName);
        }
        /// <summary>
        /// creates new SqlConnection and opens the connection on given serverName. Do not forget to use this function in a using statement
        /// </summary>
        public static SqlConnection ConnectToServer(string serverName, string? databaseName = null)
        {
            var sqlConnection = new SqlConnection("Data Source=" + serverName + ";" +
                (string.IsNullOrEmpty(databaseName) ? null : "Initial Catalog=" + databaseName + ";")
                + "Integrated Security=True;");
            return ConnectToServer(sqlConnection);
        }
        public static SqlConnection ConnectToServer(string serverName, string userName, string password, string? databaseName)
        {
            var con = new SqlConnection(
            "Data Source=" + serverName + ";" +
                (string.IsNullOrEmpty(databaseName) ? null : "Initial Catalog=" + databaseName + ";")
                + "User ID=" + userName + ";Password=" + password);
            return ConnectToServer(con);
        }
        public static SqlConnection ConnectToServer(SqlConnection sqlConnection)
        {
            try
            {
                sqlConnection.Open();
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    DebugWindow.Print("connection succesfull");
                    DebugWindow.Print("Connected to server: " + sqlConnection.ConnectionString);
                }
                else
                    DebugWindow.Print("connection fail!");
                return sqlConnection;
            }
            catch (Exception ex)
            {
                DebugWindow.Print("Exception occured while opening server connection: " + ex.Message);
                return sqlConnection;
            }
        }
        public static DataTable DataGridViewToDataTable(DataGridView dataGridView)
        {
            DataTable dataTable = new DataTable();

            // Sütunları ekleyin
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.HeaderText, column.ValueType);
            }

            // Satırları ekleyin
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

        public enum CreateTableState
        {
            SuccessfullOrExist, ConnectionError, ExecuteNonQueryError, ExecuteScalarError
        }
        public static CreateTableState CreateTable(string tableName, SqlConnection connection, (string, SQLDataType) primaryKeyNameAndType, Dictionary<string, SQLDataType> Columns)
        {
            if (connection.State != System.Data.ConnectionState.Open)
                try
                {
                    connection.Open();
                }
                catch
                {
                    return CreateTableState.ConnectionError;
                }

            string checkTableQuery = $"IF OBJECT_ID('{tableName}', 'U') IS NULL SELECT 'TableNotExist' ELSE SELECT 'TableExist'";

            using (SqlCommand command = new SqlCommand(checkTableQuery, connection))
            {
                string result = (string)command.ExecuteScalar();

                if (result == "TableNotExist")
                {
                    string createTableQuery = "CREATE TABLE " + tableName + " ("; //example: ColumnName datatype(50) NULL

                    createTableQuery += 
                        primaryKeyNameAndType.Item1 + " " + primaryKeyNameAndType.Item2 +
                        $"{(primaryKeyNameAndType.Item2.Length != null ? "(" + primaryKeyNameAndType.Item2.Length + ")" : null)}" +
                        (primaryKeyNameAndType.Item2.IsIdentity ? " IDENTITY(1,1)" : "") +
                        " PRIMARY KEY,";

                    string columnDefinitions = string.Join(", ", Columns.Select(column => $"{column.Key} " +
                    $"{column.Value}" +
                    $"{(column.Value.Length != null ? "(" + column.Value.Length + ")" : null)}" +
                    $"{(column.Value.IsNullable ? " NULL" : " NOT NULL")}"
                    ));
                    createTableQuery += columnDefinitions + ")";
                    using (SqlCommand createCommand = new SqlCommand(createTableQuery, connection))
                    {
                        DebugWindow.Print("Table have been tried to add: " + createTableQuery);
                        try
                        {
                            int i = createCommand.ExecuteNonQuery();
                            if (i == -1)
                                return CreateTableState.SuccessfullOrExist;
                        }
                        catch (Exception ex)
                        {
                            DebugWindow.Print("exception occured:" + ex.Message);
                            return CreateTableState.ExecuteNonQueryError;
                        }
                        return CreateTableState.SuccessfullOrExist;
                    }
                }
                else if (result == "TableExist")
                {
                    return CreateTableState.SuccessfullOrExist;
                }
                else
                    return CreateTableState.ExecuteScalarError;
            }
        }

        public static bool ExecuteCommand(string command, SqlConnection connection)
        {
            if (connection.State != System.Data.ConnectionState.Open)
                try { connection.Open(); }
                catch (Exception ex) { DebugWindow.Print("exception occured while opening connection: \n" + ex.Message); return false; }
            using (SqlCommand _command = new SqlCommand(command, connection))
            {
                DebugWindow.Print("Command: " + _command.CommandText);
                try
                {
                    var _res = _command.ExecuteNonQuery();
                    DebugWindow.Print("Results: " + _res);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWindow.Print("exception occured: \n" + ex.Message);
                    return false;
                }
            }
        }
        /// <summary>
        /// safely executes a scalar command
        /// </summary>
        /// <returns> scalar result</returns>
        public static object? ExecuteScalarCommand(string command, SqlConnection connection)
        {
            if (connection.State != System.Data.ConnectionState.Open)
                try { connection.Open(); }
                catch (Exception ex) { DebugWindow.Print("exception occured while opening connection: \n" + ex.Message); return false; }
            using (SqlCommand _command = new SqlCommand(command, connection))
            {
                DebugWindow.Print("Command: " + _command.CommandText);
                try
                {
                    object _res = _command.ExecuteScalar();
                    DebugWindow.Print("Results: " + _res);
                    return _res;
                }
                catch (Exception ex)
                {
                    DebugWindow.Print("exception occured: \n" + ex.Message);
                    return null;
                }
            }
        }
        public static object? ExecuteScalarCommand(string command)
        {
            using(var connection = ConnectToDefaultServer())
            {
                return ExecuteScalarCommand(command, connection);
            }
        }

        public static bool CheckRowExists(SqlConnection connection, string tableName, string columnName, object value)
        {
            if (connection.State != System.Data.ConnectionState.Open)
                 connection.Open();
            try
            {
                string query = $"SELECT COUNT(*) FROM {tableName} WHERE {columnName} = @Value";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value", value);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                DebugWindow.Print(ex.Message);
                return false;
            }
        }
        
        public static object? GetValueFromTable(SqlConnection connection, string tableName, string columnName, string findFrom, object value)
        {
            if (connection.State != ConnectionState.Open)
                try { connection.Open(); }
                catch (Exception ex) { DebugWindow.Print("exception occured while opening connection: \n" + ex.Message); return null; }

            try
            {
                string query = $"SELECT {columnName} FROM {tableName} WHERE {findFrom} = @Id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", value);
                    object result = command.ExecuteScalar();
                    return result;
                }
            }
            catch (Exception ex)
            {
                DebugWindow.Print(ex.Message);
                return null;
            }
        }
        public static DataTable GetDataAsTable(SqlConnection connection, string query)
        {
            DebugWindow.Print("Function: GetDataAsTable, query: " + query);
            DataTable dataTable = new DataTable();

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWindow.Print(ex.Message);
            }

            return dataTable;
        }
        public static DataTable GetAllTable(SqlConnection connection, string tableName)
        {
            return GetDataAsTable(connection, "SELECT * FROM " + tableName);
        }
        public static DataTable GetAllTable(string tableName)
        {
            using(var connection = ConnectToDefaultServer())
            {

                return GetAllTable(connection, tableName);
            }
        }

        public static void ReplaceTableData(string tableName, DataTable newData, SqlConnection connection)
        {
            try
            {
                string deleteCommandText = $"DELETE FROM {tableName}";
                ExecuteCommand(deleteCommandText, connection);


                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(newData);
                }
            }
            catch(Exception e)
            {
                DebugWindow.Print("exception occured in ReplaceTableData: " + e.Message);
            }
        }


        public static bool CreateDatabaseIfNotExist(SqlConnection connection, string DatabaseName)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                var command = new SqlCommand($"If(db_id(N'{DatabaseName}') IS NULL) CREATE DATABASE [{DatabaseName}]", connection);
                try
                {
                    var i = command.ExecuteNonQuery();
                    DebugWindow.Print("Database command result = " + i);
                }
                catch (Exception ex)
                {
                    DebugWindow.Print("Exception occured: " + ex.Message);
                    command.Dispose();
                    return false;
                }
                command.Dispose();
                return true;
            }

            return false;
        }

        public static class CustomerTable
        {
            public static string TableName = "Customers";

            public static class Columns
            {
                public static string CustomerId = "CustomerId";
                public static string CustomerName = "CustomerName";
                public static string CustomerLastName = "CustomerLastName";
                public static string CustomerDateOfBirth = "CustomerDateOfBirth";
                public static string CustomerMail = "CustomerMail";
                public static string PhoneNumber = "PhoneNumber";
                public static string Gender = "Gender";
            }
            public static int AddRow(SqlConnection connection, string customerId, string customerName, string customerLastName, DateTime customerDateOfBirth, string phoneNumber, bool gender, string? customerMail = null)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = "INSERT INTO Customers (CustomerId, CustomerName, CustomerLastName, CustomerDateOfBirth, CustomerMail, PhoneNumber, Gender) " +
                                   "VALUES (@CustomerId, @CustomerName, @CustomerLastName, @CustomerDateOfBirth, @CustomerMail, @PhoneNumber, @Gender)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@CustomerName", customerName);
                        command.Parameters.AddWithValue("@CustomerLastName", customerLastName);
                        command.Parameters.AddWithValue("@CustomerDateOfBirth", customerDateOfBirth);
                        command.Parameters.AddWithValue("@CustomerMail", string.IsNullOrEmpty(customerMail) ? DBNull.Value : customerMail);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@Gender", gender);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
            public static int DeleteRow(SqlConnection connection, string? customerId = null, string customerName = null, string customerLastName = null, DateTime? customerDateOfBirth = null, string customerMail = null, string phoneNumber = null, bool? gender = null)
            {
                try
                {
                    // SQL query oluşturma
                    StringBuilder queryBuilder = new StringBuilder();
                    queryBuilder.Append("DELETE FROM Customers WHERE 1=1");

                    if (!string.IsNullOrEmpty(customerId))
                        queryBuilder.Append(" AND CustomerId = @CustomerId");
                    if (!string.IsNullOrEmpty(customerName))
                        queryBuilder.Append(" AND CustomerName = @CustomerName");
                    if (!string.IsNullOrEmpty(customerLastName))
                        queryBuilder.Append(" AND CustomerLastName = @CustomerLastName");
                    if (customerDateOfBirth.HasValue)
                        queryBuilder.Append(" AND CustomerDateOfBirth = @CustomerDateOfBirth");
                    if (!string.IsNullOrEmpty(customerMail))
                        queryBuilder.Append(" AND CustomerMail = @CustomerMail");
                    if (!string.IsNullOrEmpty(phoneNumber))
                        queryBuilder.Append(" AND PhoneNumber = @PhoneNumber");
                    if (gender.HasValue)
                        queryBuilder.Append(" AND Gender = @Gender");

                    // SQL komutunu çalıştırma
                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        if (!string.IsNullOrEmpty(customerId))
                            command.Parameters.AddWithValue("@CustomerId", customerId);
                        if (!string.IsNullOrEmpty(customerName))
                            command.Parameters.AddWithValue("@CustomerName", customerName);
                        if (!string.IsNullOrEmpty(customerLastName))
                            command.Parameters.AddWithValue("@CustomerLastName", customerLastName);
                        if (customerDateOfBirth.HasValue)
                            command.Parameters.AddWithValue("@CustomerDateOfBirth", customerDateOfBirth.Value);
                        if (!string.IsNullOrEmpty(customerMail))
                            command.Parameters.AddWithValue("@CustomerMail", customerMail);
                        if (!string.IsNullOrEmpty(phoneNumber))
                            command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        if (gender.HasValue)
                            command.Parameters.AddWithValue("@Gender", gender.Value);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
            
            public static class Gender
            {
                public const string Man = "Erkek";
                public const string Woman = "Kadın";
                public static bool? ConvertGenderToBoolean(string gender)
                {
                    if (gender == Man)
                        return true;
                    else if (gender == Woman)
                        return false;
                    return null;
                }
            }
        }

        public static class RoomTable
        {
            public static string TableName = "Rooms";

            public static class Columns
            {
                public static string RoomId = "RoomId";
                public static string RoomType = "RoomType";
                public static string Price = "Price";
                public static string RoomQualityType = "RoomQualityType";
            }
            public static int AddRow(SqlConnection connection, string roomType, decimal price, string roomQualityType)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = "INSERT INTO Rooms (RoomType, Price, RoomQualityType) " +
                                   "VALUES (@RoomType, @Price, @RoomQualityType)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomType", roomType);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@RoomQualityType", roomQualityType);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
            public static int DeleteRow(SqlConnection connection, int? roomId = null, string roomType = null, decimal? price = null, string roomQualityType = null)
            {
                try
                {
                    // SQL query oluşturma
                    StringBuilder queryBuilder = new StringBuilder();
                    queryBuilder.Append("DELETE FROM Rooms WHERE 1=1");

                    if (roomId.HasValue)
                        queryBuilder.Append(" AND RoomId = @RoomId");
                    if (!string.IsNullOrEmpty(roomType))
                        queryBuilder.Append(" AND RoomType = @RoomType");
                    if (price.HasValue)
                        queryBuilder.Append(" AND Price = @Price");
                    if (!string.IsNullOrEmpty(roomQualityType))
                        queryBuilder.Append(" AND RoomQualityType = @RoomQualityType");

                    // SQL komutunu çalıştırma
                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        if (roomId.HasValue)
                            command.Parameters.AddWithValue("@RoomId", roomId.Value);
                        if (!string.IsNullOrEmpty(roomType))
                            command.Parameters.AddWithValue("@RoomType", roomType);
                        if (price.HasValue)
                            command.Parameters.AddWithValue("@Price", price.Value);
                        if (!string.IsNullOrEmpty(roomQualityType))
                            command.Parameters.AddWithValue("@RoomQualityType", roomQualityType);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
        }

        public static class EmployeeTable
        {
            public static string TableName = "Employees";

            public static class Columns
            {
                public static string EmployeeId = "EmployeeId";
                public static string FirstName = "FirstName";
                public static string LastName = "LastName";
                public static string BirthDate = "BirthDate";
                public static string PhoneNumber = "PhoneNumber";
                public static string Gender = "Gender";
                public static string Salary = "Salary";
                public static string Seniority = "Seniority";
                public static string StartDate = "StartDate";
                public static string IsAccessableToSystem = "IsAccessableToSystem";
                public static string SystemPassword = "SystemPassword";
                public static string SystemAccessPermissionsJson = "SystemAccessPermissionsJson";
            }
            public static int AddRow(SqlConnection connection, string firstName, string lastName, DateTime birthDate, string phoneNumber, bool gender, decimal salary, string seniority, DateTime startDate, bool isAccessableToSystem, string? systemPassword = null, string? systemAccessPermissionsJson = null)

            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = "INSERT INTO Employees ( FirstName, LastName, BirthDate, PhoneNumber, Gender, Salary, Seniority, StartDate, IsAccessableToSystem, SystemPassword, SystemAccessPermissionsJson) " +
                                   "VALUES ( @FirstName, @LastName, @BirthDate, @PhoneNumber, @Gender, @Salary, @Seniority, @StartDate, @IsAccessableToSystem, @SystemPassword, @SystemAccessPermissionsJson)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@BirthDate", birthDate);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@Gender", gender);
                        command.Parameters.AddWithValue("@Salary", salary);
                        command.Parameters.AddWithValue("@Seniority", seniority);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@IsAccessableToSystem", isAccessableToSystem);
                        command.Parameters.AddWithValue("@SystemPassword", string.IsNullOrEmpty(systemPassword) ? DBNull.Value : systemPassword);
                        command.Parameters.AddWithValue("@SystemAccessPermissionsJson", string.IsNullOrEmpty(systemAccessPermissionsJson) ? DBNull.Value : systemAccessPermissionsJson);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
            public static int DeleteRow(SqlConnection connection, int? employeeId = null, string firstName = null, string lastName = null, DateTime? birthDate = null, string phoneNumber = null, bool? gender = null, decimal? salary = null, string seniority = null, DateTime? startDate = null, bool? isAccessibleToSystem = null, string systemPassword = null, string systemAccessPermissionsJson = null)
            {
                try
                {
                    StringBuilder queryBuilder = new StringBuilder();
                    queryBuilder.Append("DELETE FROM Employees WHERE 1=1");

                    if (employeeId.HasValue)
                        queryBuilder.Append(" AND EmployeeId = @EmployeeId");
                    if (!string.IsNullOrEmpty(firstName))
                        queryBuilder.Append(" AND FirstName = @FirstName");
                    if (!string.IsNullOrEmpty(lastName))
                        queryBuilder.Append(" AND LastName = @LastName");
                    if (birthDate.HasValue)
                        queryBuilder.Append(" AND BirthDate = @BirthDate");
                    if (!string.IsNullOrEmpty(phoneNumber))
                        queryBuilder.Append(" AND PhoneNumber = @PhoneNumber");
                    if (gender.HasValue)
                        queryBuilder.Append(" AND Gender = @Gender");
                    if (salary.HasValue)
                        queryBuilder.Append(" AND Salary = @Salary");
                    if (!string.IsNullOrEmpty(seniority))
                        queryBuilder.Append(" AND Seniority = @Seniority");
                    if (startDate.HasValue)
                        queryBuilder.Append(" AND StartDate = @StartDate");
                    if (isAccessibleToSystem.HasValue)
                        queryBuilder.Append(" AND IsAccessableToSystem = @IsAccessableToSystem");
                    if (!string.IsNullOrEmpty(systemPassword))
                        queryBuilder.Append(" AND SystemPassword = @SystemPassword");
                    if (!string.IsNullOrEmpty(systemAccessPermissionsJson))
                        queryBuilder.Append(" AND SystemAccessPermissionsJson = @SystemAccessPermissionsJson");

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        if (employeeId.HasValue)
                            command.Parameters.AddWithValue("@EmployeeId", employeeId.Value);
                        if (!string.IsNullOrEmpty(firstName))
                            command.Parameters.AddWithValue("@FirstName", firstName);
                        if (!string.IsNullOrEmpty(lastName))
                            command.Parameters.AddWithValue("@LastName", lastName);
                        if (birthDate.HasValue)
                            command.Parameters.AddWithValue("@BirthDate", birthDate.Value);
                        if (!string.IsNullOrEmpty(phoneNumber))
                            command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        if (gender.HasValue)
                            command.Parameters.AddWithValue("@Gender", gender.Value);
                        if (salary.HasValue)
                            command.Parameters.AddWithValue("@Salary", salary.Value);
                        if (!string.IsNullOrEmpty(seniority))
                            command.Parameters.AddWithValue("@Seniority", seniority);
                        if (startDate.HasValue)
                            command.Parameters.AddWithValue("@StartDate", startDate.Value);
                        if (isAccessibleToSystem.HasValue)
                            command.Parameters.AddWithValue("@IsAccessableToSystem", isAccessibleToSystem.Value);
                        if (!string.IsNullOrEmpty(systemPassword))
                            command.Parameters.AddWithValue("@SystemPassword", systemPassword);
                        if (!string.IsNullOrEmpty(systemAccessPermissionsJson))
                            command.Parameters.AddWithValue("@SystemAccessPermissionsJson", systemAccessPermissionsJson);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
        }

        public static class ProductTable
        {
            public static string TableName = "Products";

            public static class Columns
            {
                public static string ProductId = "ProductId";
                public static string ProductName = "ProductName";
                public static string Price = "Price";
                public static string ProductCategory = "ProductCategory";
            }
            public static int AddRow(SqlConnection connection, string productName, decimal price, string productCategory)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = "INSERT INTO Products (ProductName, Price, PoductCategory) " +
                                   "VALUES (@ProductName, @Price, @ProductCategory)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@ProductCategory", productCategory);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
            public static int DeleteRow(SqlConnection connection, int? productId = null, string productName = null, decimal? price = null, string productCategory = null)
            {
                try
                {
                    StringBuilder queryBuilder = new StringBuilder();
                    queryBuilder.Append("DELETE FROM Products WHERE 1=1");

                    if (productId.HasValue)
                        queryBuilder.Append(" AND ProductId = @ProductId");
                    if (!string.IsNullOrEmpty(productName))
                        queryBuilder.Append(" AND ProductName = @ProductName");
                    if (price.HasValue)
                        queryBuilder.Append(" AND Price = @Price");
                    if (!string.IsNullOrEmpty(productCategory))
                        queryBuilder.Append(" AND PoductCategory = @PoductCategory");

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        if (productId.HasValue)
                            command.Parameters.AddWithValue("@ProductId", productId.Value);
                        if (!string.IsNullOrEmpty(productName))
                            command.Parameters.AddWithValue("@ProductName", productName);
                        if (price.HasValue)
                            command.Parameters.AddWithValue("@Price", price.Value);
                        if (!string.IsNullOrEmpty(productCategory))
                            command.Parameters.AddWithValue("@PoductCategory", productCategory);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
        }

        public static class OrderTable
        {
            public static string TableName = "Orders";

            public static class Columns
            {
                public static string OrderId = "OrderId";
                public static string ReservationId = "ReservationId";
                public static string ProductId = "ProductId";
                public static string EmployeeId = "EmployeeId";
                public static string Quantity = "Quantity";
                public static string OrderDateTime = "OrderDateTime";
            }
            public static int AddRow(SqlConnection connection, int reservationId, int productId, int employeeId, int quantity, DateTime orderDateTime)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = "INSERT INTO Orders (ReservationId, ProductId, EmployeeId, Quantity, OrderDateTime) " +
                                   "VALUES (@ReservationId, @ProductId, @EmployeeId, @Quantity, @OrderDateTime)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationId", reservationId);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@EmployeeId", employeeId);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@OrderDateTime", orderDateTime);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
            public static int DeleteRow(SqlConnection connection, int? orderId = null, int? reservationId = null, int? productId = null, int? employeeId = null, int? quantity = null, DateTime? orderDateTime = null)
            {
                try
                {
                    StringBuilder queryBuilder = new StringBuilder();
                    queryBuilder.Append("DELETE FROM Orders WHERE 1=1");

                    if (orderId.HasValue)
                        queryBuilder.Append(" AND OrderId = @OrderId");
                    if (reservationId.HasValue)
                        queryBuilder.Append(" AND ReservationId = @ReservationId");
                    if (productId.HasValue)
                        queryBuilder.Append(" AND ProductId = @ProductId");
                    if (employeeId.HasValue)
                        queryBuilder.Append(" AND EmployeeId = @EmployeeId");
                    if (quantity.HasValue)
                        queryBuilder.Append(" AND Quantity = @Quantity");
                    if (orderDateTime.HasValue)
                        queryBuilder.Append(" AND OrderDateTime = @OrderDateTime");

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        if (orderId.HasValue)
                            command.Parameters.AddWithValue("@OrderId", orderId.Value);
                        if (reservationId.HasValue)
                            command.Parameters.AddWithValue("@ReservationId", reservationId.Value);
                        if (productId.HasValue)
                            command.Parameters.AddWithValue("@ProductId", productId.Value);
                        if (employeeId.HasValue)
                            command.Parameters.AddWithValue("@EmployeeId", employeeId.Value);
                        if (quantity.HasValue)
                            command.Parameters.AddWithValue("@Quantity", quantity.Value);
                        if (orderDateTime.HasValue)
                            command.Parameters.AddWithValue("@OrderDateTime", orderDateTime.Value);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
        }

        public static class ReservationTable
        {
            public static string TableName = "Reservations";

            public static class Columns
            {
                public static string ReservationId = "ReservationId";
                public static string RoomId = "RoomId";
                public static string EmployeeId = "EmployeeId";
                public static string CheckInDate = "CheckInDate";
                public static string CheckOutDate = "CheckOutDate";
                public static string TotalPrice = "TotalPrice";
                public static string PaymentMethod = "PaymentMethod";
            }
            public static int AddRow(SqlConnection connection, int roomId, int employeeId, DateTime checkInDate, DateTime checkOutDate, int totalPrice, string paymentMethod)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = "INSERT INTO Reservations ( RoomId, EmployeeId, CheckInDate, CheckOutDate, TotalPrice, PaymentMethod) " +
                                   "VALUES ( @RoomId, @EmployeeId, @CheckInDate, @CheckOutDate, @TotalPrice, @PaymentMethod)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomId", roomId);
                        command.Parameters.AddWithValue("@EmployeeId", employeeId);
                        command.Parameters.AddWithValue("@CheckInDate", checkInDate);
                        command.Parameters.AddWithValue("@CheckOutDate", checkOutDate);
                        command.Parameters.AddWithValue("@TotalPrice", totalPrice);
                        command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
            public static int DeleteRow(SqlConnection connection, int? reservationId = null, int? roomId = null, int? employeeId = null, DateTime? checkInDate = null, DateTime? checkOutDate = null, decimal? totalPrice = null, string paymentMethod = null)
            {
                try
                {
                    StringBuilder queryBuilder = new StringBuilder();
                    queryBuilder.Append("DELETE FROM Reservations WHERE 1=1");

                    if (reservationId.HasValue)
                        queryBuilder.Append(" AND ReservationId = @ReservationId");
                    if (roomId.HasValue)
                        queryBuilder.Append(" AND RoomId = @RoomId");
                    if (employeeId.HasValue)
                        queryBuilder.Append(" AND EmployeeId = @EmployeeId");
                    if (checkInDate.HasValue)
                        queryBuilder.Append(" AND CheckInDate = @CheckInDate");
                    if (checkOutDate.HasValue)
                        queryBuilder.Append(" AND CheckOutDate = @CheckOutDate");
                    if (totalPrice.HasValue)
                        queryBuilder.Append(" AND TotalPrice = @TotalPrice");
                    if (!string.IsNullOrEmpty(paymentMethod))
                        queryBuilder.Append(" AND PaymentMethod = @PaymentMethod");

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        if (reservationId.HasValue)
                            command.Parameters.AddWithValue("@ReservationId", reservationId.Value);
                        if (roomId.HasValue)
                            command.Parameters.AddWithValue("@RoomId", roomId.Value);
                        if (employeeId.HasValue)
                            command.Parameters.AddWithValue("@EmployeeId", employeeId.Value);
                        if (checkInDate.HasValue)
                            command.Parameters.AddWithValue("@CheckInDate", checkInDate.Value);
                        if (checkOutDate.HasValue)
                            command.Parameters.AddWithValue("@CheckOutDate", checkOutDate.Value);
                        if (totalPrice.HasValue)
                            command.Parameters.AddWithValue("@TotalPrice", totalPrice.Value);
                        if (!string.IsNullOrEmpty(paymentMethod))
                            command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
            
            public static class PaymentMethods
            {
                public const string CreditCard = "Kredi Kartı";
                public const string Cash = "Nakit";
                public const string Transfer = "Havale";
            }
        }

        public static class ReservationCustomersTable
        {
            public static string TableName = "ReservationCustomers";

            public static class Columns
            {
                public static string ReservationCustomerId = "ReservationCustomerId";
                public static string ReservationId = "ReservationId";
                public static string CustomerId = "CustomerId";
            }
            public static int AddRow(SqlConnection connection, int reservationId, int customerId)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = "INSERT INTO ReservationCustomers (ReservationId, CustomerId) " +
                                   "VALUES (@ReservationId, @CustomerId)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationId", reservationId);
                        command.Parameters.AddWithValue("@CustomerId", customerId);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
            public static int DeleteRow(SqlConnection connection, int? reservationCustomerId = null, int? reservationId = null, int? customerId = null)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // SQL query oluşturma
                    StringBuilder queryBuilder = new StringBuilder();
                    queryBuilder.Append("DELETE FROM ReservationCustomers WHERE 1=1");

                    if (reservationCustomerId.HasValue)
                        queryBuilder.Append(" AND ReservationCustomerId = @ReservationCustomerId");
                    if (reservationId.HasValue)
                        queryBuilder.Append(" AND ReservationId = @ReservationId");
                    if (customerId.HasValue)
                        queryBuilder.Append(" AND CustomerId = @CustomerId");

                    // SQL komutunu çalıştırma
                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        if (reservationCustomerId.HasValue)
                            command.Parameters.AddWithValue("@ReservationCustomerId", reservationCustomerId.Value);
                        if (reservationId.HasValue)
                            command.Parameters.AddWithValue("@ReservationId", reservationId.Value);
                        if (customerId.HasValue)
                            command.Parameters.AddWithValue("@CustomerId", customerId.Value);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    DebugWindow.Print(ex.Message);
                    return -2;
                }
            }
        }

        public static void FillDatabaseWithSampleData()
        {
            if (!FillDBWithSampleData)
                return;
            using(var connection = ConnectToDefaultServer())
            {

                // Otel odaları ekleniyor
                SQLHelper.RoomTable.AddRow(connection, "RoomType1", 150, "RoomQualityType1");
                SQLHelper.RoomTable.AddRow(connection, "RoomType2", 120, "RoomQualityType2");
                SQLHelper.RoomTable.AddRow(connection, "RoomType3", 180, "RoomQualityType1");
                // ... diğer odaların ekleme işlemleri buraya eklenebilir

                // Ürünler ekleniyor
                SQLHelper.ProductTable.AddRow(connection, "Product1", 10, "Category1");
                SQLHelper.ProductTable.AddRow(connection, "Product2", 15, "Category2");
                SQLHelper.ProductTable.AddRow(connection, "Product3", 8, "Category1");
                // ... diğer ürünlerin ekleme işlemleri buraya eklenebilir

                // Çalışanlar ekleniyor
                SQLHelper.EmployeeTable.AddRow(connection, "EmployeeFirstName1", "EmployeeLastName1", new DateTime(1990, 1, 1), "111", true, 3000, "Seniority1", new DateTime(2018, 1, 1), true, "Password1");
                SQLHelper.EmployeeTable.AddRow(connection, "EmployeeFirstName2", "EmployeeLastName2", new DateTime(1991, 2, 2), "222", false, 2500, "Seniority2", new DateTime(2019, 2, 2), true, "Password2");
                SQLHelper.EmployeeTable.AddRow(connection, "EmployeeFirstName3", "EmployeeLastName3", new DateTime(1992, 3, 3), "333", true, 3500, "Seniority3", new DateTime(2020, 3, 3), false, "Password3");
                // ... diğer çalışanların ekleme işlemleri buraya eklenebilir

                // Müşteriler ekleniyor
                SQLHelper.CustomerTable.AddRow(connection, "55","CustomerFirstName1", "CustomerLastName1", new DateTime(1980, 1, 1), "111", true, "customer1@example.com");
                SQLHelper.CustomerTable.AddRow(connection, "66", "CustomerFirstName2", "CustomerLastName2", new DateTime(1981, 2, 2), "222", false, "customer2@example.com");
                SQLHelper.CustomerTable.AddRow(connection, "77", "CustomerFirstName3", "CustomerLastName3", new DateTime(1982, 3, 3), "333", true, "customer3@example.com");
                // ... diğer müşterilerin ekleme işlemleri buraya eklenebilir

                // Rezervasyonlar ekleniyor
                SQLHelper.ReservationTable.AddRow(connection, 1, 1, DateTime.Now.AddDays(new Random().Next(-10,-1)), DateTime.Now.AddDays(new Random().Next(10)), 800, "CreditCard");
                SQLHelper.ReservationTable.AddRow(connection, 1, 1, DateTime.Now.AddDays(new Random().Next(-10, -1)), DateTime.Now.AddDays(new Random().Next(10)), 1200, "Cash");
                SQLHelper.ReservationTable.AddRow(connection, 1, 1, DateTime.Now.AddDays(new Random().Next(-10, -1)), DateTime.Now.AddDays(new Random().Next(10)), 1000, "CreditCard");
                // ... diğer rezervasyonların ekleme işlemleri buraya eklenebilir

                // Rezervasyon-müşteri ilişkisi oluşturuluyor
                SQLHelper.ReservationCustomersTable.AddRow(connection, 1, 1);
                SQLHelper.ReservationCustomersTable.AddRow(connection, 1, 1);
                SQLHelper.ReservationCustomersTable.AddRow(connection, 1, 1);
                SQLHelper.ReservationCustomersTable.AddRow(connection, 1, 1);
                SQLHelper.ReservationCustomersTable.AddRow(connection, 1, 1);
                // ... diğer rezervasyon-müşteri ilişkilerinin ekleme işlemleri buraya eklenebilir

                // Siparişler ekleniyor
                SQLHelper.OrderTable.AddRow(connection, 1, 1, 1, 5, new DateTime(2023, 6, 1));
                SQLHelper.OrderTable.AddRow(connection, 2, 2, 2, 2, new DateTime(2023, 6, 2));
                SQLHelper.OrderTable.AddRow(connection, 1, 1, 1, 1, new DateTime(2023, 6, 3));
                // ... diğer siparişlerin ekleme işlemleri buraya eklenebilir
            }
        }
        public static string DataTableToString(DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    sb.Append(row[column].ToString());
                    sb.Append(" | ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
        public static string GetAllTablesAsString()
        {
            string returnValue = "";
            using(var connection = ConnectToDefaultServer())
            {
                returnValue += "\n------------------------------------------------";
                returnValue += "\n" + "Rooms:" + "\n" + DataTableToString(GetAllTable(connection, RoomTable.TableName));
                returnValue += "\n------------------------------------------------";
                returnValue += "\n" + "Customers:" + "\n" + DataTableToString(GetAllTable(connection, CustomerTable.TableName));
                returnValue += "\n------------------------------------------------";
                returnValue += "\n" + "Employees:" + "\n" + DataTableToString(GetAllTable(connection, EmployeeTable.TableName));
                returnValue += "\n------------------------------------------------";
                returnValue += "\n" + "Products:" + "\n" + DataTableToString(GetAllTable(connection, ProductTable.TableName));
                returnValue += "\n------------------------------------------------";
                returnValue += "\n" + "Orders:" + "\n" + DataTableToString(GetAllTable(connection, OrderTable.TableName));
                returnValue += "\n------------------------------------------------";
                returnValue += "\n" + "Reservations:" + "\n" + DataTableToString(GetAllTable(connection, ReservationTable.TableName));
                returnValue += "\n------------------------------------------------";
                returnValue += "\n" + "ReservationCustomers:" + "\n" + DataTableToString(GetAllTable(connection, ReservationCustomersTable.TableName));
                returnValue += "\n------------------------------------------------";
            }
            return returnValue;
        }
        public static string PrintAllData()
        {
            string data = GetAllTablesAsString();
            DebugWindow.Print("All data print:\n\n" + data);
            return data;
        }
    }
}
