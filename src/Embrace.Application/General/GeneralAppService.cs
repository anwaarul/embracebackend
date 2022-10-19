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
using Embrace.Entites.Alert.Dto;
using Embrace.Entites.SubCategory.Dto;
using Embrace.Entities;
using Embrace.Entities.Blog.Dto;
using Embrace.Entities.BlogCategory.Dto;
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

        private readonly IRepository<ProductCategoryInfo, long> _productCategoryRepository;
        private readonly IRepository<ProductImageAllocationInfo, long> _productImageAllocationRepository;
        private readonly IRepository<BlogInfo, long> _blogRepository;
        private readonly IRepository<BlogCategoryInfo, long> _blogCategoryRepository;
        private readonly IRepository<AlertInfo, long> alert_repo;
        private readonly UserManager _userManager;

        public GeneralAppService(
        IRepository<User, long> repository,
        UserManager userManager,
        IRepository<SubscriptionInfo, long> subscriptionRepository,
        IRepository<SubscriptionOrderPayementAllocationInfo, long> subscriptionOrderPayementRepository,
        IRepository<SubscriptionTypeInfo, long> subscriptionTypeRepository,
       IRepository<OrderPlacementInfo, long> orderPlacementRepository,

        IRepository<ProductImageAllocationInfo, long> productImageAllocationRepository,
        IRepository<ProductCategoryInfo, long> productCategoryRepository,

        IRepository<UniqueNameAndDateInfo, long> uniqueNameAndDateInfoRepository,
        IRepository<SubCategoryAndDateInfo, long> subCategoryAndDateRepository,
        IRepository<MenstruationDetailsInfo, long> menstruationDetailsRepository,
        IRepository<SubCategoryImageAllocationInfo, long> subCategoryImageAllocationRepository,
        IRepository<SubCategoryInfo, long> subCategoryRepository,
        IRepository<CategoryInfo, long> categoryRepository,
        IRepository<ProductParametersInfo, long> productParametersRepository,
        IRepository<BlogInfo, long> blogRepository,
        IRepository<BlogCategoryInfo, long> blogCategoryRepository,
        IRepository<AlertInfo, long> alert_repo
          ) : base()
        {
            _subscriptionRepository = subscriptionRepository;
            _subscriptionOrderPayementRepository = subscriptionOrderPayementRepository;
            _subscriptionTypeRepository = subscriptionTypeRepository;
            _productCategoryRepository = productCategoryRepository;
            _productImageAllocationRepository = productImageAllocationRepository;
            _userManager = userManager;
            _subCategoryAndDateRepository = subCategoryAndDateRepository;
            _subCategoryImageAllocationRepository = subCategoryImageAllocationRepository;
            _menstruationDetailsRepository = menstruationDetailsRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _orderPlacementRepository = orderPlacementRepository;
            _uniqueNameAndDateInfoRepository = uniqueNameAndDateInfoRepository;
            _productParametersRepository = productParametersRepository;
            _blogRepository = blogRepository;
            _blogCategoryRepository = blogCategoryRepository;
            this.alert_repo = alert_repo;
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
            GetAllMenstruationDetails menstruation = new GetAllMenstruationDetails();
            UniqueNameAndDateInfo result = new UniqueNameAndDateInfo();
            var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == input.UniqueKey && x.TenantId == input.TenantId).OrderByDescending(x => x.Id).FirstOrDefault();
            var addtwentyoneday = dataUniqueKey.StartDatePeriod.AddDays(20);
            var addfourtyday = dataUniqueKey.StartDatePeriod.AddDays(39);
            if (input.DateAndTime.Date < addtwentyoneday.Date)
            {
                //TimeSpan datediffernce = input.DateAndTime.Date.Subtract(dataUniqueKey.StartDatePeriod.Date);

                var datediffernce = input.DateAndTime - dataUniqueKey.StartDatePeriod;
                result = ObjectMapper.Map<UniqueNameAndDateInfo>(dataUniqueKey);
                result.Name = dataUniqueKey.Name;
                result.StartDatePeriod = input.DateAndTime;
                result.TenantId = input.TenantId;
                result.UniqueKey = dataUniqueKey.UniqueKey;
                result.DateAndTime = dataUniqueKey.DateAndTime;
                await _uniqueNameAndDateInfoRepository.UpdateAsync(result);
                result.IsActive = true;

                menstruation = GetAllMenstruation(result.StartDatePeriod);
                var menstration = new MenstruationDetailsInfo();
                menstration.TenantId = input.TenantId;
                menstration.UniqueKey = result.UniqueKey;
                menstration.CycleDay = 28;
                menstration.MyCycle = input.DateAndTime.Date;
                menstration.Ovulation_date = menstruation.Ovulation_date;
                menstration.Last_Mens_Start = menstruation.Last_Mens_Start;
                menstration.Last_Mens_End = menstruation.Last_Mens_End;
                menstration.Next_Mens_Start = menstruation.Next_Mens_Start;
                menstration.Next_Mens_End = menstruation.Next_Mens_End;
                menstration.Ovulation_Window_Start = menstruation.Ovulation_Window_Start;
                menstration.Ovulation_Window_End = menstruation.Ovulation_Window_End;
                menstration.Status = "Normal";
                await _menstruationDetailsRepository.InsertAsync(menstration);
                menstration.IsActive = true;

                CurrentUnitOfWork.SaveChanges();
                var menstruationDetails = _menstruationDetailsRepository.GetAll().Where(x => x.UniqueKey == result.UniqueKey).ToList();
                var item = menstruationDetails[menstruationDetails.Count - 2];
                var updatemenstration = ObjectMapper.Map<MenstruationDetailsInfo>(item);
                updatemenstration.Status = "Abnormal";
                updatemenstration.CycleDay = ((int)datediffernce.TotalDays);
                updatemenstration.Last_Mens_End = menstration.Last_Mens_Start.AddDays(-1);

                await _menstruationDetailsRepository.UpdateAsync(menstration);

                updatemenstration.IsActive = true;

                CurrentUnitOfWork.SaveChanges();
            }
            if (input.DateAndTime.Date > addfourtyday.Date)
            {
                var datediffernce = input.DateAndTime - dataUniqueKey.StartDatePeriod;

                result = ObjectMapper.Map<UniqueNameAndDateInfo>(dataUniqueKey);
                result.Name = dataUniqueKey.Name;
                result.StartDatePeriod = input.DateAndTime;
                result.TenantId = input.TenantId;
                result.UniqueKey = dataUniqueKey.UniqueKey;
                result.DateAndTime = dataUniqueKey.DateAndTime;
                await _uniqueNameAndDateInfoRepository.UpdateAsync(result);
                result.IsActive = true;

                menstruation = GetAllMenstruation(result.StartDatePeriod);
                var menstration = new MenstruationDetailsInfo();
                menstration.TenantId = input.TenantId;
                menstration.CycleDay = 28;
                menstration.MyCycle = input.DateAndTime.Date;
                menstration.UniqueKey = result.UniqueKey;
                menstration.MyCycle = menstruation.MyCycle;
                menstration.Ovulation_date = menstruation.Ovulation_date;
                menstration.Last_Mens_Start = menstruation.Last_Mens_Start;
                menstration.Last_Mens_End = menstruation.Last_Mens_End;
                menstration.Next_Mens_Start = menstruation.Next_Mens_Start;
                menstration.Next_Mens_End = menstruation.Next_Mens_End;
                menstration.Ovulation_Window_Start = menstruation.Ovulation_Window_Start;
                menstration.Ovulation_Window_End = menstruation.Ovulation_Window_End;
                menstration.Status = "Normal";
                await _menstruationDetailsRepository.InsertAsync(menstration);
                menstration.IsActive = true;

                CurrentUnitOfWork.SaveChanges();
                var menstruationDetails = _menstruationDetailsRepository.GetAll().Where(x => x.UniqueKey == result.UniqueKey).ToList();
                var item = menstruationDetails[menstruationDetails.Count - 2];

                var updatemenstration = ObjectMapper.Map<MenstruationDetailsInfo>(item);
                updatemenstration.Status = "Abnormal";
                updatemenstration.CycleDay = ((int)datediffernce.TotalDays);
                updatemenstration.Last_Mens_End = menstration.Last_Mens_Start.AddDays(-1);
                await _menstruationDetailsRepository.UpdateAsync(menstration);

                updatemenstration.IsActive = true;

                CurrentUnitOfWork.SaveChanges();

            }
            if (input.DateAndTime.Date <= addfourtyday.Date && input.DateAndTime.Date >= addtwentyoneday.Date)
            {
                var datediffernce = input.DateAndTime - dataUniqueKey.StartDatePeriod;

                result = ObjectMapper.Map<UniqueNameAndDateInfo>(dataUniqueKey);
                result.Name = dataUniqueKey.Name;
                result.StartDatePeriod = input.DateAndTime;
                result.TenantId = input.TenantId;
                result.UniqueKey = dataUniqueKey.UniqueKey;
                result.DateAndTime = dataUniqueKey.DateAndTime;
                await _uniqueNameAndDateInfoRepository.UpdateAsync(result);
                result.IsActive = true;
                CurrentUnitOfWork.SaveChanges();
                menstruation = GetAllMenstruation(result.StartDatePeriod);
                var menstration = new MenstruationDetailsInfo();
                menstration.TenantId = input.TenantId;
                menstration.UniqueKey = result.UniqueKey;
                menstration.CycleDay = 28;
                menstration.MyCycle = input.DateAndTime.Date;
                menstration.Ovulation_date = menstruation.Ovulation_date;
                menstration.Last_Mens_Start = menstruation.Last_Mens_Start;
                menstration.Last_Mens_End = menstruation.Last_Mens_End;
                menstration.Next_Mens_Start = menstruation.Next_Mens_Start;
                menstration.Next_Mens_End = menstruation.Next_Mens_End;
                menstration.Ovulation_Window_Start = menstruation.Ovulation_Window_Start;
                menstration.Ovulation_Window_End = menstruation.Ovulation_Window_End;
                menstration.Status = "Normal";
                await _menstruationDetailsRepository.InsertAsync(menstration);
                menstration.IsActive = true;
                CurrentUnitOfWork.SaveChanges();

                var menstruationDetails = _menstruationDetailsRepository.GetAll().Where(x => x.UniqueKey == result.UniqueKey).ToList();
                var item = menstruationDetails[menstruationDetails.Count - 2];

                var updatemenstration = ObjectMapper.Map<MenstruationDetailsInfo>(item);
                updatemenstration.Status = "Normal";
                updatemenstration.Last_Mens_End = menstration.Last_Mens_Start.AddDays(-1);

                await _menstruationDetailsRepository.UpdateAsync(updatemenstration);
                updatemenstration.CycleDay = ((int)datediffernce.TotalDays);
                updatemenstration.IsActive = true;

                CurrentUnitOfWork.SaveChanges();
            }
            var data = ObjectMapper.Map<UniqueNameAndDateUniqueKeyDto>(result);
            return data;

        }
        public async Task<List<GetAllOrderPlacementbyIdDto>> CreateOrderPlacement(CreateOrderPlacementBulkDto createOrderPlacementDto)
        {
            List<GetAllOrderPlacementbyIdDto> getAllOrderPlacementbyIdDto1 = new List<GetAllOrderPlacementbyIdDto>();
            foreach (CreateOrderPlacementDto input in createOrderPlacementDto.createBulkOrderPlacement)
            {
                var data = ObjectMapper.Map<OrderPlacementInfo>(input);

                data.LastModificationTime = DateTime.Now;
                data.LastModifierUserId = Convert.ToInt32(AbpSession.UserId);
                data.TenantId = input.TenantId;
                await _orderPlacementRepository.InsertAsync(data);

                data.IsActive = true;
                CurrentUnitOfWork.SaveChanges();
                GetAllOrderPlacementbyIdDto getAllOrderPlacementbyIdDto = new GetAllOrderPlacementbyIdDto
                {
                    OrderId = data.Id,
                    Message = "OrderPlacement Create Successfully"

                };
                getAllOrderPlacementbyIdDto1.Add(getAllOrderPlacementbyIdDto);
            }

            return getAllOrderPlacementbyIdDto1;
        }
        public async Task<string> CreateSubscription(CreateSubscriptionDto input, int tenant)
        {
            SubscriptionInfo subscription = new SubscriptionInfo();
            var dataUniqueKey = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.UniqueKey == input.UniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();
            if (dataUniqueKey == null)
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
            var dataSubscriptionType = _subscriptionTypeRepository.GetAll().Where(x => x.Name == input.SubscriptionName && x.TenantId == tenant).FirstOrDefault();
            if (dataSubscriptionType == null)
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

        public async Task<string> CreateSubscriptionbyUniqueKey(string uniqueKey, int tenant)
        {

            var dataSubscription = _subscriptionRepository.GetAll().Where(x => x.UniqueKey == uniqueKey && x.TenantId == tenant).OrderByDescending(x => x.Id).FirstOrDefault();

            var getsubscriptiontype = _subscriptionTypeRepository.GetAll().Where(x => x.Id == dataSubscription.SubscriptionTypeId && x.TenantId == tenant).FirstOrDefault();
            if (getsubscriptiontype.Name == "Monthly")
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

            var dataSubscription = _subscriptionRepository.GetAll().Where(x => x.TenantId == tenant).ToList();
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

            GetAllMenstruationDetails menstruation = new GetAllMenstruationDetails();
            menstruation = GetAllMenstruation(uniquekey.StartDatePeriod);


            var result = new MenstruationDetailsInfo();
            result.TenantId = input.TenantId;
            result.UniqueKey = uniquekey.UniqueKey;
            result.MyCycle = menstruation.MyCycle;
            result.Ovulation_date = menstruation.Ovulation_date;
            result.Last_Mens_Start = menstruation.Last_Mens_Start;
            result.Last_Mens_End = menstruation.Last_Mens_End;
            result.Next_Mens_Start = menstruation.Next_Mens_Start;
            result.Next_Mens_End = menstruation.Next_Mens_End;
            result.Ovulation_Window_Start = menstruation.Ovulation_Window_Start;
            result.Ovulation_Window_End = menstruation.Ovulation_Window_End;

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
        public Task<PagedResultDto<UniqueNameAndDateUniqueKeyDto>> GetAllWithUniqueKey(PagedResultRequestDto input, string uniqueKey, int tenantId)
        {
            var query = _uniqueNameAndDateInfoRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == tenantId && x.UniqueKey == uniqueKey);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<UniqueNameAndDateUniqueKeyDto>(query.Count(), ObjectMapper.Map<List<UniqueNameAndDateUniqueKeyDto>>(statelist));
            return Task.FromResult(result);
        }
        protected GetAllMenstruationDetails GetAllMenstruation(DateTime currentMensDate)
        {

            var totalDays = currentMensDate.AddDays(28);
            var ovulation_day = (totalDays.AddDays(11));
            var last_mens_start = (totalDays.AddDays(-28));
            var last_mens_end = (totalDays.AddDays(-24));
            var next_mens_start = (totalDays.AddDays(-1));
            var next_mens_end = (totalDays.AddDays(3));
            var ovulation_window_start = (ovulation_day.AddDays(-2));
            var ovulation_window_end = (ovulation_day.AddDays(2));



            GetAllMenstruationDetails getAllMenstruationDetails = new GetAllMenstruationDetails
            {

                MyCycle = totalDays.Date,
                Ovulation_date = ovulation_day.Date,
                Last_Mens_Start = last_mens_start.Date,
                Last_Mens_End = last_mens_end.Date,
                Next_Mens_Start = next_mens_start.Date,
                Next_Mens_End = next_mens_end.Date,
                Ovulation_Window_Start = ovulation_window_start.Date,
                Ovulation_Window_End = ovulation_window_end.Date,

            };
            return getAllMenstruationDetails;
        }

        public GetAllMenstruationDetails GetAllMenstruationDetailsbyDate(DateTime currentMensDate)
        {
            var totalDays = currentMensDate.AddDays(28);
            var ovulation_day = (totalDays.AddDays(11));
            var last_mens_start = (totalDays.AddDays(-28));
            var last_mens_end = (totalDays.AddDays(-24));
            var next_mens_start = (totalDays.AddDays(-1));
            var next_mens_end = (totalDays.AddDays(3));
            var ovulation_window_start = (ovulation_day.AddDays(-2));
            var ovulation_window_end = (ovulation_day.AddDays(2));



            GetAllMenstruationDetails getAllMenstruationDetails = new GetAllMenstruationDetails
            {

                MyCycle = totalDays.Date,
                Ovulation_date = ovulation_day.Date,
                Last_Mens_Start = last_mens_start.Date,
                Last_Mens_End = last_mens_end.Date,
                Next_Mens_Start = next_mens_start.Date,
                Next_Mens_End = next_mens_end.Date,
                Ovulation_Window_Start = ovulation_window_start.Date,
                Ovulation_Window_End = ovulation_window_end.Date,

            };
            return getAllMenstruationDetails;
        }
        public List<GetAllMenstruationDetails> GetAllMenstruationDetailsbyUniqueKey(string uniquekey, int tenantId)
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
                Ovulation_date = menstruationdata.Ovulation_date,
                Last_Mens_Start = menstruationdata.Last_Mens_Start,
                Last_Mens_End = menstruationdata.Last_Mens_End,
                Next_Mens_Start = menstruationdata.Next_Mens_Start,
                Next_Mens_End = menstruationdata.Next_Mens_End,
                Ovulation_Window_Start = menstruationdata.Ovulation_Window_Start,
                Ovulation_Window_End = menstruationdata.Ovulation_Window_End,

            };
            menstruationDetails.Add(getAllMenstruationDetails);
            // FOR NEXT MONTH

            var totalDays = menstruationDetails[0].MyCycle.AddDays(28);
            var ovulation_date = totalDays.AddDays(11);
            var last_mens_start = totalDays.AddDays(-28);
            var last_mens_end = totalDays.AddDays(-24);
            var next_mens_start = totalDays.AddDays(-1);
            var next_mens_end = totalDays.AddDays(3);
            var ovulation_window_start = ovulation_date.AddDays(-2);
            var ovulation_window_end = ovulation_date.AddDays(2);



            getAllMenstruationDetails = new GetAllMenstruationDetails
            {
                UniqueKey = uniquekey,
                MyCycle = totalDays.Date,
                Ovulation_date = ovulation_date.Date,
                Last_Mens_Start = last_mens_start.Date,
                Last_Mens_End = last_mens_end.Date,
                Next_Mens_Start = next_mens_start.Date,
                Next_Mens_End = next_mens_end.Date,
                Ovulation_Window_Start = ovulation_window_start.Date,
                Ovulation_Window_End = ovulation_window_end.Date,

            };
            menstruationDetails.Add(getAllMenstruationDetails);

            // FOR NEXT MONTH


            var totalDays1 = menstruationDetails[1].MyCycle.AddDays(28);
            var ovulation_date1 = totalDays.AddDays(11);
            var last_mens_start1 = totalDays.AddDays(-28);
            var last_mens_end1 = totalDays.AddDays(-24);
            var next_mens_start1 = totalDays.AddDays(-1);
            var next_mens_end1 = totalDays.AddDays(3);
            var ovulation_window_start1 = ovulation_date.AddDays(-2);
            var ovulation_window_end1 = ovulation_date.AddDays(2);

            getAllMenstruationDetails = new GetAllMenstruationDetails
            {

                UniqueKey = uniquekey,
                MyCycle = totalDays1.Date,
                Ovulation_date = ovulation_date1.Date,
                Last_Mens_Start = last_mens_start1.Date,
                Last_Mens_End = last_mens_end1.Date,
                Next_Mens_Start = next_mens_start1.Date,
                Next_Mens_End = next_mens_end1.Date,
                Ovulation_Window_Start = ovulation_window_start1.Date,
                Ovulation_Window_End = ovulation_window_end1.Date,

            };
            menstruationDetails.Add(getAllMenstruationDetails);
            return menstruationDetails;


        }
        public PagedResultDto<GetAllMenstruation> GetAllMenstruationDetailsAgainstUniqueKey(string uniquekey, int tenantId)
        {
            List<GetAllMenstruation> menstruationDetails = new List<GetAllMenstruation>();
            var menstruationdata = _menstruationDetailsRepository.GetAll().Where(x => x.UniqueKey == uniquekey && x.TenantId == tenantId).ToList();
            if (menstruationdata.Count == 0)
            {
                throw new UserFriendlyException("Invalid Unique Key");
            }
            foreach (var item in menstruationdata)
            {
                GetAllMenstruation getAllMenstruationDetails = new GetAllMenstruation
                {
                    Id = item.Id,
                    UniqueKey = item.UniqueKey,
                    MyCycle = item.MyCycle,
                    Ovulation_date = item.Ovulation_date,
                    Last_Mens_Start = item.Last_Mens_Start,
                    Last_Mens_End = item.Last_Mens_End,
                    Next_Mens_Start = item.Next_Mens_Start,
                    Next_Mens_End = item.Next_Mens_End,
                    Ovulation_Window_Start = item.Ovulation_Window_Start,
                    Ovulation_Window_End = item.Ovulation_Window_End,
                    Status = item.Status,

                };
                menstruationDetails.Add(getAllMenstruationDetails);
            }

            menstruationDetails.Reverse();
            var result = new PagedResultDto<GetAllMenstruation>(menstruationDetails.Count(), ObjectMapper.Map<List<GetAllMenstruation>>(menstruationDetails));
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
                    Ovulation_date = item.Ovulation_date,
                    Last_Mens_Start = item.Last_Mens_Start,
                    Last_Mens_End = item.Last_Mens_End,
                    Next_Mens_Start = item.Next_Mens_Start,
                    Next_Mens_End = item.Next_Mens_End,
                    Ovulation_Window_Start = item.Ovulation_Window_Start,
                    Ovulation_Window_End = item.Ovulation_Window_End,

                };
                menstruationDetails.Add(getAllMenstruationDetails);
            }
            var last3index = menstruationDetails
                                .Where(c => c.UniqueKey == uniquekey).Skip(Math.Max(0, menstruationDetails.Count() - 3)).ToList();
            menstruationDetails.Skip(Math.Max(0, menstruationDetails.Count() - 3));
            var result = new PagedResultDto<GetAllMenstruationDetails>(last3index.Count(), ObjectMapper.Map<List<GetAllMenstruationDetails>>(last3index));
            return result;
        }
        public PagedResultDto<GetAllSubCategoryDto> GetAllSubCategorybyCategoryName(PagedResultRequestDto input, string categoryName, int tenantId)
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

        public PagedResultDto<GetAllBlogDto> GetAllBlogsbyCategoryName(string categoryName, int tenantId)
        {
            var category = _blogCategoryRepository.GetAll().Where(x => x.Name.Contains(categoryName) && x.TenantId == tenantId).FirstOrDefault();
            if (category != null)
            {
                var query = from sb in _blogRepository.GetAll().Where(x => x.IsActive == true && x.CategoryId == category.Id && x.TenantId == tenantId)
                            join ca in _blogCategoryRepository.GetAll() on sb.CategoryId equals ca.Id
                            select new GetAllBlogDto()
                            {
                                Id = sb.Id,
                                CategoryId = ca.Id,
                                CategoryName = ca.Name,
                                Title = sb.Title,
                                Description = sb.Description,
                                ImageUrl = sb.ImageUrl,
                            };
                var result = new PagedResultDto<GetAllBlogDto>(query.Count(), ObjectMapper.Map<List<GetAllBlogDto>>(query));
                return result;
            }

            return new PagedResultDto<GetAllBlogDto>();
        }
        public PagedResultDto<GetAllSubCategoryAndDateDto> GetAllSubCategoryAndDate(PagedResultRequestDto input, int tenantId)
        {

            var query = from sb in _subCategoryAndDateRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == tenantId)
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
        public PagedResultDto<GetAllImageSubCategoryDto> GetAllSubcategoryImageAllocation(PagedResultRequestDto input, int tenantId)
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
            var query = from sb in _subCategoryAndDateRepository.GetAll().Where(x => x.UniqueKey == uniqueKey && x.DateAndTime.Date == date.Date && x.TenantId == tenantId)
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
        public PagedResultDto<GetAllSubCategoryDataDto> GetAllSubCategoryWithUniquekey(PagedResultRequestDto input, int tenantId, string uniqueKey)
        {
            var query = from sb in _subCategoryAndDateRepository.GetAll().Where(x => x.UniqueKey == uniqueKey && x.TenantId == tenantId)
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
            categoryName = categoryName != null ? categoryName : string.Empty;
            List<GetAllProductParametersDto> productParametersDtos = new List<GetAllProductParametersDto>();

            var productCategoryData = _productCategoryRepository.GetAll().Where(x => x.IsActive == true
            && x.Name == categoryName).FirstOrDefault();
            if (productCategoryData == null)
            {
                throw new UserFriendlyException("No Product Category Found");
            }
            var productparameters = _productParametersRepository.GetAll().Where(x => x.ProductCategoryId == productCategoryData.Id).ToList();
            if (productparameters == null)
            {
                throw new UserFriendlyException("No Product Parameters Found");
            }
            foreach (var productparametersItem in productparameters)
            {


                GetAllProductParametersDto getAllProduct = new GetAllProductParametersDto()
                {

                    Id = productparametersItem.Id,
                    ProductName = productparametersItem.ProductName,
                    ProductImage = productparametersItem.ProductImage,
                    Description = productparametersItem.Description,
                    Price = productparametersItem.Price,
                    ProductCategoryId = productCategoryData.Id,
                    ProductCategoryName = productCategoryData.Name,

                };
                productParametersDtos.Add(getAllProduct);

            }
            //var productvariant = _productParameterVariantAllocationRepository.GetAll().Where(x => x.ProductParameterId == productparameters.Id).ToList();
            //if (productvariant.Count == 0)
            //{
            //    throw new UserFriendlyException("No Product Parameter Variant Allocation Found");
            //}


            var result = new PagedResultDto<GetAllProductParametersDto>(productParametersDtos.Count(), ObjectMapper.Map<List<GetAllProductParametersDto>>(productParametersDtos));

            return result;

        }
        public PagedResultDto<GetAllProductParametersDto> GetAllProductParameters(PagedResultRequestDto input)
        {
            List<GetAllProductParametersDto> productParametersDtos = new List<GetAllProductParametersDto>();


            var productparameters = _productParametersRepository.GetAll().ToList();

            foreach (var product in productparameters)
            {


                var productCategoryData = _productCategoryRepository.GetAll().Where(x => x.Id == product.ProductCategoryId && x.IsActive == true).FirstOrDefault();

                GetAllProductParametersDto getAllProduct = new GetAllProductParametersDto()
                {

                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    Description = product.Description,
                    ProductCategoryId = productCategoryData.Id,
                    ProductCategoryName = productCategoryData.Name,
                    Price = product.Price,
                };
                productParametersDtos.Add(getAllProduct);

            }



            var result = new PagedResultDto<GetAllProductParametersDto>(productParametersDtos.Count(), ObjectMapper.Map<List<GetAllProductParametersDto>>(productParametersDtos));

            return result;

        }
        public PagedResultDto<GetAllProductParametersDetailsDto> GetAllProductsDetailsById(PagedResultRequestDto input, long productId)
        {
            List<ProductImageAllocationInfo> productimage = new List<ProductImageAllocationInfo>();
            List<GetAllProductParametersDetailsDto> productParametersDtos = new List<GetAllProductParametersDetailsDto>();
            List<GetAllProductImageDto> getAllProductImageDtos = new List<GetAllProductImageDto>();
            //cache
            productimage = _productImageAllocationRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToList();

            var products = _productParametersRepository.GetAll().Where(x => x.Id == productId && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            var productCategoryData = _productCategoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == AbpSession.TenantId && x.Id == products.ProductCategoryId).FirstOrDefault();
            var allocatedImages = _productImageAllocationRepository.GetAll().Where(x => x.ProductParameterId == productId && x.TenantId == AbpSession.TenantId).ToList();
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



            GetAllProductParametersDetailsDto getAllProduct = new GetAllProductParametersDetailsDto()
            {

                Id = products.Id,
                ProductName = products.ProductName,
                ProductImage = products.ProductImage,
                Description = products.Description,
                ProductCategoryId = productCategoryData.Id,
                ProductCategoryName = productCategoryData.Name,
                Images = getAllProductImageDtos,
                Price = products.Price,
            };
            productParametersDtos.Add(getAllProduct);

            var result = new PagedResultDto<GetAllProductParametersDetailsDto>(productParametersDtos.Count(), ObjectMapper.Map<List<GetAllProductParametersDetailsDto>>(productParametersDtos));

            return result;

        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> SendMail(string text)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
        public PagedResultDto<GetAllBlogDto> GetAllBlogs(PagedResultRequestDto input, long tenantId)
        {

            var query = from b in _blogRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == tenantId)
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
            var result = new PagedResultDto<Entities.Blog.Dto.GetAllBlogDto>(query.Count(), ObjectMapper.Map<List<GetAllBlogDto>>(dataList));

            return result;

        }
        public PagedResultDto<GetAllBlogDto> GetAllBlogsByCategoryId(PagedResultRequestDto input, long categoryId, long tenantId)
        {

            var query = from b in _blogRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == tenantId
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
        public PagedResultDto<BlogCategoryDto> GetAllBlogCategory(PagedResultRequestDto input, long tenantId)
        {

            var dataBlogCategory = _blogCategoryRepository.GetAll().Where(x => x.IsActive == true && x.TenantId == tenantId).ToList();
            var statelist = dataBlogCategory.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var result = new PagedResultDto<BlogCategoryDto>(statelist.Count(), ObjectMapper.Map<List<BlogCategoryDto>>(statelist));
            return result;
        }
        public PagedResultDto<GetAllBlogsWithBlogCategoryDto> GetAllBlogsWithBlogCategory(PagedResultRequestDto input)
        {
            List<BlogCategoryInfo> blogCategories = new List<BlogCategoryInfo>();
            List<BlogInfo> blogs = new List<BlogInfo>();

            List<GetAllBlogsWithBlogCategoryDto> blogsWithBlogCategoryDto = new List<GetAllBlogsWithBlogCategoryDto>();

            blogCategories = _blogCategoryRepository.GetAll().ToList();
            blogs = _blogRepository.GetAll().ToList();


            foreach (var blogCategoriesItem in blogCategories)
            {
                List<GetAllGeneralBlogDto> blogList = new List<GetAllGeneralBlogDto>();

                var blogsData = blogs.Where(x => x.CategoryId == blogCategoriesItem.Id).ToList();
                foreach (var blogsDataItem in blogsData)
                {
                    GetAllGeneralBlogDto getAllBlogs = new GetAllGeneralBlogDto()
                    {
                        Id = blogsDataItem.Id,
                        Title = blogsDataItem.Title,
                        Description = blogsDataItem.Description,
                        CategoryId = blogsDataItem.CategoryId,
                        ImageUrl = blogsDataItem.ImageUrl
                    };
                    blogList.Add(getAllBlogs);
                }

                GetAllBlogsWithBlogCategoryDto getAllBlog = new GetAllBlogsWithBlogCategoryDto()
                {
                    Id = blogCategoriesItem.Id,
                    CategoryName = blogCategoriesItem.Name,
                    BlogsData = blogList
                };
                blogsWithBlogCategoryDto.Add(getAllBlog);

            }

            var result = new PagedResultDto<GetAllBlogsWithBlogCategoryDto>(blogsWithBlogCategoryDto.Count(), ObjectMapper.Map<List<GetAllBlogsWithBlogCategoryDto>>(blogsWithBlogCategoryDto));
            return result;

        }

        public AlertDto CreateAlert(AlertDto input, int TenantId)
        {
            var dto = ObjectMapper.Map<AlertInfo>(input);
            dto.TenantId = TenantId;
            alert_repo.Insert(dto);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AlertDto>(dto);
        }

        public PagedResultDto<AlertDto> GetAllAlerts(PagedResultRequestDto input, int TenantId)
        {
            var query = alert_repo.GetAll().Where(x => x.TenantId == TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<AlertDto>(query.Count(), ObjectMapper.Map<List<AlertDto>>(statelist));
            return result;
        }

        public PagedResultDto<AlertDto> GetAlertsByUniqueKey(PagedResultRequestDto input, string UniqueKey, int TenantId)
        {
            var query = alert_repo.GetAll().Where(x => x.UniqueKey == UniqueKey && x.TenantId == TenantId);
            var statelist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<AlertDto>(query.Count(), ObjectMapper.Map<List<AlertDto>>(statelist));
            return result;
        }
    }
}
