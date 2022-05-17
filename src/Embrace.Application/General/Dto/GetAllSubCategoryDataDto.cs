using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    [AutoMapTo(typeof(SubCategoryAndDateInfo)), AutoMapFrom(typeof(SubCategoryAndDateInfo))]

    public class GetAllSubCategoryDataDto : EntityDto<long>
    {
        public long SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public DateTime DateAndTime { get; set; }
        public string UniqueKey { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
