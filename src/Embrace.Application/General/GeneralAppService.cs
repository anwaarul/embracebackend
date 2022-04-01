using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Embrace.Authorization.Users;
using Embrace.Entities;
using Embrace.General.Dto;

namespace Embrace.General
{
    public class GeneralAppService : EmbraceAppServiceBase, IGeneralAppService
    {
        // Lookups
        private readonly IRepository<UniqueNameAndDateInfo, long> _uniqueNameAndDateInfoRepository;       
        private readonly UserManager _userManager;

        public GeneralAppService(
        IRepository<User, long> repository,
        UserManager userManager,
        IRepository<UniqueNameAndDateInfo, long> uniqueNameAndDateInfoRepository
       
          ) : base()
        {
            _userManager = userManager;
            _uniqueNameAndDateInfoRepository = uniqueNameAndDateInfoRepository;

        }

        public async Task<UniqueNameAndDateUniqueKeyDto> CreateUniqueKeyWithNameAndDateTime(UniqueNameAndDateDto input)
        {
            //Converting DateTime to Date(string)
            var Date = input.DateAndTime.ToShortDateString();            

            //Removing Extra Spaces from Name
            
            var nameWithoutSpaces = input.Name.Replace(" ", String.Empty);

            var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.Name == input.Name && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (dataUniqueKey != null)
            {
                throw new UserFriendlyException("there is already Name entered with that name");
            }
            var result = ObjectMapper.Map<UniqueNameAndDateInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);
            result.UniqueKey = nameWithoutSpaces + Date;

            await _uniqueNameAndDateInfoRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<UniqueNameAndDateUniqueKeyDto>(result);
            return data;

        }
        public Task<PagedResultDto<UniqueNameAndDateUniqueKeyDto>> GetAllWithUniqueKey(PagedResultRequestDto input, string uniqueKey)
        {
            var query = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.UniqueKey == uniqueKey);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<UniqueNameAndDateUniqueKeyDto>(query.Count(), ObjectMapper.Map<List<UniqueNameAndDateUniqueKeyDto>>(statelist));
            return Task.FromResult(result);
        }
    }
}
