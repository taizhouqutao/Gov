using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllRight
    {
        private DalRight dal = new DalRight();
        private DalUser bllUser = new DalUser();
        private DalBizLog dalLog = new DalBizLog();

        public async Task<List<RightTreeDto>> GetRightTreeAsync(RightReqDto req)
        {
            return await dal.GetRightTreeAsync(req);
        }
        public async Task SaveRoleRights(RightReqDto req)
        {
            var user = await bllUser.GetUserByIdAsync(req?.UserId ?? 0);
            await dal.SaveRoleRights(req);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "角色管理",
                CreateTime = DateTime.Now,
                CreateUserId = req?.UserId ?? 0,
                IfDel = 0,
                ModelTitle = "系统设置",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "分配权限",
                ActionRemark = "分配角色权限，角色编码为：" + (req?.roleId ?? 0),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = req?.UserId ?? 0,
            });
        }
    }
}