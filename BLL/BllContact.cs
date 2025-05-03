using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllContact
    {
      private DalContact dal=new DalContact();
      public async Task<List<Contact>> GetContactsByAsync(ContactReqDto req)
      {
        return await dal.GetContactsByAsync(req);
      }
    }
}