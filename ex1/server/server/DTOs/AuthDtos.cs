namespace server.DTOs
{
    public class LogintRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
   public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; }
        public UserResponseDto User { get; set; } = null!;

    }
}
