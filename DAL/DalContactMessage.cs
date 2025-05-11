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

    public async Task<List<ContactMessage>> GetContactMessagesByAsync(ContactMessageReqDto req)
    {
      List<ContactMessage> res = new List<ContactMessage>();
      using (var context = new webapplicationContext())
      {
        var Query = context.ContactMessages.AsQueryable();
        res = await Query.Where(i =>
          (i.IfDel == 0) &&
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
  }
}