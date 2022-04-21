using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Embrace.Authorization;
using Embrace.Authorization.Users;
using Embrace.Entities.ProductParameterSizeAllocation.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterSizeAllocation
{
    [AbpAuthorize(PermissionNames.LookUps_ProductParameterSizeAllocation)]
    public class ProductParameterSizeAllocationAppService : AsyncCrudAppService<ProductParameterSizeAllocationInfo, ProductParameterSizeAllocationDto, long, PagedResultRequestDto, CreateProductParameterSizeAllocationDto, ProductParameterSizeAllocationDto>, IProductParameterSizeAllocationAppService
    {
        private readonly IRepository<ProductParameterSizeAllocationInfo, long> _ProductParameterSizeAllocationRepository;
        private readonly IRepository<ProductParametersInfo, long> _productParametersRepository;
        private readonly IRepository<SizeInfo, long> _sizeRepository;
        
        private readonly UserManager _userManager;
        private readonly IPermissionManager _permissionManager;

        public ProductParameterSizeAllocationAppService(
            IRepository<ProductParameterSizeAllocationInfo, long> _repository,
            IRepository<ProductParametersInfo, long> productParametersRepository,
            IRepository<SizeInfo, long> sizeRepository,
            
             IRepository<User, long> repository,
            UserManager userManager,
            IPermissionManager _Manager) : base(_repository)
        {
            _userManager = userManager;
            _ProductParameterSizeAllocationRepository = _repository;
            _productParametersRepository = productParametersRepository;           
            _permissionManager = _Manager;
            _sizeRepository = sizeRepository;
            
        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductParameterSizeAllocation_Create)]
        public override async Task<ProductParameterSizeAllocationDto> CreateAsync(CreateProductParameterSizeAllocationDto input)
        {
            //ProductParameter check
            var ProductParameterData = _productParametersRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id== input.ProductParameterId).FirstOrDefault();
            if (ProductParameterData == null)
            {
                throw new UserFriendlyException("No Product Found to Allocate");
            }
            //Size/s check
            var sizeId = input.SizeId.Split(',').Select(long.Parse).ToList();
            foreach (var i in sizeId)
            {
                var SizeData = _sizeRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id == i).FirstOrDefault();
                if (SizeData == null)
                {
                    throw new UserFriendlyException("No Size/s Found to Allocate");
                }
            }

            List<ProductParameterSizeAllocationInfo> ProductParameterSizeAllocationInfos = new List<ProductParameterSizeAllocationInfo>();
            sizeId = input.SizeId.Split(',').Select(long.Parse).ToList();

            var AllocatedParentData = _ProductParameterSizeAllocationRepository.GetAll().Where(
               t => sizeId.Contains(t.SizeId) && t.ProductParameterId == input.ProductParameterId).ToList();
            foreach (var Size in sizeId)
            {
                var ifExists = AllocatedParentData.FindAll(x => x.SizeId == Size && x.ProductParameterId == input.ProductParameterId).Count;
                if (ifExists != 0)
                {
                    throw new UserFriendlyException("Allocation already exists");
                }
                
                    ProductParameterSizeAllocationInfo ProductParameterSizeAllocationInfo = new ProductParameterSizeAllocationInfo
                    {

                        ProductParameterId = input.ProductParameterId,
                        SizeId = Size
                    };

                    var result = ObjectMapper.Map<ProductParameterSizeAllocationInfo>(ProductParameterSizeAllocationInfo);
                    result.CreationTime = DateTime.Now;
                    result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
                    result.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    result.IsActive = true;
                    await _ProductParameterSizeAllocationRepository.InsertAsync(result);
                    CurrentUnitOfWork.SaveChanges();
                    ProductParameterSizeAllocationInfos.Add(result);
                
            }
            var data = new ProductParameterSizeAllocationDto();
            return data;

        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductParameterSizeAllocation_Update)]
        public override async Task<ProductParameterSizeAllocationDto> UpdateAsync(ProductParameterSizeAllocationDto input)
        {

            var data = ObjectMapper.Map<ProductParameterSizeAllocationInfo>(input);
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
            data.IsActive = true;
            data.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _ProductParameterSizeAllocationRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<ProductParameterSizeAllocationDto>(data);
            return result;
        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductParameterSizeAllocation_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {

            var result = _ProductParameterSizeAllocationRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _ProductParameterSizeAllocationRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }
        //[AbpAuthorize(PermissionNames.LookUps_ProductParameterSizeAllocation_Read)]
        public override Task<PagedResultDto<ProductParameterSizeAllocationDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _ProductParameterSizeAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductParameterSizeAllocationDto>(query.Count(), ObjectMapper.Map<List<ProductParameterSizeAllocationDto>>(statelist));
            return Task.FromResult(result);

        }

        public PagedResultDto<GetAllProductParameterSizeAllocationDto> GetAllProductParameterSizeAllocation(PagedResultRequestDto input)
        {

            var query = from sp in _ProductParameterSizeAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId)
                        join pp in _productParametersRepository.GetAll() on sp.ProductParameterId equals pp.Id
                        join s in _sizeRepository.GetAll() on sp.SizeId equals s.Id

                        select new GetAllProductParameterSizeAllocationDto()
                        {
                            Id = sp.Id,
                            ProductParameterId = sp.ProductParameterId,
                            ProductParameterName = pp.ProductName,
                            SizeId = sp.SizeId,
                            SizeName = s.Name,

                        };

            var dataList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<GetAllProductParameterSizeAllocationDto>(query.Count(), ObjectMapper.Map<List<GetAllProductParameterSizeAllocationDto>>(dataList));

            return result;

        }

    }
}
