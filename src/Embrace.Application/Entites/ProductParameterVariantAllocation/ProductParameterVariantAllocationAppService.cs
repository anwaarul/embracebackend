using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Embrace.Authorization;
using Embrace.Authorization.Users;
using Embrace.Entities.ProductParameterVariantAllocation.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterVariantAllocation
{
    [AbpAuthorize(PermissionNames.LookUps_ProductParameterVariantAllocation)]
    public class ProductParameterVariantAllocationAppService : AsyncCrudAppService<ProductParameterVariantAllocationInfo, ProductParameterVariantAllocationDto, long, PagedResultRequestDto, CreateProductParameterVariantAllocationDto, ProductParameterVariantAllocationDto>, IProductParameterVariantAllocationAppService
    {
        private readonly IRepository<ProductParameterVariantAllocationInfo, long> _ProductParameterVariantAllocationRepository;
        private readonly IRepository<ProductParametersInfo, long> _productParametersRepository;
        private readonly IRepository<ProductVariantsInfo, long> _productVariantsRepository;
        
        private readonly UserManager _userManager;
        private readonly IPermissionManager _permissionManager;

        public ProductParameterVariantAllocationAppService(
            IRepository<ProductParameterVariantAllocationInfo, long> _repository,
            IRepository<ProductParametersInfo, long> productParametersRepository,
            IRepository<ProductVariantsInfo, long> productVariantsRepository,
            
             IRepository<User, long> repository,
            UserManager userManager,
            IPermissionManager _Manager) : base(_repository)
        {
            _userManager = userManager;
            _ProductParameterVariantAllocationRepository = _repository;
            _productParametersRepository = productParametersRepository;           
            _permissionManager = _Manager;
            _productVariantsRepository = productVariantsRepository;
            
        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductParameterVariantAllocation_Create)]
        public override async Task<ProductParameterVariantAllocationDto> CreateAsync(CreateProductParameterVariantAllocationDto input)
        {
            //ProductParameter check
            var ProductParameterData = _productParametersRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id== input.ProductParameterId).FirstOrDefault();
            if (ProductParameterData == null)
            {
                throw new UserFriendlyException("No Product Found to Allocate");
            }
            //ProductVariant/s check
            var productVariantId = input.ProductVariantId.Split(',').Select(long.Parse).ToList();
            foreach (var i in productVariantId)
            {
                var ProductVariantData = _productVariantsRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id == i).FirstOrDefault();
                if (ProductVariantData == null)
                {
                    throw new UserFriendlyException("No ProductVariant/s Found to Allocate");
                }
            }

            List<ProductParameterVariantAllocationInfo> ProductParameterVariantAllocationInfos = new List<ProductParameterVariantAllocationInfo>();
            productVariantId = input.ProductVariantId.Split(',').Select(long.Parse).ToList();

            var AllocatedParentData = _ProductParameterVariantAllocationRepository.GetAll().Where(
               t => productVariantId.Contains(t.ProductVariantId) && t.ProductParameterId == input.ProductParameterId).ToList();
            foreach (var Variant in productVariantId)
            {
                var ifExists = AllocatedParentData.FindAll(x => x.ProductVariantId == Variant && x.ProductParameterId == input.ProductParameterId).Count;
                if (ifExists != 0)
                {
                    throw new UserFriendlyException("Allocation already exists");
                }
                
                    ProductParameterVariantAllocationInfo ProductParameterVariantAllocationInfo = new ProductParameterVariantAllocationInfo
                    {

                        ProductParameterId = input.ProductParameterId,
                        ProductVariantId = Variant
                    };

                    var result = ObjectMapper.Map<ProductParameterVariantAllocationInfo>(ProductParameterVariantAllocationInfo);
                    result.CreationTime = DateTime.Now;
                    result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
                    result.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    result.IsActive = true;
                    await _ProductParameterVariantAllocationRepository.InsertAsync(result);
                    CurrentUnitOfWork.SaveChanges();
                    ProductParameterVariantAllocationInfos.Add(result);
                
            }
            var data = new ProductParameterVariantAllocationDto();
            return data;

        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductParameterVariantAllocation_Update)]
        public override async Task<ProductParameterVariantAllocationDto> UpdateAsync(ProductParameterVariantAllocationDto input)
        {

            var data = ObjectMapper.Map<ProductParameterVariantAllocationInfo>(input);
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
            data.IsActive = true;
            data.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _ProductParameterVariantAllocationRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<ProductParameterVariantAllocationDto>(data);
            return result;
        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductParameterVariantAllocation_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {

            var result = _ProductParameterVariantAllocationRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _ProductParameterVariantAllocationRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }
        //[AbpAuthorize(PermissionNames.LookUps_ProductParameterVariantAllocation_Read)]
        public override Task<PagedResultDto<ProductParameterVariantAllocationDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _ProductParameterVariantAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductParameterVariantAllocationDto>(query.Count(), ObjectMapper.Map<List<ProductParameterVariantAllocationDto>>(statelist));
            return Task.FromResult(result);

        }

        public PagedResultDto<GetAllProductParameterVariantAllocationDto> GetAllProductParameterVariantAllocation(PagedResultRequestDto input)
        {

            var query = from sp in _ProductParameterVariantAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId)
                        join pp in _productParametersRepository.GetAll() on sp.ProductParameterId equals pp.Id
                        join pv in _productVariantsRepository.GetAll() on sp.ProductVariantId equals pv.Id

                        select new GetAllProductParameterVariantAllocationDto()
                        {
                            Id = sp.Id,
                            ProductParameterId = sp.ProductParameterId,
                            ProductParameterName = pp.ProductName,
                            ProductVariantId = sp.ProductVariantId,
                            ProductVariantName = pv.Name,                            

                        };

            var dataList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<GetAllProductParameterVariantAllocationDto>(query.Count(), ObjectMapper.Map<List<GetAllProductParameterVariantAllocationDto>>(dataList));

            return result;

        }

    }
}
