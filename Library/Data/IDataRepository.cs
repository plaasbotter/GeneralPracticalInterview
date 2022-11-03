namespace Library.Data
{
    /// <summary>
    /// Generic data respository for Base Entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Returns entity by id
        /// </summary>
        /// <param name="id">id of model</param>
        /// <returns>The base entity</returns>
        public Task<T> GetById(object id);
        /// <summary>
        /// Inserts entity into whatever storage
        /// </summary>
        /// <param name="entity">input entity</param>
        public Task InsertAsync(T entity);
        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="entity">input entity that should contain id</param>
        public Task UpdateAsync(T entity);
        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="entity">input entity that should contain id</param>
        public Task DeleteAsync(T entity);
        /// <summary>
        /// Gets all entitys of type T as List
        /// </summary>
        /// <returns>List of entities</returns>
        public Task<List<T>> GetAllAsync();
    }
}
