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
using Embrace.Entities.Blog.Dto;
using Embrace.Entities.Blog;
using System.IO;

namespace Embrace.Entites.Blog
{
    [AbpAuthorize(PermissionNames.LookUps_Blog)]
    public class BlogAppService : AsyncCrudAppService<BlogInfo, GetAllBlogDto, long, PagedResultRequestDto, GetAllBlogDto, GetAllBlogDto>, IBlogAppService
    {
        private readonly IRepository<BlogInfo, long> _blogRepository;
        private readonly IRepository<BlogCategoryInfo, long> _blogCategoryRepository;
        private readonly IPermissionManager _permissionManager;
        public BlogAppService(
            IRepository<BlogInfo, long> _repository,
            IRepository<BlogCategoryInfo, long> blogCategoryRepository,

            IPermissionManager _Manager) : base(_repository)
        {

            _blogRepository = _repository;
            _blogCategoryRepository = blogCategoryRepository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_Blog_Create)]
        public override async Task<GetAllBlogDto> CreateAsync(GetAllBlogDto input)
        {
            var imgUrl = "";
            imgUrl = SaveImage(input.ImageUrl);

            var result = ObjectMapper.Map<BlogInfo>(input);
            result.ImageUrl = imgUrl;
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _blogRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<GetAllBlogDto>(result);
            return data;

        }
        [AbpAuthorize(PermissionNames.LookUps_Blog_Update)]
        public override async Task<GetAllBlogDto> UpdateAsync(GetAllBlogDto input)
        {
            var prevdataBlog = _blogRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            var imgUrl = "";
            imgUrl = SaveImage(input.ImageUrl);

            var data = ObjectMapper.Map<BlogInfo>(prevdataBlog);
            data.Title = input.Title;
            data.Description = input.Description;
            data.CategoryId = input.CategoryId;
            data.ImageUrl = imgUrl;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _blogRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<GetAllBlogDto>(data);
            return result;

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
        [AbpAuthorize(PermissionNames.LookUps_Blog_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _blogRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _blogRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_Blog_Read)]
        public override Task<PagedResultDto<GetAllBlogDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _blogRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<GetAllBlogDto>(query.Count(), ObjectMapper.Map<List<GetAllBlogDto>>(statelist));
            return Task.FromResult(result);
        }
        public PagedResultDto<GetAllBlogDto> GetAllBlogFilter(PagedResultRequestDto input, string filter)
        {

            if (filter == null)
            {
                filter = "";
            }

            var dataBlog = _blogRepository.GetAll().Where(x => x.Title.Contains(filter) && x.TenantId == AbpSession.TenantId).ToList();
            var statelist = dataBlog.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<GetAllBlogDto>(statelist.Count(), ObjectMapper.Map<List<GetAllBlogDto>>(statelist));
            return result;
        }
        public PagedResultDto<GetAllBlogDto> GetAllBlogs(PagedResultRequestDto input)
        {

            var query = from b in _blogRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId)
                        join bc in _blogCategoryRepository.GetAll() on b.CategoryId equals bc.Id
                        
                        select new GetAllBlogDto()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Description = b.Description,
                            CategoryId = b.CategoryId,
                            CategoryName = bc.Name,
                            ImageUrl = b.ImageUrl
                        };

            var dataList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<GetAllBlogDto>(query.Count(), ObjectMapper.Map<List<GetAllBlogDto>>(dataList));

            return result;

        }
        public PagedResultDto<GetAllBlogDto> GetAllBlogsByCategoryId(PagedResultRequestDto input, long categoryId)
        {

            var query = from b in _blogRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId
                        && x.CategoryId == categoryId)
                        join bc in _blogCategoryRepository.GetAll() on b.CategoryId equals bc.Id

                        select new GetAllBlogDto()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Description = b.Description,
                            CategoryId = b.CategoryId,
                            CategoryName = bc.Name,
                            ImageUrl = b.ImageUrl
                        };

            var dataList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<GetAllBlogDto>(query.Count(), ObjectMapper.Map<List<GetAllBlogDto>>(dataList));

            return result;
        }
         
    }
}
