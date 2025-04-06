using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
namespace DAL
{
    public class DalRole
    {
        public async Task<PageList<Role>> GetRolesByPageAsync(PageReq<RoleDto.Req> req) {
            var res=new List<Role>();
            var total=0;
            using (var context = new webapplicationContext())
            {
                var Query= context.Roles.AsQueryable();
                Query.Where(i=>
                    (i.IfDel==0) &&
                    (string.IsNullOrEmpty(req.Query.RoleName)?true:i.RoleName.Contains(req.Query.RoleName))
                );
                res = await Query.Skip(req.PageSize*(req.PageIndex-1)).Take(req.PageSize).ToListAsync();
                total= await Query.CountAsync();
            }
            return new PageList<Role>(){
                Datas=res,
                TotalCount=total
            };
        }
    }
}