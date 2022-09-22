using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.Alert
{
    public class AlertAppService : ApplicationService
    {
        readonly IRepository<AlertInfo, long> alert_Repo;

        public AlertAppService(IRepository<AlertInfo, long> alert_repo)
        {
            alert_Repo = alert_repo;
        }


    }

    [AutoMap(typeof(AlertInfo))]
    public class AlterDto
    {
        public string Message { get; set; }
    }
}
