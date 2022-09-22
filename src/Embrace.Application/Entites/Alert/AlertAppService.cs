using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Embrace.Authorization;
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
        readonly IRepository<AlertInfo, long> alert_repo;

        public AlertAppService(IRepository<AlertInfo, long> alert_repo)
        {
            this.alert_repo = alert_repo;
        }

        public AlertDto Create(AlertDto input)
        {
            var dto = ObjectMapper.Map<AlertInfo>(input);
            dto.TenantId = AbpSession.TenantId.Value;
            alert_repo.Insert(dto);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AlertDto>(dto);
        }

        public PagedResultDto<AlertDto> GetAll(PagedResultRequestDto input)
        {
            var query = alert_repo.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<AlertDto>(query.Count(), ObjectMapper.Map<List<AlertDto>>(statelist));
            return result;
        }

        public PagedResultDto<AlertDto> GetAllByUniqueKey(PagedResultRequestDto input, string UniqueKey)
        {
            var query = alert_repo.GetAll().Where(x => x.UniqueKey == UniqueKey && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<AlertDto>(query.Count(), ObjectMapper.Map<List<AlertDto>>(statelist));
            return result;
        }

        public AlertDto Update(AlertDto input)
        {
            var dto = ObjectMapper.Map<AlertInfo>(input);
            dto.TenantId = AbpSession.TenantId.Value;
            alert_repo.Update(dto);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AlertDto>(dto);
        }

        public async Task Delete(EntityDto<long> input)
        {
            var result = alert_repo.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await alert_repo.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }
        }
    }

    [AutoMap(typeof(AlertInfo))]
    public class AlertDto : EntityDto<long>
    {
        public string UniqueKey { get; set; }
        public string Message { get; set; }
    }
}
