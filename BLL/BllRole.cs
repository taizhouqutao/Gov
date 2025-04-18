using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllRole
    {
        private DalRole dal=new DalRole();
        public async Task<PageList<Role>> GetRolesByPageAsync(PageReq<RoleReqDto> req) {
            return await dal.GetRolesByPageAsync(req);
        }

        public async Task<Role> GetRoleByIdAsync(int Id)
        {
            return await dal.GetRoleByIdAsync(Id);
        }

        public async Task<Role> AddRoleAsync(Role role)
        {
            return await dal.AddRoleAsync(role);
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            return await dal.UpdateRoleAsync(role);
        }

        public async Task DelRoleAsync(List<int> Ids)
        {
            return await dal.DelRoleAsync(Ids);
        }
    }
}