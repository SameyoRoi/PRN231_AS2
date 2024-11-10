using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BO;
using Newtonsoft.Json;
using System.Text;
using PE_SE173338_PE.DTO;

namespace PE_SE173338_PE.Pages.JeweryPage
{
    public class EditModel : PageModel
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
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

                var json = JsonConvert.SerializeObject(SilverJewelry);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"https://localhost:7113/api/Silver/{SilverJewelry.SilverJewelryId}", content);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    TempData["Message"] = "Update successfully!";
                    return RedirectToPage("/JeweryPage/Index");
                }
                else
                {
                    TempData["Message"] = "Update failed!";
                    return Page();
                }
            }


        }
    }
}
