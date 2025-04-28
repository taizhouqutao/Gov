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

        public async Task<List<User>> GetUsersAsync(UserReqDto req) {
            return await dal.GetUsersAsync(req);
        }

        public async Task<List<User>> GetUsersByIdAsync(List<int> Ids)
        {
            return await dal.GetUsersByIdAsync(Ids);
        }

        public async Task<User?> GetUserByIdAsync(int Id)
        {
            return await dal.GetUserByIdAsync(Id);
        }

        public async Task<User?> LoginAsync(LoginReqDto req) {
            return await dal.LoginAsync(req);
        }


        public async Task<User?> AddUserAsync(User entity)
        {
            return await dal.AddUserAsync(entity);
        }

        public async Task<User?> UpdateUserAsync(User entity)
        {
            return await dal.UpdateUserAsync(entity);
        }

        public async Task UpdateUsersAsync(List<User> entitys)
        {
            await dal.UpdateUsersAsync(entitys);
        }

        public async Task DelUserAsync(List<int> Ids)
        {
            await dal.DelUserAsync(Ids);
        }
    }
}