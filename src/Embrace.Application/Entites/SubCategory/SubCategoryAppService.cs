using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Embrace.Authorization;
using Embrace.Entites.SubCategory;
using Embrace.Entites.SubCategory.Dto;
using Embrace.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rabat.Entites.SubCategory
{
    [AbpAuthorize(PermissionNames.LookUps_SubCategory)]
    public class SubCategoryAppService : AsyncCrudAppService<SubCategoryInfo, SubCategoryDto, long, PagedResultRequestDto, SubCategoryDto, SubCategoryDto>, ISubCategoryAppService
    {
        private readonly IRepository<SubCategoryInfo, long> _subCategoryRepository;
        private readonly IRepository<CategoryInfo, long> _categoryRepository;
        private readonly IPermissionManager _permissionManager;
        public SubCategoryAppService(IRepository<SubCategoryInfo, long> _repository,
              IRepository<CategoryInfo, long> categoryRepository,
            IPermissionManager _Manager) : base(_repository)
        {

            _categoryRepository = categoryRepository;
            _subCategoryRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_SubCategory_Create)]
        public override async Task<SubCategoryDto> CreateAsync(SubCategoryDto input)
        {
            SubCategoryInfo subCategory = new SubCategoryInfo();
            var dataSubCategory = _subCategoryRepository.GetAll().Where(x => x.Name == input.Name && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            var dataCategory = _categoryRepository.GetAll().Where(x => x.Id == input.CategoryId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (dataCategory == null)
            {
                throw new UserFriendlyException("Category is not valid");
            }

            if (dataSubCategory == null)
            {
                var imgUrl = "";
                imgUrl = SaveImage(input.ImageUrl);
                subCategory = ObjectMapper.Map<SubCategoryInfo>(input);
                subCategory.Name = input.Name;
                subCategory.CategoryId = dataCategory.Id;
                subCategory.ImageUrl = imgUrl;
                subCategory.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
                subCategory.TenantId = Convert.ToInt32(AbpSession.TenantId);

                await _subCategoryRepository.InsertAsync(subCategory);

                subCategory.IsActive = true;

                CurrentUnitOfWork.SaveChanges();
                
            }
            var data = ObjectMapper.Map<SubCategoryDto>(subCategory);
            return data;

        }
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
        [AbpAuthorize(PermissionNames.LookUps_SubCategory_Update)]
        public override async Task<SubCategoryDto> UpdateAsync(SubCategoryDto input)
        {
            var prevdataSubCategory = _subCategoryRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            var dataSubCategoryNew = _subCategoryRepository.GetAll().Where(x => x.Name == input.Name && x.Id != prevdataSubCategory.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (prevdataSubCategory.Name != input.Name)
            {
                if (dataSubCategoryNew != null)
                {
                    throw new UserFriendlyException("there is already SubCategory entered with that name");
                }
            }
            var dataCategory = _categoryRepository.GetAll().Where(x => x.Id == input.CategoryId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (dataCategory == null)
            {
                throw new UserFriendlyException("Category is not valid");
            }
            var imgUrl = "";
            imgUrl = SaveImage(input.ImageUrl);
            var data = ObjectMapper.Map<SubCategoryInfo>(prevdataSubCategory);
            data.Name = input.Name;
            data.ImageUrl = imgUrl;
            data.CategoryId = dataCategory.Id;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _subCategoryRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<SubCategoryDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_SubCategory_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _subCategoryRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _subCategoryRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_SubCategory_Read)]
        public override Task<PagedResultDto<SubCategoryDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _subCategoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<SubCategoryDto>(query.Count(), ObjectMapper.Map<List<SubCategoryDto>>(statelist));
            return Task.FromResult(result);
        }
       
        public async Task<String> CreateSubCategoryBulk(CreateSubCategoryBulkDto SubCategoryDto)
        {
            int index = 0;
            foreach (CreateSubCategoryDto input in SubCategoryDto.createBulkSubCategory)
            {
                try
                {
                    index++;

                    var SubCategorydata = _subCategoryRepository.GetAll().Where(x => x.Name == input.Name && x.TenantId == AbpSession.TenantId).FirstOrDefault();


                    if (SubCategorydata != null)
                    {
                        throw new UserFriendlyException("Data is not correct at Row:" + index);
                    }
                    var dataCategory = _categoryRepository.GetAll().Where(x => x.Name == input.CategoryName && x.TenantId == AbpSession.TenantId).FirstOrDefault();
                    if (dataCategory == null)
                    {
                        throw new UserFriendlyException("Category is not valid");
                    }
                    var imgUrl = "";
                    imgUrl = SaveImage(input.ImageUrl);
                    var result = ObjectMapper.Map<SubCategoryInfo>(input);

                    result.Name = input.Name;
                    result.ImageUrl = imgUrl;
                    result.CategoryId = dataCategory.Id;
                    result.CreationTime = DateTime.Now;
                    result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
                    result.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    result.IsActive = true;

                    await _subCategoryRepository.InsertAsync(result);
                    CurrentUnitOfWork.SaveChanges();

                }
                catch
                {
                    throw new UserFriendlyException("Data is not correct at Row:" + index);
                }
            }
            return new String("SubCategory Created Successfully");

        }
        public PagedResultDto<GetAllSubCategoryDto> GetAllSubCategory (PagedResultRequestDto input)
        {
            var query = from sb in _subCategoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId)
                        join ca in _categoryRepository.GetAll() on sb.CategoryId equals ca.Id
                        select new GetAllSubCategoryDto()
                        {
                            Id = sb.Id,
                            CategoryId = ca.Id,
                            CategoryName = ca.Name,
                            Name = sb.Name,
                            ImageUrl = sb.ImageUrl,
                        };
            
            var result = new PagedResultDto<GetAllSubCategoryDto>(query.Count(), ObjectMapper.Map<List<GetAllSubCategoryDto>>(query));
            return result;
        }

    }
}
