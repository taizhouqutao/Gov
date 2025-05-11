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
  }
}