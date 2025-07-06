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
    public async Task<PageList<Carousel>> GetCarouselsByPageAsync(PageReq<CarouselReqDto> req)
    {
      var res = new List<Carousel>();
      int total = 0, allcount = 0;
      using (var context = new webapplicationContext())
      {
        var Query = context.Carousels.AsQueryable();
        var QureyRes = Query.Where(i =>
            (i.IfDel == 0) &&
            ((req.Query == null || req.Query.isPublic == null) ? true : req.Query.isPublic == i.IsPublic) &&
            ((req.Query == null || req.Query.cityIds == null) ? true : ((i.CityId ?? 0) == 0 || req.Query.cityIds.Contains(i.CityId ?? 0))) &&
            ((req.search == null || string.IsNullOrEmpty(req.search.value)) ? true : i.Title.Contains(req.search.value))
        );
        QureyRes = QureyRes.OrderByDescending(j => j.Id);
        res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
        total = await QureyRes.CountAsync();
        allcount = await context.Carousels.CountAsync(i => i.IfDel == 0);
      }
      return new PageList<Carousel>()
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