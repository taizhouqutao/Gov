using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
namespace DAL
{
    public class DalRole
    {
        public async Task<PageList<Role>> GetRolesByPageAsync(PageReq<RoleReqDto> req) {
            var res=new List<Role>();
            int total=0,allcount=0;
            using (var context = new webapplicationContext())
            {
                var Query= context.Roles.AsQueryable();
                var QureyRes = Query.Where(i=>
                    (i.IfDel==0) &&
                    ((req.search==null||string.IsNullOrEmpty(req.search.value))?true:i.RoleName.Contains(req.search.value))
                );
                res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                total= await QureyRes.CountAsync();
                allcount=await context.Roles.CountAsync();
            }
            return new PageList<Role>(){
                data=res,
                recordsFiltered=total,
                draw=req.draw,
                recordsTotal=allcount
            };
        }
    }
}