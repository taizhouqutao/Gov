using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Reflection;
using System.Text.Json.Serialization;
namespace DAL
{
    public class DalComment{
        public async Task<PageList<CommentResDto>> GetCommentsByPageAsync(PageReq<CommentReqDto> req) {
            var res=new List<CommentResDto>();
            int total=0,allcount=0;
            using (var context = new webapplicationContext())
            {
                var QuerComments= context.Comments.AsQueryable();
                var QueryNews= context.News.AsQueryable();
                var QureyRes = (from QuerComment in QuerComments
                join QueryNew in QueryNews on QuerComment.NewId equals QueryNew.Id into p_QueryNew
                from QueryNew_Join in p_QueryNew.DefaultIfEmpty()
                where 
                    (QuerComment.IfDel==0) &&
                    ((req==null||req.Query==null||req.Query.newTypeId==null)?true:QueryNew_Join.NewTypeId==req.Query.newTypeId) &&
                    ((req.search==null||string.IsNullOrEmpty(req.search.value))?true:(
                        QuerComment.Content.Contains(req.search.value)||
                        (!string.IsNullOrEmpty(QuerComment.PersonCellphone) && QuerComment.PersonCellphone.Contains(req.search.value))||
                        (!string.IsNullOrEmpty(QuerComment.PersonName) && QuerComment.PersonName.Contains(req.search.value)) ||
                        (!string.IsNullOrEmpty(QueryNew_Join.NewTitle) && QueryNew_Join.NewTitle.Contains(req.search.value)) ||
                        (!string.IsNullOrEmpty(QuerComment.Content) && QuerComment.Content.Contains(req.search.value)) 
                    ))
                select new CommentResDto {
                    Id=QuerComment.Id,
                    Content=QuerComment.Content,
                    CreateTime=QuerComment.CreateTime,
                    CreateUserId=QuerComment.CreateUserId,
                    FatherCommentId=QuerComment.FatherCommentId,
                    IfDeal=QuerComment.IfDeal,
                    IsShow=QuerComment.IsShow,
                    NewId=QuerComment.NewId,
                    NewTitle=QueryNew_Join.NewTitle,
                    PersonCellphone=QuerComment.PersonCellphone,
                    PersonName=QuerComment.PersonName,
                    RoleType=QuerComment.RoleType,
                    UpdateTime=QuerComment.UpdateTime,
                    UpdateUserId=QuerComment.UpdateUserId,
                    UserId=QuerComment.UserId
                }).OrderByDescending(j=>j.Id);
                res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                total= await QureyRes.CountAsync();
                allcount=await context.Comments.CountAsync(i=>i.IfDel==0);
            }
            return new PageList<CommentResDto>(){
                data=res,
                recordsFiltered=total,
                draw=req.draw,
                recordsTotal=allcount
            };
        }
        public async Task<List<Comment>> GetCommentsByAsync(CommentReqDto req)
        {
            var res=new List<Comment>();
            using (var context = new webapplicationContext())
            {
                var Query= context.Comments.AsQueryable();
                var QureyRes = Query.Where(i=>
                    (i.IfDel==0) &&
                    ((req==null||req.newId==null)?true:i.NewId==req.newId)
                );
                QureyRes = QureyRes.OrderBy(j =>j.Id);
                res = await QureyRes.ToListAsync();
            }
            return res;
        }

        public async Task<Comment> AddCommentAsync(Comment entity)
        {
            Comment res = null;
            using (var context = new webapplicationContext())
            {
                res = (await context.Comments.AddAsync(entity)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }

        public async Task DelCommentAsync(List<int> Ids)
        {
            using (var context = new webapplicationContext())
            {
                var res =await context.Comments.Where(i=>Ids.Contains(i.Id)).ToListAsync();
                res.ForEach(i=>{
                  i.IfDel=1;
                });
                context.Comments.UpdateRange(res);
                await context.SaveChangesAsync();
            }
        }

        public async Task SetCommentShowAsync(CommentReqDto req)
        {
            using (var context = new webapplicationContext())
            {
                var res =await context.Comments.Where(i=>req.ids.Contains(i.Id)).ToListAsync();
                res.ForEach(i=>{
                  i.IsShow=Convert.ToInt32(req.isShow);
                });
                context.Comments.UpdateRange(res);
                await context.SaveChangesAsync();
            }
        }

        public async Task<CommentResDetailDto?> GetCommentByIdAsync(int Id)
        {
            CommentResDetailDto? res = null;
            using (var context = new webapplicationContext())
            {
                var QuerComments= context.Comments.AsQueryable();
                var QueryNews= context.News.AsQueryable();
                res = await (from QuerComment in QuerComments
                join QueryNew in QueryNews on QuerComment.NewId equals QueryNew.Id into p_QueryNew
                from QueryNew_Join in p_QueryNew.DefaultIfEmpty()
                where 
                    (QuerComment.Id==Id)
                select new CommentResDetailDto {
                    Id=QuerComment.Id,
                    Content=QuerComment.Content,
                    CreateTime=QuerComment.CreateTime,
                    CreateUserId=QuerComment.CreateUserId,
                    FatherCommentId=QuerComment.FatherCommentId,
                    IfDeal=QuerComment.IfDeal,
                    IsShow=QuerComment.IsShow,
                    NewId=QuerComment.NewId,
                    NewTitle=QueryNew_Join.NewTitle,
                    PersonCellphone=QuerComment.PersonCellphone,
                    PersonName=QuerComment.PersonName,
                    RoleType=QuerComment.RoleType,
                    UpdateTime=QuerComment.UpdateTime,
                    UpdateUserId=QuerComment.UpdateUserId,
                    UserId=QuerComment.UserId
                }).FirstOrDefaultAsync();
            }
            return res;
        }
    }
}