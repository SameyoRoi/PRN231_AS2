using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BO;
using Newtonsoft.Json;
using PE_SE173338_PE.DTO;

namespace PE_SE173338_PE.Pages.JeweryPage
{
    public class DetailsModel : PageModel
    {
        public List<CategoryDTO> Category { get; set; } = new List<CategoryDTO>();

        [BindProperty]
        public SilverJewelryDTO SilverJewelry { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
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

                var response = await httpClient.GetAsync($"https://localhost:7113/api/Category");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Category = JsonConvert.DeserializeObject<List<CategoryDTO>>(content);
                }

                var currentResponse = await httpClient.GetAsync($"https://localhost:7113/api/Sliver/{id}");

                if (currentResponse.IsSuccessStatusCode)
                {
                    var content = await currentResponse.Content.ReadAsStringAsync();
                    SilverJewelry = JsonConvert.DeserializeObject<SilverJewelryDTO>(content);
                    return Page();
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
