using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Principal;
namespace DAL
{
    public class DalDuty
    {
        public async Task<List<DutyDetailDto>> GetDutyDetailsByAsync(DutyReqDto req)
        {
            List<DutyDetailDto> res=new List<DutyDetailDto>();
            using (var context = new webapplicationContext())
            {
                var QueryDutys = context.Dutys.AsQueryable();
                var QuerContacts= context.Contacts.AsQueryable();
                DateTime? StartDate=null;
                DateTime? EndDate=null;
                if(!string.IsNullOrEmpty(req.startDateStr))
                {
                    StartDate=Convert.ToDateTime(Convert.ToDateTime(req.startDateStr).ToString("yyyy-MM-dd"));
                }
                if(!string.IsNullOrEmpty(req.endDateStr))
                {
                    EndDate=Convert.ToDateTime(Convert.ToDateTime(req.endDateStr).ToString("yyyy-MM-dd"));
                }
                res =await (from QueryDuty in QueryDutys
                join QuerContact in QuerContacts on QueryDuty.ContactId equals QuerContact.Id into p_QuerContact
                from QuerContact_Join in p_QuerContact.DefaultIfEmpty()
                where 
                    (QueryDuty.IfDel==0) &&
                    ((req==null||req.startDate==null)?true: req.startDate<=QueryDuty.StartDate) &&
                    ((req==null||req.endDate==null)?true: req.endDate>=QueryDuty.EndDate) &&
                    ((StartDate==null)?true: StartDate<=QueryDuty.StartDate) &&
                    ((EndDate==null)?true: EndDate>=QueryDuty.EndDate) 
                select new DutyDetailDto{
                    allDay=QueryDuty.AllDay,
                    contactId=QueryDuty.ContactId,
                    endDate=QueryDuty.EndDate,
                    startDate=QueryDuty.StartDate,
                    id=QueryDuty.Id,
                    post=QuerContact_Join.Post,
                    personName=QuerContact_Join.PersonName??""
                }).OrderByDescending(j=>j.id).ToListAsync();
            }
            return res;
        }

        public async Task SaveDuty(DutyReqDto req)
        {
            using (var context = new webapplicationContext())
            {
                var QueryDutys = context.Dutys.AsQueryable();
                var StartDate=Convert.ToDateTime(Convert.ToDateTime(req.startDateStr).ToString("yyyy-MM-dd"));
                var EndDate=Convert.ToDateTime(Convert.ToDateTime(req.endDateStr).ToString("yyyy-MM-dd"));
                var TodayList = QueryDutys.Where(i=>i.IfDel==0 && i.StartDate==StartDate).ToList();
                var NeedDelList = TodayList.Where(i=>!req.contactIds.Contains(i.ContactId)).ToList();
                if(NeedDelList.Count>0)
                {
                    NeedDelList.ForEach(i=>{
                        i.IfDel=1;
                    });
                    context.Dutys.UpdateRange(NeedDelList);
                }
                var NeedAddList = req.contactIds.Where(i=>!TodayList.ConvertAll(j=>j.ContactId).Contains(i)).ToList().ConvertAll(i=>new Duty(){
                    AllDay=(bool) req.allDay,
                    ContactId=i,
                    CreateTime=DateTime.Now,
                    CreateUserId=1,
                    EndDate=EndDate,
                    StartDate=StartDate,
                    IfDel=0,
                });
                if(NeedAddList.Count>0)
                {
                    context.Dutys.AddRange(NeedAddList);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}