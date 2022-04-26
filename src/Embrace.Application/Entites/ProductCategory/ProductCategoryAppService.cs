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
using Embrace.Entities.ProductCategory.Dto;
using Embrace.Entities.ProductCategory;

namespace Embrace.Entites.ProductCategory
{
    [AbpAuthorize(PermissionNames.LookUps_ProductCategory)]
    public class ProductCategoryAppService : AsyncCrudAppService<ProductCategoryInfo, ProductCategoryDto, long, PagedResultRequestDto, ProductCategoryDto, ProductCategoryDto>, IProductCategoryAppService
    {
        private readonly IRepository<ProductCategoryInfo, long> _productParametersRepository;
        private readonly IRepository<CategoryInfo, long> _categoriesRepository;
        private readonly IPermissionManager _permissionManager;
        public ProductCategoryAppService(
            IRepository<ProductCategoryInfo, long> _repository,
            IRepository<CategoryInfo, long> categoriesRepository,


            IPermissionManager _Manager) : base(_repository)
        {

            _productParametersRepository = _repository;
            _categoriesRepository = categoriesRepository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_ProductCategory_Create)]
        public override async Task<ProductCategoryDto> CreateAsync(ProductCategoryDto input)
        {
           
            var result = ObjectMapper.Map<ProductCategoryInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _productParametersRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<ProductCategoryDto>(result);
            return data;

        }
        [AbpAuthorize(PermissionNames.LookUps_ProductCategory_Update)]
        public override async Task<ProductCategoryDto> UpdateAsync(ProductCategoryDto input)
        {
            var prevdataProductCategory = _productParametersRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();
           
            var data = ObjectMapper.Map<ProductCategoryInfo>(prevdataProductCategory);
            data.Name = input.Name;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
            data.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _productParametersRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<ProductCategoryDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_ProductCategory_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _productParametersRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _productParametersRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductCategory_Read)]
        public override Task<PagedResultDto<ProductCategoryDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _productParametersRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductCategoryDto>(query.Count(), ObjectMapper.Map<List<ProductCategoryDto>>(statelist));
            return Task.FromResult(result);
        }

    }
}
