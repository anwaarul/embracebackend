using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Embrace.Authorization;
using Embrace.Authorization.Users;
using Embrace.Entities.SubscriptionOrderPayementAllocation.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.SubscriptionOrderPayementAllocation
{
    [AbpAuthorize(PermissionNames.LookUps_SubscriptionOrderPayementAllocation)]
    public class SubscriptionOrderPayementAllocationAppService : AsyncCrudAppService<SubscriptionOrderPayementAllocationInfo, SubscriptionOrderPayementAllocationDto, long, PagedResultRequestDto, CreateSubscriptionOrderPayementAllocationDto, SubscriptionOrderPayementAllocationDto>, ISubscriptionOrderPayementAllocationAppService
    {
        private readonly IRepository<SubscriptionOrderPayementAllocationInfo, long> _SubscriptionOrderPayementAllocationRepository;
        private readonly IRepository<SubscriptionInfo, long> _subscriptionRepository;
        private readonly IRepository<OrderPlacementInfo, long> _orderPlacementRepository;
        
        private readonly UserManager _userManager;
        private readonly IPermissionManager _permissionManager;

        public SubscriptionOrderPayementAllocationAppService(
            IRepository<SubscriptionOrderPayementAllocationInfo, long> _repository,
            IRepository<OrderPlacementInfo, long> orderPlacementRepository,
            IRepository<SubscriptionInfo, long> subscriptionRepository,
            
            
             IRepository<User, long> repository,
            UserManager userManager,
            IPermissionManager _Manager) : base(_repository)
        {
            _orderPlacementRepository = orderPlacementRepository;
            _userManager = userManager;
            _SubscriptionOrderPayementAllocationRepository = _repository;
            _subscriptionRepository = subscriptionRepository;           
            _permissionManager = _Manager;
            _orderPlacementRepository = orderPlacementRepository;
            
        }

        //[AbpAuthorize(PermissionNames.LookUps_SubscriptionOrderPayementAllocation_Create)]
        public override async Task<SubscriptionOrderPayementAllocationDto> CreateAsync(CreateSubscriptionOrderPayementAllocationDto input)
        {
            //ProductParameter check
            var ProductParameterData = _subscriptionRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id== input.SubscriptionId).FirstOrDefault();
            if (ProductParameterData == null)
            {
                throw new UserFriendlyException("No  Found to Allocate");
            }
            //OrderPlacement/s check
            var orderPaymentId = input.OrderPaymentId.Split(',').Select(long.Parse).ToList();
            foreach (var i in orderPaymentId)
            {
                var OrderPlacementData = _orderPlacementRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id == i).FirstOrDefault();
                if (OrderPlacementData == null)
                {
                    throw new UserFriendlyException("No OrderPlacement/s Found to Allocate");
                }
            }

            List<SubscriptionOrderPayementAllocationInfo> SubscriptionOrderPayementAllocationInfos = new List<SubscriptionOrderPayementAllocationInfo>();
            orderPaymentId = input.OrderPaymentId.Split(',').Select(long.Parse).ToList();

            var AllocatedParentData = _SubscriptionOrderPayementAllocationRepository.GetAll().Where(
               t => orderPaymentId.Contains(t.OrderPaymentId) && t.SubscriptionId == input.SubscriptionId).ToList();
            foreach (var OrderPlacement in orderPaymentId)
            {
                var ifExists = AllocatedParentData.FindAll(x => x.OrderPaymentId == OrderPlacement && x.SubscriptionId == input.SubscriptionId).Count;
                if (ifExists != 0)
                {
                    throw new UserFriendlyException("Allocation already exists");
                }
                
                    SubscriptionOrderPayementAllocationInfo SubscriptionOrderPayementAllocationInfo = new SubscriptionOrderPayementAllocationInfo
                    {

                        SubscriptionId = input.SubscriptionId,
                        OrderPaymentId = OrderPlacement
                    };

                    var result = ObjectMapper.Map<SubscriptionOrderPayementAllocationInfo>(SubscriptionOrderPayementAllocationInfo);
                    result.CreationTime = DateTime.Now;
                    result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
                    result.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    result.IsActive = true;
                    await _SubscriptionOrderPayementAllocationRepository.InsertAsync(result);
                    CurrentUnitOfWork.SaveChanges();
                    SubscriptionOrderPayementAllocationInfos.Add(result);
                
            }
            var data = new SubscriptionOrderPayementAllocationDto();
            return data;

        }

        //[AbpAuthorize(PermissionNames.LookUps_SubscriptionOrderPayementAllocation_Update)]
        public override async Task<SubscriptionOrderPayementAllocationDto> UpdateAsync(SubscriptionOrderPayementAllocationDto input)
        {

            var data = ObjectMapper.Map<SubscriptionOrderPayementAllocationInfo>(input);
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
            data.IsActive = true;
            data.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _SubscriptionOrderPayementAllocationRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<SubscriptionOrderPayementAllocationDto>(data);
            return result;
        }

        //[AbpAuthorize(PermissionNames.LookUps_SubscriptionOrderPayementAllocation_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {

            var result = _SubscriptionOrderPayementAllocationRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _SubscriptionOrderPayementAllocationRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }
        //[AbpAuthorize(PermissionNames.LookUps_SubscriptionOrderPayementAllocation_Read)]
        public override Task<PagedResultDto<SubscriptionOrderPayementAllocationDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _SubscriptionOrderPayementAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<SubscriptionOrderPayementAllocationDto>(query.Count(), ObjectMapper.Map<List<SubscriptionOrderPayementAllocationDto>>(statelist));
            return Task.FromResult(result);

        }

        //public PagedResultDto<GetAllSubscriptionOrderPayementAllocationDto> GetAllSubscriptionOrderPayementAllocation(PagedResultRequestDto input)
        //{

        //    var query = from sp in _SubscriptionOrderPayementAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId)
        //                join pp in _subscriptionRepository.GetAll() on sp.SubscriptionId equals pp.Id
        //                join s in _orderPlacementRepository.GetAll() on sp.OrderPaymentId equals s.Id

        //                select new GetAllSubscriptionOrderPayementAllocationDto()
        //                {
        //                    Id = sp.Id,
        //                    OrderPaymentId = s.Id,
        //                    OrderPaymentName = s.or,
        //                    SizeId = sp.SizeId,
        //                    SizeName = s.Name,

        //                };

        //    var dataList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
        //    var result = new PagedResultDto<GetAllSubscriptionOrderPayementAllocationDto>(query.Count(), ObjectMapper.Map<List<GetAllSubscriptionOrderPayementAllocationDto>>(dataList));

        //    return result;

        //}

    }
}
