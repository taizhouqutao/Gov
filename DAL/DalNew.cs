using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DAL
{
    public class DalNew
    {
       public async Task<PageList<New>> GetNewsByPageAsync(PageReq<NewReqDto> req) {
            var res=new List<New>();
            int total=0,allcount=0;
            using (var context = new webapplicationContext())
            {
                var Query= context.News.AsQueryable();
                var QureyRes = Query.Where(i=>
                    (i.IfDel==0) &&
                    ((req.Query==null||req.Query.newTypeId==null)?true: req.Query.newTypeId==i.NewTypeId) &&
                    ((req.Query==null||req.Query.isPublic==null)?true: req.Query.isPublic==i.IsPublic) &&
                    ((req.search==null||string.IsNullOrEmpty(req.search.value))?true:i.NewTitle.Contains(req.search.value))
                );
                QureyRes = QureyRes.OrderByDescending(j =>j.Id);
                res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                total= await QureyRes.CountAsync();
                allcount=await context.News.CountAsync(i=>i.IfDel==0 && ((req.Query==null||req.Query.newTypeId==null)?true: req.Query.newTypeId==i.NewTypeId));
            }
            return new PageList<New>(){
                data=res,
                recordsFiltered=total,
                draw=req.draw,
                recordsTotal=allcount
            };
        }

        public async Task<New> GetNewByIdAsync(int Id)
        {
            New res = null;
            using (var context = new webapplicationContext())
            {
                var Query = context.News.AsQueryable();
                res = await Query.FirstOrDefaultAsync(i =>
                    (i.IfDel == 0) &&
                    (i.Id==Id)
                );
            }
            return res;
        }

        public async Task<List<New>> GetNewsByIdAsync(List<int> Ids)
        {
            List<New> res=new List<New>();
            using (var context = new webapplicationContext())
            {
                var Query = context.News.AsQueryable();
                res = await Query.Where(i =>
                    (i.IfDel == 0) &&
                    (Ids.Contains(i.Id))
                ).ToListAsync();
            }
            return res;
        }

        public async Task<New> AddNewAsync(New entity)
        {
            New res = null;
            using (var context = new webapplicationContext())
            {
                res = (await context.News.AddAsync(entity)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }

        public async Task<New> UpdateNewAsync(New entity)
        {
            New res = null;
            using (var context = new webapplicationContext())
            {
                res = (context.News.Update(entity)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }
        public async Task DelNewAsync(List<int> Ids)
        {
            using (var context = new webapplicationContext())
            {
                var res =await context.News.Where(i=>Ids.Contains(i.Id)).ToListAsync();
                res.ForEach(i=>{
                  i.IfDel=1;
                });
                context.News.UpdateRange(res);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateNewsAsync(List<New> entitys)
        {
            using (var context = new webapplicationContext())
            {
                context.News.UpdateRange(entitys);
                await context.SaveChangesAsync();
            }
        }
    }
}