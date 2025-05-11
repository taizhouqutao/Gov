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

    public async Task<ContactMessage?> GetContactMessageByIdAsync(int Id)
    {
      return await dal.GetContactMessageByIdAsync(Id);
    }

    public async Task<CommentResDetailDto?> GetContactMessageDetailByIdAsync(int Id)
    {
      return await dal.GetContactMessageDetailByIdAsync(Id);
    }

    public async Task DelMessagesAsync(List<int> Ids)
    {
      await dal.DelMessagesAsync(Ids);
    }

    public async Task SetMessageShow(ContactMessageReqDto req)
    {
      await dal.SetMessageShow(req);
    }

    public async Task<ContactMessage> UpdateContactMessageAsync(ContactMessage entity)
    {
      return await dal.UpdateContactMessageAsync(entity);
    }
  }
}