using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dotnet.Services.Http
{
    public class Api
    {

        private HttpClient httpClient;

        private string defaultHost;
        private IDictionary<string,string> hosts;
        public Api(string host)
        {
            httpClient = new HttpClient();
            defaultHost = host;
        }

        public Api(Action<IDictionary<string,string>> SetHosts)
        {
            httpClient = new HttpClient();
            hosts = new Dictionary<string,string>();
            SetHosts(hosts);
        }

        public async Task<string> Get(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(defaultHost + url);
                return await response.Content.ReadAsStringAsync();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<string> Get(string hostKey,string url)
        {
            try
            {
                string host = default;
                if(!hosts.TryGetValue(hostKey,out host)) return default;
                var response = await httpClient.GetAsync(host + url);
                return await response.Content.ReadAsStringAsync();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<string> Post(string url,IDictionary<string,object> obj)
        {
            try
            {
                var strfiedObj = new Dictionary<string,string>();
                foreach(var key in obj.Keys)
                {
                    strfiedObj[key] = JsonConvert.SerializeObject(obj[key]);
                }
                var content = new FormUrlEncodedContent(strfiedObj);
                var response = await httpClient.PostAsync(defaultHost + url,content);
                return await response.Content.ReadAsStringAsync();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<string> Post(string hostKey,string url,IDictionary<string,object> obj)
        {
            try
            {
                string host = default;
                if(!hosts.TryGetValue(hostKey,out host)) return default;
                var strfiedObj = new Dictionary<string,string>();
                foreach(var key in obj.Keys)
                {
                    strfiedObj[key] = JsonConvert.SerializeObject(obj[key]);
                }
                var content = new FormUrlEncodedContent(strfiedObj);
                var response = await httpClient.PostAsync(host + url,content);
                return await response.Content.ReadAsStringAsync();
            }catch(Exception e)
            {
                throw e;
            }
        }
    }
}