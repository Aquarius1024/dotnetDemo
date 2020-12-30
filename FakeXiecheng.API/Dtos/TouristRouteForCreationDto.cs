using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;    // 配置属性值验证
using FakeXiecheng.API.ValidationAttributes;

namespace FakeXiecheng.API.Dtos
{
    public class TouristRouteForCreationDto : TouristRouteForManipulationDto// : IValidatableObject    // 自定义报错信息
    {
        
        // 自定义报错信息
        //public IEnumerable<ValidationResult> Validate(
        //    ValidationContext validationContext
        //)
        //{
        //    if(Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "路线名称必须与路线描述不同",            // 错误信息
        //            new[] { "TouristRouteForCreationDto" }  // 错误路径
        //        );
        //    }
        //}
    }
}
