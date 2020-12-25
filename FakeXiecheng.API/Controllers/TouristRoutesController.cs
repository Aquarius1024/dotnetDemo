using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.RegularExpressions;
using FakeXiecheng.API.ResourceParameters;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository, 
            IMapper mapper
        )
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        // api/touristRoutes?keyword=***
        [HttpGet]
        [HttpHead]
        public IActionResult GetTouristRoutes(
            [FromQuery] TouristRouteResourceParameters parameters
            //[FromQuery] string keyword,
            //string rating  // 小于lessThan, 大于largeThan, 等于equalTo, lessThan3, largeThan4, equalTo5    
        ) // FormQuery vs FormBody
        {
            Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");

            string operatorType = "";
            int ratingValue = -1;
            Match match = regex.Match(parameters.Rating);
            if(match.Success)
            {
                operatorType = match.Groups[1].Value;
                ratingValue = Int32.Parse(match.Groups[2].Value);
            }

            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoutes(parameters.Keyword, operatorType, ratingValue);
            if(touristRouteFromRepo == null || touristRouteFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRouteDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRouteFromRepo);
            return Ok(touristRouteFromRepo);
        }

        // api/touristRoutes/{touristRouteId}
        [HttpGet("{touristRouteId:Guid}")]
        [HttpHead]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound($"旅游路线{touristRouteId}找不到");
            }

            // 原始映射方法
            //var touristRouteDto = new TouristRouteDto()
            //{
            //Id = touristRouteFromRepo.Id,
            //Title = touristRouteFromRepo.Title,
            //Description = touristRouteFromRepo.Description,
            //Price = touristRouteFromRepo.OriginalPrice * (decimal)(touristRouteFromRepo.DiscountPresent ?? 1),
            //CreateTime = touristRouteFromRepo.CreateTime,
            //UpdateTime = touristRouteFromRepo.UpdateTime,
            //Features = touristRouteFromRepo.Features,
            //Fees = touristRouteFromRepo.Fees,
            //Notes = touristRouteFromRepo.Notes,
            //Rating = touristRouteFromRepo.Rating,
            //TravelDays = touristRouteFromRepo.TravelDays.ToString(),
            //TripType = touristRouteFromRepo.TripType.ToString(),
            //DepartureCity = touristRouteFromRepo.DepartureCity.ToString()
            //};

            // 使用automapper映射方法
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);

            return Ok(touristRouteDto);
        }
    }
}
