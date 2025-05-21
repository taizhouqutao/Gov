using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
  public class BllCarousel
  {
    private DalCarousel dal = new DalCarousel();
    private DalBizLog dalLog = new DalBizLog();
    private DalUser bllUser = new DalUser();
    public async Task<PageList<Carousel>> GetCarouselsByPageAsync(PageReq<CarouselReqDto> req)
    {
      return await dal.GetCarouselsByPageAsync(req);
    }

    public async Task<List<Carousel>> GetCarouselsByIdAsync(List<int> Ids)
    {
      return await dal.GetCarouselsByIdAsync(Ids);
    }

    public async Task<Carousel?> GetCarouselByIdAsync(int Id)
    {
      return await dal.GetCarouselByIdAsync(Id);
    }

    public async Task<Carousel> AddCarouselAsync(Carousel entity)
    {
      var res = await dal.AddCarouselAsync(entity);
      var user = await bllUser.GetUserByIdAsync(entity.CreateUserId);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "首页轮播图",
        CreateTime = DateTime.Now,
        CreateUserId = entity.CreateUserId,
        IfDel = 0,
        ModelTitle = "公告公示管理",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = "添加",
        ActionRemark = "添加轮播图，轮播图编码为：" + res.Id,
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = entity.CreateUserId
      });
      return res;
    }

    public async Task<Carousel> UpdateCarouselAsync(Carousel entity)
    {
      var user = await bllUser.GetUserByIdAsync(entity.UpdateUserId ?? 0);
      var res = await dal.UpdateCarouselAsync(entity);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "首页轮播图",
        CreateTime = DateTime.Now,
        CreateUserId = entity.CreateUserId,
        IfDel = 0,
        ModelTitle = "公告公示管理",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = "编辑",
        ActionRemark = "编辑轮播图，轮播图编码为：" + res.Id,
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = entity.CreateUserId
      });
      return res;
    }

    public async Task UpdateCarouselsAsync(List<Carousel> entitys)
    {
      var user = await bllUser.GetUserByIdAsync(entitys[0].UpdateUserId ?? 0);
      await dal.UpdateCarouselsAsync(entitys);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "首页轮播图",
        CreateTime = DateTime.Now,
        CreateUserId = user?.Id ?? 0,
        IfDel = 0,
        ModelTitle = "公告公示管理",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = "编辑",
        ActionRemark = "编辑轮播图，轮播图编码为：" + string.Join("、", entitys.ConvertAll(i => i.Id)),
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = user?.Id ?? 0,
      });
    }

    public async Task DelCarouselAsync(List<int> Ids, int CreateUserId = 0)
    {
      var user = await bllUser.GetUserByIdAsync(CreateUserId);
      await dal.DelCarouselAsync(Ids);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "首页轮播图",
        CreateTime = DateTime.Now,
        CreateUserId = CreateUserId,
        IfDel = 0,
        ModelTitle = "公告公示管理",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = "删除",
        ActionRemark = "删除轮播图，轮播图编码为：" + string.Join("、", Ids),
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = CreateUserId
      });
    }
  }
}