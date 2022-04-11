using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Embrace.Authorization;
using Embrace.Entites.Category;
using Embrace.Entites.Category.Dto;
using Embrace.Entites.SubCategory.Dto;
using Embrace.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rabat.Entites.Category
{
    [AbpAuthorize(PermissionNames.LookUps_Category)]
    public class CategoryAppService : AsyncCrudAppService<CategoryInfo, CategoryDto, long, PagedResultRequestDto, CategoryDto, CategoryDto>, ICategoryAppService
    {
        private readonly IRepository<CategoryInfo, long> _categoryRepository;
        private readonly IRepository<SubCategoryInfo, long> _subCategoryInfoRepository;
        private readonly IRepository<SubCategoryImageAllocationInfo, long> _subCategoryImageAllocationRepository;
        private readonly IPermissionManager _permissionManager;
        public CategoryAppService(IRepository<CategoryInfo, long> _repository,
            IRepository<SubCategoryInfo, long> subCategoryInfoRepository,
            IRepository<SubCategoryImageAllocationInfo, long> subCategoryImageAllocationRepository,
        IPermissionManager _Manager) : base(_repository)
        {

            _subCategoryImageAllocationRepository = subCategoryImageAllocationRepository;
           
            _subCategoryInfoRepository = subCategoryInfoRepository;
            _categoryRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_Category_Create)]
        public override async Task<CategoryDto> CreateAsync(CategoryDto input)
        {
            var dataCategory = _categoryRepository.GetAll().Where(x => x.Name == input.Name && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (dataCategory != null)
            {
                throw new UserFriendlyException("there is already Category entered with that name");
            }
            var result = ObjectMapper.Map<CategoryInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _categoryRepository.InsertAsync(result);
            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<CategoryDto>(result);
            return data;

        }
        //public async Task<CreateCategorywithImages> CreateCategory(CreateCategorywithImages input)
        //{
        //    var result = new CategoryInfo
        //    {
        //        Name = input.Name,
        //        IsActive = true,
        //        CreatorUserId = Convert.ToInt32(AbpSession.UserId),
        //        TenantId = Convert.ToInt32(AbpSession.TenantId)
        //    };
        //    await _categoryRepository.InsertAsync(result);
        //    CurrentUnitOfWork.SaveChanges();

        //    // SubCategory
        //    foreach (var subCategory in input.BulkSubCategoryId)
        //    {
        //        var subcategory = _subCategoryInfoRepository.GetAll().Where(x => x.Name == subCategory.SubCategoryName && x.TenantId == AbpSession.TenantId).FirstOrDefault();
        //        if (subcategory == null)
        //        {
        //            var imgUrl = "";
        //            imgUrl = SaveImage(subCategory.ImageUrl);
        //            var subCategory1 = new SubCategoryInfo
        //            {
        //                CategoryId = result.Id,
        //                ImageUrl = imgUrl,
        //                Name = subCategory.SubCategoryName,
        //                CreationTime = DateTime.Now,
        //                CreatorUserId = Convert.ToInt32(AbpSession.UserId),
        //                TenantId = Convert.ToInt32(AbpSession.TenantId),
        //                IsActive = true

        //            };
        //            await _subCategoryInfoRepository.InsertAsync(subCategory1);
        //            CurrentUnitOfWork.SaveChanges();
                   
        //            foreach (var imageBase in input.BulkImage)
        //            {
        //                if (imageBase.ImageUrl != null)
        //                {
        //                    Uri uriResult;
        //                    bool uRL = Uri.TryCreate(imageBase.ImageUrl, UriKind.Absolute, out uriResult)
        //                        && uriResult.Scheme == Uri.UriSchemeHttp;
        //                    if (uRL == true)
        //                    {
        //                        var imagedata1 = new SubCategoryImageAllocationInfo
        //                        {
        //                            SubCategoryId = subCategory1.Id,

        //                            ImageUrl = imageBase.ImageUrl,
        //                            CreationTime = DateTime.Now,
        //                            CreatorUserId = Convert.ToInt32(AbpSession.UserId),
        //                            TenantId = Convert.ToInt32(AbpSession.TenantId),
        //                            IsActive = true

        //                        };
        //                        await _subCategoryImageAllocationRepository.InsertAsync(imagedata1);
        //                        CurrentUnitOfWork.SaveChanges();

        //                    }
        //                    else
        //                    {
                               
        //                        imgUrl = SaveImage(imageBase.ImageUrl);


        //                        var imagedata = new SubCategoryImageAllocationInfo
        //                        {
        //                            SubCategoryId = subCategory1.Id,

        //                            ImageUrl = imgUrl,
        //                            CreationTime = DateTime.Now,
        //                            CreatorUserId = Convert.ToInt32(AbpSession.UserId),
        //                            TenantId = Convert.ToInt32(AbpSession.TenantId),
        //                            IsActive = true

        //                        };
        //                        await _subCategoryImageAllocationRepository.InsertAsync(imagedata);
        //                        CurrentUnitOfWork.SaveChanges();

        //                    }

        //                }
        //            }
              
        //        }

        //    }

        //    var data = ObjectMapper.Map<CreateCategorywithImages>(result);
        //    return data;
        //}
        protected string SaveImage(string ImgStr)
        {
            // create path
            String folder = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot\\UserImage\\");
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

            return "http://54.37.97.142:81/" + "/" + "UserImage" + "/" + "/" + imageName;
        }

        [AbpAuthorize(PermissionNames.LookUps_Category_Update)]
        public override async Task<CategoryDto> UpdateAsync(CategoryDto input)
        {
            var prevdataCategory = _categoryRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            var dataCategoryNew = _categoryRepository.GetAll().Where(x => x.Name == input.Name && x.Id != prevdataCategory.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (prevdataCategory.Name != input.Name)
            {
                if (dataCategoryNew != null)
                {
                    throw new UserFriendlyException("there is already Category entered with that name");
                }
            }
            var data = ObjectMapper.Map<CategoryInfo>(prevdataCategory);
            data.Name = input.Name;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _categoryRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<CategoryDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_Category_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _categoryRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _categoryRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_Category_Read)]
        public override Task<PagedResultDto<CategoryDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _categoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<CategoryDto>(query.Count(), ObjectMapper.Map<List<CategoryDto>>(statelist));
            return Task.FromResult(result);
        }
       
        public async Task<String> CreateCategoryBulk(CreateCategoryBulkDto CategoryDto)
        {
            int index = 0;
            foreach (CreateCategoryDto input in CategoryDto.createBulkCategory)
            {
                try
                {
                    index++;

                    var Categorydata = _categoryRepository.GetAll().Where(x => x.Name == input.Name && x.TenantId == AbpSession.TenantId).FirstOrDefault();


                    if (Categorydata != null)
                    {
                        throw new UserFriendlyException("Data is not correct at Row:" + index);
                    }

                    var result = ObjectMapper.Map<CategoryInfo>(input);

                    result.Name = input.Name;

                    result.CreationTime = DateTime.Now;
                    result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
                    result.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    result.IsActive = true;

                    await _categoryRepository.InsertAsync(result);
                    CurrentUnitOfWork.SaveChanges();

                }
                catch
                {
                    throw new UserFriendlyException("Data is not correct at Row:" + index);
                }
            }
            return new String("Category Created Successfully");

        }
    }
}
