using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    public class GetAllGeneralBlogDto : EntityDto<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public string UniqueKey { get; set; }
        public bool IsSavedPost { get; set; }
    }
}
