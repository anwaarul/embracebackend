using Abp.AutoMapper;
using Abp.Domain.Entities;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    [AutoMapTo(typeof(UserBlogInfo)), AutoMapFrom(typeof(UserBlogInfo))]
    public class UpateblogDto : Entity<long>
    {
        public string UniqueKey { get; set; }
        public long BlogId { get; set; }
        public int TenantId { get; set; }
        public bool IsSavedPost { get; set; }
    }
}
