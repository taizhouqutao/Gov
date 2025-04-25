using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DAL
{
    public class DalComment{
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
    }
}