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
        public async Task<List<ContactPlusDto>> GetContactsByAsync(ContactReqDto req)
        {
            List<ContactPlusDto> res = new List<ContactPlusDto>();
            using (var context = new webapplicationContext())
            {
                var QueryContacts = context.Contacts.AsQueryable();
                var QueryCities = context.Cities.AsQueryable();
                res = await (from QueryContact in QueryContacts
                             join QueryCity in QueryCities on QueryContact.CityId equals QueryCity.Id into p_QueryCity
                             from QueryCity_Join in p_QueryCity.DefaultIfEmpty()
                             where
                                 (QueryContact.IfDel == 0) &&
                                 ((req.cityIds == null || req.cityIds.Count == 0) ? true : ((QueryContact.CityId ?? 0) == 0 || req.cityIds.Contains(QueryContact.CityId ?? 0))) &&
                                 (req.id == null ? true : req.id == QueryContact.Id) &&
                                 (req.ids == null ? true : req.ids.Contains(QueryContact.Id))
                             select new ContactPlusDto()
                             {
                                 Cellphone = QueryContact.Cellphone,
                                 CreateTime = QueryContact.CreateTime,
                                 CreateUserId = QueryContact.CreateUserId,
                                 IfDel = QueryContact.IfDel,
                                 PersonName = QueryContact.PersonName,
                                 Post = QueryContact.Post,
                                 CityId = QueryContact.CityId,
                                 Depent = QueryContact.Depent,
                                 PersonHead = QueryContact.PersonHead,
                                 Id = QueryContact.Id,
                                 UpdateTime = QueryContact.UpdateTime,
                                 Desc = QueryContact.Desc,
                                 UpdateUserId = QueryContact.UpdateUserId,
                                 CityName = ((QueryContact.CityId ?? 0) == 0) ? "全部" : QueryCity_Join.CityName ?? "",
                             }).ToListAsync();
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