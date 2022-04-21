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
using Embrace.Entities.ProductType.Dto;
using Embrace.Entities.ProductType;

namespace Embrace.Entites.ProductType
{
    [AbpAuthorize(PermissionNames.LookUps_ProductType)]
    public class ProductTypeAppService : AsyncCrudAppService<ProductTypeInfo, ProductTypeDto, long, PagedResultRequestDto, ProductTypeDto, ProductTypeDto>, IProductTypeAppService
    {
        private readonly IRepository<ProductTypeInfo, long> _productTypeRepository;
        private readonly IPermissionManager _permissionManager;
        public ProductTypeAppService(IRepository<ProductTypeInfo, long> _repository,

            IPermissionManager _Manager) : base(_repository)
        {

            _productTypeRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_ProductType_Create)]
        public override async Task<ProductTypeDto> CreateAsync(ProductTypeDto input)
        {
            var dataProductType = _productTypeRepository.GetAll().Where(x => x.Name == input.Name && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (dataProductType != null)
            {
                throw new UserFriendlyException("there is already Size entered with that name");
            }
            var result = ObjectMapper.Map<ProductTypeInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _productTypeRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<ProductTypeDto>(result);
            return data;

        }
        [AbpAuthorize(PermissionNames.LookUps_ProductType_Update)]
        public override async Task<ProductTypeDto> UpdateAsync(ProductTypeDto input)
        {
            var prevdataProductType = _productTypeRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            var dataProductTypeNew = _productTypeRepository.GetAll().Where(x => x.Name == input.Name && x.Id != prevdataProductType.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (prevdataProductType.Name != input.Name)
            {
                if (dataProductTypeNew != null)
                {
                    throw new UserFriendlyException("there is already ProductType entered with that name");
                }
            }
            var data = ObjectMapper.Map<ProductTypeInfo>(prevdataProductType);
            data.Name = input.Name;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _productTypeRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<ProductTypeDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_ProductType_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _productTypeRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _productTypeRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductType_Read)]
        public override Task<PagedResultDto<ProductTypeDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _productTypeRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductTypeDto>(query.Count(), ObjectMapper.Map<List<ProductTypeDto>>(statelist));
            return Task.FromResult(result);
        }
        public PagedResultDto<ProductTypeDto> GetAllProductTypeFilter(PagedResultRequestDto input, string filter)
        {

            if (filter == null)
            {
                filter = "";
            }

            var dataProductType = _productTypeRepository.GetAll().Where(x => x.Name.Contains(filter) && x.TenantId == AbpSession.TenantId).ToList();
            var statelist = dataProductType.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductTypeDto>(statelist.Count(), ObjectMapper.Map<List<ProductTypeDto>>(statelist));
            return result;
        }
        public PagedResultDto<ProductTypeDto> GetAllProductTypeSearchFilter(PagedProductTypeResultRequestExtendedDto input)
        {
            var query = _productTypeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.ToLower().Contains(input.Keyword.ToLower()))
                .ToList();

            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<ProductTypeDto>(query.Count, ObjectMapper.Map<List<ProductTypeDto>>(statelist));
            return result;
        }
        
    }
}
