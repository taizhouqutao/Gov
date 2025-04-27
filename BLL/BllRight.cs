using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllRight
    {
        private DalRight dal=new DalRight();

        public async Task<List<RightTreeDto>> GetRightTreeAsync(RightReqDto req) {
            return await dal.GetRightTreeAsync(req);
        }

    }
}