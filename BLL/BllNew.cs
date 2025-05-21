using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllNew
    {
        private DalNew dal = new DalNew();
        private DalBizLog dalLog = new DalBizLog();
        private BllUser bllUser = new BllUser();
        private BllNewType bllNewType = new BllNewType();
        public async Task<PageList<New>> GetNewsByPageAsync(PageReq<NewReqDto> req)
        {
            return await dal.GetNewsByPageAsync(req);
        }

        public async Task<List<New>> GetNewsByIdAsync(List<int> Ids)
        {
            return await dal.GetNewsByIdAsync(Ids);
        }

        public async Task<New> GetNewByIdAsync(int Id)
        {
            return await dal.GetNewByIdAsync(Id);
        }

        public async Task<New> AddNewAsync(New entity)
        {
            var user = await bllUser.GetUserByIdAsync(entity.CreateUserId);
            var res = await dal.AddNewAsync(entity);
            var newType = await bllNewType.GetNewTypeByIdAsync(entity.NewTypeId ?? 0);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "内容管理",
                CreateTime = DateTime.Now,
                CreateUserId = user?.Id ?? 0,
                IfDel = 0,
                ModelTitle = $"{newType?.NewTypeName ?? "内容"}管理",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "添加",
                ActionRemark = "添加内容，内容编码为：" + entity.Id,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = user?.Id ?? 0,
            });
            return res;
        }

        public async Task<New> UpdateNewAsync(New entity)
        {
            var user = await bllUser.GetUserByIdAsync(entity.UpdateUserId ?? 0);
            var res = await dal.UpdateNewAsync(entity);
            var newType = await bllNewType.GetNewTypeByIdAsync(entity.NewTypeId ?? 0);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "内容管理",
                CreateTime = DateTime.Now,
                CreateUserId = user?.Id ?? 0,
                IfDel = 0,
                ModelTitle = $"{newType?.NewTypeName ?? "内容"}管理",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "编辑",
                ActionRemark = "编辑内容，内容编码为：" + entity.Id,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = user?.Id ?? 0,
            });
            return res;
        }

        public async Task UpdateNewsAsync(List<New> entitys)
        {
            var user = await bllUser.GetUserByIdAsync(entitys[0].UpdateUserId ?? 0);
            await dal.UpdateNewsAsync(entitys);
            var newType = await bllNewType.GetNewTypeByIdAsync(entitys[0].NewTypeId ?? 0);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "内容管理",
                CreateTime = DateTime.Now,
                CreateUserId = user?.Id ?? 0,
                IfDel = 0,
                ModelTitle = $"{newType?.NewTypeName ?? "内容"}管理",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = entitys[0].IsPublic == 1 ? "发布" : "下线",
                ActionRemark = (entitys[0].IsPublic == 1 ? "发布" : "下线") + "内容，内容编码为：" + string.Join("、", entitys.ConvertAll(i => i.Id)),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = user?.Id ?? 0,
            });
        }

        public async Task DelNewAsync(List<int> Ids, int CreateUserId = 0)
        {
            var user = await bllUser.GetUserByIdAsync(CreateUserId);
            var New = await GetNewByIdAsync(Ids[0]);
            var newType = await bllNewType.GetNewTypeByIdAsync(New?.NewTypeId ?? 0);
            await dal.DelNewAsync(Ids);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "内容管理",
                CreateTime = DateTime.Now,
                CreateUserId = user?.Id ?? 0,
                IfDel = 0,
                ModelTitle = $"{newType?.NewTypeName ?? "内容"}管理",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "删除",
                ActionRemark = "删除内容，内容编码为：" + string.Join("、", Ids),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = user?.Id ?? 0,
            });
        }
    }
}