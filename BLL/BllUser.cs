using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllUser
    {
        private DalUser dal = new DalUser();
        private DalBizLog dalLog = new DalBizLog();
        public async Task<PageList<User>> GetUsersByPageAsync(PageReq<UserReqDto> req)
        {
            return await dal.GetUsersByPageAsync(req);
        }

        public async Task<List<User>> GetUsersAsync(UserReqDto req)
        {
            return await dal.GetUsersAsync(req);
        }

        public async Task<List<User>> GetUsersByIdAsync(List<int> Ids)
        {
            return await dal.GetUsersByIdAsync(Ids);
        }

        public async Task<User?> GetUserByIdAsync(int Id)
        {
            return await dal.GetUserByIdAsync(Id);
        }

        public async Task<User?> LoginAsync(LoginReqDto req)
        {
            var res = await dal.LoginAsync(req);
            if (res != null)
            {
                await dalLog.AddBizLogAsync(new BizLog()
                {
                    ActionType = "用户管理",
                    CreateTime = DateTime.Now,
                    CreateUserId = res?.Id ?? 0,
                    IfDel = 0,
                    ModelTitle = $"系统设置",
                    OptUserName = res?.UserName ?? "系统",
                    ActionDesc = "登录",
                    ActionRemark = "用户登录，用户为：" + res?.UserName,
                    ActionJson = "{}",
                    UpdateTime = DateTime.Now,
                    UpdateUserId = res?.Id ?? 0
                });
            }
            return res;
        }


        public async Task<User?> AddUserAsync(User entity)
        {
            var user = await GetUserByIdAsync(entity.CreateUserId);
            var res = await dal.AddUserAsync(entity);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "用户管理",
                CreateTime = DateTime.Now,
                CreateUserId = entity.CreateUserId,
                IfDel = 0,
                ModelTitle = $"系统设置",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "添加",
                ActionRemark = "用户新增，用户为：" + res?.UserName,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = entity.CreateUserId
            });
            return res;
        }

        public async Task<User?> UpdateUserAsync(User entity)
        {
            var user = await GetUserByIdAsync(entity.UpdateUserId ?? 0);
            var res = await dal.UpdateUserAsync(entity);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "用户管理",
                CreateTime = DateTime.Now,
                CreateUserId = entity.UpdateUserId ?? 0,
                IfDel = 0,
                ModelTitle = $"系统设置",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "编辑",
                ActionRemark = "用户编辑，用户为：" + res?.UserName,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = entity.UpdateUserId ?? 0
            });
            return res;
        }

        public async Task UpdateUsersAsync(List<User> entitys)
        {
            var user = await GetUserByIdAsync(entitys[0].UpdateUserId ?? 0);
            await dal.UpdateUsersAsync(entitys);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "用户管理",
                CreateTime = DateTime.Now,
                CreateUserId = entitys[0].UpdateUserId ?? 0,
                IfDel = 0,
                ModelTitle = $"系统设置",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = entitys[0].Enable == 1 ? "启用" : "禁用",
                ActionRemark = "用户" + (entitys[0].Enable == 1 ? "启用" : "禁用") + "，用户为：" + string.Join("、", entitys.ConvertAll(i => i.UserName)),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = entitys[0].UpdateUserId ?? 0
            });
        }

        public async Task DelUserAsync(List<int> Ids, int CreateUserId = 0)
        {
            var user = await GetUserByIdAsync(CreateUserId);
            await dal.DelUserAsync(Ids);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "用户管理",
                CreateTime = DateTime.Now,
                CreateUserId = user?.Id ?? 0,
                IfDel = 0,
                ModelTitle = $"系统设置",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "删除",
                ActionRemark = "用户删除，用户编码为：" + string.Join("、", Ids),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = user?.Id ?? 0
            });
        }

        public async Task SaveUserRole(User entity, List<int> RoleIds)
        {
            await dal.SaveUserRole(entity, RoleIds);
        }

        public async Task<List<string>> GetRightCodeByUser(int UserId)
        {
            return await dal.GetRightCodeByUser(UserId);
        }
    }
}