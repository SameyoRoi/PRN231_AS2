using BO;
using DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repo;

namespace PE_SE173338.Controllers
{
    [EnableCors("AllowAll")]
    public class SilverJewelryController : ODataController
    {
        private readonly ISilverJewelryRepo _repo;

        public SilverJewelryController(ISilverJewelryRepo repo)
        {
            _repo = repo;
        }

        [Authorize(Policy = "AdminOrStaff")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<SilverJewelry>>> GetSilverJewelry()
        {
            try
            {
                // Check if there is an $expand parameter in the query string
                var expandQuery = HttpContext.Request.Query.ContainsKey("$expand");

                // If not, add $expand=Category with the necessary $select fields
                if (!expandQuery)
                {
                    // Construct a new URL with $expand and $select clauses
                    var newUrl = $"{Request.Path}?$expand=Category($select=CategoryName)";
                    return Redirect(newUrl);
                }

                // Fetch the data from the repository
                var silvers = await _repo.GetAllS();

                // Return the data with Ok result
                return Ok(silvers);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }



        [HttpGet("/api/SilverDTOs")]
        [Authorize(Policy = "AdminOrStaff")]
        public async Task<ActionResult<List<SilverDTO>>> GetAllSilverDTOs()
        {
            try
            {
                var silverDTOs = await _repo.GetAllSDTO();
                return Ok(silverDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }


        [HttpGet("/api/Category")]
        [EnableQuery]
        [Authorize(Policy = "AdminOrStaff")]

        public async Task<ActionResult<List<Category>>> GetCategory()
        {
            try
            {
                var cate = await _repo.GetCategory();
                return Ok(cate);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpGet("/api/Sliver/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<SilverJewelry>> GetS(string id)
        {
            try
            {
                var silver = await _repo.GetSById(id);
                return Ok(silver);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpPost("/api/Silver")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<SilverJewelry>> AddS([FromBody] SilverJewelry silver)
        {
            try
            {
                var newSilver = await _repo.AddS(silver);
                return Ok(newSilver);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpPut("/api/Silver/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<SilverJewelry>> UpdateS(string id, [FromBody] SilverJewelry silver)
        {
            try
            {
                var updateS = await _repo.UpdateS(silver);
                return Ok(updateS);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpDelete("/api/Silver/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<SilverJewelry>> DeleteS(string id)
        {
            try
            {
                var deleteS = await _repo.DeleteS(id);
                return Ok(deleteS);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            var results = await _repo.SearchJewelryByNameOrWeightAsync(searchTerm);
            if (results == null || !results.Any())
            {
                return NotFound("No matching jewelry items found.");
            }
            return Ok(results);
        }
    }
}
