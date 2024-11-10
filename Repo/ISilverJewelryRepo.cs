using BO;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public interface ISilverJewelryRepo
    {
        Task<List<SilverJewelry>> GetAllS();
        Task<List<SilverDTO>> GetAllSDTO();
        Task<SilverJewelry> GetSById(string id);
        Task<SilverJewelry> AddS(SilverJewelry silverJewelry);
        Task<SilverJewelry> UpdateS(SilverJewelry silverJewelry);
        Task<SilverJewelry> DeleteS(string id);
        Task<List<Category>> GetCategory();
        Task<List<SilverJewelry>> SearchJewelryByNameOrWeightAsync(string searchTerm);
    }
}
