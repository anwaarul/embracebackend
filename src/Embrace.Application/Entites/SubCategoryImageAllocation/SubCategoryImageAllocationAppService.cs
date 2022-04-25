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
using Embrace.Entites.SubCategoryImageAllocation.Dto;
using Embrace.Entites.SubCategoryImageAllocation;
using System.IO;

namespace Embrace.Entites.SubCategoryImageAllocation
{
    [AbpAuthorize(PermissionNames.LookUps_SubCategoryImageAllocation)]
    public class SubCategoryImageAllocationAppService : AsyncCrudAppService<SubCategoryImageAllocationInfo, SubCategoryImageAllocationDto, long, PagedResultRequestDto, SubCategoryImageAllocationDto, SubCategoryImageAllocationDto>, ISubCategoryImageAllocationAppService
    {

        private readonly IRepository<SubCategoryImageAllocationInfo, long> _subCategoryImageAllocationRepository;
        private readonly IRepository<SubCategoryInfo, long> _subCategoryInfoRepository;
        private readonly IPermissionManager _permissionManager;

        public SubCategoryImageAllocationAppService(IRepository<SubCategoryImageAllocationInfo, long> _repository,
            IRepository<SubCategoryInfo, long> subCategoryInfoRepository,
            IPermissionManager _Manager) : base(_repository)
        {
            _subCategoryInfoRepository = subCategoryInfoRepository;
            _subCategoryImageAllocationRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_SubCategoryImageAllocation_Create)]
        public override async Task<SubCategoryImageAllocationDto> CreateAsync(SubCategoryImageAllocationDto input)
        {

            var result = ObjectMapper.Map<SubCategoryImageAllocationInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _subCategoryImageAllocationRepository.InsertAsync(result);
            result.IsActive = true;
            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<SubCategoryImageAllocationDto>(result);
            return data;

        }

        public async Task<CreateSubCategoryImageAllocationDto> CreateSubCategoryImageAllocation(CreateSubCategoryImageAllocationDto input)
        {
            SubCategoryImageAllocationInfo imagedata = new SubCategoryImageAllocationInfo();
            var subcategory = _subCategoryInfoRepository.GetAll().Where(x => x.Id == input.SubCategoryId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (subcategory == null)
            {
                throw new UserFriendlyException("SubCategory not found.");
            }
            foreach (var imageBase in input.BulkImage)
            {
                if (imageBase.ImageUrl != null)
                {

                    Uri uriResult;
                    bool uRL = Uri.TryCreate(imageBase.ImageUrl, UriKind.Absolute, out uriResult)
                        && uriResult.Scheme == Uri.UriSchemeHttp;
                    if (uRL == true)
                    {
                        imagedata = new SubCategoryImageAllocationInfo
                        {
                            SubCategoryId = subcategory.Id,

                            ImageUrl = imageBase.ImageUrl,
                            CreationTime = DateTime.Now,
                            CreatorUserId = Convert.ToInt32(AbpSession.UserId),
                            TenantId = Convert.ToInt32(AbpSession.TenantId),
                            IsActive = true

                        };
                        await _subCategoryImageAllocationRepository.InsertAsync(imagedata);
                        CurrentUnitOfWork.SaveChanges();

                    }
                    else
                    {
                        var imgUrl = "";

                        imgUrl = SaveImage(imageBase.ImageUrl);


                        imagedata = new SubCategoryImageAllocationInfo
                        {
                            SubCategoryId = subcategory.Id,

                            ImageUrl = imgUrl,
                            CreationTime = DateTime.Now,
                            CreatorUserId = Convert.ToInt32(AbpSession.UserId),
                            TenantId = Convert.ToInt32(AbpSession.TenantId),
                            IsActive = true

                        };
                        await _subCategoryImageAllocationRepository.InsertAsync(imagedata);
                        CurrentUnitOfWork.SaveChanges();

                    }

                }
            }
            var data = ObjectMapper.Map<CreateSubCategoryImageAllocationDto>(imagedata);
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
        [AbpAuthorize(PermissionNames.LookUps_SubCategoryImageAllocation_Update)]
        public override async Task<SubCategoryImageAllocationDto> UpdateAsync(SubCategoryImageAllocationDto input)
        {

            var data = ObjectMapper.Map<SubCategoryImageAllocationInfo>(input);

            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
            data.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _subCategoryImageAllocationRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<SubCategoryImageAllocationDto>(data);
            return result;

        }

        public async Task DeleteSubCategoryImageAllocationbySubCategoryId(long subCategoryId)
        {
            var subcategoryA = _subCategoryImageAllocationRepository.GetAll().Where(x => x.SubCategoryId == subCategoryId && x.TenantId == AbpSession.TenantId).ToList();

            foreach (var subCategory in subcategoryA)
            {
                await _subCategoryImageAllocationRepository.DeleteAsync(subCategory);
                CurrentUnitOfWork.SaveChanges();
            }

        }
        public override Task<PagedResultDto<SubCategoryImageAllocationDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _subCategoryImageAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<SubCategoryImageAllocationDto>(query.Count(), ObjectMapper.Map<List<SubCategoryImageAllocationDto>>(statelist));
            return Task.FromResult(result);
        }
      

    }
}
