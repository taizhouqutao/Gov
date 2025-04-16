using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
namespace DAL
{
    public class DalUser
    {
        public async Task<PageList<User>> GetUsersByPageAsync(PageReq<UserReqDto> req) {
            var res=new List<User>();
            int total=0,allcount=0;
            using (var context = new webapplicationContext())
            {
                var Query= context.Users.AsQueryable();
                var QureyRes = Query.Where(i=>
                    (i.IfDel==0) &&
                    ((req.search==null||string.IsNullOrEmpty(req.search.value))?true:i.UserName.Contains(req.search.value))
                );
                if (req.order == null || req.order.Count == 0)
                {
                    QureyRes = QureyRes.OrderByDescending(i => i.Id);
                }
                res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                total= await QureyRes.CountAsync();
                allcount=await context.Users.CountAsync();
            }
            return new PageList<User>(){
                data=res,
                recordsFiltered=total,
                draw=req.draw,
                recordsTotal=allcount
            };
        }
    }
}