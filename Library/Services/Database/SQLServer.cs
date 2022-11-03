using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Reflection;

namespace Library.Services.Database
{
    public class SQLServer : IDatabase
    {
        private readonly object _mutex;
        private readonly SqlConnection _con;
        private readonly ILogger _logger;

        public SQLServer(string connectionString, ILogger logger)
        {
            _mutex = new object();
            _con = new SqlConnection(connectionString);
            _con.OpenAsync().Wait();
            if (_con.State != System.Data.ConnectionState.Open)
            {
                throw new Exception("Could not connect to database");
            }
            _logger = logger;
        }


        #region Startup
        public void CheckAndCreateTables(string tableName, object tableObject, bool forceDrop)
        {
            lock (_mutex)
            {
                while (IsConnected() == false)
                {
                    _logger.LogWarning("[{0}] [{1}]", "SQLServer.CheckAndCreateTables", "Reconnecting...");
                }
                if (forceDrop)
                {
                    string deleteQuery = $"DROP TABLE \"{tableName}\"";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, _con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                bool exists = false;
                //string query = $"SELECT EXISTS(SELECT FROM information_schema.tables WHERE table_schema = 'public' AND table_name = '{tableName}')";
                string query = $"SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_NAME] = \'{tableName}\'";
                using (SqlCommand cmd = new SqlCommand(query, _con))
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            exists = rdr.GetInt32(0) == 1;
                        }
                    }
                }
                if (exists == false)
                {
                    CreateTableWithObject(tableName, tableObject);
                }
                else
                {
                    _logger.LogInformation($"Table {tableName} exists", "DatabaseContext.CheckAndCreateTables");
                }
            }
        }

        public void CreateTableWithObject(string tableName, object input)
        {
            while (IsConnected() == false)
            {
                _logger.LogWarning("[{0}] [{1}]", "SQLServer.CreateTableWithObject", "Reconnecting...");
            }
            List<string> primaryKeys = new List<string>();
            bool isIdentity = false;
            string query = $"CREATE TABLE [dbo].[{tableName}] (";
            foreach (PropertyInfo prop in input.GetType().GetProperties())
            {
                if (prop.CustomAttributes.Count() > 0)
                {
                    List<CustomAttributeData>? custumAttributes = prop.CustomAttributes.ToList();
                    foreach (CustomAttributeData custumAttribute in custumAttributes)
                    {
                        switch (custumAttribute.AttributeType.Name)
                        {
                            case "PrimaryKeyAttribute":
                                primaryKeys.Add(prop.Name);
                                break;
                            case "PrimaryKeyAutoIncrementAttribute":
                                primaryKeys.Add(prop.Name);
                                isIdentity = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (type != null)
                {
                    query += $"[{prop.Name}]";
                    if (type == typeof(string))
                    {
                        query += $" nvarchar(128) ";
                    }
                    if (type == typeof(int))
                    {
                        query += $" int ";
                    }
                    if (type == typeof(DateTime))
                    {
                        query += $" datetime2 ";
                    }
                    if (isIdentity == true)
                    {
                        query += " IDENTITY (1, 1) NOT ";
                        isIdentity = false;
                    }
                    query += "NULL, ";
                }
            }
            if (primaryKeys.Count > 0)
            {
                query += $" PRIMARY KEY ([{string.Join("],[", primaryKeys)}])";
            }
            query += ");";
            using (SqlCommand cmd = new SqlCommand(query, _con))
            {
                cmd.ExecuteNonQuery();
            }
            _logger.LogInformation($"Created Table {tableName}", "SQLServer.CreateTableWithObject");
        }

        public bool IsConnected()
        {
            if (_con.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    _con.OpenAsync().Wait();
                }
                catch (Exception err)
                {
                    _logger.LogError(err, "[{0}]", "SQLServer.TestConnection");
                    Thread.Sleep(112358);
                    return false;
                }
            }
            return true;
        }
        #endregion

        public async Task<List<object[]>> GetAllObjects(string tableName)
        {
            List<object[]> returnValue = new List<object[]>();
            lock (_mutex)
            {
                while (IsConnected() == false)
                {
                    _logger.LogWarning("[{0}] [{1}]", "SQLServer.CreateTableWithObject", "Reconnecting...");
                }
            }
            string query = $"SELECT * FROM [dbo].[{tableName}]";
            using (SqlCommand cmd = new SqlCommand(query, _con))
            {
                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        object[]? tempObject = null;
                        if (tempObject == null)
                        {
                            tempObject = new object[rdr.FieldCount];
                        }
                        rdr.GetValues(tempObject);
                        returnValue.Add(tempObject);
                    }
                }
            }
            return returnValue;
        }

        public async Task InsertAsync(string tableName, object row)
        {
            lock (_mutex)
            {
                while (IsConnected() == false)
                {
                    _logger.LogWarning("[{0}] [{1}]", "SQLServer.CreateTableWithObject", "Reconnecting...");
                }
            }
            using (SqlCommand cmd = new SqlCommand(string.Empty, _con))
            {
                string query = $"INSERT INTO [dbo].[{tableName}]";
                List<string> columns = new List<string>();
                List<string> values = new List<string>();
                int counter = 0;
                foreach (PropertyInfo prop in row.GetType().GetProperties())
                {
                    //prop.Name
                    if (prop.CustomAttributes.Count() == 0)
                    {
                        columns.Add("[" + prop.Name + "]");
                        values.Add($"@par_{counter}");
                        cmd.Parameters.AddWithValue($"@par_{counter}", prop.GetValue(row, null));
                        counter++;
                    }
                }
                cmd.CommandText = query + " (" + string.Join(",", columns) + ") VALUES (" + string.Join(",", values) + ");";
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected < 0 || rowsAffected > 1)
                {
                    _logger.LogError("[{0}] [{1}]", "SQLServer.InsertAsync", $"Erronous insert");
                }
                _logger.LogInformation("[{0}] [{1}]", "SQLServer.InsertAsync", $"Inserted {rowsAffected} rows");
            }
        }
    }
}