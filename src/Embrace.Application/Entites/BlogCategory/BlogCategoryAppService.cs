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
using Embrace.Entities.BlogCategory.Dto;
using Embrace.Entities.BlogCategory;
using System.IO;

namespace Embrace.Entites.BlogCategory
{
    [AbpAuthorize(PermissionNames.LookUps_BlogCategory)]
    public class BlogCategoryAppService : AsyncCrudAppService<BlogCategoryInfo, BlogCategoryDto, long, PagedResultRequestDto, BlogCategoryDto, BlogCategoryDto>, IBlogCategoryAppService
    {
        private readonly IRepository<BlogCategoryInfo, long> _blogCategoryRepository;
        private readonly IPermissionManager _permissionManager;
        public BlogCategoryAppService(IRepository<BlogCategoryInfo, long> _repository,

            IPermissionManager _Manager) : base(_repository)
        {

            _blogCategoryRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_BlogCategory_Create)]
        public override async Task<BlogCategoryDto> CreateAsync(BlogCategoryDto input)
        {
            var imgUrl = "";
            imgUrl = SaveImage(input.ImageUrl);

            var result = ObjectMapper.Map<BlogCategoryInfo>(input);
            result.ImageUrl = imgUrl;
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _blogCategoryRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<BlogCategoryDto>(result);
            return data;

        }
        protected string SaveImage(string ImgStr)
        {
            // create path
            String folder = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot\\BlogImage\\");
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

            return "http://54.37.97.142:81/" + "/" + "BlogImage" + "/" + "/" + imageName;
        }

        [AbpAuthorize(PermissionNames.LookUps_BlogCategory_Update)]
        public override async Task<BlogCategoryDto> UpdateAsync(BlogCategoryDto input)
        {
            var prevdataBlogCategory = _blogCategoryRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            var imgUrl = "";
            imgUrl = SaveImage(input.ImageUrl);

            var data = ObjectMapper.Map<BlogCategoryInfo>(prevdataBlogCategory);
            data.Name = input.Name;
            data.ColourCode = input.ColourCode;
            data.ImageUrl = imgUrl;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _blogCategoryRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<BlogCategoryDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_BlogCategory_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _blogCategoryRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _blogCategoryRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_BlogCategory_Read)]
        public override Task<PagedResultDto<BlogCategoryDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _blogCategoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<BlogCategoryDto>(query.Count(), ObjectMapper.Map<List<BlogCategoryDto>>(statelist));
            return Task.FromResult(result);
        }
        public PagedResultDto<BlogCategoryDto> GetAllBlogCategoryFilter(PagedResultRequestDto input, string filter)
        {

            if (filter == null)
            {
                filter = "";
            }

            var dataBlogCategory = _blogCategoryRepository.GetAll().Where(x => x.Name.Contains(filter) && x.TenantId == AbpSession.TenantId).ToList();
            var statelist = dataBlogCategory.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<BlogCategoryDto>(statelist.Count(), ObjectMapper.Map<List<BlogCategoryDto>>(statelist));
            return result;
        }
       
    }
}
