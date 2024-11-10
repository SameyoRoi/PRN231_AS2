using System.ComponentModel.DataAnnotations;

namespace PE_SE173338_PE.DTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email Is not empty")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is not empty")]
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string AccountId { get; set; }
    }
}
