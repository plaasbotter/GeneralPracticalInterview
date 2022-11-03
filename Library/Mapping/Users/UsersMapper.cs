using Library.Data.Users;
using Library.Models.Users;
using Library.Utils;

namespace Library.Mapping.Users
{
    public class UsersMapper : IMapper<UsersDto, UsersModel>
    {
        public UsersDto MapForward(UsersModel input)
        {
            return new UsersDto
            {
                Name = input.Name,
                Surname = input.Surname,
                Email = input.Email,
                Department = input.Department,
                CreatedOn = input.CreatedOn,
                SockColour = input.SockColour,
                Id = 0
            };
        }

        [TODO]
        public UsersModel MapReverse(UsersDto input)
        {
            throw new NotImplementedException();
        }
    }
}
