using BO;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class BranchAccountDAO
    {
        private static BranchAccountDAO instance = null;
        private readonly SilverJewelry2023DbContext context;

        private BranchAccountDAO()
        {
            context = new SilverJewelry2023DbContext();
        }

        public static BranchAccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BranchAccountDAO();
                }
                return instance;
            }
        }

        public async Task<BranchAccount> Login(string email, string password)
        {
            var account = await context.BranchAccounts.FirstOrDefaultAsync(account => account.EmailAddress == email && account.AccountPassword == password);
            return account;
        }
    }
}
