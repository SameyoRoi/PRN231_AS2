using BO;
using DAO;

namespace Repo
{
    public class SilverJewelryRepo : ISilverJewelryRepo
    {
        public async Task<SilverJewelry> AddS(SilverJewelry silverJewelry)
        {
            return await SilverJewelryDAO.Instance.AddS(silverJewelry);
        }

        public async Task<SilverJewelry> DeleteS(string id)
        {
            return await SilverJewelryDAO.Instance.DeleteS(id);
        }

        public async Task<SilverJewelry> GetSById(string id)
        {
            return await SilverJewelryDAO.Instance.GetById(id);
        }

        public async Task<List<SilverJewelry>> GetAllS()
        {
            // Fetch the SilverJewelry list
            var silverJewelryList = await SilverJewelryDAO.Instance.GetAll();
            return silverJewelryList;
        }


        public async Task<List<SilverDTO>> GetAllSDTO()
        {
            // Fetch the SilverJewelry list
            var silverJewelryList = await SilverJewelryDAO.Instance.GetAll();

            // Map SilverJewelry to SilverDTO
            var silverDTOList = silverJewelryList.Select(silver => new SilverDTO
            {
                SilverJewelryId = silver.SilverJewelryId,
                SilverJewelryName = silver.SilverJewelryName,
                SilverJewelryDescription = silver.SilverJewelryDescription,
                MetalWeight = silver.MetalWeight,
                Price = silver.Price,
                ProductionYear = silver.ProductionYear,
                CreatedDate = silver.CreatedDate,
                CategoryName = silver.Category.CategoryName
            }).ToList();

            return silverDTOList;
        }





        public async Task<SilverJewelry> UpdateS(SilverJewelry silverJewelry)
        {
            return await SilverJewelryDAO.Instance.UpdateS(silverJewelry);
        }
        public async Task<List<Category>> GetCategory()
        {
            return await SilverJewelryDAO.Instance.GetCategory();
        }



        public async Task<List<SilverJewelry>> SearchJewelryByNameOrWeightAsync(string searchTerm)
        {
            return await SilverJewelryDAO.Instance.SearchJewelryByNameOrWeightAsync(searchTerm);
        }


    }
}
