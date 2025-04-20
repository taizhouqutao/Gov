using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllNew
    {
        private DalNew dal=new DalNew();
        public async Task<PageList<New>> GetNewsByPageAsync(PageReq<NewReqDto> req) {
            return await dal.GetNewsByPageAsync(req);
        }

        public async Task<List<New>> GetNewsByIdAsync(List<int> Ids)
        {
            return await dal.GetNewsByIdAsync(Ids);
        }

        public async Task<New> GetNewByIdAsync(int Id)
        {
            return await dal.GetNewByIdAsync(Id);
        }

        public async Task<New> AddNewAsync(New entity)
        {
            return await dal.AddNewAsync(entity);
        }

        public async Task<New> UpdateNewAsync(New entity)
        {
            return await dal.UpdateNewAsync(entity);
        }

        public async Task UpdateNewsAsync(List<New> entitys)
        {
            await dal.UpdateNewsAsync(entitys);
        }

        public async Task DelNewAsync(List<int> Ids)
        {
            await dal.DelNewAsync(Ids);
        }
    }
}