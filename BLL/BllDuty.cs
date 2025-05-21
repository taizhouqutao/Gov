using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllDuty
    {
        private DalDuty dal = new DalDuty();
        private DalBizLog dalLog = new DalBizLog();
        private DalUser bllUser = new DalUser();
        public async Task<List<DutyDetailDto>> GetDutyDetailsByAsync(DutyReqDto req)
        {
            return await dal.GetDutyDetailsByAsync(req);
        }

        public async Task SaveDuty(DutyReqDto req, int CreateUserId = 0)
        {
            var user = await bllUser.GetUserByIdAsync(CreateUserId);
            await dal.SaveDuty(req);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "值班人员管理",
                CreateTime = DateTime.Now,
                CreateUserId = user?.Id ?? 0,
                IfDel = 0,
                ModelTitle = "两委班子信息管理",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "调整值班人员",
                ActionRemark = "调整值班人员，人员编码为：" + string.Join("、", req.contactIds ?? new List<int>()),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = user?.Id ?? 0,
            });
        }
    }
}