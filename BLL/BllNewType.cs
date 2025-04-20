using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
    public class BllNewType
    {
        private DalNewType dal=new DalNewType();

        public async Task<NewType> GetNewTypeByIdAsync(int Id)
        {
            return await dal.GetNewTypeByIdAsync(Id);
        }
    }
}