using BO;

namespace PE_SE173338
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string AccountId { get; set; }

        public BranchAccount? User { get; set; }
    }
}
