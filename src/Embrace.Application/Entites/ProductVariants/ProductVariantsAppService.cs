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
using Embrace.Entities.ProductVariants.Dto;
using Embrace.Entities.ProductVariants;

namespace Embrace.Entites.ProductVariants
{
    [AbpAuthorize(PermissionNames.LookUps_ProductVariants)]
    public class ProductVariantsAppService : AsyncCrudAppService<ProductVariantsInfo, ProductVariantsDto, long, PagedResultRequestDto, ProductVariantsDto, ProductVariantsDto>, IProductVariantsAppService
    {
        private readonly IRepository<ProductVariantsInfo, long> _productVariantsRepository;
        private readonly IPermissionManager _permissionManager;
        public ProductVariantsAppService(IRepository<ProductVariantsInfo, long> _repository,

            IPermissionManager _Manager) : base(_repository)
        {

            _productVariantsRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_ProductVariants_Create)]
        public override async Task<ProductVariantsDto> CreateAsync(ProductVariantsDto input)
        {
            var dataProductVariants = _productVariantsRepository.GetAll().Where(x => x.Name == input.Name && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (dataProductVariants != null)
            {
                throw new UserFriendlyException("there is already ProductVariants entered with that name");
            }
            var result = ObjectMapper.Map<ProductVariantsInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _productVariantsRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<ProductVariantsDto>(result);
            return data;

        }
        [AbpAuthorize(PermissionNames.LookUps_ProductVariants_Update)]
        public override async Task<ProductVariantsDto> UpdateAsync(ProductVariantsDto input)
        {
            var prevdataProductVariants = _productVariantsRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            var dataProductVariantsNew = _productVariantsRepository.GetAll().Where(x => x.Name == input.Name && x.Id != prevdataProductVariants.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (prevdataProductVariants.Name != input.Name)
            {
                if (dataProductVariantsNew != null)
                {
                    throw new UserFriendlyException("there is already ProductVariants entered with that name");
                }
            }
            var data = ObjectMapper.Map<ProductVariantsInfo>(prevdataProductVariants);
            data.Name = input.Name;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _productVariantsRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<ProductVariantsDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_ProductVariants_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _productVariantsRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _productVariantsRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductVariants_Read)]
        public override Task<PagedResultDto<ProductVariantsDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _productVariantsRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductVariantsDto>(query.Count(), ObjectMapper.Map<List<ProductVariantsDto>>(statelist));
            return Task.FromResult(result);
        }
        public PagedResultDto<ProductVariantsDto> GetAllProductVariantsFilter(PagedResultRequestDto input, string filter)
        {

            if (filter == null)
            {
                filter = "";
            }

            var dataProductVariants = _productVariantsRepository.GetAll().Where(x => x.Name.Contains(filter) && x.TenantId == AbpSession.TenantId).ToList();
            var statelist = dataProductVariants.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductVariantsDto>(statelist.Count(), ObjectMapper.Map<List<ProductVariantsDto>>(statelist));
            return result;
        }
        public PagedResultDto<ProductVariantsDto> GetAllProductVariantsSearchFilter(PagedProductVariantsResultRequestExtendedDto input)
        {
            var query = _productVariantsRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.ToLower().Contains(input.Keyword.ToLower()))
                .ToList();

            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<ProductVariantsDto>(query.Count, ObjectMapper.Map<List<ProductVariantsDto>>(statelist));
            return result;
        }
        
    }
}
