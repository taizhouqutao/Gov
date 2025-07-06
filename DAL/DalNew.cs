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
       public async Task<PageList<NewPlusDto>> GetNewsByPageAsync(PageReq<NewReqDto> req) {
            var res=new List<NewPlusDto>();
            int total=0,allcount=0;
            using (var context = new webapplicationContext())
            {
                var QueryNews= context.News.AsQueryable();
                var QueryCitys= context.Cities.AsQueryable();
                var QureyRes = (from QueryNew in QueryNews
                join QueryCity in QueryCitys on QueryNew.CityId equals QueryCity.Id into p_QueryCity
                from QueryCity_Join in p_QueryCity.DefaultIfEmpty()
                where
                    (QueryNew.IfDel == 0) &&
                    ((req.Query == null || req.Query.newTypeId == null) ? true : req.Query.newTypeId == QueryNew.NewTypeId) &&
                    ((req.Query == null || req.Query.isPublic == null) ? true : req.Query.isPublic == QueryNew.IsPublic) &&
                    ((req.Query == null || req.Query.cityIds == null) ? true : ((QueryNew.CityId ?? 0) == 0 || req.Query.cityIds.Contains(QueryNew.CityId ?? 0))) &&
                    ((req.search == null || string.IsNullOrEmpty(req.search.value)) ? true : QueryNew.NewTitle.Contains(req.search.value))
                    orderby QueryNew.Id descending
                    select new NewPlusDto
                    {
                        Id = QueryNew.Id,
                        NewTitle = QueryNew.NewTitle,
                        NewContent = QueryNew.NewContent,
                        IsPublic = QueryNew.IsPublic,
                        PublicTime = QueryNew.PublicTime,
                        CityName = QueryCity_Join.CityName,
                        CityId = QueryNew.CityId,
                        CreateTime = QueryNew.CreateTime,
                        CreateUserId = QueryNew.CreateUserId,
                        UpdateTime = QueryNew.UpdateTime,
                        UpdateUserId = QueryNew.UpdateUserId,
                        IfDel = QueryNew.IfDel,
                        ViewCount = QueryNew.ViewCount,
                        CommentCount = QueryNew.CommentCount,
                        NewTypeId = QueryNew.NewTypeId,
                        PublicUserId = QueryNew.PublicUserId,
                    }
                );
                res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                total= await QueryCitys.CountAsync();
                var QureyAll = (from QueryNew in QueryNews
                                join QueryCity in QueryCitys on QueryNew.CityId equals QueryCity.Id into p_QueryCity
                                from QueryCity_Join in p_QueryCity.DefaultIfEmpty()
                                where
                                    (QueryNew.IfDel == 0) &&
                                    ((req.Query == null || req.Query.newTypeId == null) ? true : req.Query.newTypeId == QueryNew.NewTypeId) &&
                                    ((req.Query == null || req.Query.cityIds == null) ? true : ((QueryNew.CityId ?? 0) == 0 || req.Query.cityIds.Contains(QueryNew.CityId ?? 0)))
                                select new
                                {
                                    id = QueryNew.Id
                                });
                allcount=await QureyAll.CountAsync();
            }
            return new PageList<NewPlusDto>(){
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