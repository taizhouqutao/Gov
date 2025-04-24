using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DAL
{
    public class DalBizLog
    {
        public async Task<PageList<BizLog>> GetBizLogsByPageAsync(PageReq<BizLogReqDto> req) {
            var res=new List<BizLog>();
            int total=0,allcount=0;
            using (var context = new webapplicationContext())
            {
                var Query= context.BizLogs.AsQueryable();
                var QureyRes = Query.Where(i=>
                    (i.IfDel==0) &&
                    ((req.search==null||string.IsNullOrEmpty(req.search.value))?true:
                    (i.ModelTitle.Contains(req.search.value)||i.ActionType.Contains(req.search.value)))
                );
                QureyRes = QureyRes.OrderByDescending(j =>j.Id);
                res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                total= await QureyRes.CountAsync();
                allcount=await context.BizLogs.CountAsync();
            }
            return new PageList<BizLog>(){
                data=res,
                recordsFiltered=total,
                draw=req.draw,
                recordsTotal=allcount
            };
        }

        public async Task<BizLog?> GetBizLogByIdAsync(int Id)
        {
            BizLog? res = null;
            using (var context = new webapplicationContext())
            {
                var Query = context.BizLogs.AsQueryable();
                res = await Query.FirstOrDefaultAsync(i =>
                    (i.IfDel == 0) &&
                    (i.Id==Id)
                );
            }
            return res;
        }

    }
}