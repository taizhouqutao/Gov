using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllComment
    {
        private DalComment dal=new DalComment();
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
            return await dal.AddCommentAsync(entity);
        }

        public async Task<Comment> UpdateCommentAsync(Comment entity)
        {
            return await dal.UpdateCommentAsync(entity);
        }

        public async Task<PageList<CommentResDto>> GetCommentsByPageAsync(PageReq<CommentReqDto> req) {
            return await dal.GetCommentsByPageAsync(req);
        }

        public async Task DelCommentAsync(List<int> Ids)
        {
            await dal.DelCommentAsync(Ids);
        }

        public async Task SetCommentShowAsync(CommentReqDto req)
        {
            await dal.SetCommentShowAsync(req);
        }

        public async Task<CommentResDetailDto?> GetCommentDetailByIdAsync(int Id)
        {
            return await dal.GetCommentDetailByIdAsync(Id);
        }
    }
}