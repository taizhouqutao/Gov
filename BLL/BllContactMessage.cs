using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
  public class BllContactMessage
  {
    private DalContactMessage dal = new DalContactMessage();
    private DalBizLog dalLog = new DalBizLog();
    private DalUser bllUser = new DalUser();

    public async Task<List<CommentGroupResDto>> GetContactMessageGroupsByAsync(ContactMessageReqDto req)
    {
      return await dal.GetContactMessageGroupsByAsync(req);
    }

    public async Task<PageList<ContactMessage>> GetContactMessagesByPageAsync(PageReq<ContactMessageReqDto> req)
    {
      return await dal.GetContactMessagesByPageAsync(req);
    }

    public async Task<List<ContactMessage>> GetContactMessagesByAsync(ContactMessageReqDto req)
    {
      return await dal.GetContactMessagesByAsync(req);
    }

    public async Task<ContactMessage> AddContactMessageAsync(ContactMessage entity)
    {
      var user = await bllUser.GetUserByIdAsync(entity.CreateUserId);
      var res = await dal.AddContactMessageAsync(entity);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "群众留言管理",
        CreateTime = DateTime.Now,
        CreateUserId = entity.CreateUserId,
        IfDel = 0,
        ModelTitle = "两委班子信息管理",
        OptUserName = user?.UserName ?? entity.PersonName,
        ActionDesc = entity.CreateUserId == 0 ? "留言新增" : "留言回复",
        ActionRemark = (entity.CreateUserId == 0 ? "留言新增" : "留言回复") + "，留言编码为：" + entity.Id,
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = entity.CreateUserId
      });
      return res;
    }

    public async Task<ContactMessage?> GetContactMessageByIdAsync(int Id)
    {
      return await dal.GetContactMessageByIdAsync(Id);
    }

    public async Task<CommentResDetailDto?> GetContactMessageDetailByIdAsync(int Id)
    {
      return await dal.GetContactMessageDetailByIdAsync(Id);
    }

    public async Task DelMessagesAsync(List<int> Ids, int CreateUserId = 0)
    {
      var user = await bllUser.GetUserByIdAsync(CreateUserId);
      await dal.DelMessagesAsync(Ids);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "群众留言管理",
        CreateTime = DateTime.Now,
        CreateUserId = CreateUserId,
        IfDel = 0,
        ModelTitle = "两委班子信息管理",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = "删除",
        ActionRemark = "留言删除，留言编码为：" + string.Join("、", Ids),
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = CreateUserId
      });
    }
    public async Task SetMessageShow(ContactMessageReqDto req, int CreateUserId = 0)
    {
      var user = await bllUser.GetUserByIdAsync(CreateUserId);
      await dal.SetMessageShow(req);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "群众留言管理",
        CreateTime = DateTime.Now,
        CreateUserId = CreateUserId,
        IfDel = 0,
        ModelTitle = "两委班子信息管理",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = req.isShow == 1 ? "显示" : "隐藏",
        ActionRemark = "留言" + (req.isShow == 1 ? "显示" : "隐藏") + "，留言编码为：" + string.Join("、", req.ids ?? new List<int>()),
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = CreateUserId
      });
    }

    public async Task<ContactMessage> UpdateContactMessageAsync(ContactMessage entity)
    {
      var user = await bllUser.GetUserByIdAsync(entity.UpdateUserId ?? 0);
      var res = await dal.UpdateContactMessageAsync(entity);
      await dalLog.AddBizLogAsync(new BizLog()
      {
        ActionType = "群众留言管理",
        CreateTime = DateTime.Now,
        CreateUserId = user?.Id ?? 0,
        IfDel = 0,
        ModelTitle = "两委班子信息管理",
        OptUserName = user?.UserName ?? "系统",
        ActionDesc = "处理",
        ActionRemark = "留言处理，留言编码为：" + entity.Id,
        ActionJson = "{}",
        UpdateTime = DateTime.Now,
        UpdateUserId = user?.Id ?? 0,
      });
      return res;
    }
  }
}