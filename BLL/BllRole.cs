using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllRole
    {
        private DalRole dal=new DalRole();
        public async Task<PageList<Role>> GetRolesByPageAsync(PageReq<RoleDto.Req> req) {
            return await dal.GetRolesByPageAsync(req);
        }
    }
}