﻿using FakeXiecheng.API.Database;
using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            return _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefault(n => n.Id == touristRouteId);
        }

        //public IEnumerable<TouristRoute> GetTouristRoutes()
        //{
        //    // 连接两个表
        //    return _context.TouristRoutes.Include(t => t.TouristRoutePictures);
        //}

        //public IEnumerable<TouristRoute> GetTouristRoutes(string keyword)
        //{
        //    IQueryable<TouristRoute> result = _context
        //        .TouristRoutes
        //        .Include(t => t.TouristRoutePictures);

        //    if (!string.IsNullOrWhiteSpace(keyword))
        //    {
        //        keyword = keyword.Trim();
        //        result.Where(t => t.Title.Contains(keyword));
        //    }

        //    // include vs join
        //    return result.ToList();
        //}

        public IEnumerable<TouristRoute> GetTouristRoutes(
            string keyword, 
            string operatorType, 
            int? ratingValue
        )
        {
            IQueryable<TouristRoute> result = _context
                .TouristRoutes
                .Include(t => t.TouristRoutePictures);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }
            var res = result.ToList();

            if(ratingValue >= 0)
            {
                //switch (operatorType)
                //{
                //    case "largeThan":
                //        result = result.Where(t => t.Rating >= ratingValue);
                //        break;
                //    case "lessThan":
                //        result = result.Where(t => t.Rating <= ratingValue);
                //        break;
                //    case "equalTo":
                //    default:
                //        result = result.Where(t => t.Rating == ratingValue);
                //        break;
                //}
                result = operatorType switch
                {
                    "largeThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating <= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue),
                };
            }

            // include vs join
            return result.ToList();
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            return _context.TouristRoutes.Any(t => t.Id == touristRouteId);
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if(touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }

            _context.TouristRoutes.Add(touristRoute);
            //_context.SaveChanges();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public TouristRoutePicture GetPicture(int pictureId)
        {
            return _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefault();
        }

        public IEnumerable<TouristRoutePicture> GetPictureByTouristRouteId(Guid touristRouteId)
        {
            return _context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToList();
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if(touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }

            if(touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }

            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);

        }


    }
}
