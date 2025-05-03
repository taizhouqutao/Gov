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

      public async Task<Contact?> GetContactByIdAsync(int Id)
      {
          return await dal.GetContactByIdAsync(Id);
      }     

      public async Task DelContactAsync(List<int> Ids)
      {
          await dal.DelContactAsync(Ids);
      }

      public async Task<Contact> AddContactAsync(Contact entity)
      {
          return await dal.AddContactAsync(entity);
      }

      public async Task<Contact> UpdateContactAsync(Contact entity)
      {
          return await dal.UpdateContactAsync(entity);
      }
    }
}