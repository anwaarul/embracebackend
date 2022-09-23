using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.Alert.Dto
{
    [AutoMap(typeof(AlertInfo))]
    public class AlertDto : EntityDto<long>
    {
        public string UniqueKey { get; set; }
        public string Message { get; set; }
    }
}
