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
    public async Task<PageList<ContactMessage>> GetContactMessagesByPageAsync(PageReq<ContactMessageReqDto> req)
    {
      var res = new List<ContactMessage>();
      int total = 0, allcount = 0;
      using (var context = new webapplicationContext())
      {
        var Query = context.ContactMessages.AsQueryable();
        var QureyRes = Query.Where(i =>
            (i.IfDel == 0) &&
            ((req.Query == null || req.Query.contactId == null) ? true : req.Query.contactId == i.ContactId) &&
            ((req.Query == null || req.Query.fatherContactMessageId == null) ? true : req.Query.fatherContactMessageId == i.FatherContactMessageId)
        );
        QureyRes = QureyRes.OrderByDescending(j => j.Id);
        res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
        total = await QureyRes.CountAsync();
        allcount = await context.ContactMessages.CountAsync(i => i.IfDel == 0 && ((req.Query == null || req.Query.contactId == null) ? true : req.Query.contactId == i.ContactId));
      }
      return new PageList<ContactMessage>()
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