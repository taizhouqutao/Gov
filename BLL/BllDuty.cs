using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllDuty
    {
        private DalDuty dal=new DalDuty();
        public async Task<List<DutyDetailDto>> GetDutyDetailsByAsync(DutyReqDto req)
        {
            return await dal.GetDutyDetailsByAsync(req);
        }
    }
}