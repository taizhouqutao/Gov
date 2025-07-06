using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DAL
{
  public class DalCarousel
  {
    public async Task<PageList<CarouselPlusDto>> GetCarouselsByPageAsync(PageReq<CarouselReqDto> req)
    {
      var res = new List<CarouselPlusDto>();
      int total = 0, allcount = 0;
      using (var context = new webapplicationContext())
      {
        var QueryCarousels = context.Carousels.AsQueryable();
        var QueryCitys = context.Cities.AsQueryable();
        var QureyRes = (from QueryCarousel in QueryCarousels
                        join QueryCity in QueryCitys on QueryCarousel.CityId equals QueryCity.Id into p_QueryCity
                        from QueryCity_Join in p_QueryCity.DefaultIfEmpty()
                        where
                          (QueryCarousel.IfDel == 0) &&
                          ((req.Query == null || req.Query.isPublic == null) ? true : req.Query.isPublic == QueryCarousel.IsPublic) &&
                          ((req.Query == null || req.Query.cityIds == null) ? true : ((QueryCarousel.CityId ?? 0) == 0 || req.Query.cityIds.Contains(QueryCarousel.CityId ?? 0))) &&
                          ((req.search == null || string.IsNullOrEmpty(req.search.value)) ? true : QueryCarousel.Title.Contains(req.search.value))
                        orderby QueryCarousel.Id descending
                        select new CarouselPlusDto()
                        {
                          CreateTime = QueryCarousel.CreateTime,
                          CreateUserId = QueryCarousel.CreateUserId,
                          IfDel = QueryCarousel.IfDel,
                          ImageUrl = QueryCarousel.ImageUrl,
                          CityId = QueryCarousel.CityId,
                          IsPublic = QueryCarousel.IsPublic,
                          Title = QueryCarousel.Title,
                          Id = QueryCarousel.Id,
                          LinkUrl = QueryCarousel.LinkUrl,
                          PublicTime = QueryCarousel.PublicTime,
                          PublicUserId = QueryCarousel.PublicUserId,
                          UpdateTime = QueryCarousel.UpdateTime,
                          UpdateUserId = QueryCarousel.CreateUserId,
                          CityName = ((QueryCarousel.CityId ?? 0) == 0) ? "全部" : QueryCity_Join.CityName ?? "",
                        });
        res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
        total = await QureyRes.CountAsync();
        allcount = await context.Carousels.CountAsync(i =>
          (i.IfDel == 0) &&
          ((req.Query == null || req.Query.cityIds == null) ? true : ((i.CityId ?? 0) == 0 || req.Query.cityIds.Contains(i.CityId ?? 0)))
        );
      }
      return new PageList<CarouselPlusDto>()
      {
        data = res,
        recordsFiltered = total,
        draw = req.draw,
        recordsTotal = allcount
      };
    }

    public async Task<List<Carousel>> GetCarouselsByIdAsync(List<int> Ids)
    {
      List<Carousel> res = new List<Carousel>();
      using (var context = new webapplicationContext())
      {
        var Query = context.Carousels.AsQueryable();
        res = await Query.Where(i =>
            (i.IfDel == 0) &&
            (Ids.Contains(i.Id))
        ).ToListAsync();
      }
      return res;
    }

    public async Task<Carousel?> GetCarouselByIdAsync(int Id)
    {
      Carousel? res = null;
      using (var context = new webapplicationContext())
      {
        var Query = context.Carousels.AsQueryable();
        res = await Query.FirstOrDefaultAsync(i =>
            (i.IfDel == 0) &&
            (i.Id == Id)
        );
      }
      return res;
    }

    public async Task<Carousel> AddCarouselAsync(Carousel entity)
    {
      Carousel res = null;
      using (var context = new webapplicationContext())
      {
        res = (await context.Carousels.AddAsync(entity)).Entity;
        await context.SaveChangesAsync();
      }
      return res;
    }

    public async Task<Carousel> UpdateCarouselAsync(Carousel entity)
    {
      Carousel res = null;
      using (var context = new webapplicationContext())
      {
        res = (context.Carousels.Update(entity)).Entity;
        await context.SaveChangesAsync();
      }
      return res;
    }

    public async Task UpdateCarouselsAsync(List<Carousel> entitys)
    {
      using (var context = new webapplicationContext())
      {
        context.Carousels.UpdateRange(entitys);
        await context.SaveChangesAsync();
      }
    }

    public async Task DelCarouselAsync(List<int> Ids)
    {
      using (var context = new webapplicationContext())
      {
        var res = await context.Carousels.Where(i => Ids.Contains(i.Id)).ToListAsync();
        res.ForEach(i =>
        {
          i.IfDel = 1;
        });
        context.Carousels.UpdateRange(res);
        await context.SaveChangesAsync();
      }
    }
  }
}