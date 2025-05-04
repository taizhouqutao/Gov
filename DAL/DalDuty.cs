using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DAL
{
    public class DalDuty
    {
        public async Task<List<DutyDetailDto>> GetDutyDetailsByAsync(DutyReqDto req)
        {
            List<DutyDetailDto> res=new List<DutyDetailDto>();
            using (var context = new webapplicationContext())
            {
                var Query = context.Dutys.AsQueryable();
                var Dutys = await Query.Where(i =>
                    (i.IfDel == 0) 
                ).ToListAsync();
                res=Dutys.ConvertAll(i=>new DutyDetailDto{
                   
                });
            }
            return res;
        }

    }
}