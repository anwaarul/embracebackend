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
using Embrace.Entities.Subscription.Dto;
using Embrace.Entities.Subscription;

namespace Embrace.Entites.Subscription
{
    [AbpAuthorize(PermissionNames.LookUps_Subscription)]
    public class SubscriptionAppService : AsyncCrudAppService<SubscriptionInfo, SubscriptionDto, long, PagedResultRequestDto, SubscriptionDto, SubscriptionDto>, ISubscriptionAppService
    {
        private readonly IRepository<SubscriptionInfo, long> _subscriptionRepository;
        private readonly IPermissionManager _permissionManager;
        public SubscriptionAppService(IRepository<SubscriptionInfo, long> _repository,

            IPermissionManager _Manager) : base(_repository)
        {

            _subscriptionRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_Subscription_Create)]
        public override async Task<SubscriptionDto> CreateAsync(SubscriptionDto input)
        {
          
            var result = ObjectMapper.Map<SubscriptionInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _subscriptionRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<SubscriptionDto>(result);
            return data;

        }
        [AbpAuthorize(PermissionNames.LookUps_Subscription_Update)]
        public override async Task<SubscriptionDto> UpdateAsync(SubscriptionDto input)
        {
            
            var data = ObjectMapper.Map<SubscriptionInfo>(input);
            data.UniqueKey = input.UniqueKey;
            data.SubscriptionDate = input.SubscriptionDate;
            data.SubscriptionTypeId = input.SubscriptionTypeId;
            
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _subscriptionRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<SubscriptionDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_Subscription_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _subscriptionRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _subscriptionRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_Subscription_Read)]
        public override Task<PagedResultDto<SubscriptionDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _subscriptionRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<SubscriptionDto>(query.Count(), ObjectMapper.Map<List<SubscriptionDto>>(statelist));
            return Task.FromResult(result);
        }
       
        
    }
}
