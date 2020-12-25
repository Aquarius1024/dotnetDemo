using FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes(string keyword, string operatorType, int ratingValue);
        TouristRoute GetTouristRoute(Guid touristRouteId);
        bool TouristRouteExists(Guid touristRouteId);
        IEnumerable<TouristRoutePicture> GetPictureByTouristRouteId(Guid touristRouteId);
        TouristRoutePicture GetPicture(int pictureId);
    }
}
