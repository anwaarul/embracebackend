using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Modules;

using System.Net.Http;
using Newtonsoft.Json;
using Abp.Domain.Uow;
using Quartz;
using OneSignal.RestAPIv3.Client;
using OneSignal.RestAPIv3.Client.Resources.Notifications;
using OneSignal.RestAPIv3.Client.Resources;
using System.Net.Http.Headers;
using System.Text;

namespace Embrace.Quartz
{
    public class OrderJob : IJob
    {

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Execute(IJobExecutionContext context)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            JobDataMap dataMap = context.MergedJobDataMap;
            var tenant = dataMap.GetLongValue("tenant");

            Uri u = new Uri("https://localhost:44311/api/services/app/General/CreateSubscriptionbyUniqueKeyCornJob?tenant=" + tenant);
            var payload = "{\"TenantId\": 3,\"}";

            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
            var t = Task.Run(() => PostURI(u, c));
            t.Wait();

            Console.WriteLine(t.Result);
            Console.ReadLine();

            //string apiResult = "";
            //HttpClient clientApi = new HttpClient();
            //Uri url = "https://localhost:44311/api/services/app/General/CreateSubscriptionbyUniqueKeyCornJob?tenant=" + tenant;
            ////var httpResponseMessage = await clientApi.PostAsync(new Uri(url);
            //HttpResponseMessage response = await clientApi.GetAsync(url);
            //response.EnsureSuccessStatusCode();
            //if (response.IsSuccessStatusCode)
            //{
            //    string responseBody = response.Content.ReadAsStringAsync().Result;
            //    apiResult = responseBody;
            //}

            static async Task<string> PostURI(Uri u, HttpContent c)
            {
                var response = string.Empty;
                using (var client = new HttpClient())
                {
                    HttpResponseMessage result = await client.PostAsync(u, c);
                    if (result.IsSuccessStatusCode)
                    {
                        response = result.StatusCode.ToString();
                    }
                }
                return response;
            }

        }
    }

}