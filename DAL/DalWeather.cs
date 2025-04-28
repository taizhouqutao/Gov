using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
namespace DAL
{
    public class DalWeather
    {
        public async Task<List<Weather>> GetWeathersAsync(WeatherReqDto req) {
            var res=new List<Weather>();
            using (var context = new webapplicationContext())
            {
                var Query= context.Weathers.AsQueryable();
                var QureyRes = Query.Where(i=>
                    (i.IfDel==0) &&
                    ((req.StartUpdateTime==null||req.StartUpdateTime==DateTime.MinValue)?true:i.UpdateTime>=req.StartUpdateTime) &&
                    ((req.WeatherDate == null || req.WeatherDate == DateTime.MinValue) ? true : i.WeatherDate == req.WeatherDate)
                );
                res = await QureyRes.ToListAsync();
            }
            return res;
        }

        public async Task<Weather?> AddWeatherAsync(Weather entity)
        {
            Weather? res = null;
            using (var context = new webapplicationContext())
            {
                res = (await context.Weathers.AddAsync(entity)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }
    }
}