using Library.Services.Database;
using Library.Utils;

namespace Library.Data.Users
{
    public class UsersDataRepository : IDataRepository<UsersDto>
    {
        private readonly IDatabase _database;
        private const string TABLENAME = "Users";

        public UsersDataRepository(IDatabase database)
        {
            _database = database;
        }

        public Task DeleteAsync(UsersDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UsersDto>> GetAllAsync()
        {
            List<UsersDto> returnValue = new List<UsersDto>();
            List<object[]>? datarows = await _database.GetAllObjects(TABLENAME);
            if (datarows != null)
            {
                while (datarows.Count > 0)
                {
                    returnValue.Add(new UsersDto
                    {
                        Id = (int)datarows[0][0],
                        Name = (string)datarows[0][1],
                        Surname = (string)datarows[0][2],
                        Email = (string)datarows[0][3],
                        Department = (string)datarows[0][4],
                        CreatedOn = (DateTime)datarows[0][5],
                        SockColour = (string)datarows[0][6],
                    });
                    datarows.RemoveAt(0);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// It is definitely prefered to look for the id in the database rather than return all and filter
        /// The reason for keeping it as filtered, is the size of the database
        /// Could easily be adapted later
        /// </summary>
        [TODO]
        public Task<UsersDto> GetById(object id)
        {
            throw new NotImplementedException();
        }

        [TODO]
        public async Task InsertAsync(UsersDto entity)
        {
            await _database.InsertAsync(TABLENAME, entity);
        }

        [TODO]
        public Task UpdateAsync(UsersDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
