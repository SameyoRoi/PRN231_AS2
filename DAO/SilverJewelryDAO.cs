
using BO;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class SilverJewelryDAO
    {
        private static SilverJewelryDAO instance = null;

        private SilverJewelryDAO()
        {
    
        }

        public static SilverJewelryDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SilverJewelryDAO();
                }
                return instance;
            }
        }

        public async Task<List<SilverJewelry>> GetAll()
        {
            using (var context = new SilverJewelry2023DbContext())
            {
                var silverJewelryList = await context.SilverJewelries
                    .Include(s => s.Category) // Include the Category entity
                    .ToListAsync();

                return silverJewelryList;
            }
        }




        public async Task<SilverJewelry> GetById(string id)
        {
            using (var context = new SilverJewelry2023DbContext())
            {
                var sliver = await context.SilverJewelries.Include(p => p.Category).FirstOrDefaultAsync(sliver => sliver.SilverJewelryId == id);
                return sliver;
            }
        }

        private string GenerateId()
        {
            var random = new Random();
            var id = random.Next(10000, 99999);
            return id.ToString();
        }

        public async Task<SilverJewelry> AddS(SilverJewelry silver)
        {
            try
            {
                using (var context = new SilverJewelry2023DbContext())
                {
                    silver.SilverJewelryId = GenerateId();
                    await context.SilverJewelries.AddAsync(silver);
                    await context.SaveChangesAsync();
                    return silver;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<SilverJewelry> UpdateS(SilverJewelry silver)
        {
            using (var context = new SilverJewelry2023DbContext())
            {
                var silverToUpdate = await context.SilverJewelries.FirstOrDefaultAsync(p => p.SilverJewelryId == silver.SilverJewelryId);
                if (silverToUpdate == null)
                {
                    throw new Exception("Silver not found");
                }
                silverToUpdate.SilverJewelryName = silver.SilverJewelryName;
                silverToUpdate.SilverJewelryDescription = silver.SilverJewelryDescription;
                silverToUpdate.Price = silver.Price;
                silverToUpdate.MetalWeight = silver.MetalWeight;
                silverToUpdate.ProductionYear = silver.ProductionYear;
                silverToUpdate.CreatedDate = silver.CreatedDate;
                silverToUpdate.CategoryId = silver.CategoryId;

                context.Update(silverToUpdate);

                await context.SaveChangesAsync();
                return silverToUpdate;
            }
        }

        public async Task<SilverJewelry> DeleteS(string id)
        {
            try
            {
                using (var context = new SilverJewelry2023DbContext())
                {
                    var silverToDelete = await context.SilverJewelries.FirstOrDefaultAsync(p => p.SilverJewelryId.Equals(id));
                    if (silverToDelete == null)
                    {
                        throw new Exception("silver not found");
                    }

                    context.SilverJewelries.Remove(silverToDelete);
                    await context.SaveChangesAsync();

                    return silverToDelete;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Category>> GetCategory()
        {
            using (var context = new SilverJewelry2023DbContext())
            {
                var categories = await context.Categories.ToListAsync();
                return categories;
            }
        }



        public async Task<List<SilverJewelry>> SearchJewelryByNameOrWeightAsync(string searchTerm)
        {
            using (var context = new SilverJewelry2023DbContext())
            {
                var freshQuery = context.SilverJewelries.AsNoTracking()
                .Include(j => j.Category)
                .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    freshQuery = freshQuery.Where(j => j.SilverJewelryName.Contains(searchTerm) ||
                                                       j.MetalWeight.ToString().Contains(searchTerm));
                }

                return await freshQuery.ToListAsync();
            }
        }
    }
}
