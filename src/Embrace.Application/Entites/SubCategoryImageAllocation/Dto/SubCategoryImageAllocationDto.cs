using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.SubCategoryImageAllocation.Dto
{
    [AutoMapTo(typeof(SubCategoryImageAllocationInfo)), AutoMapFrom(typeof(SubCategoryImageAllocationInfo))]
    public class SubCategoryImageAllocationDto : EntityDto<long>
    {

        public long SubCategoryId { get; set; }
        public string Image { get; set; }

    }
}
