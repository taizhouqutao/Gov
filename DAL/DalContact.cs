using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DAL
{
    public class DalContact
    {
        public async Task<List<Contact>> GetContactsByAsync(ContactReqDto req)
        {
            List<Contact> res=new List<Contact>();
            using (var context = new webapplicationContext())
            {
                var Query = context.Contacts.AsQueryable();
                res = await Query.Where(i =>
                    (i.IfDel == 0) 
                ).ToListAsync();
            }
            return res;
        }

        public async Task<Contact?> GetContactByIdAsync(int Id)
        {
            Contact? res = null;
            using (var context = new webapplicationContext())
            {
                var Query = context.Contacts.AsQueryable();
                res = await Query.FirstOrDefaultAsync(i =>
                    (i.IfDel == 0) &&
                    (i.Id==Id)
                );
            }
            return res;
        }   
    }
}