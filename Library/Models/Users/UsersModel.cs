using System.ComponentModel.DataAnnotations;

namespace Library.Models.Users
{
    /// <summary>
    /// Users model with the required front-end validation flags
    /// </summary>
    public class UsersModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        [StringLength(100)]
        public string? Surname { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [StringLength(100)]
        public string? Department { get; set; }
        public DateTime CreatedOn { get; set; }
        [Required]
        [StringLength(100)]
        public string? SockColour { get; set; }
    }
}
