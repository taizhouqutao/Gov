using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
  public class BllViewLog
  {
    private DalViewLog dal = new DalViewLog();

    public async Task<ViewLog> SaveViewLog(ViewLog entity)
    {
      return await dal.SaveViewLog(entity);
    }

    public async Task<ViewLogReportResDto> GetViewLogDetailReports(ViewLogReportReqDto entity)
    {
      return await dal.GetViewLogDetailReports(entity);
    }

    public async Task<ViewLogReportResDto> GetViewLogReports(ViewLogReportReqDto entity)
    {
      return await dal.GetViewLogReports(entity);
    }

    public async Task<ViewLogLineResDto> GetViewLogLines(ViewLogReportReqDto entity)
    {
      return await dal.GetViewLogLines(entity);
    }
  }
}