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
using Embrace.Entities.Size.Dto;
using Embrace.Entities.Size;

namespace Embrace.Entites.Size
{
    [AbpAuthorize(PermissionNames.LookUps_Size)]
    public class SizeAppService : AsyncCrudAppService<SizeInfo, SizeDto, long, PagedResultRequestDto, SizeDto, SizeDto>, ISizeAppService
    {
        private readonly IRepository<SizeInfo, long> _sizeRepository;
        private readonly IPermissionManager _permissionManager;
        public SizeAppService(IRepository<SizeInfo, long> _repository,

            IPermissionManager _Manager) : base(_repository)
        {

            _sizeRepository = _repository;
            _permissionManager = _Manager;
        }

        [AbpAuthorize(PermissionNames.LookUps_Size_Create)]
        public override async Task<SizeDto> CreateAsync(SizeDto input)
        {
            var dataSize = _sizeRepository.GetAll().Where(x => x.Name == input.Name && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (dataSize != null)
            {
                throw new UserFriendlyException("there is already Size entered with that name");
            }
            var result = ObjectMapper.Map<SizeInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);

            await _sizeRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<SizeDto>(result);
            return data;

        }
        [AbpAuthorize(PermissionNames.LookUps_Size_Update)]
        public override async Task<SizeDto> UpdateAsync(SizeDto input)
        {
            var prevdataSize = _sizeRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            var dataSizeNew = _sizeRepository.GetAll().Where(x => x.Name == input.Name && x.Id != prevdataSize.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (prevdataSize.Name != input.Name)
            {
                if (dataSizeNew != null)
                {
                    throw new UserFriendlyException("there is already Size entered with that name");
                }
            }
            var data = ObjectMapper.Map<SizeInfo>(prevdataSize);
            data.Name = input.Name;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);

            await _sizeRepository.UpdateAsync(data);
            CurrentUnitOfWork.SaveChanges();

            var result = ObjectMapper.Map<SizeDto>(data);
            return result;

        }
        [AbpAuthorize(PermissionNames.LookUps_Size_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var result = _sizeRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
            if (result != null)
            {
                await _sizeRepository.DeleteAsync(result);
                CurrentUnitOfWork.SaveChanges();
            }

        }

        //[AbpAuthorize(PermissionNames.LookUps_Size_Read)]
        public override Task<PagedResultDto<SizeDto>> GetAllAsync(PagedResultRequestDto input)
        {
            var query = _sizeRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<SizeDto>(query.Count(), ObjectMapper.Map<List<SizeDto>>(statelist));
            return Task.FromResult(result);
        }
        public PagedResultDto<SizeDto> GetAllSizeFilter(PagedResultRequestDto input, string filter)
        {

            if (filter == null)
            {
                filter = "";
            }

            var dataSize = _sizeRepository.GetAll().Where(x => x.Name.Contains(filter) && x.TenantId == AbpSession.TenantId).ToList();
            var statelist = dataSize.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<SizeDto>(statelist.Count(), ObjectMapper.Map<List<SizeDto>>(statelist));
            return result;
        }
        public PagedResultDto<SizeDto> GetAllSizeSearchFilter(PagedSizeResultRequestExtendedDto input)
        {
            var query = _sizeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.ToLower().Contains(input.Keyword.ToLower()))
                .ToList();

            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<SizeDto>(query.Count, ObjectMapper.Map<List<SizeDto>>(statelist));
            return result;
        }
        
    }
}
