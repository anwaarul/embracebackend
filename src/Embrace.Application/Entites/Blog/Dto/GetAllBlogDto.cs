using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.Blog.Dto
{
    [AutoMapTo(typeof(BlogInfo)), AutoMapFrom(typeof(BlogInfo))]
    public class GetAllBlogDto : EntityDto<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }

    }
}
