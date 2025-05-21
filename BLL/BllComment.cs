using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllComment
    {
        private DalComment dal = new DalComment();
        private DalBizLog dalLog = new DalBizLog();
        private BllUser bllUser = new BllUser();
        public async Task<Comment?> GetCommentByIdAsync(int Id)
        {
            return await dal.GetCommentByIdAsync(Id);
        }

        public async Task<List<Comment>> GetCommentsByAsync(CommentReqDto req)
        {
            return await dal.GetCommentsByAsync(req);
        }
        public async Task<Comment> AddCommentAsync(Comment entity)
        {
            var user = await bllUser.GetUserByIdAsync(entity.CreateUserId);
            var res = await dal.AddCommentAsync(entity);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "在线反馈",
                CreateTime = DateTime.Now,
                CreateUserId = entity.CreateUserId,
                IfDel = 0,
                ModelTitle = "在线反馈",
                OptUserName = user?.UserName ?? entity.PersonName,
                ActionDesc = entity.CreateUserId == 0 ? "添加" : "处理",
                ActionRemark = (entity.CreateUserId == 0 ? "添加" : "处理") + "反馈，反馈编码为：" + res.Id,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = entity.CreateUserId
            });
            return res;
        }

        public async Task<Comment> UpdateCommentAsync(Comment entity)
        {
            var user = await bllUser.GetUserByIdAsync(entity.UpdateUserId ?? 0);
            var res = await dal.UpdateCommentAsync(entity);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "在线反馈",
                CreateTime = DateTime.Now,
                CreateUserId = entity.CreateUserId,
                IfDel = 0,
                ModelTitle = "在线反馈",
                OptUserName = user?.UserName ?? entity.PersonName,
                ActionDesc = "处理",
                ActionRemark = "处理反馈，反馈编码为：" + res.Id,
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = entity.CreateUserId
            });
            return res;
        }

        public async Task<PageList<CommentResDto>> GetCommentsByPageAsync(PageReq<CommentReqDto> req)
        {
            return await dal.GetCommentsByPageAsync(req);
        }
        public async Task<PageList<ContactMessage>> GetContactMessagesByPageAsync(PageReq<ContactMessageReqDto> req)
        {
            return await dal.GetContactMessagesByPageAsync(req);
        }

        public async Task<List<ContactMessage>> GetContactMessagesByAsync(ContactMessageReqDto req)
        {
            return await dal.GetContactMessagesByAsync(req);
        }

        public async Task DelCommentAsync(List<int> Ids, int CreateUserId = 0)
        {
            var user = await bllUser.GetUserByIdAsync(CreateUserId);
            await dal.DelCommentAsync(Ids);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "在线反馈",
                CreateTime = DateTime.Now,
                CreateUserId = CreateUserId,
                IfDel = 0,
                ModelTitle = "在线反馈",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = "删除",
                ActionRemark = "删除反馈，反馈编码为：" + string.Join("、", Ids),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = CreateUserId
            });
        }

        public async Task SetCommentShowAsync(CommentReqDto req, int CreateUserId = 0)
        {
            var user = await bllUser.GetUserByIdAsync(CreateUserId);
            await dal.SetCommentShowAsync(req);
            await dalLog.AddBizLogAsync(new BizLog()
            {
                ActionType = "在线反馈",
                CreateTime = DateTime.Now,
                CreateUserId = CreateUserId,
                IfDel = 0,
                ModelTitle = "在线反馈",
                OptUserName = user?.UserName ?? "系统",
                ActionDesc = req.isShow == 1 ? "显示" : "隐藏",
                ActionRemark = "设置可见反馈，反馈编码为：" + string.Join("、", req.ids ?? new List<int>()),
                ActionJson = "{}",
                UpdateTime = DateTime.Now,
                UpdateUserId = CreateUserId
            });
        }

        public async Task<CommentResDetailDto?> GetCommentDetailByIdAsync(int Id)
        {
            return await dal.GetCommentDetailByIdAsync(Id);
        }
    }
}