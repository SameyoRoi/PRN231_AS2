using BO;
using DAO;


namespace Repo
{
    public class BranchAccountRepo : IBranchAccountRepo
    {
        public async Task<BranchAccount> Login(string email, string password)
        {
            return await BranchAccountDAO.Instance.Login(email, password);
        }
    }
}
