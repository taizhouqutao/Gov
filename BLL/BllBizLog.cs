using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllBizLog{
        private DalBizLog dal=new DalBizLog();
        public async Task<PageList<BizLog>> GetBizLogsByPageAsync(PageReq<BizLogReqDto> req) {
            return await dal.GetBizLogsByPageAsync(req);
        }

        public async Task<BizLog?> GetBizLogByIdAsync(int Id)
        {
            return await dal.GetBizLogByIdAsync(Id);
        }
    }
}