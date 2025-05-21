using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllContact
    {
        private DalContact dal = new DalContact();
        private DalBizLog dalLog = new DalBizLog();
        private DalUser bllUser = new DalUser();
        public async Task<List<Contact>> GetContactsByAsync(ContactReqDto req)
        {
            return await dal.GetContactsByAsync(req);
        }

        public async Task<Contact?> GetContactByIdAsync(int Id)
        {
            return await dal.GetContactByIdAsync(Id);
        }

        public async Task DelContactAsync(List<int> Ids, int CreateUserId = 0)
        {
            var user = await bllUser.GetUserByIdAsync(CreateUserId);
            await dal.DelContactAsync(Ids);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "人员信息管理",
                CreateTime = DateTime.Now,
                CreateUserId = CreateUserId,
                IfDel = 0,
                ModelTitle = "两委班子信息管理",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "删除",
                ActionRemark = "删除人员信息管理，人员信息管理编码为：" + string.Join("、", Ids),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = CreateUserId
            });
        }

        public async Task<Contact> AddContactAsync(Contact entity)
        {
            var user = await bllUser.GetUserByIdAsync(entity.CreateUserId);
            var res = await dal.AddContactAsync(entity);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "人员信息管理",
                CreateTime = DateTime.Now,
                CreateUserId = entity.CreateUserId,
                IfDel = 0,
                ModelTitle = "两委班子信息管理",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "添加",
                ActionRemark = "添加人员信息管理，人员信息管理编码为：" + entity.Id,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = entity.CreateUserId
            });
            return res;
        }

        public async Task<Contact> UpdateContactAsync(Contact entity)
        {
            var user = await bllUser.GetUserByIdAsync(entity.UpdateUserId ?? 0);
            var res = await dal.UpdateContactAsync(entity);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "人员信息管理",
                CreateTime = DateTime.Now,
                CreateUserId = entity.CreateUserId,
                IfDel = 0,
                ModelTitle = "两委班子信息管理",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "编辑",
                ActionRemark = "编辑人员信息管理，人员信息管理编码为：" + entity.Id,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = entity.CreateUserId
            });
            return res;
        }
    }
}