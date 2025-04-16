using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
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
                if (req.order == null || req.order.Count == 0)
                {
                    for (int i = 0; i < req.order.Count; i++)
                    {
                        if (i == 0 && req.order[i].dir.ToUpper()=="DESC")
                        {
                            QureyRes = QureyRes.OrderByDescending(j =>j.GetType().GetProperty(req.order[i]). i.Id);
                        }
                        else if (i == 0 && req.order[i].dir.ToUpper() == "ASC")
                        {
                            QureyRes = QureyRes.OrderBy(i => i.Id);
                        }
                    }
                }
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

        public async Task<Role> GetRoleByIdAsync(int Id)
        {
            Role res = null;
            int total = 0, allcount = 0;
            using (var context = new webapplicationContext())
            {
                var Query = context.Roles.AsQueryable();
                res = await Query.FirstOrDefaultAsync(i =>
                    (i.IfDel == 0) &&
                    (i.Id==Id)
                );
            }
            return res;
        }

        public async Task<Role> AddRoleAsync(Role role)
        {
            Role res = null;
            using (var context = new webapplicationContext())
            {
                res = (await context.Roles.AddAsync(role)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            Role res = null;
            using (var context = new webapplicationContext())
            {
                res = (context.Roles.Update(role)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }
    }
}