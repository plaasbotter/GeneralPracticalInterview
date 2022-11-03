namespace Library.Test.Users
{
    public class UsersMockDataRepository : IDataRepository<UsersDto>
    {
        private List<UsersDto> _users = new List<UsersDto>();
        public UsersMockDataRepository()
        {
            _users.Add(new UsersDto
            {
                Name = "test1",
                Surname = "test1",
                Email = "test1@test1.test1",
                Department = "test1",
                Id = 1,
                CreatedOn = DateTime.Now,
                SockColour = "purple"
            });
            _users.Add(new UsersDto
            {
                Name = "test2",
                Surname = "test2",
                Email = "test2@test2.test2",
                Department = "test2",
                Id = 2,
                CreatedOn = DateTime.Now,
                SockColour = "black"
            });
        }

        public Task DeleteAsync(UsersDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UsersDto>> GetAllAsync()
        {
            List<UsersDto> returnValue = new List<UsersDto>();
            foreach (var user in _users)
            {
                returnValue.Add(user);
            }
            return returnValue;
        }

        public Task<UsersDto> GetById(object id)
        {
            UsersDto returnValue = _users.Where(model => model.Id == (int)id).First();
            return Task.FromResult(returnValue);
        }

        public Task InsertAsync(UsersDto input)
        {
            _users.Add(input);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(UsersDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
