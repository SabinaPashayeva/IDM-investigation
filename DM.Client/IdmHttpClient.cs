using System;
using System.Net.Http;
using Newtonsoft.Json;
using DM.Shared;
using DM.Client.Model;

namespace DM.Client
{
    public static class IdmHttpClient
    {
        public static EmployeeModel GetEmployeeDetails(int employeeId, string applicationId)
        {
            var idmApiUrl = GetIdmApiUrl();
            var url = $"{idmApiUrl}Employees?id={employeeId}&applicationId={applicationId}";

            try
            {
                var json = ExecuteRemoteCall(url, false);
                var employee = JsonConvert.DeserializeObject<EmployeeModel>(json);

                return employee;
            }
            catch (AggregateException exc)
            {
                Console.WriteLine(exc.StackTrace);
            }

            return new EmployeeModel
            {
                RoleId = 143,
                EmployeeGuid = "1234gdsdfgh"
            };
        }

        private static string GetIdmApiUrl()
        {
            return $"{ConfigSettings.IdentityServiceUrl}{ConfigSettings.IdentityServiceBaseDirectory}";
        }

        private static string ExecuteRemoteCall(string url, bool useApiVersion2)
        {
            var response = GetServiceResponse(url, useApiVersion2);
            var json = Convert.ToString(response.Content.ReadAsStringAsync().Result);
            return json;
        }

        private static HttpResponseMessage GetServiceResponse(string url, bool useApiVersion2)
        {
            using (var client = new HttpClient())
            {
                if (useApiVersion2)
                {
                    client.DefaultRequestHeaders.Add("X-API-VERSION", "2");
                }
                var response = client.GetAsync(url).Result;
                return response;
            }
        }
    }
}
