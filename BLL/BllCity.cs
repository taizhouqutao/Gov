using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
  public class BllCity
  {
    private DalCity dal = new DalCity();
    private DalBizLog dalLog = new DalBizLog();
    private DalUser bllUser = new DalUser();
    public async Task<PageList<City>> GetCitysByPageAsync(PageReq<CityReqDto> req)
    {
      return await dal.GetCitysByPageAsync(req);
    }

    public async Task<List<City>> GetCitysAsync(CityReqDto req)
    {
      return await dal.GetCitysAsync(req);
    }

    public async Task<City?> GetCityByIdAsync(int Id)
    {
      return await dal.GetCityByIdAsync(Id);
    }

    public async Task<City> AddCityAsync(City entity)
    {
      var res = await dal.AddCityAsync(entity);
      var user = await bllUser.GetUserByIdAsync(entity.CreateUserId);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "区域管理",
        CreateTime = DateTime.Now,
        CreateUserId = entity.CreateUserId,
        IfDel = 0,
        ModelTitle = "系统设置",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = "添加",
        ActionRemark = "添加区域，区域编码为：" + res.Id,
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = entity.CreateUserId
      });
      return res;
    }

    public async Task<City> UpdateCityAsync(City entity)
    {
      var user = await bllUser.GetUserByIdAsync(entity.UpdateUserId ?? 0);
      var res = await dal.UpdateCityAsync(entity);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "区域管理",
        CreateTime = DateTime.Now,
        CreateUserId = entity.CreateUserId,
        IfDel = 0,
        ModelTitle = "系统设置",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = "编辑",
        ActionRemark = "编辑区域，区域编码为：" + res.Id,
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = entity.CreateUserId
      });
      return res;
    }

    public async Task DelCityAsync(List<int> Ids, int UserId)
    {
      var user = await bllUser.GetUserByIdAsync(UserId);
      await dal.DelCityAsync(Ids);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "区域管理",
        CreateTime = DateTime.Now,
        CreateUserId = UserId,
        IfDel = 0,
        ModelTitle = "系统设置",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = "删除",
        ActionRemark = "删除区域，区域编码为：" + string.Join("、", Ids),
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = UserId
      });
    }
  }
}