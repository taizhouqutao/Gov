using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DAL
{
  public class DalContactMessage
  {
    public async Task<List<CommentGroupResDto>> GetContactMessageGroupsByAsync(ContactMessageReqDto req)
    {
      var res = new List<CommentGroupResDto>();
      using (var context = new webapplicationContext())
      {
        var QueryContactMessages = context.ContactMessages.AsQueryable();
        var QureyRes = (from QueryContactMessage in QueryContactMessages
                        where
                            (QueryContactMessage.IfDel == 0) &&
                            ((req == null || req.fatherContactMessageId == null) ? true : req.fatherContactMessageId == QueryContactMessage.FatherContactMessageId) &&
                            ((req == null || req.ifDeal == null) ? true : QueryContactMessage.IfDeal == req.ifDeal)
                        select new
                        {
                          ContactMessageId = QueryContactMessage.Id,
                          CreateTime = QueryContactMessage.CreateTime
                        });
        var Count = await QureyRes.CountAsync();
        if (Count > 0)
        {
          var createTime = (await QureyRes.MaxAsync(i => i.CreateTime));
          res.Add(new CommentGroupResDto()
          {
            Link = "Contact/Message",
            newTypeName = "群众留言",
            tabName = "群众留言管理",
            newTypeId = 0,
            count = Count,
            createTime = createTime.ToString("yyyy-MM-dd HH:mm"),
          });
        }
      }
      return res;
    }

    public async Task<PageList<ContactMessageResDto>> GetContactMessagesByPageAsync(PageReq<ContactMessageReqDto> req)
    {
      var res = new List<ContactMessageResDto>();
      int total = 0, allcount = 0;
      using (var context = new webapplicationContext())
      {
        var QueryContactMessages = context.ContactMessages.AsQueryable();
        var QueryCitys = context.Cities.AsQueryable();
        var QueryContacts = context.Contacts.AsQueryable();
        var QureyRes = (from QueryContactMessage in QueryContactMessages
                        join QueryContact in QueryContacts on QueryContactMessage.ContactId equals QueryContact.Id into p_QueryContact
                        from QueryContact_Join in p_QueryContact.DefaultIfEmpty()
                        join QueryCity in QueryCitys on QueryContact_Join.CityId equals QueryCity.Id into p_QueryCity
                        from QueryCity_Join in p_QueryCity.DefaultIfEmpty()
                        orderby QueryContactMessage.IfDeal ascending, QueryContactMessage.Id descending
                        where
                           (QueryContactMessage.IfDel == 0) &&
                            ((req.Query == null || req.Query.contactId == null) ? true : req.Query.contactId == QueryContactMessage.ContactId) &&
                            ((req.Query == null || req.Query.cityIds == null) ? true : ((QueryContact_Join.CityId ?? 0) == 0 || req.Query.cityIds.Contains(QueryContact_Join.CityId ?? 0))) &&
                            ((req.Query == null || req.Query.fatherContactMessageId == null) ? true : req.Query.fatherContactMessageId == QueryContactMessage.FatherContactMessageId) &&
                            ((req.Query == null || req.Query.isShow == null) ? true : req.Query.isShow == QueryContactMessage.IsShow)
                        select new ContactMessageResDto()
                        {
                          ContactId = QueryContactMessage.ContactId,
                          ContactName = QueryContact_Join.PersonName ?? "",
                          Content = QueryContactMessage.Content,
                          CreateTime = QueryContactMessage.CreateTime,
                          CityName = ((QueryContact_Join.CityId ?? 0) == 0) ? "全部" : QueryCity_Join.CityName ?? "",
                          CreateUserId = QueryContactMessage.CreateUserId,
                          FatherContactMessageId = QueryContactMessage.FatherContactMessageId,
                          IfDeal = QueryContactMessage.IfDeal,
                          PersonName = QueryContactMessage.PersonName,
                          RoleType = QueryContactMessage.RoleType,
                          Id = QueryContactMessage.Id,
                          IsShow = QueryContactMessage.IsShow,
                          PersonCellphone = QueryContactMessage.PersonCellphone,
                          UpdateTime = QueryContactMessage.UpdateTime,
                          UpdateUserId = QueryContactMessage.UpdateUserId,
                          UserId = QueryContactMessage.UserId
                        });
        res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
        total = await QureyRes.CountAsync();
        var QureyAll = (from QueryContactMessage in QueryContactMessages
                        join QueryContact in QueryContacts on QueryContactMessage.ContactId equals QueryContact.Id into p_QueryContact
                        from QueryContact_Join in p_QueryContact.DefaultIfEmpty()
                        join QueryCity in QueryCitys on QueryContact_Join.CityId equals QueryCity.Id into p_QueryCity
                        from QueryCity_Join in p_QueryCity.DefaultIfEmpty()
                        orderby QueryContactMessage.IfDeal ascending, QueryContactMessage.Id descending
                        where
                           (QueryContactMessage.IfDel == 0 &&
                          ((req.Query == null || req.Query.contactId == null) ? true : req.Query.contactId == QueryContactMessage.ContactId) &&
                          ((req.Query == null || req.Query.cityIds == null) ? true : ((QueryContact_Join.CityId ?? 0) == 0 || req.Query.cityIds.Contains(QueryContact_Join.CityId ?? 0))) &&
                          ((req.Query == null || req.Query.fatherContactMessageId == null) ? true : req.Query.fatherContactMessageId == QueryContactMessage.FatherContactMessageId)
                          )
                        select new
                        {
                          Id = QueryContactMessage.Id,
                        });
        allcount = await QureyAll.CountAsync();
      }
      return new PageList<ContactMessageResDto>()
      {
        data = res,
        recordsFiltered = total,
        draw = req.draw,
        recordsTotal = allcount
      };
    }

    public async Task DelMessagesAsync(List<int> Ids)
    {
      using (var context = new webapplicationContext())
      {
        var res = await context.ContactMessages.Where(i => Ids.Contains(i.Id)).ToListAsync();
        res.ForEach(i =>
        {
          i.IfDel = 1;
        });
        context.ContactMessages.UpdateRange(res);
        await context.SaveChangesAsync();
      }
    }

    public async Task SetMessageShow(ContactMessageReqDto req)
    {
      using (var context = new webapplicationContext())
      {
        var res = await context.ContactMessages.Where(i => req.ids.Contains(i.Id)).ToListAsync();
        res.ForEach(i =>
        {
          i.IsShow = Convert.ToInt32(req.isShow);
        });
        context.ContactMessages.UpdateRange(res);
        await context.SaveChangesAsync();
      }
    }

    public async Task<List<ContactMessage>> GetContactMessagesByAsync(ContactMessageReqDto req)
    {
      List<ContactMessage> res = new List<ContactMessage>();
      using (var context = new webapplicationContext())
      {
        var Query = context.ContactMessages.AsQueryable();
        res = await Query.Where(i =>
          (i.IfDel == 0) &&
          ((req == null || req.fatherContactMessageId == null) ? true : req.fatherContactMessageId == i.FatherContactMessageId) &&
          ((req == null || req.fatherContactMessageIds == null) ? true : req.fatherContactMessageIds.Contains(i.FatherContactMessageId))
        ).ToListAsync();
      }
      return res;
    }

    public async Task<ContactMessage> AddContactMessageAsync(ContactMessage entity)
    {
      ContactMessage res = null;
      using (var context = new webapplicationContext())
      {
        res = (await context.ContactMessages.AddAsync(entity)).Entity;
        await context.SaveChangesAsync();
      }
      return res;
    }

    public async Task<ContactMessage?> GetContactMessageByIdAsync(int Id)
    {
      ContactMessage? res = null;
      using (var context = new webapplicationContext())
      {
        var Query = context.ContactMessages.AsQueryable();
        res = await Query.FirstOrDefaultAsync(i =>
            (i.IfDel == 0) &&
            (i.Id == Id)
        );
      }
      return res;
    }

    public async Task<CommentResDetailDto?> GetContactMessageDetailByIdAsync(int Id)
    {
      CommentResDetailDto? res = null;
      using (var context = new webapplicationContext())
      {
        var Query = context.ContactMessages.AsQueryable();
        var result = await Query.FirstOrDefaultAsync(i =>
            (i.IfDel == 0) &&
            (i.Id == Id)
        );
        if (result != null)
        {
          res = new CommentResDetailDto()
          {
            Content = result.Content,
            CreateTime = result.CreateTime,
            CreateUserId = result.CreateUserId,
            FatherCommentId = result.FatherContactMessageId,
            Id = result.Id,
            IfDeal = result.IfDeal,
            IsShow = result.IsShow,
            PersonCellphone = result.PersonCellphone,
            PersonName = result.PersonName,
            NewId = result.ContactId,
            UpdateTime = result.UpdateTime,
            UpdateUserId = result.UpdateUserId,
            UserId = result.UserId,
            RoleType = result.RoleType,
          };
        }
      }
      return res;
    }

    public async Task<ContactMessage> UpdateContactMessageAsync(ContactMessage entity)
    {
      ContactMessage res = null;
      using (var context = new webapplicationContext())
      {
        res = (context.ContactMessages.Update(entity)).Entity;
        await context.SaveChangesAsync();
      }
      return res;
    }
  }
}