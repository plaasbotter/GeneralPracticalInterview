using Library.Services.Database;

namespace Library.Utils
{
    /// <summary>
    /// Utility to create the database tables should they not exist
    /// </summary>
    public class DatabaseGenerator : IDisposable
    {
        private readonly IDatabase _database;
        public DatabaseGenerator(IDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Calls the Check and Create tables from the database interface
        /// </summary>
        /// <param name="tableObjects">List of tables that need constructing</param>
        public void GenerateTables(List<TableConstructObject> tableObjects)
        {
            foreach (var tableObject in tableObjects)
            {
                _database.CheckAndCreateTables(tableObject.TableName, tableObject.DataObject, tableObject.ForceDrop);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Class containing information about how the table needs to be constructed
    /// </summary>
    public class TableConstructObject
    {
        public TableConstructObject(object dataObject, string tableName, bool forceDrop = false)
        {
            DataObject = dataObject;
            TableName = tableName;
            ForceDrop = forceDrop;
        }
        public string TableName { get; set; }
        public object DataObject { get; set; }
        public bool ForceDrop { get; set; }
    }
}
