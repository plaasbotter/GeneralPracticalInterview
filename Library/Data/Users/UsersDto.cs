using Library.Utils;

namespace Library.Data.Users
{
    /// <summary>
    /// Users object that moves into database
    /// </summary>
    public class UsersDto : BaseEntity
    {
        [PrimaryKeyAutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? SockColour { get; set; }
    }
}
