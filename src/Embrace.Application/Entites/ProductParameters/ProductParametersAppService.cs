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
using Embrace.Entities.ProductParameters.Dto;
using Embrace.Entities.ProductParameters;
using System.IO;

namespace Embrace.Entites.ProductParameters
{
    [AbpAuthorize(PermissionNames.LookUps_ProductParameters)]
    public class ProductParametersAppService : AsyncCrudAppService<ProductParametersInfo, ProductParametersDto, long, PagedResultRequestDto, ProductParametersDto, ProductParametersDto>, IProductParametersAppService
    {
        private readonly IRepository<ProductParametersInfo, long> _productParametersRepository;
        private readonly IRepository<CategoryInfo, long> _categoriesRepository;
        private readonly IPermissionManager _permissionManager;
        public ProductParametersAppService(
            IRepository<ProductParametersInfo, long> _repository,
            IRepository<CategoryInfo, long> categoriesRepository,


            IPermissionManager _Manager) : base(_repository)
        {

            _productParametersRepository = _repository;
            _categoriesRepository = categoriesRepository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_ProductParameters_Create)]
        public override async Task<ProductParametersDto> CreateAsync(ProductParametersDto input)
        {
            var dataproduct = _productParametersRepository.GetAll().Where(x => x.ProductName == input.ProductName && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (dataproduct != null)
            {
                throw new UserFriendlyException("there is already Product entered with that name");
            }
            var imgUrl = "";
            imgUrl = SaveImage(input.ProductImage);

            var result = ObjectMapper.Map<ProductParametersInfo>(input);
            result.ProductImage = imgUrl;
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _productParametersRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<ProductParametersDto>(result);
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

        [AbpAuthorize(PermissionNames.LookUps_ProductParameters_Update)]
        public override async Task<ProductParametersDto> UpdateAsync(ProductParametersDto input)
        {
            var prevdataProductParameters = _productParametersRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            var imgUrl = "";
            imgUrl = SaveImage(input.ProductImage);

            var data = ObjectMapper.Map<ProductParametersInfo>(prevdataProductParameters);
            data.ProductCategoryId = input.ProductCategoryId;
            data.SKU = input.SKU;
            data.ProductName = input.ProductName;
            data.Description = input.Description;
            data.Price = input.Price;
            data.SalePrice = input.SalePrice;
            data.ProductImage = imgUrl;
            data.Quantity = input.Quantity;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
            data.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _productParametersRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<ProductParametersDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_ProductParameters_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _productParametersRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _productParametersRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_ProductParameters_Read)]
        public override Task<PagedResultDto<ProductParametersDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _productParametersRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<ProductParametersDto>(query.Count(), ObjectMapper.Map<List<ProductParametersDto>>(statelist));
            return Task.FromResult(result);
        }

    }
}
