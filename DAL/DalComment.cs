using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
namespace DAL
{
    public class DalComment
    {
        public async Task<Comment?> GetCommentByIdAsync(int Id)
        {
            Comment? res = null;
            using (var context = new webapplicationContext())
            {
                var Query = context.Comments.AsQueryable();
                res = await Query.FirstOrDefaultAsync(i =>
                    (i.IfDel == 0) &&
                    (i.Id == Id)
                );
            }
            return res;
        }


        public async Task<List<ContactMessage>> GetContactMessagesByAsync(ContactMessageReqDto req)
        {
            List<ContactMessage> res = new List<ContactMessage>();
            using (var context = new webapplicationContext())
            {
                var Query = context.Comments.AsQueryable();
                var temps = await Query.Where(i =>
                  (i.IfDel == 0) &&
                  ((req == null || req.fatherContactMessageId == null) ? true : req.fatherContactMessageId == i.FatherCommentId) &&
                  ((req == null || req.fatherContactMessageIds == null) ? true : req.fatherContactMessageIds.Contains(i.FatherCommentId))
                ).ToListAsync();
                res = temps.ConvertAll(i => new ContactMessage
                {
                    ContactId = i.NewId,
                    Content = i.Content,
                    CreateTime = i.CreateTime,
                    CreateUserId = i.CreateUserId,
                    FatherContactMessageId = i.FatherCommentId,
                    Id = i.Id,
                    IfDeal = i.IfDeal,
                    IfDel = i.IfDel,
                    IsShow = i.IsShow,
                    PersonCellphone = i.PersonCellphone,
                    PersonName = i.PersonName,
                    RoleType = i.RoleType,
                    UpdateUserId = i.UpdateUserId,
                    UpdateTime = i.UpdateTime,
                    UserId = i.UserId
                });
            }
            return res;
        }

        public async Task<PageList<ContactMessage>> GetContactMessagesByPageAsync(PageReq<ContactMessageReqDto> req)
        {
            var res = new List<ContactMessage>();
            int total = 0, allcount = 0;
            using (var context = new webapplicationContext())
            {
                var Query = context.Comments.AsQueryable();
                var QureyRes = Query.Where(i =>
                    (i.IfDel == 0) &&
                    ((req.Query == null || req.Query.contactId == null) ? true : req.Query.contactId == i.NewId) &&
                    ((req.Query == null || req.Query.fatherContactMessageId == null) ? true : req.Query.fatherContactMessageId == i.FatherCommentId) &&
                    ((req.Query == null || req.Query.isShow == null) ? true : req.Query.isShow == i.IsShow)
                );
                QureyRes = QureyRes.OrderByDescending(j => j.Id);
                var temp = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                res = temp.ConvertAll(i => new ContactMessage
                {
                    ContactId = i.NewId,
                    Content = i.Content,
                    CreateTime = i.CreateTime,
                    CreateUserId = i.CreateUserId,
                    FatherContactMessageId = i.FatherCommentId,
                    Id = i.Id,
                    IfDeal = i.IfDeal,
                    IfDel = i.IfDel,
                    IsShow = i.IsShow,
                    PersonCellphone = i.PersonCellphone,
                    PersonName = i.PersonName,
                    RoleType = i.RoleType,
                    UpdateUserId = i.UpdateUserId,
                    UpdateTime = i.UpdateTime,
                    UserId = i.UserId
                });
                total = await QureyRes.CountAsync();
                allcount = await context.Comments.CountAsync(i => i.IfDel == 0 &&
                ((req.Query == null || req.Query.contactId == null) ? true : req.Query.contactId == i.NewId) &&
                ((req.Query == null || req.Query.fatherContactMessageId == null) ? true : req.Query.fatherContactMessageId == i.FatherCommentId)
                );
            }
            return new PageList<ContactMessage>()
            {
                data = res,
                recordsFiltered = total,
                draw = req.draw,
                recordsTotal = allcount
            };
        }
        public async Task<PageList<CommentResDto>> GetCommentsByPageAsync(PageReq<CommentReqDto> req)
        {
            var res = new List<CommentResDto>();
            int total = 0, allcount = 0;
            if (req != null && req.Query != null && req.Query.newTypeId != null && req.Query.newTypeId == 0)
            {
                using (var context = new webapplicationContext())
                {
                    var QuerComments = context.Comments.AsQueryable();
                    var QueryCitys = context.Cities.AsQueryable();
                    var QureyRes = (from QuerComment in QuerComments
                                    join QueryCity in QueryCitys on QuerComment.CityId equals QueryCity.Id into p_QueryCity
                                    from QueryCity_Join in p_QueryCity.DefaultIfEmpty()
                                    where
                                        (QuerComment.IfDel == 0) &&
                                        (QuerComment.NewId == 0) &&
                                        ((req == null || req.Query == null || req.Query.fatherCommentId == null) ? true : QuerComment.FatherCommentId == req.Query.fatherCommentId) &&
                                        ((req == null || req.Query == null || req.Query.cityIds == null) ? true : ((QuerComment.CityId ?? 0) == 0 || req.Query.cityIds.Contains(QuerComment.CityId ?? 0))) &&
                                        ((req == null || req.search == null || string.IsNullOrEmpty(req.search.value)) ? true : (
                                            QuerComment.Content.Contains(req.search.value) ||
                                            (!string.IsNullOrEmpty(QuerComment.PersonCellphone) && QuerComment.PersonCellphone.Contains(req.search.value)) ||
                                            (!string.IsNullOrEmpty(QuerComment.PersonName) && QuerComment.PersonName.Contains(req.search.value)) ||
                                            (!string.IsNullOrEmpty(QuerComment.Content) && QuerComment.Content.Contains(req.search.value))
                                        ))
                                    select new CommentResDto
                                    {
                                        Id = QuerComment.Id,
                                        Content = QuerComment.Content,
                                        CreateTime = QuerComment.CreateTime,
                                        CreateUserId = QuerComment.CreateUserId,
                                        FatherCommentId = QuerComment.FatherCommentId,
                                        IfDeal = QuerComment.IfDeal,
                                        IsShow = QuerComment.IsShow,
                                        CityName = ((QuerComment.CityId ?? 0) == 0) ? "全部" : QueryCity_Join.CityName??"",
                                        NewId = QuerComment.NewId,
                                        PersonCellphone = QuerComment.PersonCellphone,
                                        PersonName = QuerComment.PersonName,
                                        RoleType = QuerComment.RoleType,
                                        UpdateTime = QuerComment.UpdateTime,
                                        UpdateUserId = QuerComment.UpdateUserId,
                                        UserId = QuerComment.UserId
                                    }).OrderBy(j => j.IfDeal).ThenByDescending(j => j.Id);
                    res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                    total = await QureyRes.CountAsync();
                    allcount = await context.Comments.CountAsync(i => i.IfDel == 0 &&
                    i.NewId == 0 &&
                    ((req == null || req.Query == null || req.Query.fatherCommentId == null) ? true : i.FatherCommentId == req.Query.fatherCommentId) &&
                    ((req == null || req.Query == null || req.Query.cityIds == null) ? true : ((i.CityId ?? 0) == 0 || req.Query.cityIds.Contains(i.CityId ?? 0))) 
                    );
                }
            }
            else
            {
                using (var context = new webapplicationContext())
                {
                    var QuerComments = context.Comments.AsQueryable();
                    var QueryNews = context.News.AsQueryable();
                    var QueryCitys = context.Cities.AsQueryable();
                    var QureyRes = (from QuerComment in QuerComments
                                    join QueryNew in QueryNews on QuerComment.NewId equals QueryNew.Id into p_QueryNew
                                    from QueryNew_Join in p_QueryNew.DefaultIfEmpty()
                                    join QueryCity in QueryCitys on QueryNew_Join.CityId equals QueryCity.Id into p_QueryCity
                                    from QueryCity_Join in p_QueryCity.DefaultIfEmpty()
                                    where
                                        (QuerComment.IfDel == 0) &&
                                        ((req == null || req.Query == null || req.Query.newTypeId == null) ? true : QueryNew_Join.NewTypeId == req.Query.newTypeId) &&
                                        ((req == null || req.Query == null || req.Query.fatherCommentId == null) ? true : QuerComment.FatherCommentId == req.Query.fatherCommentId) &&
                                        ((req == null || req.Query == null || req.Query.cityIds == null) ? true : ((QueryNew_Join.CityId ?? 0) == 0 || req.Query.cityIds.Contains(QueryNew_Join.CityId ?? 0))) &&
                                        ((req == null || req.search == null || string.IsNullOrEmpty(req.search.value)) ? true : (
                                            QuerComment.Content.Contains(req.search.value) ||
                                            (!string.IsNullOrEmpty(QuerComment.PersonCellphone) && QuerComment.PersonCellphone.Contains(req.search.value)) ||
                                            (!string.IsNullOrEmpty(QuerComment.PersonName) && QuerComment.PersonName.Contains(req.search.value)) ||
                                            (!string.IsNullOrEmpty(QueryNew_Join.NewTitle) && QueryNew_Join.NewTitle.Contains(req.search.value)) ||
                                            (!string.IsNullOrEmpty(QuerComment.Content) && QuerComment.Content.Contains(req.search.value))
                                        ))
                                    select new CommentResDto
                                    {
                                        Id = QuerComment.Id,
                                        Content = QuerComment.Content,
                                        CreateTime = QuerComment.CreateTime,
                                        CreateUserId = QuerComment.CreateUserId,
                                        FatherCommentId = QuerComment.FatherCommentId,
                                        IfDeal = QuerComment.IfDeal,
                                        IsShow = QuerComment.IsShow,
                                        NewId = QuerComment.NewId,
                                        CityName = ((QueryNew_Join.CityId ?? 0) == 0) ? "全部" : QueryCity_Join.CityName??"",
                                        NewTitle = QueryNew_Join.NewTitle,
                                        PersonCellphone = QuerComment.PersonCellphone,
                                        PersonName = QuerComment.PersonName,
                                        RoleType = QuerComment.RoleType,
                                        UpdateTime = QuerComment.UpdateTime,
                                        UpdateUserId = QuerComment.UpdateUserId,
                                        UserId = QuerComment.UserId
                                    }).OrderBy(j => j.IfDeal).ThenByDescending(j => j.Id);
                    res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                    total = await QureyRes.CountAsync();

                    int AllQureyRes = await ((from QuerComment in QuerComments
                                              join QueryNew in QueryNews on QuerComment.NewId equals QueryNew.Id into p_QueryNew
                                              from QueryNew_Join in p_QueryNew.DefaultIfEmpty()
                                              where
                                               (QuerComment.IfDel == 0) &&
                                               ((req == null || req.Query == null || req.Query.newTypeId == null) ? true : QueryNew_Join.NewTypeId == req.Query.newTypeId) &&
                                               ((req == null || req.Query == null || req.Query.fatherCommentId == null) ? true : QuerComment.FatherCommentId == req.Query.fatherCommentId) &&
                                               ((req == null || req.Query == null || req.Query.cityIds == null) ? true : ((QueryNew_Join.CityId ?? 0) == 0 || req.Query.cityIds.Contains(QueryNew_Join.CityId ?? 0)))
                                              select new CommentResDto { Id = QuerComment.Id }
                                            ).CountAsync());
                    allcount = AllQureyRes;
                }
            }
            return new PageList<CommentResDto>()
            {
                data = res,
                recordsFiltered = total,
                draw = req.draw,
                recordsTotal = allcount
            };
        }

        public async Task<List<CommentGroupResDto>> GetCommentGroupsByAsync(CommentReqDto req)
        {
            var res = new List<CommentGroupResDto>();
            using (var context = new webapplicationContext())
            {
                var QueryComments = context.Comments.AsQueryable();
                var QueryNews = context.News.AsQueryable();
                var QueryNewTypes = context.NewTypes.AsQueryable();
                var QureyRes = (from QueryComment in QueryComments
                                join QueryNew in QueryNews on QueryComment.NewId equals QueryNew.Id into p_QueryNew
                                from QueryNew_Join in p_QueryNew.DefaultIfEmpty()
                                join QueryNewType in QueryNewTypes on QueryNew_Join.NewTypeId equals QueryNewType.Id into p_QueryNewType
                                from QueryNewType_Join in p_QueryNewType.DefaultIfEmpty()
                                where
                                    (QueryComment.IfDel == 0) &&
                                    ((req == null || req.fatherCommentId == null) ? true : QueryComment.FatherCommentId == req.fatherCommentId) &&
                                    ((req == null || req.newTypeId == null) ? true : QueryNew_Join.NewTypeId == req.newTypeId) &&
                                    ((req == null || req.cityIds == null) ? true : ((QueryNew_Join.CityId ?? 0) == 0 || req.cityIds.Contains(QueryNew_Join.CityId ?? 0))) &&
                                    ((req == null || req.newTypeIds == null) ? true : req.newTypeIds.Contains(QueryNew_Join.NewTypeId ?? 0)) &&
                                    ((req == null || req.ifDeal == null) ? true : QueryComment.IfDeal == req.ifDeal)
                                select new
                                {
                                    NewTypeId = QueryNew_Join.NewTypeId ?? 0,
                                    NewId = QueryComment.NewId,
                                    NewTypeName = QueryNewType_Join.NewTypeName,
                                    CreateTime = QueryComment.CreateTime
                                }).GroupBy(i => new
                                {
                                    NewTypeId = i.NewTypeId,
                                    NewTypeName = i.NewTypeName
                                }).Select(i => new
                                {
                                    NewTypeId = i.Key.NewTypeId,
                                    NewTypeName = i.Key.NewTypeName,
                                    NewId = i.Max(j => j.NewId),
                                    Count = i.Count(),
                                    CreateTime = i.Max(j => j.CreateTime)
                                }).OrderByDescending(i => i.CreateTime);
                var tempres = await QureyRes.ToListAsync();
                res = tempres.ConvertAll(i => new CommentGroupResDto()
                {
                    count = i.Count,
                    newTypeId = i.NewTypeId,
                    tabName = i.NewId == 0 ? "意见收集" : HtmlHelp.GetTabName(i.NewTypeId),
                    newTypeName = i.NewId == 0 ? "群众意见收集" : i.NewTypeName.Substring(0, i.NewTypeName.Length - 2),
                    Link = i.NewId == 0 ? "Suggest/Index" : $"New/Comment?NewTypeId={i.NewTypeId}",
                    createTime = i.CreateTime.ToString("yyyy-MM-dd HH:mm")
                });
            }
            return res;
        }



        public async Task<List<Comment>> GetCommentsByAsync(CommentReqDto req)
        {
            var res = new List<Comment>();
            using (var context = new webapplicationContext())
            {
                var Query = context.Comments.AsQueryable();
                var QureyRes = Query.Where(i =>
                    (i.IfDel == 0) &&
                    ((req == null || req.newId == null) ? true : i.NewId == req.newId) &&
                    ((req == null || req.fatherCommentId == null) ? true : i.FatherCommentId == req.fatherCommentId)
                );
                QureyRes = QureyRes.OrderBy(j => j.Id);
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

        public async Task<Comment> UpdateCommentAsync(Comment entity)
        {
            Comment res = null;
            using (var context = new webapplicationContext())
            {
                res = (context.Comments.Update(entity)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }

        public async Task DelCommentAsync(List<int> Ids)
        {
            using (var context = new webapplicationContext())
            {
                var res = await context.Comments.Where(i => Ids.Contains(i.Id)).ToListAsync();
                res.ForEach(i =>
                {
                    i.IfDel = 1;
                });
                context.Comments.UpdateRange(res);
                await context.SaveChangesAsync();
            }
        }

        public async Task SetCommentShowAsync(CommentReqDto req)
        {
            using (var context = new webapplicationContext())
            {
                var res = await context.Comments.Where(i => req.ids.Contains(i.Id)).ToListAsync();
                res.ForEach(i =>
                {
                    i.IsShow = Convert.ToInt32(req.isShow);
                });
                context.Comments.UpdateRange(res);
                await context.SaveChangesAsync();
            }
        }

        public async Task<CommentResDetailDto?> GetCommentDetailByIdAsync(int Id)
        {
            CommentResDetailDto? res = null;
            using (var context = new webapplicationContext())
            {
                var QuerComments = context.Comments.AsQueryable();
                var QueryNews = context.News.AsQueryable();
                res = await (from QuerComment in QuerComments
                             join QueryNew in QueryNews on QuerComment.NewId equals QueryNew.Id into p_QueryNew
                             from QueryNew_Join in p_QueryNew.DefaultIfEmpty()
                             where
                                 (QuerComment.Id == Id)
                             select new CommentResDetailDto
                             {
                                 Id = QuerComment.Id,
                                 Content = QuerComment.Content,
                                 CreateTime = QuerComment.CreateTime,
                                 CreateUserId = QuerComment.CreateUserId,
                                 FatherCommentId = QuerComment.FatherCommentId,
                                 IfDeal = QuerComment.IfDeal,
                                 IsShow = QuerComment.IsShow,
                                 NewId = QuerComment.NewId,
                                 NewTitle = QueryNew_Join.NewTitle,
                                 PersonCellphone = QuerComment.PersonCellphone,
                                 PersonName = QuerComment.PersonName,
                                 RoleType = QuerComment.RoleType,
                                 UpdateTime = QuerComment.UpdateTime,
                                 UpdateUserId = QuerComment.UpdateUserId,
                                 UserId = QuerComment.UserId
                             }).FirstOrDefaultAsync();
            }
            return res;
        }
    }
}