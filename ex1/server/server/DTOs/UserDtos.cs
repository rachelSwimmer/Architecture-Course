using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class UserCreateDto
    {
        [Required]
        public string Id { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; }
        [Required, MinLength(5)]
        public string UserName { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

    }
    public class UserUpdateDto
    {
        [MaxLength(100)]
        public string FullName { get; set; }
        [ MinLength(5)]
        public string UserName { get; set; }
   
        [ EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }

    public class UserResponseDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
