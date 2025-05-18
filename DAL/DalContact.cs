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
            List<Contact> res = new List<Contact>();
            using (var context = new webapplicationContext())
            {
                var Query = context.Contacts.AsQueryable();
                res = await Query.Where(i =>
                    (i.IfDel == 0) &&
                    (req.id == null ? true : req.id == i.Id) &&
                    (req.ids == null ? true : req.ids.Contains(i.Id))
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
                    (i.Id == Id)
                );
            }
            return res;
        }

        public async Task DelContactAsync(List<int> Ids)
        {
            using (var context = new webapplicationContext())
            {
                var res = await context.Contacts.Where(i => Ids.Contains(i.Id)).ToListAsync();
                res.ForEach(i =>
                {
                    i.IfDel = 1;
                });
                context.Contacts.UpdateRange(res);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Contact> AddContactAsync(Contact entity)
        {
            Contact res = null;
            using (var context = new webapplicationContext())
            {
                res = (await context.Contacts.AddAsync(entity)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }

        public async Task<Contact> UpdateContactAsync(Contact entity)
        {
            Contact res = null;
            using (var context = new webapplicationContext())
            {
                res = (context.Contacts.Update(entity)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }
    }
}