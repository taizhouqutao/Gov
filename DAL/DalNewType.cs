using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DAL
{
    public class DalNewType
    {
        public async Task<NewType> GetNewTypeByIdAsync(int Id)
        {
            NewType res = null;
            int total = 0, allcount = 0;
            using (var context = new webapplicationContext())
            {
                var Query = context.NewTypes.AsQueryable();
                res = await Query.FirstOrDefaultAsync(i =>
                    (i.IfDel == 0) &&
                    (i.Id==Id)
                );
            }
            return res;
        }

    }
}