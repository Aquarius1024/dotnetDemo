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

        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
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

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(
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
            return await result.ToListAsync();
        }
        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }
        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await _context.TouristRoutePictures
                .Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<TouristRoutePicture>> GetPictureByTouristRouteIdAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePictures
                .Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }
        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids)
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
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
        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }

            _context.TouristRoutes.Add(touristRoute);
            //_context.SaveChanges();
        }
        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }
        public void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture)
        {
            _context.TouristRoutePictures.Remove(touristRoutePicture);
        }
        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }
    }
}
