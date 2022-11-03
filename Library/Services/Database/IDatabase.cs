namespace Library.Services.Database
{
    public interface IDatabase
    {
        /// <summary>
        /// Checks if database is still connected
        /// </summary>
        /// <returns>Is connected</returns>
        bool IsConnected();
        /// <summary>
        /// Checks if the table exists
        /// If the table does not exist, creates the tables
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="tableObject">Object that is parsed into columns</param>
        /// <param name="forceDrop">Ignore the checks and deletes the table anyway</param>
        void CheckAndCreateTables(string tableName, object tableObject, bool forceDrop);
        /// <summary>
        /// Function to create a table by parsing the input object
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="input">Object that is parsed into columns</param>
        void CreateTableWithObject(string tableName, object input);
        /// <summary>
        /// Generic function to return all data rows from a table
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <returns>List of rows as object arrays</returns>
        Task<List<object[]>> GetAllObjects(string tableName);
        /// <summary>
        /// Generic function to insert row into table
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="row">The object that is parsed and inserted</param>
        Task InsertAsync(string tableName, object row);
    }
}
