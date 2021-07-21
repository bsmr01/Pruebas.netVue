namespace Ibero.Services.Utilitary.Infrastructure
{
    using Ibero.Services.Utilitary.Core.Models;
    using Ibero.Services.Utilitary.Domain.Infrastructure.Abstract;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Utilitary.Infrastructure.Configuration;


    public class ExternalService : IExternalService
    {
        private Transaction transaction;

        public async Task<Transaction> PUTExternalService(object registrationForm, string registrationUrl)
        {
            using (var client1 = new HttpClient())
            {
                var dataExternalService = JsonConvert.SerializeObject(registrationForm).ToString();
                var content = new StringContent(dataExternalService, Encoding.UTF8, "application/json");

                var ResponseTask = await client1.PutAsync(registrationUrl, content);
                string Result = ResponseTask.Content.ReadAsStringAsync().Result;

                transaction = JsonConvert.DeserializeObject<Transaction>(Result, Converter.Settings);
            }
            return transaction;
        }



        public Transaction PUTExternalSyncService(object registrationForm, string registrationUrl)
        {
            using (var client1 = new HttpClient())
            {
                var dataExternalService = JsonConvert.SerializeObject(registrationForm).ToString();
                var content = new StringContent(dataExternalService, Encoding.UTF8, "application/json");

                var ResponseTask = client1.PutAsync(registrationUrl, content);
                string Result = ResponseTask.Result.Content.ReadAsStringAsync().Result;

                transaction = JsonConvert.DeserializeObject<Transaction>(Result, Converter.Settings);
            }
            return transaction;
        }

        public async Task<Transaction> POSTExternalServiceToken(object registrationForm, string registrationUrl, string tokenRef, bool automatic)
        {
            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri(registrationUrl);
                client1.DefaultRequestHeaders.Accept.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client1.DefaultRequestHeaders.Add("Authorization", tokenRef);

                var dataExternalService = JsonConvert.SerializeObject(registrationForm).ToString();
                var content = new StringContent(dataExternalService, Encoding.UTF8, "application/json");
                var ResponseTask = await client1.PostAsync(registrationUrl, content);
                string Result = ResponseTask.Content.ReadAsStringAsync().Result;
                if (automatic == true)
                {
                    transaction = JsonConvert.DeserializeObject<Transaction>(Result, Converter.Settings);
                }
                else
                {
                    transaction = new Transaction { message = Result.ToString() };
                }

            }
            return transaction;
        }
      

        public async Task<Transaction> POSTExternalService(object registrationForm, string registrationUrl, bool automatic = true)
        {
            using (var client1 = new HttpClient())
            {
                var dataExternalService = JsonConvert.SerializeObject(registrationForm).ToString();
                var content = new StringContent(dataExternalService, Encoding.UTF8, "application/json");

                var ResponseTask = await client1.PostAsync(registrationUrl, content);
                string Result = ResponseTask.Content.ReadAsStringAsync().Result;


                if (automatic == true)
                {
                    transaction = new Transaction { message = Result.ToString() };
                    transaction = new Transaction { code = "200" };
                }
                else
                {
                    transaction = new Transaction { message = Result.ToString() };
                }
            }
            return transaction;
        }

        public async Task<Transaction> POSTExternalServiceRefeshToken (object registrationForm, string registrationUrl,string tokenRef,string clientid,string Secretid)
        {
            using (var client1 = new HttpClient())
            {
                var client = new RestClient(registrationUrl);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "refresh_token=" + tokenRef + "&grant_type=refresh_token&client_id=" + clientid + "&client_secret=" + Secretid +"", ParameterType.RequestBody);

                IRestResponse response =  client.Execute(request);
                transaction = new Transaction { message = response.Content.ToString() };
               
            }
            return transaction;
        }
       

        public async Task<Transaction> POSTExternalServiceJSON(string registrationForm, string registrationUrl, bool automatic = true)
        {
            using (var client1 = new HttpClient())
            {
                var dataExternalService = registrationForm;
                var content = new StringContent(dataExternalService, Encoding.UTF8, "application/json");

                var ResponseTask = await client1.PostAsync(registrationUrl, content);
                string Result = ResponseTask.Content.ReadAsStringAsync().Result;

                if (automatic == true)
                {
                    transaction = JsonConvert.DeserializeObject<Transaction>(Result, Converter.Settings);
                }
                else
                {
                    transaction = new Transaction { message = Result.ToString() };
                }

            }
            return transaction;
        }

        public async Task<string> GETExternalService(string registrationUrl)
        {
            string Result = "";
            using (var client1 = new HttpClient())
            {
                var ResponseTask = await client1.GetAsync(registrationUrl);
                Result = ResponseTask.Content.ReadAsStringAsync().Result;
            }
            return Result;

        }

        public async Task<string> GETExternalServiceToken(string registrationUrl, string tokenRef)
        {
            string Result = "";

            using ( var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri(registrationUrl);
                client1.DefaultRequestHeaders.Accept.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client1.DefaultRequestHeaders.Add("Authorization", tokenRef);
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["token"].ToString());
                client1.Timeout = TimeSpan.FromMinutes(180);
                var ResponseTask = await client1.GetAsync(registrationUrl);
                Result = ResponseTask.Content.ReadAsStringAsync().Result;

            }
            return Result;
        }

        public async Task<string> GETExternalServiceTokenFile(string registrationUrl, string tokenRef, string direction, string NAmeFile)
        {
            string Result = "";
             
            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri(registrationUrl);
                client1.DefaultRequestHeaders.Accept.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client1.DefaultRequestHeaders.Add("Authorization", tokenRef);
                client1.Timeout = TimeSpan.FromMinutes(180);
                var ResponseTask = await client1.GetAsync(registrationUrl);

                File.WriteAllBytes(direction + NAmeFile, ResponseTask.Content.ReadAsByteArrayAsync().Result);

                Result = ResponseTask.Content.ReadAsStringAsync().Result;

            }
            return Result;
        }

        public async Task<Transaction> POSTExternalServiceFileToken (object registrationForm, string registrationUrl, string tokenRef, string NAmeFile, byte[] data )
        {
            using (var client1 = new HttpClient())
            {               
                using (var content = new MultipartFormDataContent())
                {
                    client1.BaseAddress = new Uri(registrationUrl);
                    client1.DefaultRequestHeaders.Accept.Clear();
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client1.DefaultRequestHeaders.Add("Authorization", tokenRef);
                    var dataExternalService = JsonConvert.SerializeObject(registrationForm).ToString();                    
                    content.Add( new StringContent(dataExternalService, Encoding.UTF8, "application/json"), "attributes");
                    content.Add(new StreamContent(new MemoryStream(data)), "file", "upload.pdf");                    
                    var ResponseTask = await client1.PostAsync(registrationUrl, content);
                    Thread.Sleep(1000);
                    string Result = ResponseTask.Content.ReadAsStringAsync().Result;                  
                    transaction = new Transaction { message = Result.ToString() };                    
                }

            }
            return transaction;
        }

    }
}




