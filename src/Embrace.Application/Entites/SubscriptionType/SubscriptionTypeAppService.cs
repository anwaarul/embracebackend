using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Embrace.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Embrace.Entities;
using Embrace.Entities.SubscriptionType.Dto;
using Embrace.Entities.SubscriptionType;

namespace Embrace.Entites.SubscriptionType
{
    [AbpAuthorize(PermissionNames.LookUps_SubscriptionType)]
    public class SubscriptionTypeAppService : AsyncCrudAppService<SubscriptionTypeInfo, SubscriptionTypeDto, long, PagedResultRequestDto, SubscriptionTypeDto, SubscriptionTypeDto>, ISubscriptionTypeAppService
    {
        private readonly IRepository<SubscriptionTypeInfo, long> _subscriptionRepository;
        private readonly IPermissionManager _permissionManager;
        public SubscriptionTypeAppService(IRepository<SubscriptionTypeInfo, long> _repository,

            IPermissionManager _Manager) : base(_repository)
        {

            _subscriptionRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_SubscriptionType_Create)]
        public override async Task<SubscriptionTypeDto> CreateAsync(SubscriptionTypeDto input)
        {
          
            var result = ObjectMapper.Map<SubscriptionTypeInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _subscriptionRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();

            var data = ObjectMapper.Map<SubscriptionTypeDto>(result);
            return data;

        }
        [AbpAuthorize(PermissionNames.LookUps_SubscriptionType_Update)]
        public override async Task<SubscriptionTypeDto> UpdateAsync(SubscriptionTypeDto input)
        {
            
            var data = ObjectMapper.Map<SubscriptionTypeInfo>(input);
            data.Name = input.Name;
           
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _subscriptionRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<SubscriptionTypeDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_SubscriptionType_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _subscriptionRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _subscriptionRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_SubscriptionType_Read)]
        public override Task<PagedResultDto<SubscriptionTypeDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _subscriptionRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<SubscriptionTypeDto>(query.Count(), ObjectMapper.Map<List<SubscriptionTypeDto>>(statelist));
            return Task.FromResult(result);
        }
       
        
    }
}
