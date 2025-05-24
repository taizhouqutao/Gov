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

    public async Task<ViewLogReportResDto> GetViewLogDetailReports(ViewLogReportReqDto req)
    {
      ViewLogReportResDto result = null;
      using (var context = new webapplicationContext())
      {
        var QuerViewLogs = context.ViewLogs.AsQueryable();
        var QueryNews = context.News.AsQueryable();
        var QureyRes = (from QuerViewLog in QuerViewLogs
                        join QueryNew in QueryNews on QuerViewLog.NewId equals QueryNew.Id into p_QueryNew
                        from QueryNew_Join in p_QueryNew.DefaultIfEmpty()
                        where
                            (QuerViewLog.IfDel == 0) &&
                            ((req == null || req.viewType == null) ? true : QuerViewLog.ViewType == req.viewType) &&
                            ((req == null || req.newTypeId == null) ? true : QuerViewLog.NewTypeId == req.newTypeId) &&
                            ((req == null || req.StartDate == null) ? true : QuerViewLog.ViewData >= req.StartDate) &&
                            ((req == null || req.EndDate == null) ? true : QuerViewLog.ViewData <= req.EndDate)
                        select new
                        {
                          NewId = QuerViewLog.NewId,
                          NewTitle = QueryNew_Join.NewTitle,
                          ViewCount = QuerViewLog.ViewCount
                        }).GroupBy(i => new
                        {
                          NewId = i.NewId,
                          NewTitle = i.NewTitle
                        }).Select(i => new
                        {
                          NewId = i.Key.NewId,
                          NewTitle = i.Key.NewTitle,
                          ViewCount = i.Sum(j => j.ViewCount)
                        }).OrderByDescending(i => i.ViewCount);
        var res = await QureyRes.Take(9).ToListAsync();
        result = new ViewLogReportResDto()
        {
          newTypeName = res.ConvertAll(i => i.NewTitle.Length > 10 ? i.NewTitle.Substring(0, 10) + "..." : i.NewTitle),
          details = res.ConvertAll(i => new ViewLogReportResItemDto()
          {
            name = i.NewTitle.Length > 10 ? i.NewTitle.Substring(0, 10) + "..." : i.NewTitle,
            value = i.ViewCount
          })
        };
      }
      return result;
    }

    public async Task<ViewLogReportResDto> GetViewLogReports(ViewLogReportReqDto req)
    {
      ViewLogReportResDto result = null;
      using (var context = new webapplicationContext())
      {
        var QuerViewLogs = context.ViewLogs.AsQueryable();
        var QueryNewTypes = context.NewTypes.AsQueryable();
        var QureyRes = (from QuerViewLog in QuerViewLogs
                        join QueryNewType in QueryNewTypes on QuerViewLog.NewTypeId equals QueryNewType.Id into p_QueryNewType
                        from QueryNewType_Join in p_QueryNewType.DefaultIfEmpty()
                        where
                            (QuerViewLog.IfDel == 0) &&
                            ((req == null || req.viewType == null) ? true : QuerViewLog.ViewType == req.viewType) &&
                            ((req == null || req.newTypeId == null) ? true : QuerViewLog.NewTypeId == req.newTypeId) &&
                            ((req == null || req.StartDate == null) ? true : QuerViewLog.ViewData >= req.StartDate) &&
                            ((req == null || req.EndDate == null) ? true : QuerViewLog.ViewData <= req.EndDate)
                        select new
                        {
                          NewId = QuerViewLog.NewId,
                          NewTypeId = QuerViewLog.NewTypeId,
                          NewTypeName = QueryNewType_Join.NewTypeName,
                          ViewCount = QuerViewLog.ViewCount
                        }).GroupBy(i => new
                        {
                          NewTypeId = i.NewTypeId,
                          NewTypeName = i.NewTypeName
                        }).Select(i => new
                        {
                          NewTypeId = i.Key.NewTypeId,
                          NewTypeName = i.Key.NewTypeName,
                          ViewCount = i.Sum(j => j.ViewCount)
                        }).OrderByDescending(i => i.ViewCount);
        var res = await QureyRes.ToListAsync();
        result = new ViewLogReportResDto()
        {
          newTypeName = res.ConvertAll(i => i.NewTypeName.Substring(0, i.NewTypeName.Length - 2)),
          details = res.ConvertAll(i => new ViewLogReportResItemDto()
          {
            name = i.NewTypeName.Substring(0, i.NewTypeName.Length - 2),
            value = i.ViewCount
          })
        };
      }
      return result;
    }

    public async Task<ViewLogLineResDto> GetViewLogLines(ViewLogReportReqDto req)
    {
      ViewLogLineResDto result = null;
      using (var context = new webapplicationContext())
      {
        var QuerViewLogs = context.ViewLogs.AsQueryable();
        var QueryNewTypes = context.NewTypes.AsQueryable();
        var QureyRes = (from QuerViewLog in QuerViewLogs
                        join QueryNewType in QueryNewTypes on QuerViewLog.NewTypeId equals QueryNewType.Id into p_QueryNewType
                        from QueryNewType_Join in p_QueryNewType.DefaultIfEmpty()
                        where
                        (QuerViewLog.IfDel == 0) &&
                        ((req == null || req.viewType == null) ? true : QuerViewLog.ViewType == req.viewType) &&
                        ((req == null || req.newTypeId == null) ? true : QuerViewLog.NewTypeId == req.newTypeId) &&
                        ((req == null || req.StartDate == null) ? true : QuerViewLog.ViewData >= req.StartDate) &&
                        ((req == null || req.EndDate == null) ? true : QuerViewLog.ViewData <= req.EndDate)
                        select new
                        {
                          ViewData = QuerViewLog.ViewData,
                          ViewCount = QuerViewLog.ViewCount
                        }).GroupBy(i => i.ViewData).Select(i => new
                        {
                          ViewData = i.Key,
                          ViewCount = i.Sum(j => j.ViewCount)
                        }).OrderBy(i => i.ViewData);
        var res = await QureyRes.ToListAsync();
        var ViewDatas = res.ConvertAll(i => i.ViewData);
        result = new ViewLogLineResDto()
        {
          DayOfWeek = ViewDatas.ConvertAll(i => "周" + new[] { "日", "一", "二", "三", "四", "五", "六" }[(int)i.DayOfWeek]),
          ViewCount = res.ConvertAll(i => i.ViewCount)
        };
      }
      return result;
    }
  }
}