using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Embrace.Authorization;
using Embrace.Authorization.Users;
using Embrace.Entities.UserProductAllocation.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.UserProductAllocation
{
    [AbpAuthorize(PermissionNames.LookUps_UserProductAllocation)]
    public class UserProductAllocationAppService : AsyncCrudAppService<UserProductAllocationInfo, UserProductAllocationDto, long, PagedResultRequestDto, CreateUserProductAllocationDto, UserProductAllocationDto>, IUserProductAllocationAppService
    {
        private readonly IRepository<UserProductAllocationInfo, long> _UserProductAllocationRepository;
        private readonly IRepository<ProductParametersInfo, long> _productParametersRepository;
        private readonly IRepository<UniqueNameAndDateInfo, long> _uniqueNameAndDateInfoRepository;
        
        private readonly UserManager _userManager;
        private readonly IPermissionManager _permissionManager;

        public UserProductAllocationAppService(
            IRepository<UserProductAllocationInfo, long> _repository,
            IRepository<ProductParametersInfo, long> productParametersRepository,
            IRepository<UniqueNameAndDateInfo, long> uniqueNameAndDateInfoRepository,
            
             IRepository<User, long> repository,
            UserManager userManager,
            IPermissionManager _Manager) : base(_repository)
        {
            _userManager = userManager;
            _UserProductAllocationRepository = _repository;
            _productParametersRepository = productParametersRepository;           
            _permissionManager = _Manager;
            _uniqueNameAndDateInfoRepository = uniqueNameAndDateInfoRepository;
            
        }

        //[AbpAuthorize(PermissionNames.LookUps_UserProductAllocation_Create)]
        public override async Task<UserProductAllocationDto> CreateAsync(CreateUserProductAllocationDto input)
        {
            //User check
            var UserData = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id== input.UserId).FirstOrDefault();
            if (UserData == null)
            {
                throw new UserFriendlyException("No User Found to Allocate");
            }
            //ProductParameter/s check
            var productParameterId = input.ProductId.Split(',').Select(long.Parse).ToList();
            foreach (var i in productParameterId)
            {
                var ProductParameterData = _productParametersRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id == i).FirstOrDefault();
                if (ProductParameterData == null)
                {
                    throw new UserFriendlyException("No Product/s Found to Allocate");
                }
            }

            List<UserProductAllocationInfo> UserProductAllocationInfos = new List<UserProductAllocationInfo>();
            productParameterId = input.ProductId.Split(',').Select(long.Parse).ToList();

            var AllocatedParentData = _UserProductAllocationRepository.GetAll().Where(
               t => productParameterId.Contains(t.ProductId) && t.UserId == input.UserId).ToList();
            foreach (var Product in productParameterId)
            {
                var ifExists = AllocatedParentData.FindAll(x => x.ProductId == Product && x.UserId == input.UserId).Count;
                if (ifExists != 0)
                {
                    throw new UserFriendlyException("Allocation already exists");
                }
                
                    UserProductAllocationInfo UserProductAllocationInfo = new UserProductAllocationInfo
                    {

                        UserId = input.UserId,
                        ProductId = Product
                    };

                    var result = ObjectMapper.Map<UserProductAllocationInfo>(UserProductAllocationInfo);
                    result.CreationTime = DateTime.Now;
                    result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
                    result.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    result.IsActive = true;
                    await _UserProductAllocationRepository.InsertAsync(result);
                    CurrentUnitOfWork.SaveChanges();
                    UserProductAllocationInfos.Add(result);
                
            }
            var data = new UserProductAllocationDto();
            return data;

        }

        //[AbpAuthorize(PermissionNames.LookUps_UserProductAllocation_Update)]
        public override async Task<UserProductAllocationDto> UpdateAsync(UserProductAllocationDto input)
        {

            var data = ObjectMapper.Map<UserProductAllocationInfo>(input);
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
            data.IsActive = true;
            data.TenantId = Convert.ToInt32(AbpSession.TenantId);
            await _UserProductAllocationRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<UserProductAllocationDto>(data);
            return result;
        }

        //[AbpAuthorize(PermissionNames.LookUps_UserProductAllocation_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {

            var result = _UserProductAllocationRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _UserProductAllocationRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }
        //[AbpAuthorize(PermissionNames.LookUps_UserProductAllocation_Read)]
        public override Task<PagedResultDto<UserProductAllocationDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _UserProductAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<UserProductAllocationDto>(query.Count(), ObjectMapper.Map<List<UserProductAllocationDto>>(statelist));
            return Task.FromResult(result);

        }

        public PagedResultDto<GetAllUserProductAllocationDto> GetAllUserProductAllocation(PagedResultRequestDto input)
        {

            var query = from sp in _UserProductAllocationRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId)
                        join pp in _productParametersRepository.GetAll() on sp.ProductId equals pp.Id
                        join u in _uniqueNameAndDateInfoRepository.GetAll() on sp.UserId equals u.Id

                        select new GetAllUserProductAllocationDto()
                        {
                            Id = sp.Id,
                            UserId = sp.UserId,
                            UserName = u.Name,
                            UniqueKey = u.UniqueKey,
                            ProductId = sp.ProductId,
                            ProductName = pp.ProductName
                        };

            var dataList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<GetAllUserProductAllocationDto>(query.Count(), ObjectMapper.Map<List<GetAllUserProductAllocationDto>>(dataList));

            return result;

        }

    }
}
