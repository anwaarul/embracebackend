using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Embrace.Authorization;
using Embrace.Authorization.Users;
using Embrace.Entities.BlogBlogCategoryAllocation.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.BlogBlogCategoryAllocation
{
    [AbpAuthorize(PermissionNames.LookUps_BlogBlogCategoryAllocation)]
    public class BlogBlogCategoryAllocationAppService : AsyncCrudAppService<BlogBlogCategoryAllocationInfo, BlogBlogCategoryAllocationDto, long, PagedResultRequestDto, CreateBlogBlogCategoryAllocationDto, BlogBlogCategoryAllocationDto>, IBlogBlogCategoryAllocationAppService
    {
        private readonly IRepository<BlogBlogCategoryAllocationInfo, long> _BlogBlogCategoryAllocationRepository;
        private readonly IRepository<BlogInfo, long> _blogRepository;
        private readonly IRepository<BlogCategoryInfo, long> _blogCategoryRepository;
        
        private readonly UserManager _userManager;
        private readonly IPermissionManager _permissionManager;

        public BlogBlogCategoryAllocationAppService(
            IRepository<BlogBlogCategoryAllocationInfo, long> _repository,
            IRepository<BlogInfo, long> blogRepository,
            IRepository<BlogCategoryInfo, long> blogCategoryRepository,
            
             IRepository<User, long> repository,
            UserManager userManager,
            IPermissionManager _Manager) : base(_repository)
        {
            _userManager = userManager;
            _BlogBlogCategoryAllocationRepository = _repository;
            _blogRepository = blogRepository;           
            _permissionManager = _Manager;
            _blogCategoryRepository = blogCategoryRepository;
            
        }

        //[AbpAuthorize(PermissionNames.LookUps_BlogBlogCategoryAllocation_Create)]
        public override async Task<BlogBlogCategoryAllocationDto> CreateAsync(CreateBlogBlogCategoryAllocationDto input)
        {
            //Blog check
            var BlogData = _blogRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id== input.BlogId).FirstOrDefault();
            if (BlogData == null)
            {
                throw new UserFriendlyException("No Blog Found to Allocate");
            }
            //BlogCategory/s check
            var blogCategoryId = input.BlogCategoryId.Split(',').Select(long.Parse).ToList();
            foreach (var i in blogCategoryId)
            {
                var BlogCategoryData = _blogCategoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id == i).FirstOrDefault();
                if (BlogCategoryData == null)
                {
                    throw new UserFriendlyException("No BlogCategory/s Found to Allocate");
                }
            }

            List<BlogBlogCategoryAllocationInfo> BlogBlogCategoryAllocationInfos = new List<BlogBlogCategoryAllocationInfo>();
            blogCategoryId = input.BlogCategoryId.Split(',').Select(long.Parse).ToList();

            var AllocatedParentData = _BlogBlogCategoryAllocationRepository.GetAll().Where(
               t => blogCategoryId.Contains(t.BlogCategoryId) && t.BlogId == input.BlogId).ToList();
            foreach (var BlogCategory in blogCategoryId)
            {
                var ifExists = AllocatedParentData.FindAll(x => x.BlogCategoryId == BlogCategory && x.BlogId == input.BlogId).Count;
                if (ifExists != 0)
                {
                    throw new UserFriendlyException("Allocation already exists");
                }
                
                    BlogBlogCategoryAllocationInfo BlogBlogCategoryAllocationInfo = new BlogBlogCategoryAllocationInfo
                    {

                        BlogId = input.BlogId,
                        BlogCategoryId = BlogCategory
                    };

                    var result = ObjectMapper.Map<BlogBlogCategoryAllocationInfo>(BlogBlogCategoryAllocationInfo);
                    result.CreationTime = DateTime.Now;
                    result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
                    result.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    result.IsActive = true;
                    await _BlogBlogCategoryAllocationRepository.InsertAsync(result);
                    CurrentUnitOfWork.SaveChanges();
                    BlogBlogCategoryAllocationInfos.Add(result);
                
            }
            var data = new BlogBlogCategoryAllocationDto();
            return data;

        }

        //[AbpAuthorize(PermissionNames.LookUps_BlogBlogCategoryAllocation_Update)]
        public override async Task<BlogBlogCategoryAllocationDto> UpdateAsync(BlogBlogCategoryAllocationDto input)
        {

            var data = ObjectMapper.Map<BlogBlogCategoryAllocationInfo>(input);
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
            data.IsActive = true;
            data.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _BlogBlogCategoryAllocationRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<BlogBlogCategoryAllocationDto>(data);
            return result;
        }

        //[AbpAuthorize(PermissionNames.LookUps_BlogBlogCategoryAllocation_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {

            var result = _BlogBlogCategoryAllocationRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _BlogBlogCategoryAllocationRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }
        //[AbpAuthorize(PermissionNames.LookUps_BlogBlogCategoryAllocation_Read)]
        public override Task<PagedResultDto<BlogBlogCategoryAllocationDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _BlogBlogCategoryAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<BlogBlogCategoryAllocationDto>(query.Count(), ObjectMapper.Map<List<BlogBlogCategoryAllocationDto>>(statelist));
            return Task.FromResult(result);

        }

        public PagedResultDto<GetAllBlogBlogCategoryAllocationDto> GetAllBlogBlogCategoryAllocation(PagedResultRequestDto input)
        {

            var query = from bca in _BlogBlogCategoryAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId)
                        join b in _blogRepository.GetAll() on bca.BlogId equals b.Id
                        join bc in _blogCategoryRepository.GetAll() on bca.BlogCategoryId equals bc.Id

                        select new GetAllBlogBlogCategoryAllocationDto()
                        {
                            Id = bca.Id,
                            BlogId = bca.BlogId,
                            BlogName = b.Title,
                            BlogCategoryId = bca.BlogCategoryId,
                            BlogCategoryName = bc.Name,

                        };

            var dataList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<GetAllBlogBlogCategoryAllocationDto>(query.Count(), ObjectMapper.Map<List<GetAllBlogBlogCategoryAllocationDto>>(dataList));

            return result;

        }

    }
}
