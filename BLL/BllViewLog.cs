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
  }
}