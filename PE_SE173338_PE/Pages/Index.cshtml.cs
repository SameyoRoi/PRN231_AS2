using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PE_SE173338_PE.DTO;

namespace PE_SE173338_PE.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public LoginRequestDTO LoginRequestDTO { get; set; }

        public string Message { get; set; } = default!;

        public IndexModel()
        {
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://localhost:7113/api/Login/Login", LoginRequestDTO);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                        // Assuming result.Role is a string and we need to convert it to int
                        if (int.TryParse(result.Role, out int roleId))
                        {
                            if (roleId == 3 || roleId == 4)
                            {
                                Message = "Unauthorized access."; // Set unauthorized message
                                return Page(); // Return to the same page with the unauthorized message
                            }
                        }
                        else
                        {
                            // Handle the case where the Role cannot be parsed
                            Message = "Invalid role format.";
                            return Page();
                        }

                        // Set session variables for authorized users
                        HttpContext.Session.SetString("Token", result.Token);
                        HttpContext.Session.SetString("Role", result.Role); // Store Role as string
                        HttpContext.Session.SetString("AccountId", result.AccountId);

                        return RedirectToPage("/JeweryPage/Index");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        if (errorContent.Length != 0)
                        {
                            throw new Exception(errorContent);
                        }
                        else
                        {
                            throw new Exception("Field is not null!");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Page();
            }
        }
    }
}
