using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BO;
using Newtonsoft.Json;
using System.Text;
using PE_SE173338_PE.DTO;

namespace PE_SE173338_PE.Pages.JeweryPage
{
    public class CreateModel : PageModel
    {
        public List<CategoryDTO> Categoty { get; set; } = new List<CategoryDTO>();

        public async Task<IActionResult> OnGet()
        {
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
                    Categoty = JsonConvert.DeserializeObject<List<CategoryDTO>>(content);
                }

                return Page();
            }
        }

        [BindProperty]
        public SilverJewelryDTO SilverJewelry { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Log or debug to see validation issues
                }
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

                var response = await httpClient.PostAsync("https://localhost:7113/api/Silver", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createdItem = JsonConvert.DeserializeObject<SilverJewelryDTO>(responseContent);
                    TempData["Message"] = $"Create successfully! ID: {createdItem.SilverJewelryId}";
                    return RedirectToPage("/JeweryPage/Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["Message"] = $"Create failed! Error: {errorContent}";
                    return await OnGet();
                }


            }
        }
    }
}
