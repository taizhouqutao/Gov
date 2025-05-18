using Common;
using DAL;
using DAL.Modles;
namespace BLL
{
  public class BllCarousel
  {
    private DalCarousel dal = new DalCarousel();
    public async Task<PageList<Carousel>> GetCarouselsByPageAsync(PageReq<CarouselReqDto> req)
    {
      return await dal.GetCarouselsByPageAsync(req);
    }

    public async Task<List<Carousel>> GetCarouselsByIdAsync(List<int> Ids)
    {
      return await dal.GetCarouselsByIdAsync(Ids);
    }

    public async Task<Carousel?> GetCarouselByIdAsync(int Id)
    {
      return await dal.GetCarouselByIdAsync(Id);
    }

    public async Task<Carousel> AddCarouselAsync(Carousel entity)
    {
      return await dal.AddCarouselAsync(entity);
    }

    public async Task<Carousel> UpdateCarouselAsync(Carousel entity)
    {
      return await dal.UpdateCarouselAsync(entity);
    }

    public async Task UpdateCarouselsAsync(List<Carousel> entitys)
    {
      await dal.UpdateCarouselsAsync(entitys);
    }

    public async Task DelCarouselAsync(List<int> Ids)
    {
      await dal.DelCarouselAsync(Ids);
    }
  }
}