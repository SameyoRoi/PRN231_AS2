using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PE_SE173338_PE.DTO;

namespace PE_SE173338_PE.Pages.JeweryPage
{
    public class IndexModel : PageModel
    {
        public IList<SilverJewelryDTO> SilverJewelry { get; set; } = new List<SilverJewelryDTO>();
        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; } = default!;

        public string Message { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (TempData["Message"] != null)
                {
                    Message = TempData["Message"].ToString();
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

                    var query = new List<string> { "$expand=Category", "$count=true" };

                    if (!string.IsNullOrEmpty(SearchName))
                    {
                        query.Add($"$filter=contains(SilverJewelryName,'{SearchName}') or contains(SilverJewelryDescription,'{SearchName}')");
                    }

                    var queryString = string.Join("&", query);
                    var url = $"https://localhost:7113/odata/SilverJewelry?{queryString}"; // Use HTTPS

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        // Deserialize the response content into the model
                        var result = JsonConvert.DeserializeObject<OdataResponse<SilverJewelryDTO>>(content);
                        SilverJewelry = result.Value;
                    }
                    else
                    {
                        // Handle errors
                        Message = "Error fetching data";
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "An error occurred: " + ex.Message;
            }

            return Page();
        }

    }
}
