using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DAL
{
  public class DalCity
  {
    public async Task<PageList<City>> GetCitysByPageAsync(PageReq<CityReqDto> req)
    {
      var res = new List<City>();
      int total = 0, allcount = 0;
      using (var context = new webapplicationContext())
      {
        var Query = context.Cities.AsQueryable();
        var QureyRes = Query.Where(i =>
            (i.IfDel == 0) &&
            ((req.search == null || string.IsNullOrEmpty(req.search.value)) ? true : i.CityName.Contains(req.search.value))
        );
        QureyRes = QureyRes.OrderByDescending(j => j.Id);
        res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
        total = await QureyRes.CountAsync();
        allcount = await context.Cities.CountAsync(i => i.IfDel == 0);
      }
      return new PageList<City>()
      {
        data = res,
        recordsFiltered = total,
        draw = req.draw,
        recordsTotal = allcount
      };
    }

    public async Task<List<CityResDto>> GetCitysByIdAsync(List<int> Ids)
    {
      List<CityResDto> res = new List<CityResDto>();
      using (var context = new webapplicationContext())
      {
        var Citys = await context.Cities.Where(i => Ids.Contains(i.Id)).ToListAsync();
        res = Citys.ConvertAll(i => new CityResDto()
        {
          cityid = i.Id,
          cityName = i.CityName,
          ifCheck = 0
        });
      }
      return res;
    }
    public async Task<List<City>> GetCitysAsync(CityReqDto req)
    {
      using (var context = new webapplicationContext())
      {
        return await context.Cities.Where(i => i.IfDel == 0).ToListAsync();
      }
    }

    public async Task<City?> GetCityByIdAsync(int Id)
    {
      using (var context = new webapplicationContext())
      {
        return await context.Cities.FirstOrDefaultAsync(i => i.Id == Id && i.IfDel == 0);
      }
    }

    public async Task<City> AddCityAsync(City entity)
    {
      City res = null;
      using (var context = new webapplicationContext())
      {
        res = (await context.Cities.AddAsync(entity)).Entity;
        await context.SaveChangesAsync();
      }
      return res;
    }

    public async Task<City> UpdateCityAsync(City entity)
    {
      City res = null;
      using (var context = new webapplicationContext())
      {
        res = (context.Cities.Update(entity)).Entity;
        await context.SaveChangesAsync();
      }
      return res;
    }

    public async Task DelCityAsync(List<int> Ids)
    {
      using (var context = new webapplicationContext())
      {
        var res = await context.Cities.Where(i => Ids.Contains(i.Id)).ToListAsync();
        res.ForEach(i =>
        {
          i.IfDel = 1;
        });
        context.Cities.UpdateRange(res);
        await context.SaveChangesAsync();
      }
    }
  }
}