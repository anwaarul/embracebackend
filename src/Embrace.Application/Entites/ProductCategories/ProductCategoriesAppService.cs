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
using Embrace.Entities.ProductCategories.Dto;
using Embrace.Entities.ProductCategories;

namespace Embrace.Entites.ProductCategories
{
    [AbpAuthorize(PermissionNames.LookUps_ProductCategories)]
    public class ProductCategoriesAppService : AsyncCrudAppService<ProductCategoriesInfo, ProductCategoriesDto, long, PagedResultRequestDto, ProductCategoriesDto, ProductCategoriesDto>, IProductCategoriesAppService
    {
        private readonly IRepository<ProductCategoriesInfo, long> _productCategoriesRepository;
        private readonly IRepository<ProductParametersInfo, long> _productParametersRepository;
        private readonly IRepository<ProductVariantsInfo, long> _productVariantsRepository;
        private readonly IPermissionManager _permissionManager;
        public ProductCategoriesAppService(
            IRepository<ProductCategoriesInfo, long> _repository,
            IRepository<ProductParametersInfo, long> productParametersRepository,
            IRepository<ProductVariantsInfo, long> productVariantsRepository,

            IPermissionManager _Manager) : base(_repository)
        {

            _productCategoriesRepository = _repository;
            _productParametersRepository = productParametersRepository;
            _productVariantsRepository = productVariantsRepository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_ProductCategories_Create)]
        public override async Task<ProductCategoriesDto> CreateAsync(ProductCategoriesDto input)
        {
            var dataProductCategories = _productCategoriesRepository.GetAll().Where(x => x.Name == input.Name && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (dataProductCategories != null)
            {
                throw new UserFriendlyException("there is already ProductCategories entered with that name");
            }
            var result = ObjectMapper.Map<ProductCategoriesInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _productCategoriesRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<ProductCategoriesDto>(result);
            return data;

        }
        [AbpAuthorize(PermissionNames.LookUps_ProductCategories_Update)]
        public override async Task<ProductCategoriesDto> UpdateAsync(ProductCategoriesDto input)
        {
            var prevdataProductCategories = _productCategoriesRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            var dataProductCategoriesNew = _productCategoriesRepository.GetAll().Where(x => x.Name == input.Name && x.Id != prevdataProductCategories.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (prevdataProductCategories.Name != input.Name)
            {
                if (dataProductCategoriesNew != null)
                {
                    throw new UserFriendlyException("there is already ProductCategories entered with that name");
                }
            }
            var data = ObjectMapper.Map<ProductCategoriesInfo>(prevdataProductCategories);
            data.Name = input.Name;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _productCategoriesRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<ProductCategoriesDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_ProductCategories_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _productCategoriesRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _productCategoriesRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductCategories_Read)]
        public override Task<PagedResultDto<ProductCategoriesDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _productCategoriesRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductCategoriesDto>(query.Count(), ObjectMapper.Map<List<ProductCategoriesDto>>(statelist));
            return Task.FromResult(result);
        }
        public PagedResultDto<ProductCategoriesDto> GetAllProductCategoriesFilter(PagedResultRequestDto input, string filter)
        {

            if (filter == null)
            {
                filter = "";
            }

            var dataProductCategories = _productCategoriesRepository.GetAll().Where(x => x.Name.Contains(filter) && x.TenantId == AbpSession.TenantId).ToList();
            var statelist = dataProductCategories.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductCategoriesDto>(statelist.Count(), ObjectMapper.Map<List<ProductCategoriesDto>>(statelist));
            return result;
        }
        public PagedResultDto<ProductCategoriesDto> GetAllProductCategoriesSearchFilter(PagedProductCategoriesResultRequestExtendedDto input)
        {
            var query = _productCategoriesRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.ToLower().Contains(input.Keyword.ToLower()))
                .ToList();

            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<ProductCategoriesDto>(query.Count, ObjectMapper.Map<List<ProductCategoriesDto>>(statelist));
            return result;
        }

        
    }
}
