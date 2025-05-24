using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
namespace DAL
{
  public class DalViewLog
  {
    public async Task<ViewLog> SaveViewLog(ViewLog entity)
    {
      ViewLog? res = null;
      using (var context = new webapplicationContext())
      {
        res = context.ViewLogs.FirstOrDefault(i => i.ViewData == entity.ViewData && i.ViewType == entity.ViewType && i.NewId == entity.NewId);
        if (res == null)
        {
          res = (await context.ViewLogs.AddAsync(entity)).Entity;
        }
        else
        {
          res.ViewCount = res.ViewCount + 1;
          res = (context.ViewLogs.Update(res)).Entity;
        }
        await context.SaveChangesAsync();
      }
      return res;
    }
  }
}