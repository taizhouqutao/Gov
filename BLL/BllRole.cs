using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllRole
    {
        private DalRole dal = new DalRole();
        private DalBizLog dalLog = new DalBizLog();
        private DalUser bllUser = new DalUser();
        public async Task<PageList<Role>> GetRolesByPageAsync(PageReq<RoleReqDto> req)
        {
            return await dal.GetRolesByPageAsync(req);
        }

        public async Task<Role> GetRoleByIdAsync(int Id)
        {
            return await dal.GetRoleByIdAsync(Id);
        }

        public async Task<Role> AddRoleAsync(Role role)
        {
            var user = await bllUser.GetUserByIdAsync(role.CreateUserId);
            var res = await dal.AddRoleAsync(role);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "角色管理",
                CreateTime = DateTime.Now,
                CreateUserId = user?.Id ?? 0,
                IfDel = 0,
                ModelTitle = $"系统设置",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "添加",
                ActionRemark = "添加角色，角色编码为：" + role.Id,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = user?.Id ?? 0,
            });
            return res;
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            var user = await bllUser.GetUserByIdAsync(role.UpdateUserId ?? 0);
            var res = await dal.UpdateRoleAsync(role);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "角色管理",
                CreateTime = DateTime.Now,
                CreateUserId = user?.Id ?? 0,
                IfDel = 0,
                ModelTitle = $"系统设置",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "编辑",
                ActionRemark = "编辑角色，角色编码为：" + role.Id,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = user?.Id ?? 0,
            });
            return res;
        }

        public async Task DelRoleAsync(List<int> Ids, int CreateUserId = 0)
        {
            var user = await bllUser.GetUserByIdAsync(CreateUserId);
            await dal.DelRoleAsync(Ids);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "角色管理",
                CreateTime = DateTime.Now,
                CreateUserId = user?.Id ?? 0,
                IfDel = 0,
                ModelTitle = $"系统设置",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "删除",
                ActionRemark = "删除角色，角色编码为：" + string.Join("、", Ids),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = user?.Id ?? 0,
            });
        }
    }
}