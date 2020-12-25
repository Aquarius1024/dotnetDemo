using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.ResourceParameters
{
    public class TouristRouteResourceParameters
    {
        public string Keyword { get; set; }
        public string RatingOperator { get; set; }
        public int? RatingValue { get; set; }
        private string _rating;
        public string Rating { 
            get { return _rating; } 
            set {
                _rating = value;
            }
        }
    }
}
