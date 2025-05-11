using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
  public class BllContactMessage
  {
    private DalContactMessage dal = new DalContactMessage();
    public async Task<PageList<ContactMessage>> GetContactMessagesByPageAsync(PageReq<ContactMessageReqDto> req)
    {
      return await dal.GetContactMessagesByPageAsync(req);
    }

    public async Task<List<ContactMessage>> GetContactMessagesByAsync(ContactMessageReqDto req)
    {
      return await dal.GetContactMessagesByAsync(req);
    }

    public async Task<ContactMessage> AddContactMessageAsync(ContactMessage entity)
    {
      return await dal.AddContactMessageAsync(entity);
    }
  }
}