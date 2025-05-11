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
  }
}