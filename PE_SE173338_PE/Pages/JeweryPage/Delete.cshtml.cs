using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BO;

namespace PE_SE173338_PE.Pages.JeweryPage
{
    public class DeleteModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Index");
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.DeleteAsync($"https://localhost:7113/api/Silver/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Delete successfully!";
                    return RedirectToPage("/JeweryPage/Index");
                }
                else
                {
                    return Page();
                }

            }
        }
    }
}
