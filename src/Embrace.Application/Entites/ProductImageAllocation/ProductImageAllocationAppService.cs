using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using Embrace.Entities;
using Embrace.Authorization;
using Embrace.Entites.ProductImageAllocation.Dto;
using Embrace.Entites.ProductImageAllocation;
using System.IO;

namespace Embrace.Entites.ProductImageAllocation
{
    [AbpAuthorize(PermissionNames.LookUps_ProductImageAllocation)]
    public class ProductImageAllocationAppService : AsyncCrudAppService<ProductImageAllocationInfo, ProductImageAllocationDto, long, PagedResultRequestDto, ProductImageAllocationDto, ProductImageAllocationDto>, IProductImageAllocationAppService
    {

        private readonly IRepository<ProductImageAllocationInfo, long> _productImageAllocationRepository;
        private readonly IRepository<ProductParametersInfo, long> _productParametersRepository;
        private readonly IPermissionManager _permissionManager;

        public ProductImageAllocationAppService(
            IRepository<ProductImageAllocationInfo, long> _repository,
            IRepository<ProductParametersInfo, long> productParametersRepository,
            IPermissionManager _Manager) : base(_repository)
        {
            _productParametersRepository = productParametersRepository;
            _productImageAllocationRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_ProductImageAllocation_Create)]
        public override async Task<ProductImageAllocationDto> CreateAsync(ProductImageAllocationDto input)
        {

            var result = ObjectMapper.Map<ProductImageAllocationInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _productImageAllocationRepository.InsertAsync(result);
            result.IsActive = true;
            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<ProductImageAllocationDto>(result);
            return data;

        }
        public async Task<CreateProductImagesAllocationDto> CreateProductImageAllocation(CreateProductImagesAllocationDto input)
        {
            ProductImageAllocationInfo imagedata = new ProductImageAllocationInfo();
            var product = _productParametersRepository.GetAll().Where(x => x.Id == input.ProductParameterId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (product == null)
            {
                throw new UserFriendlyException("SubCategory not found.");
            }
            foreach (var imageBase in input.BulkImage)
            {
                if (imageBase.BaseUrl != null)
                {

                    Uri uriResult;
                    bool uRL = Uri.TryCreate(imageBase.BaseUrl, UriKind.Absolute, out uriResult)
                        && uriResult.Scheme == Uri.UriSchemeHttp;
                    if (uRL == true)
                    {
                        imagedata = new ProductImageAllocationInfo
                        {
                            ProductParameterId = product.Id,

                            ImageUrl = imageBase.BaseUrl,
                            CreationTime = DateTime.Now,
                            CreatorUserId = Convert.ToInt32(AbpSession.UserId),
                            TenantId = Convert.ToInt32(AbpSession.TenantId),
                            IsActive = true

                        };
                        await _productImageAllocationRepository.InsertAsync(imagedata);
                        CurrentUnitOfWork.SaveChanges();

                    }
                    else
                    {
                        var imgUrl = "";

                        imgUrl = SaveImage(imageBase.BaseUrl);


                        imagedata = new ProductImageAllocationInfo
                        {
                            ProductParameterId = product.Id,

                            ImageUrl = imgUrl,
                            CreationTime = DateTime.Now,
                            CreatorUserId = Convert.ToInt32(AbpSession.UserId),
                            TenantId = Convert.ToInt32(AbpSession.TenantId),
                            IsActive = true

                        };
                        await _productImageAllocationRepository.InsertAsync(imagedata);
                        CurrentUnitOfWork.SaveChanges();

                    }

                }
            }
            var data = ObjectMapper.Map<CreateProductImagesAllocationDto>(imagedata);
            return data;
        }
        protected string SaveImage(string ImgStr)
        {
            // create path
            String folder = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot\\ProductImage\\");
            DirectoryInfo di;
            if (!Directory.Exists(folder))
            {
                // Try to create the directory.
                try
                {

                    di = Directory.CreateDirectory(folder);
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);
                }
            }

            //Check if directory exist
            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder); //Create directory if it doesn't exist
            }

            string imageName = Guid.NewGuid().ToString() + ".jpg";

            //set the image path
            string imgPath = Path.Combine(folder, imageName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            File.WriteAllBytes(imgPath, imageBytes);

            return "http://54.37.97.142:81/" + "/" + "ProductImage" + "/" + "/" + imageName;
        }


        [AbpAuthorize(PermissionNames.LookUps_ProductImageAllocation_Update)]
        public override async Task<ProductImageAllocationDto> UpdateAsync(ProductImageAllocationDto input)
        {

            var data = ObjectMapper.Map<ProductImageAllocationInfo>(input);

            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
            data.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _productImageAllocationRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<ProductImageAllocationDto>(data);
            return result;

        }

        public async Task DeleteProductImageAllocationbySubCategoryId(long subCategoryId)
        {
            var subcategoryA = _productImageAllocationRepository.GetAll().Where(x => x.ProductParameterId == subCategoryId 
            && x.TenantId == AbpSession.TenantId).ToList();

            foreach (var subCategory in subcategoryA)
            {
                await _productImageAllocationRepository.DeleteAsync(subCategory);
                CurrentUnitOfWork.SaveChanges();
            }

        }
        public override Task<PagedResultDto<ProductImageAllocationDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _productImageAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductImageAllocationDto>(query.Count(), ObjectMapper.Map<List<ProductImageAllocationDto>>(statelist));
            return Task.FromResult(result);
        }
        public PagedResultDto<GetAllProductImageAllocationDto> GetAllProductImageAllocation(PagedResultRequestDto input, int tenantId)
        {
            var query = from sb in _productImageAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == tenantId)
                        join ca in _productParametersRepository.GetAll() on sb.ProductParameterId equals ca.Id
                        select new GetAllProductImageAllocationDto()
                        {
                            Id = sb.Id,
                            ProductParameterId = ca.Id,
                            ProductParameterName = ca.ProductName,
                            Image = sb.ImageUrl,
                           
                        };

            var result = new PagedResultDto<GetAllProductImageAllocationDto>(query.Count(), ObjectMapper.Map<List<GetAllProductImageAllocationDto>>(query));
            return result;
        }

    }
}
