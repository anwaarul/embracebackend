using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Embrace.Authorization.Users;
using Embrace.Entites.SubCategory.Dto;
using Embrace.Entities;
using Embrace.Entities.OrderPlacement.Dto;
using Embrace.General.Dto;
using Embrace.Quartz;
using Quartz;
using Quartz.Impl;

namespace Embrace.General
{
    public class GeneralAppService : EmbraceAppServiceBase, IGeneralAppService
    {
        // Lookups
        private readonly IRepository<OrderPlacementInfo, long> _orderPlacementRepository;
        private readonly IRepository<UniqueNameAndDateInfo, long> _uniqueNameAndDateInfoRepository;
        private readonly IRepository<SubCategoryInfo, long> _subCategoryRepository;
        private readonly IRepository<CategoryInfo, long> _categoryRepository;
        private readonly IRepository<SubscriptionInfo, long> _subscriptionRepository;
        private readonly IRepository<SubscriptionOrderPayementAllocationInfo, long> _subscriptionOrderPayementRepository;
        private readonly IRepository<SubscriptionTypeInfo, long> _subscriptionTypeRepository;
        private readonly IRepository<SubCategoryImageAllocationInfo, long> _subCategoryImageAllocationRepository;
        private readonly IRepository<MenstruationDetailsInfo, long> _menstruationDetailsRepository;
        private readonly IRepository<SubCategoryAndDateInfo, long> _subCategoryAndDateRepository;
        private readonly IRepository<ProductParametersInfo, long> _productParametersRepository;
        private readonly IRepository<CategoryInfo, long> _categoriesRepository;
        private readonly IRepository<ProductVariantsInfo, long> _productVariantsRepository;
        private readonly IRepository<ProductParameterVariantAllocationInfo, long> _productParameterVariantAllocationRepository;
        private readonly IRepository<ProductCategoryInfo, long> _productCategoryRepository;
        private readonly IRepository<ProductImageAllocationInfo, long> _productImageAllocationRepository;
        private readonly IRepository<SizeInfo, long> _sizeRepository;
        private readonly IRepository<ProductParameterSizeAllocationInfo, long> _productParameterSizeAllocationRepository;
        private readonly UserManager _userManager;

        public GeneralAppService(
        IRepository<User, long> repository,
        UserManager userManager,
        IRepository<SubscriptionInfo, long> subscriptionRepository,
        IRepository<SubscriptionOrderPayementAllocationInfo, long> subscriptionOrderPayementRepository,
        IRepository<SubscriptionTypeInfo, long> subscriptionTypeRepository,
        IRepository<ProductVariantsInfo, long> productVariantsRepository,
        IRepository<OrderPlacementInfo, long> orderPlacementRepository,
        IRepository<SizeInfo, long> sizeRepository,
       IRepository<ProductParameterSizeAllocationInfo, long> productParameterSizeAllocationRepository,

        IRepository<ProductImageAllocationInfo, long> productImageAllocationRepository,
        IRepository<ProductCategoryInfo, long> productCategoryRepository,
        IRepository<ProductParameterVariantAllocationInfo, long> productParameterVariantAllocationRepository,

        IRepository<UniqueNameAndDateInfo, long> uniqueNameAndDateInfoRepository,
        IRepository<SubCategoryAndDateInfo, long> subCategoryAndDateRepository,
        IRepository<MenstruationDetailsInfo, long> menstruationDetailsRepository,
        IRepository<SubCategoryImageAllocationInfo, long> subCategoryImageAllocationRepository,
        IRepository<SubCategoryInfo, long> subCategoryRepository,
        IRepository<CategoryInfo, long> categoryRepository,
        IRepository<ProductParametersInfo, long> productParametersRepository

          ) : base()
        {
            _productParameterSizeAllocationRepository = productParameterSizeAllocationRepository;
            _subscriptionRepository = subscriptionRepository;
            _subscriptionOrderPayementRepository = subscriptionOrderPayementRepository;
            _subscriptionTypeRepository = subscriptionTypeRepository;
            _sizeRepository = sizeRepository;
            _productParameterVariantAllocationRepository = productParameterVariantAllocationRepository;
            _productCategoryRepository = productCategoryRepository;
            _productImageAllocationRepository = productImageAllocationRepository;
            _productVariantsRepository = productVariantsRepository;
            _userManager = userManager;
            _subCategoryAndDateRepository = subCategoryAndDateRepository;
            _subCategoryImageAllocationRepository = subCategoryImageAllocationRepository;
            _menstruationDetailsRepository = menstruationDetailsRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _orderPlacementRepository = orderPlacementRepository;
            _uniqueNameAndDateInfoRepository = uniqueNameAndDateInfoRepository;
            _productParametersRepository = productParametersRepository;

        }

        public async Task<UniqueNameAndDateUniqueKeyDto> CreateUniqueKeyWithNameAndDateTime(UniqueNameAndDateDto input)
        {
            UniqueNameAndDateInfo result = new UniqueNameAndDateInfo();
            //Converting DateTime to Date(string)
            var Date = input.DateAndTime.ToShortDateString();

            //Removing Extra Spaces from Name

            var nameWithoutSpaces = input.Name.Replace(" ", String.Empty);
           
            var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.Name == input.Name && x.DateAndTime.Date == input.DateAndTime.Date && x.TenantId == input.TenantId).FirstOrDefault();

            if (dataUniqueKey != null)
            {
                result = ObjectMapper.Map<UniqueNameAndDateInfo>(dataUniqueKey);

                result.DateAndTime = dataUniqueKey.DateAndTime;
                result.StartDatePeriod = dataUniqueKey.StartDatePeriod;
                result.EndDatePeriod = dataUniqueKey.EndDatePeriod;
                result.UniqueKey = dataUniqueKey.UniqueKey;
              
            }
            else
            {
                result = ObjectMapper.Map<UniqueNameAndDateInfo>(input);
                result.TenantId = input.TenantId;
                result.UniqueKey = nameWithoutSpaces + Date;

                await _uniqueNameAndDateInfoRepository.InsertAsync(result);

                result.IsActive = true;

                CurrentUnitOfWork.SaveChanges();
            }
            var data = ObjectMapper.Map<UniqueNameAndDateUniqueKeyDto>(result);
            return data;

        }
        public async Task<UniqueNameAndDateUniqueKeyDto> CreateUniqueNameWithDate(CreateUniqueNameWithDateDto input)
        {
            var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == input.UniqueKey && x.TenantId == input.TenantId).OrderByDescending(x=>x.Id).FirstOrDefault();

            var result = ObjectMapper.Map<UniqueNameAndDateInfo>(input);
            result.Name = dataUniqueKey.Name;
            result.StartDatePeriod = dataUniqueKey.EndDatePeriod;
            result.EndDatePeriod = input.DateAndTime;
            result.TenantId = input.TenantId;
            result.UniqueKey = dataUniqueKey.UniqueKey;
            result.DateAndTime = dataUniqueKey.DateAndTime;       
            await _uniqueNameAndDateInfoRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            GetAllMenstruationDetails menstruation = new GetAllMenstruationDetails();
            menstruation = GetAllMenstruation(result.StartDatePeriod, result.EndDatePeriod);

            var menstration = new MenstruationDetailsInfo();
            menstration.TenantId = input.TenantId;
            menstration.UniqueKey = result.UniqueKey;
            menstration.MyCycle = menstruation.MyCycle;
            menstration.Ovulation_day = menstruation.Ovulation_day;
            menstration.Last_ovulation = menstruation.Last_ovulation;
            menstration.Next_mens = menstruation.Next_mens;
            menstration.Next_ovulation = menstruation.Next_ovulation;
            menstration.Ovulation_window1 = menstruation.Ovulation_window1;
            menstration.Ovulation_window2 = menstruation.Ovulation_window2;
            menstration.Ovulation_window3 = menstruation.Ovulation_window3;
            menstration.Safe_period1 = menstruation.Safe_period1;
            menstration.Safe_period2 = menstruation.Safe_period2;
            menstration.Safe_period3 = menstruation.Safe_period3;
            menstration.Safe_period4 = menstruation.Safe_period4;

            await _menstruationDetailsRepository.InsertAsync(menstration);

            menstration.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<UniqueNameAndDateUniqueKeyDto>(result);
            return data;

        }
        public async Task<string> CreateOrderPlacement(CreateOrderPlacementBulkDto createOrderPlacementDto)
        {
            foreach (CreateOrderPlacementDto input in createOrderPlacementDto.createBulkOrderPlacement)
            {
                var data = ObjectMapper.Map<OrderPlacementInfo>(input);
               
                data.LastModificationTime = DateTime.Now;
                data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                data.TenantId = input.TenantId;
                await _orderPlacementRepository.InsertAsync(data);

                data.IsActive = true;
                CurrentUnitOfWork.SaveChanges();
            }
        
            
            return new string("OrderPlacement Create Successfully");

        }
        public async Task<string> CreateSubscription(CreateSubscriptionDto input ,int tenant)
        {
            SubscriptionInfo subscription = new SubscriptionInfo();
            var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == input.UniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
            if (dataUniqueKey == null)
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
            var dataSubscriptionType = _subscriptionTypeRepository.GetAll().Where(x => x.Name == input.SubscriptionName && x.TenantId == tenant).FirstOrDefault();
            if (dataUniqueKey == null)
            {
                throw new UserFriendlyException("Invalid SubscriptionType");
            }
            var checksubscription = _subscriptionRepository.GetAll().Where(x => x.UniqueKey == input.UniqueKey && x.TenantId == tenant).FirstOrDefault();
            if (checksubscription != null)
            {
                await _subscriptionRepository.DeleteAsync(checksubscription.Id);
                CurrentUnitOfWork.SaveChanges();

                subscription = new SubscriptionInfo();
                subscription.Id = 0;
                subscription.UniqueKey = dataUniqueKey.UniqueKey;
                subscription.SubscriptionDate = DateTime.Now;
                subscription.SubscriptionTypeId = dataSubscriptionType.Id;
                subscription.TenantId = tenant;
                await _subscriptionRepository.InsertAsync(subscription);

                subscription.IsActive = true;

                CurrentUnitOfWork.SaveChanges();
                foreach (var order in input.orderlist)
                {

                    var orderPlacement = ObjectMapper.Map<OrderPlacementInfo>(order);

                    orderPlacement.LastModificationTime = DateTime.Now;
                    orderPlacement.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                    orderPlacement.TenantId = tenant;
                    await _orderPlacementRepository.InsertAsync(orderPlacement);

                    orderPlacement.IsActive = true;
                    CurrentUnitOfWork.SaveChanges();

                    var allocation = _subscriptionOrderPayementRepository.GetAll().Where(x => x.SubscriptionId == checksubscription.Id && x.TenantId == tenant).ToList();
                    foreach (var item in allocation)
                    {
                        await _subscriptionOrderPayementRepository.DeleteAsync(item.Id);
                        CurrentUnitOfWork.SaveChanges();

                    }
                    var orderPlacementAllocation = new SubscriptionOrderPayementAllocationInfo();
                    orderPlacementAllocation.Id = 0;
                    orderPlacementAllocation.OrderPaymentId = orderPlacement.Id;
                    orderPlacementAllocation.SubscriptionId = subscription.Id;
                    orderPlacementAllocation.LastModificationTime = DateTime.Now;
                    orderPlacementAllocation.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                    orderPlacementAllocation.TenantId = tenant;
                    await _subscriptionOrderPayementRepository.InsertAsync(orderPlacementAllocation);

                    orderPlacementAllocation.IsActive = true;
                    CurrentUnitOfWork.SaveChanges();

                }

            }
            else
            {
                subscription = new SubscriptionInfo();
                subscription.UniqueKey = dataUniqueKey.UniqueKey;
                subscription.SubscriptionDate = DateTime.Now;
                subscription.SubscriptionTypeId = dataSubscriptionType.Id;
                subscription.TenantId = tenant;
                await _subscriptionRepository.InsertAsync(subscription);

                subscription.IsActive = true;

                CurrentUnitOfWork.SaveChanges();
                foreach (var order in input.orderlist)
                {

                    var orderPlacement = ObjectMapper.Map<OrderPlacementInfo>(order);

                    orderPlacement.LastModificationTime = DateTime.Now;
                    orderPlacement.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                    orderPlacement.TenantId = tenant;
                    await _orderPlacementRepository.InsertAsync(orderPlacement);

                    orderPlacement.IsActive = true;
                    CurrentUnitOfWork.SaveChanges();

                    var orderPlacementAllocation = new SubscriptionOrderPayementAllocationInfo();
                    orderPlacementAllocation.OrderPaymentId = orderPlacement.Id;
                    orderPlacementAllocation.SubscriptionId = subscription.Id;
                    orderPlacementAllocation.LastModificationTime = DateTime.Now;
                    orderPlacementAllocation.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                    orderPlacementAllocation.TenantId = tenant;
                    await _subscriptionOrderPayementRepository.InsertAsync(orderPlacementAllocation);

                    orderPlacementAllocation.IsActive = true;
                    CurrentUnitOfWork.SaveChanges();

                }

            }



            return new string("Subcription Created Successfully");
        }

        public async Task<String> StartSimpleJob(int tenantId)
        {

            string jobName = Guid.NewGuid().ToString();
            string jobGroup = Guid.NewGuid().ToString();
            string triggerName = Guid.NewGuid().ToString();
            string triggerGroup = Guid.NewGuid().ToString();

            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            //IJobDetail job = JobBuilder.Create<SimpleJobNew>()
            IJobDetail job = JobBuilder.Create<OrderJob>()
                .UsingJobData("tenant", tenantId)
                .UsingJobData("title", "Title")
                .UsingJobData("message", "Message")
                                   .WithIdentity(jobName, jobGroup)
                                   .StoreDurably()
                                   .Build();


            ITrigger trigger = TriggerBuilder.Create()
                                        .WithIdentity(triggerName, triggerGroup)
                                        .StartAt(DateTime.Now.AddSeconds(10))
                                        .WithSimpleSchedule(x => x
                                        .WithIntervalInSeconds(5)
                                        )
                                        .Build();
            Debug.WriteLine(DateTime.Now);



            await scheduler.ScheduleJob(job, trigger);


            return new String("Order Place");
        }

        public async Task<string> CreateSubscriptionbyUniqueKey (string uniqueKey , int tenant)
        {

            var dataSubscription = _subscriptionRepository.GetAll().Where(x => x.UniqueKey == uniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
           
            var getsubscriptiontype = _subscriptionTypeRepository.GetAll().Where(x => x.Id == dataSubscription.SubscriptionTypeId && x.TenantId == tenant).FirstOrDefault();
            if (getsubscriptiontype.Name == "Monthly" )
            {
                var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == uniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                if (dataUniqueKey == null)
                {
                    throw new UserFriendlyException("Invalid Unique Key");
                }
                var dataorder = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                var addthirtydays = (dataorder.CreationTime.AddDays(30));
                if (addthirtydays.Date >= DateTime.Now)
                {
                    var orderPlacements = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).ToList();
                    foreach (var item in orderPlacements)
                    {
                        
                        var orderPlacement = ObjectMapper.Map<OrderPlacementInfo>(item);
                        orderPlacement.Id = 0;
                        orderPlacement.LastModificationTime = DateTime.Now;
                        orderPlacement.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                        orderPlacement.TenantId = tenant;
                        await _orderPlacementRepository.InsertAsync(orderPlacement);

                        orderPlacement.IsActive = true;
                        CurrentUnitOfWork.SaveChanges();
                       
                    }
                }

            }
            if (getsubscriptiontype.Name == "Bimonthly")
            {
                var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == uniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                if (dataUniqueKey == null)
                {
                    throw new UserFriendlyException("Invalid Unique Key");
                }
                var dataorder = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                var addthirtydays = (dataorder.CreationTime.AddDays(60));
                if (addthirtydays.Date >= DateTime.Now)
                {
                    var orderPlacements = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).ToList();
                    foreach (var item in orderPlacements)
                    {

                        var orderPlacement = ObjectMapper.Map<OrderPlacementInfo>(item);
                        orderPlacement.Id = 0;
                        orderPlacement.LastModificationTime = DateTime.Now;
                        orderPlacement.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                        orderPlacement.TenantId = tenant;
                        await _orderPlacementRepository.InsertAsync(orderPlacement);

                        orderPlacement.IsActive = true;
                        CurrentUnitOfWork.SaveChanges();
                        
                    }
                }

            }
            if (getsubscriptiontype.Name == "Quarterly")
            {
                var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == uniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                if (dataUniqueKey == null)
                {
                    throw new UserFriendlyException("Invalid Unique Key");
                }
                var dataorder = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                var addthirtydays = (dataorder.CreationTime.AddDays(90));
                if (addthirtydays.Date >= DateTime.Now)
                {
                    var orderPlacements = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).ToList();
                    foreach (var item in orderPlacements)
                    {

                        var orderPlacement = ObjectMapper.Map<OrderPlacementInfo>(item);
                        orderPlacement.Id = 0;
                        orderPlacement.LastModificationTime = DateTime.Now;
                        orderPlacement.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                        orderPlacement.TenantId = tenant;
                        await _orderPlacementRepository.InsertAsync(orderPlacement);

                        orderPlacement.IsActive = true;
                        CurrentUnitOfWork.SaveChanges();
                      
                    }
                }

            }
            return new string("Subcription Created Successfully");
        }
        public async Task<string> CreateSubscriptionbyUniqueKeyCornJob(int tenant)
        {

            var dataSubscription = _subscriptionRepository.GetAll().Where(x=> x.TenantId == tenant).ToList();
            foreach (var item in dataSubscription)
            {
                var getsubscriptiontype = _subscriptionTypeRepository.GetAll().Where(x => x.Id == item.SubscriptionTypeId && x.TenantId == tenant).FirstOrDefault();
                if (getsubscriptiontype.Name == "Monthly")
                {
                    var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == item.UniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (dataUniqueKey == null)
                    {
                        throw new UserFriendlyException("Invalid Unique Key");
                    }
                    var dataorder = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                    var addthirtydays = (dataorder.CreationTime.AddDays(30));
                    if (addthirtydays.Date >= DateTime.Now)
                    {
                        var orderPlacements = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).ToList();
                        foreach (var order in orderPlacements)
                        {

                            var orderPlacement = ObjectMapper.Map<OrderPlacementInfo>(order);
                            orderPlacement.Id = 0;
                            orderPlacement.LastModificationTime = DateTime.Now;
                            orderPlacement.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                            orderPlacement.TenantId = tenant;
                            await _orderPlacementRepository.InsertAsync(orderPlacement);

                            orderPlacement.IsActive = true;
                            CurrentUnitOfWork.SaveChanges();

                        }
                    }

                }
                if (getsubscriptiontype.Name == "Bimonthly")
                {
                    var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == item.UniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (dataUniqueKey == null)
                    {
                        throw new UserFriendlyException("Invalid Unique Key");
                    }
                    var dataorder = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                    var addthirtydays = (dataorder.CreationTime.AddDays(60));
                    if (addthirtydays.Date >= DateTime.Now)
                    {
                        var orderPlacements = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).ToList();
                        foreach (var order in orderPlacements)
                        {

                            var orderPlacement = ObjectMapper.Map<OrderPlacementInfo>(order);
                            orderPlacement.Id = 0;
                            orderPlacement.LastModificationTime = DateTime.Now;
                            orderPlacement.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                            orderPlacement.TenantId = tenant;
                            await _orderPlacementRepository.InsertAsync(orderPlacement);

                            orderPlacement.IsActive = true;
                            CurrentUnitOfWork.SaveChanges();

                        }
                    }

                }
                if (getsubscriptiontype.Name == "Quarterly")
                {
                    var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == item.UniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (dataUniqueKey == null)
                    {
                        throw new UserFriendlyException("Invalid Unique Key");
                    }
                    var dataorder = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
                    var addthirtydays = (dataorder.CreationTime.AddDays(90));
                    if (addthirtydays.Date >= DateTime.Now)
                    {
                        var orderPlacements = _orderPlacementRepository.GetAll().Where(x => x.UserUniqueName == dataUniqueKey.Name && x.TenantId == tenant).ToList();
                        foreach (var order in orderPlacements)
                        {

                            var orderPlacement = ObjectMapper.Map<OrderPlacementInfo>(order);
                            orderPlacement.Id = 0;
                            orderPlacement.LastModificationTime = DateTime.Now;
                            orderPlacement.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                            orderPlacement.TenantId = tenant;
                            await _orderPlacementRepository.InsertAsync(orderPlacement);

                            orderPlacement.IsActive = true;
                            CurrentUnitOfWork.SaveChanges();

                        }
                    }

                }
            }
           
            return new string("Subcription Created Successfully");
        }

        public async Task<MenstruationDetailsInfo> CreateMenstruationDetails(MenstruationDetailsDto input)
        {
            var uniquekey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == input.UniqueKey && x.TenantId == input.TenantId).FirstOrDefault();
            if (uniquekey == null)
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
            
            GetAllMenstruationDetails menstruation =new GetAllMenstruationDetails();
            menstruation = GetAllMenstruation(uniquekey.StartDatePeriod, uniquekey.EndDatePeriod);
            
            var result = new MenstruationDetailsInfo();
            result.TenantId = input.TenantId;
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
            var uniquekey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == input.UniqueKey && x.TenantId == input.TenantId).FirstOrDefault();
            if (uniquekey == null) 
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
            var subCategory = _subCategoryRepository.GetAll().Where(x => x.Id == input.SubCategoryId && x.TenantId == input.TenantId).FirstOrDefault();
            if (uniquekey == null)
            {
                throw new UserFriendlyException("Invalid SubCategory");
            }

            var result = ObjectMapper.Map<SubCategoryAndDateInfo>(input);
            result.TenantId = input.TenantId;
            result.UniqueKey = uniquekey.UniqueKey;
            result.DateAndTime = input.DateAndTime;
            result.SubCategoryId = input.SubCategoryId;
            
            await _subCategoryAndDateRepository.InsertAsync(result);

            result.IsActive = true;

            CurrentUnitOfWork.SaveChanges();
            var data = ObjectMapper.Map<SubCategoryAndDateDto>(result);
            return data;

        }
        public Task<PagedResultDto<UniqueNameAndDateUniqueKeyDto>> GetAllWithUniqueKey(PagedResultRequestDto input, string uniqueKey , int tenantId)
         {
            var query = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == tenantId && x.UniqueKey == uniqueKey);
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
        public List<GetAllMenstruationDetails> GetAllMenstruationDetailsbyUniqueKey(string uniquekey , int tenantId)
        {
            List<GetAllMenstruationDetails> menstruationDetails = new List<GetAllMenstruationDetails>();
            var menstruationdata = _menstruationDetailsRepository.GetAll().Where(x => x.UniqueKey == uniquekey && x.TenantId == tenantId).OrderByDescending(x => x.Id).FirstOrDefault();
            if (menstruationdata == null)
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
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
            menstruationDetails.Add(getAllMenstruationDetails);
            // FOR NEXT MONTH

            var date_diff = (menstruationDetails[0].Next_mens.AddDays(menstruationDetails[0].MyCycle) - menstruationDetails[0].Next_mens );
            var ovulation_day = (date_diff.Days - 14);
            var last_ovulation = (menstruationDetails[0].Next_mens.AddDays(-14));
            var next_mens = (menstruationDetails[0].Next_mens.AddDays(date_diff.Days));
            var next_ovulation = (next_mens.AddDays(-14));
            var ovulation_window1 = (menstruationDetails[0].Next_mens.AddDays(-18));
            var ovulation_window2 = (menstruationDetails[0].Next_mens.AddDays(-14));
            var ovulation_window3 = (next_mens.AddDays(-18));
            var ovulation_window4 = (next_mens.AddDays(-14));
            var safe_period1 = menstruationDetails[0].Next_mens.Date;
            var safe_period2 = (menstruationDetails[0].Next_mens.AddDays(9));
            var safe_period3 = (menstruationDetails[0].Next_mens.AddDays(15));
            var safe_period4 = (menstruationDetails[0].Next_mens.AddDays(menstruationDetails[0].MyCycle).AddDays(37));


             getAllMenstruationDetails = new GetAllMenstruationDetails
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
            menstruationDetails.Add(getAllMenstruationDetails);

            // FOR NEXT MONTH

              date_diff = (menstruationDetails[1].Next_mens.AddDays(menstruationDetails[1].MyCycle) - menstruationDetails[1].Next_mens);
              ovulation_day = (date_diff.Days - 14);
              last_ovulation = (menstruationDetails[1].Next_mens.AddDays(-14));
              next_mens = (menstruationDetails[1].Next_mens.AddDays(date_diff.Days));
              next_ovulation = (next_mens.AddDays(-14));
              ovulation_window1 = (menstruationDetails[1].Next_mens.AddDays(-18));
              ovulation_window2 = (menstruationDetails[1].Next_mens.AddDays(-14));
              ovulation_window3 = (next_mens.AddDays(-18));
              ovulation_window4 = (next_mens.AddDays(-14));
              safe_period1 = menstruationDetails[1].Next_mens.Date;
              safe_period2 = (menstruationDetails[1].Next_mens.AddDays(9));
              safe_period3 = (menstruationDetails[1].Next_mens.AddDays(15));
              safe_period4 = (menstruationDetails[1].Next_mens.AddDays(37));

            getAllMenstruationDetails = new GetAllMenstruationDetails
            {

                UniqueKey = uniquekey,
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
            menstruationDetails.Add(getAllMenstruationDetails);
            return  menstruationDetails;


        }
        public PagedResultDto<GetAllMenstruationDetails> GetAllMenstruationDetailsAgainstUniqueKey(string uniquekey, int tenantId)
        {
            List<GetAllMenstruationDetails> menstruationDetails = new List<GetAllMenstruationDetails>();
            var menstruationdata = _menstruationDetailsRepository.GetAll().Where(x => x.UniqueKey == uniquekey && x.TenantId == tenantId).ToList();
            if (menstruationdata.Count == 0)
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
            foreach (var item in menstruationdata)
            {
                GetAllMenstruationDetails getAllMenstruationDetails = new GetAllMenstruationDetails
                {
                    UniqueKey = item.UniqueKey,
                    MyCycle = item.MyCycle,
                    Ovulation_day = item.Ovulation_day,
                    Last_ovulation = item.Last_ovulation,
                    Next_mens = item.Next_mens,
                    Next_ovulation = item.Next_ovulation,
                    Ovulation_window1 = item.Ovulation_window1,
                    Ovulation_window2 = item.Ovulation_window2,
                    Ovulation_window3 = item.Ovulation_window3,
                    Ovulation_window4 = item.Ovulation_window4,
                    Safe_period1 = item.Safe_period1,
                    Safe_period2 = item.Safe_period2,
                    Safe_period3 = item.Safe_period3,
                    Safe_period4 = item.Safe_period4,
                };
                menstruationDetails.Add(getAllMenstruationDetails);
            }


            var result = new PagedResultDto<GetAllMenstruationDetails>(menstruationDetails.Count(), ObjectMapper.Map<List<GetAllMenstruationDetails>>(menstruationDetails));
            return result;
        }
        public PagedResultDto<GetAllMenstruationDetails> GetAllMenstruationDetailsLastThreeCycle(string uniquekey, int tenantId)
        {
            List<GetAllMenstruationDetails> menstruationDetails = new List<GetAllMenstruationDetails>();
            var menstruationdata = _menstruationDetailsRepository.GetAll().Where(x => x.UniqueKey == uniquekey && x.TenantId == tenantId).ToList();
            if (menstruationdata.Count == 0)
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
            foreach (var item in menstruationdata)
            {
                GetAllMenstruationDetails getAllMenstruationDetails = new GetAllMenstruationDetails
                {
                    UniqueKey = item.UniqueKey,
                    MyCycle = item.MyCycle,
                    Ovulation_day = item.Ovulation_day,
                    Last_ovulation = item.Last_ovulation,
                    Next_mens = item.Next_mens,
                    Next_ovulation = item.Next_ovulation,
                    Ovulation_window1 = item.Ovulation_window1,
                    Ovulation_window2 = item.Ovulation_window2,
                    Ovulation_window3 = item.Ovulation_window3,
                    Ovulation_window4 = item.Ovulation_window4,
                    Safe_period1 = item.Safe_period1,
                    Safe_period2 = item.Safe_period2,
                    Safe_period3 = item.Safe_period3,
                    Safe_period4 = item.Safe_period4,
                };
                menstruationDetails.Add(getAllMenstruationDetails);
            }
            var last3index = menstruationDetails
                                .Where(c => c.UniqueKey == uniquekey).Skip(Math.Max(0, menstruationDetails.Count() - 3)).ToList();
            menstruationDetails.Skip(Math.Max(0, menstruationDetails.Count() - 3));
            var result = new PagedResultDto<GetAllMenstruationDetails>(last3index.Count(), ObjectMapper.Map<List<GetAllMenstruationDetails>>(last3index));
            return result;
        }
        public PagedResultDto<GetAllSubCategoryDto> GetAllSubCategorybyCategoryName(PagedResultRequestDto input , string categoryName , int tenantId)
        {
            var category = _categoryRepository.GetAll().Where(x => x.Name == categoryName.ToLower() && x.TenantId == tenantId).FirstOrDefault();

            var query = from sb in _subCategoryRepository.GetAll().Where(x => x.IsActive == true && x.CategoryId == category.Id && x.TenantId == tenantId)
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
        public PagedResultDto<GetAllSubCategoryAndDateDto> GetAllSubCategoryAndDate(PagedResultRequestDto input, int tenantId)
        {
           
            var query = from sb in _subCategoryAndDateRepository.GetAll().Where(x => x.IsActive == true  && x.TenantId == tenantId)
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
        public PagedResultDto<GetAllImageSubCategoryDto> GetAllSubcategoryImageAllocation(PagedResultRequestDto input , int tenantId)
        {
            var query = from sb in _subCategoryRepository.GetAll().Where(x => x.IsActive == true /*&& x.Id == subCategoryId*/ && x.TenantId == tenantId).ToList()
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
        public PagedResultDto<GetAllSubCategoryDto> GetAllSubCategory(PagedResultRequestDto input, int tenantId)
        {
            var query = from sb in _subCategoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == tenantId)
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
        public PagedResultDto<GetAllSubCategoryDataDto> GetAllSubCategoryWithDate(PagedResultRequestDto input, int tenantId, string uniqueKey, DateTime date)
        {
            var query = from sb in _subCategoryAndDateRepository.GetAll().Where(x =>  x.UniqueKey == uniqueKey && x.DateAndTime.Date == date.Date && x.TenantId == tenantId)
                        join sub in _subCategoryRepository.GetAll() on sb.SubCategoryId equals sub.Id
                        join ca in _categoryRepository.GetAll() on sub.CategoryId equals ca.Id
                        select new GetAllSubCategoryDataDto()
                        {
                            Id = sb.Id,
                            CategoryId = ca.Id,
                            CategoryName = ca.Name,
                            SubCategoryId = sub.Id,
                            SubCategoryName = sub.Name,
                            ImageUrl = sub.ImageUrl,
                            DateAndTime = sb.DateAndTime,
                            UniqueKey = sb.UniqueKey,
                        };

            var result = new PagedResultDto<GetAllSubCategoryDataDto>(query.Count(), ObjectMapper.Map<List<GetAllSubCategoryDataDto>>(query));
            return result;
        }
        public PagedResultDto<GetAllProductParametersDto> GetAllProductsByCategoryName(PagedResultRequestDto input, string categoryName)
        {
            List<ProductVariantsInfo> productVariants = new List<ProductVariantsInfo>();
            List<GetAllProductParametersDto> productParametersDtos = new List<GetAllProductParametersDto>();
            List<GetAllProductVariantsDto> productvariants = new List<GetAllProductVariantsDto>();
            
            productVariants = _productVariantsRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToList();
            var productCategoryData = _productCategoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Name == categoryName.ToLower()).FirstOrDefault();
            var productparameters = _productParametersRepository.GetAll().Where(x => x.ProductCategoryId == productCategoryData.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            var productvariant = _productParameterVariantAllocationRepository.GetAll().Where(x => x.ProductParameterId == productparameters.Id).ToList();
            
            foreach (var item in productvariant)
            {
                var variants = productVariants.Where(x => x.Id == item.ProductVariantId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
                GetAllProductVariantsDto getAllvaranits = new GetAllProductVariantsDto()
                {

                    VariantId = variants.Id,
                    VariantName = variants.Name
                      
                };
                productvariants.Add(getAllvaranits);
            }
            
            GetAllProductParametersDto getAllProduct = new GetAllProductParametersDto()
            {

                Id = productparameters.Id,
                ProductName = productparameters.ProductName,
                ProductImage = productparameters.ProductImage,
                Description = productparameters.Description,
                ProductCategoryId = productCategoryData.Id,
                ProductCategoryName = productCategoryData.Name,
                ProductVariants = productvariants,
               
                Price = productparameters.Price,
            };
            productParametersDtos.Add(getAllProduct);
           
            var result = new PagedResultDto<GetAllProductParametersDto>(productParametersDtos.Count(), ObjectMapper.Map<List<GetAllProductParametersDto>>(productParametersDtos));

            return result;

        }
        public PagedResultDto<GetAllProductParametersDto> GetAllProductParameters(PagedResultRequestDto input)
        {
            List<ProductVariantsInfo> productVariants = new List<ProductVariantsInfo>();
            List<SizeInfo> productSizes = new List<SizeInfo>();

            List<GetAllProductParametersDto> productParametersDtos = new List<GetAllProductParametersDto>();

            productVariants = _productVariantsRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToList();
            productSizes = _sizeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToList();

            var productparameters = _productParametersRepository.GetAll().Where(x =>  x.TenantId == AbpSession.TenantId).ToList();
            var productlist = productparameters.Select(x => x.Id).ToList();

            var productvariant = _productParameterVariantAllocationRepository.GetAll().Where(x => productlist.Contains(x.ProductParameterId)).ToList();
            var productsize = _productParameterSizeAllocationRepository.GetAll().Where(x => productlist.Contains(x.ProductParameterId)).ToList();

            foreach (var product in productparameters)
            {
                List<GetAllProductVariantsDto> productvariants = new List<GetAllProductVariantsDto>();
                List<GetAllProductSizeDto> productsizes = new List<GetAllProductSizeDto>();

                var productCategoryData = _productCategoryRepository.GetAll().Where(x => x.Id == product.ProductCategoryId && x.IsActive == true && x.TenantId == AbpSession.TenantId).FirstOrDefault();

                foreach (var itemVariant in productvariant)
                {
                    var variants = productVariants.Where(x => x.Id == itemVariant.ProductVariantId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
                    GetAllProductVariantsDto getAllvaranits = new GetAllProductVariantsDto()
                    {

                        VariantId = variants.Id,
                        VariantName = variants.Name

                    };
                    productvariants.Add(getAllvaranits);

                }
                foreach (var itemSize in productsize)
                {
                    var sizes = productSizes.Where(x => x.Id == itemSize.SizeId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
                    GetAllProductSizeDto getAllsizes = new GetAllProductSizeDto()
                    {

                        SizeId = sizes.Id,
                        SizeName = sizes.Name

                    };
                    productsizes.Add(getAllsizes);

                }
                GetAllProductParametersDto getAllProduct = new GetAllProductParametersDto()
                {

                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    Description = product.Description,
                    ProductCategoryId = productCategoryData.Id,
                    ProductCategoryName = productCategoryData.Name,
                    ProductVariants = productvariants,
                    ProductSizes = productsizes,
                    Price = product.Price,
                };
                productParametersDtos.Add(getAllProduct);

            }



            var result = new PagedResultDto<GetAllProductParametersDto>(productParametersDtos.Count(), ObjectMapper.Map<List<GetAllProductParametersDto>>(productParametersDtos));

            return result;

        }
        public PagedResultDto<GetAllProductParametersDetailsDto> GetAllProductsDetailsById(PagedResultRequestDto input, long productId)
        {
            List<ProductVariantsInfo> productVariants = new List<ProductVariantsInfo>();
            List<SizeInfo> sizeInfos = new List<SizeInfo>();
            List<ProductImageAllocationInfo> productimage = new List<ProductImageAllocationInfo>();
            List<GetAllProductParametersDetailsDto> productParametersDtos = new List<GetAllProductParametersDetailsDto>();
            List<GetAllProductVariantsDto> productvariants = new List<GetAllProductVariantsDto>();
            List<GetAllProductImageDto> getAllProductImageDtos = new List<GetAllProductImageDto>();
            List<GetAllProductSizeDto> getAllProductSizeDtos = new List<GetAllProductSizeDto>();
            //cache
            sizeInfos = _sizeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToList();
            productimage = _productImageAllocationRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToList();
            productVariants = _productVariantsRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToList();
            
            var products = _productParametersRepository.GetAll().Where(x => x.Id == productId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            var productCategoryData = _productCategoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id == products.ProductCategoryId).FirstOrDefault();
            var productvariant = _productParameterVariantAllocationRepository.GetAll().Where(x => x.ProductParameterId == products.Id).ToList();
            var allocatedImages = _productImageAllocationRepository.GetAll().Where(x => x.ProductParameterId == productId && x.TenantId == AbpSession.TenantId).ToList();
            var sizealloction = _productParameterSizeAllocationRepository.GetAll().Where(x => x.ProductParameterId == productId && x.TenantId == AbpSession.TenantId).ToList();
            foreach (var item in allocatedImages)
            {
                var getimages = productimage.Where(x => x.ProductParameterId == productId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
                GetAllProductImageDto getAllimages = new GetAllProductImageDto()
                {

                    ProductId = getimages.ProductParameterId,
                    ImageUrl = getimages.ImageUrl

                };
                getAllProductImageDtos.Add(getAllimages);
            }
            foreach (var item in sizealloction)
            {
                var getsize = sizeInfos.Where(x => x.Id == item.SizeId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
                GetAllProductSizeDto getAllsize = new GetAllProductSizeDto()
                {

                    SizeId = getsize.Id,
                    SizeName = getsize.Name

                };
                getAllProductSizeDtos.Add(getAllsize);
            }
            foreach (var item in productvariant)
            {
                var variants = productVariants.Where(x => x.Id == item.ProductVariantId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
                GetAllProductVariantsDto getAllvaranits = new GetAllProductVariantsDto()
                {

                    VariantId = variants.Id,
                    VariantName = variants.Name

                };
                productvariants.Add(getAllvaranits);
            }

            GetAllProductParametersDetailsDto getAllProduct = new GetAllProductParametersDetailsDto()
            {

                Id = products.Id,
                ProductName = products.ProductName,
                ProductImage = products.ProductImage,
                Description = products.Description,
                ProductCategoryId = productCategoryData.Id,
                ProductCategoryName = productCategoryData.Name,
                ProductVariants = productvariants,
                Images = getAllProductImageDtos,
                Size = getAllProductSizeDtos,
                Price = products.Price,
            };
            productParametersDtos.Add(getAllProduct);

            var result = new PagedResultDto<GetAllProductParametersDetailsDto>(productParametersDtos.Count(), ObjectMapper.Map<List<GetAllProductParametersDetailsDto>>(productParametersDtos));

            return result;

        }

        public async Task<string> SendMail(string text )
        {
            try
            {

                SmtpClient smtpClient = new SmtpClient("mail.madamporter.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false; // uncomment if you don't want to use the network credentials
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new System.Net.NetworkCredential("rafia@madamporter.com", "mM1?7S49XXfO");

                //Setting From , To and CC
                MailMessage msg = new MailMessage();


                var htmlString = new StringBuilder();
                htmlString.Append(@"<html>
                <head>
                <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                <title>Email for OTP for Signup</title>
                </head>
                      <body>
                         
                           <p style =' 
                            text-align: center;
                            font-size: 32px;
                            margin: 4px 2px;'>

                             %Text%
                         </p>
                      </body>
                </html>  "
                );
                htmlString.Replace("%Text%", text.ToString());
                msg.IsBodyHtml = true;

                msg.IsBodyHtml = true;

                msg.Subject = " A query has been received";
                msg.From = new MailAddress("rafia@madamporter.com");
                msg.To.Add(new MailAddress("taqadus@vrox.co.uk"));
                msg.CC.Add(new MailAddress("taqadus@vrox.co.uk"));

                msg.Body = htmlString.ToString();

                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }



            return "Sending Mail Successfully";

          
        }
    }
}
