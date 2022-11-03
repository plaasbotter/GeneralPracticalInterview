using Library.Data;
using Library.Models;

namespace Library.Mapping
{
    /// <summary>
    /// Generic mapper interface
    /// </summary>
    /// <typeparam name="T">Dto object as BaseEntity</typeparam>
    /// <typeparam name="U">Domain object as BaseModel</typeparam>
    public interface IMapper<T, U>
        where T : BaseEntity
        where U : BaseModel
    {
        /// <summary>
        /// Converts the BaseModel to BaseEntity
        /// </summary>
        /// <param name="entity">BaseModel as domain object to be converted</param>
        /// <returns>BaseEntity as dto object</returns>
        T MapForward(U entity);

        /// <summary>
        /// Converts the BaseEntity to BaseModel
        /// </summary>
        /// <param name="entity">BaseEntity as dto object to be converted</param>
        /// <returns>BaseModel as domain object</returns>
        U MapReverse(T entity);
    }
}
