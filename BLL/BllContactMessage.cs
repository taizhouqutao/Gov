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
  }
}