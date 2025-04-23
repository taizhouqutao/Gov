using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllUser
    {
        private DalUser dal=new DalUser();
        public async Task<PageList<User>> GetUsersByPageAsync(PageReq<UserReqDto> req) {
            return await dal.GetUsersByPageAsync(req);
        }

        public async Task<User?> LoginAsync(LoginReqDto req) {
            return await dal.LoginAsync(req);
        }
    }
}