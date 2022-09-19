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
using Embrace.Entities.OrderPlacement.Dto;
using Embrace.Entities.OrderPlacement;

namespace Embrace.Entites.OrderPlacement
{
    [AbpAuthorize(PermissionNames.LookUps_OrderPlacement)]
    public class OrderPlacementAppService : AsyncCrudAppService<OrderPlacementInfo, OrderPlacementDto, long, PagedResultRequestDto, OrderPlacementDto, OrderPlacementDto>, IOrderPlacementAppService
    {
        private readonly IRepository<OrderPlacementInfo, long> _sizeRepository;
        private readonly IPermissionManager _permissionManager;
        public OrderPlacementAppService(IRepository<OrderPlacementInfo, long> _repository,

            IPermissionManager _Manager) : base(_repository)
        {

            _sizeRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_OrderPlacement_Create)]
        public override async Task<OrderPlacementDto> CreateAsync(OrderPlacementDto input)
        {
           
            var result = ObjectMapper.Map<OrderPlacementInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _sizeRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<OrderPlacementDto>(result);
            return data;

        }
        [AbpAuthorize(PermissionNames.LookUps_OrderPlacement_Update)]
        public override async Task<OrderPlacementDto> UpdateAsync(OrderPlacementDto input)
        {
           
            var data = ObjectMapper.Map<OrderPlacementInfo>(input);
            data.UserUniqueName = input.UserUniqueName;
            data.ProductName = input.ProductName; 
            data.Address = input.Address; 
            data.CityName = input.CityName; 
            data.FirstName = input.FirstName; 
            data.LastName = input.LastName; 
            data.ContactNumber = input.ContactNumber; 
            data.CountryName = input.CountryName; 
            data.ZipPostalCode = input.ZipPostalCode; 
            data.Quantity = input.Quantity; 
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _sizeRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<OrderPlacementDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_OrderPlacement_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _sizeRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _sizeRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_OrderPlacement_Read)]
        public override Task<PagedResultDto<OrderPlacementDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _sizeRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<OrderPlacementDto>(query.Count(), ObjectMapper.Map<List<OrderPlacementDto>>(statelist));
            return Task.FromResult(result);
        }
        //public PagedResultDto<OrderPlacementDto> GetAllOrderPlacementFilter(PagedResultRequestDto input, string filter)
        //{

        //    if (filter == null)
        //    {
        //        filter = "";
        //    }

        //    var dataOrderPlacement = _sizeRepository.GetAll().Where(x => x.Name.Contains(filter) && x.TenantId == AbpSession.TenantId).ToList();
        //    var statelist = dataOrderPlacement.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        //    var result = new PagedResultDto<OrderPlacementDto>(statelist.Count(), ObjectMapper.Map<List<OrderPlacementDto>>(statelist));
        //    return result;
        //}
        
    }
}
