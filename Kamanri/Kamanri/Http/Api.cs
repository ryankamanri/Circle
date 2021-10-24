using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Newtonsoft.Json;

namespace Kamanri.Http
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

        public async Task<object> Get(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(defaultHost + url);
                return (await response.Content.ReadAsStringAsync()).ToObject<object>();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<Type> Get<Type>(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(defaultHost + url);
                return (await response.Content.ReadAsStringAsync()).ToObject<Type>();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<object> Get(string hostKey,string url)
        {
            try
            {
                string host = default;
                if(!hosts.TryGetValue(hostKey,out host)) return default;
                var response = await httpClient.GetAsync(host + url);
                return (await response.Content.ReadAsStringAsync()).ToObject<object>();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<Type> Get<Type>(string hostKey,string url)
        {
            try
            {
                string host = default;
                if(!hosts.TryGetValue(hostKey,out host)) return default;
                var response = await httpClient.GetAsync(host + url);
                return (await response.Content.ReadAsStringAsync()).ToObject<Type>();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<object> Post(string url,JsonObject obj)
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
                return (await response.Content.ReadAsStringAsync()).ToObject<object>();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<Type> Post<Type>(string url,JsonObject obj)
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
                return (await response.Content.ReadAsStringAsync()).ToObject<Type>();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<object> Post(string hostKey,string url,JsonObject obj)
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
                return (await response.Content.ReadAsStringAsync()).ToObject<object>();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<Type> Post<Type>(string hostKey,string url,JsonObject obj)
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
                return (await response.Content.ReadAsStringAsync()).ToObject<Type>();
            }catch(Exception e)
            {
                throw e;
            }
        }
    }
}