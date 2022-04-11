using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Embrace.Authorization.Users;
using Embrace.Entites.SubCategory.Dto;
using Embrace.Entities;
using Embrace.General.Dto;

namespace Embrace.General
{
    public class GeneralAppService : EmbraceAppServiceBase, IGeneralAppService
    {
        // Lookups
        private readonly IRepository<UniqueNameAndDateInfo, long> _uniqueNameAndDateInfoRepository;
        private readonly IRepository<SubCategoryInfo, long> _subCategoryRepository;
        private readonly IRepository<CategoryInfo, long> _categoryRepository;
        private readonly IRepository<SubCategoryImageAllocationInfo, long> _subCategoryImageAllocationRepository;
        private readonly IRepository<MenstruationDetailsInfo, long> _menstruationDetailsRepository;
        private readonly IRepository<SubCategoryAndDateInfo, long> _subCategoryAndDateRepository;
        private readonly UserManager _userManager;

        public GeneralAppService(
        IRepository<User, long> repository,
        UserManager userManager,
        IRepository<UniqueNameAndDateInfo, long> uniqueNameAndDateInfoRepository,
        IRepository<SubCategoryAndDateInfo, long> subCategoryAndDateRepository,
        IRepository<MenstruationDetailsInfo, long> menstruationDetailsRepository,
        IRepository<SubCategoryImageAllocationInfo, long> subCategoryImageAllocationRepository,
        IRepository<SubCategoryInfo, long> subCategoryRepository,
        IRepository<CategoryInfo, long> categoryRepository

          ) : base()
        {
            _userManager = userManager;
            _subCategoryAndDateRepository = subCategoryAndDateRepository;
            _subCategoryImageAllocationRepository = subCategoryImageAllocationRepository;
            _menstruationDetailsRepository = menstruationDetailsRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _uniqueNameAndDateInfoRepository = uniqueNameAndDateInfoRepository;

        }

        public async Task<UniqueNameAndDateUniqueKeyDto> CreateUniqueKeyWithNameAndDateTime(UniqueNameAndDateDto input)
        {
            //Converting DateTime to Date(string)
            var Date = input.DateAndTime.ToShortDateString();

            //Removing Extra Spaces from Name

            var nameWithoutSpaces = input.Name.Replace(" ", String.Empty);

          
            var result = ObjectMapper.Map<UniqueNameAndDateInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);
            result.UniqueKey = nameWithoutSpaces + Date;
            var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.Name == input.Name && x.DateAndTime.Date == input.DateAndTime.Date && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            if (dataUniqueKey != null)
            {
                throw new UserFriendlyException("there is already Name entered with that" , result.UniqueKey);
            }
            await _uniqueNameAndDateInfoRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<UniqueNameAndDateUniqueKeyDto>(result);
            return data;

        }
        public async Task<MenstruationDetailsInfo> CreateMenstruationDetails(MenstruationDetailsDto input)
        {
            var uniquekey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == input.UniqueKey && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (uniquekey == null)
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
            
            GetAllMenstruationDetails menstruation =new GetAllMenstruationDetails();
            menstruation = GetAllMenstruation(uniquekey.StartDatePeriod, uniquekey.EndDatePeriod);
            
            var result = new MenstruationDetailsInfo();
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);
            result.UniqueKey = uniquekey.UniqueKey;
            result.MyCycle = menstruation.MyCycle;
            result.Ovulation_day = menstruation.Ovulation_day;
            result.Last_ovulation = menstruation.Last_ovulation;
            result.Next_mens = menstruation.Next_mens;
            result.Next_ovulation = menstruation.Next_ovulation;
            result.Ovulation_window1 = menstruation.Ovulation_window1;
            result.Ovulation_window2 = menstruation.Ovulation_window2;
            result.Ovulation_window3 = menstruation.Ovulation_window3;
            result.Safe_period1 = menstruation.Safe_period1;
            result.Safe_period2 = menstruation.Safe_period2;
            result.Safe_period3 = menstruation.Safe_period3;
            result.Safe_period4 = menstruation.Safe_period4;
            
            await _menstruationDetailsRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<MenstruationDetailsInfo>(result);
            return data;

        }

        public async Task<SubCategoryAndDateDto> CreateSubCategoryAndDate(SubCategoryAndDateDto input)
        {
            var uniquekey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == input.UniqueKey && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (uniquekey == null)
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
            var subCategory = _subCategoryRepository.GetAll().Where(x => x.Id == input.SubCategoryId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (uniquekey == null)
            {
                throw new UserFriendlyException("Invalid SubCategory");
            }

            var result = ObjectMapper.Map<SubCategoryAndDateInfo>(input);
            result.CreatorUserId = Convert.ToInt32(AbpSession.UserId);
            result.TenantId = Convert.ToInt32(AbpSession.TenantId);
            result.UniqueKey = uniquekey.UniqueKey;
            result.DateAndTime = input.DateAndTime;
            result.SubCategoryId = input.SubCategoryId;
            
            await _subCategoryAndDateRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<SubCategoryAndDateDto>(result);
            return data;

        }
        public Task<PagedResultDto<UniqueNameAndDateUniqueKeyDto>> GetAllWithUniqueKey(PagedResultRequestDto input, string uniqueKey)
        {
            var query = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.UniqueKey == uniqueKey);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<UniqueNameAndDateUniqueKeyDto>(query.Count(), ObjectMapper.Map<List<UniqueNameAndDateUniqueKeyDto>>(statelist));
            return Task.FromResult(result);
        }
        protected GetAllMenstruationDetails GetAllMenstruation(DateTime currentMensDate, DateTime previuosMensDate)
        {
            var date_diff = (currentMensDate.Date - previuosMensDate.Date);
            var ovulation_day = (date_diff.Days - 14);
            var last_ovulation = (currentMensDate.AddDays(-14));
            var next_mens = (currentMensDate.AddDays(date_diff.Days));
            var next_ovulation = (next_mens.AddDays(-14));
            var ovulation_window1 = (currentMensDate.AddDays(-18));
            var ovulation_window2 = (currentMensDate.AddDays(-14));
            var ovulation_window3 = (next_mens.AddDays(-18));
            var ovulation_window4 = (next_mens.AddDays(-14));
            var safe_period1 = currentMensDate.Date;
            var safe_period2 = (currentMensDate.AddDays(9));
            var safe_period3 = (currentMensDate.AddDays(15));
            var safe_period4 = (currentMensDate.AddDays(37));


            GetAllMenstruationDetails getAllMenstruationDetails = new GetAllMenstruationDetails
            {

                MyCycle = date_diff.Days,
                Ovulation_day = ovulation_day,
                Last_ovulation = last_ovulation.Date,
                Next_mens = next_mens.Date,
                Next_ovulation = next_ovulation.Date,
                Ovulation_window1 = ovulation_window1.Date,
                Ovulation_window2 = ovulation_window2.Date,
                Ovulation_window3 = ovulation_window3.Date,
                Ovulation_window4 = ovulation_window4.Date,
                Safe_period1 = safe_period1.Date,
                Safe_period2 = safe_period2.Date,
                Safe_period3 = safe_period3.Date,
                Safe_period4 = safe_period4.Date,

            };
            return getAllMenstruationDetails;
        }
        public GetAllMenstruationDetails GetAllMenstruationDetailsbyDate(DateTime currentMensDate , DateTime previuosMensDate)
        {
            var date_diff = (currentMensDate.Date - previuosMensDate.Date);  
            var ovulation_day = (date_diff.Days - 14);
            var last_ovulation = (currentMensDate.AddDays(-14));
            var next_mens = (currentMensDate.AddDays(date_diff.Days));
            var next_ovulation = (next_mens.AddDays(-14));
            var ovulation_window1 = (currentMensDate.AddDays(-18));
            var ovulation_window2 = (currentMensDate.AddDays(-14));
            var ovulation_window3 = (next_mens.AddDays(-18));
            var ovulation_window4 = (next_mens.AddDays(-14));
            var safe_period1 = currentMensDate.Date;    
            var safe_period2 = (currentMensDate.AddDays(9));
            var safe_period3 = (currentMensDate.AddDays(15));
            var safe_period4 = (currentMensDate.AddDays(37));


            GetAllMenstruationDetails getAllMenstruationDetails = new GetAllMenstruationDetails
            {

                MyCycle = date_diff.Days,
                Ovulation_day = ovulation_day,
                Last_ovulation = last_ovulation.Date,
                Next_mens = next_mens.Date,
                Next_ovulation = next_ovulation.Date,
                Ovulation_window1 = ovulation_window1.Date,
                Ovulation_window2 = ovulation_window2.Date,
                Ovulation_window3 = ovulation_window3.Date,
                Ovulation_window4 = ovulation_window4.Date,
                Safe_period1 = safe_period1.Date,
                Safe_period2 = safe_period2.Date,
                Safe_period3 = safe_period3.Date,
                Safe_period4 = safe_period4.Date,

            };
            return getAllMenstruationDetails;
        }
        public GetAllMenstruationDetails GetAllMenstruationDetailsbyUniqueKey(string uniquekey)
        {
            var menstruationdata = _menstruationDetailsRepository.GetAll().Where(x => x.UniqueKey == uniquekey && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            GetAllMenstruationDetails getAllMenstruationDetails = new GetAllMenstruationDetails
            {
                UniqueKey = menstruationdata.UniqueKey,
                MyCycle = menstruationdata.MyCycle,
                Ovulation_day = menstruationdata.Ovulation_day,
                Last_ovulation = menstruationdata.Last_ovulation,
                Next_mens = menstruationdata.Next_mens,
                Next_ovulation = menstruationdata.Next_ovulation,
                Ovulation_window1 = menstruationdata.Ovulation_window1,
                Ovulation_window2 = menstruationdata.Ovulation_window2,
                Ovulation_window3 = menstruationdata.Ovulation_window3,
                Ovulation_window4 = menstruationdata.Ovulation_window4,
                Safe_period1 = menstruationdata.Safe_period1,
                Safe_period2 = menstruationdata.Safe_period2,
                Safe_period3 = menstruationdata.Safe_period3,
                Safe_period4 = menstruationdata.Safe_period4,
            };

          
            return getAllMenstruationDetails;
        }
    
        public PagedResultDto<GetAllSubCategoryDto> GetAllSubCategorybyCategoryName(PagedResultRequestDto input , string categoryName)
        {
            var category = _categoryRepository.GetAll().Where(x => x.Name == categoryName.ToLower() && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            var query = from sb in _subCategoryRepository.GetAll().Where(x => x.IsActive == true && x.CategoryId == category.Id && x.TenantId == AbpSession.TenantId)
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
        public PagedResultDto<GetAllSubCategoryAndDateDto> GetAllSubCategoryAndDate(PagedResultRequestDto input)
        {
           
            var query = from sb in _subCategoryAndDateRepository.GetAll().Where(x => x.IsActive == true  && x.TenantId == AbpSession.TenantId)
                        join ca in _subCategoryRepository.GetAll() on sb.SubCategoryId equals ca.Id
                        select new GetAllSubCategoryAndDateDto()
                        {
                            Id = sb.Id,
                            SubCategoryId = ca.Id,
                            SubCategoryName = ca.Name,
                            DateAndTime = sb.DateAndTime,
                            UniqueKey = sb.UniqueKey,
                        };

            var result = new PagedResultDto<GetAllSubCategoryAndDateDto>(query.Count(), ObjectMapper.Map<List<GetAllSubCategoryAndDateDto>>(query));
            return result;
        }
        public PagedResultDto<GetAllImageSubCategoryDto> GetAllSubcategoryImageAllocation(PagedResultRequestDto input)
        {
            var query = from sb in _subCategoryRepository.GetAll().Where(x => x.IsActive == true /*&& x.Id == subCategoryId*/ && x.TenantId == AbpSession.TenantId).ToList()
                        join im in _subCategoryImageAllocationRepository.GetAll().ToList() on sb.Id equals im.SubCategoryId
                        into ImageGroups
                        select new GetAllImageSubCategoryDto()
                        {
                            Id = sb.Id,
                            SubCategoryId = sb.Id,
                            SubCategoryName = sb.Name,
                            BulkImage = ImageGroups
                        };

            var result = new PagedResultDto<GetAllImageSubCategoryDto>(query.Count(), ObjectMapper.Map<List<GetAllImageSubCategoryDto>>(query));
            return result;
        }
    }
}
