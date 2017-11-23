using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Indu.Server
{
    class Server
    {
        private HttpListener listen = new HttpListener();
        private HttpClient client = new HttpClient();


        public string[] s = new string[200];

        private string token { get; set; }
        public string Token
        {
            get
            {
                return token;
            }
        }



        public void Start(string prefix)
        {
            listen.Prefixes.Add(prefix);
            listen.Start();
            HttpListenerContext context = listen.GetContext();
            Process(context);
        }

        private void Process(HttpListenerContext context)
        {
            if(context.Request.QueryString.AllKeys.Length > 0)
            {
                HttpListenerRequest res = context.Request;

                string[] authcode = res.QueryString.GetValues("code");

                ResponebuildAsync(authcode[0]);
                
            }
            else
            {
                listen.Close();
            }            
        }

        private async void ResponebuildAsync(string authcode)
        {
            List<KeyValuePair<string, string>> val = new List<KeyValuePair<string, string>>();

            val.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            val.Add(new KeyValuePair<string, string>("code", authcode));

            var content = new FormUrlEncodedContent(val);

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("UTF-8").GetBytes("2523a6cfe3b74756b95ff162c02675e8" + ":" + "AehGxdW2RKBwVvKZQTxEyu88CX6adDiEw4rkLF1t"));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

            var answer = await Task.Run(() => client.PostAsync("https://login.eveonline.com/oauth/token", content));
            client.Dispose();


            JObject liste = JObject.Parse(answer.Content.ReadAsStringAsync().Result);

            ResponeProcess(liste);
        }

        private void ResponeProcess(JObject ans)
        {
            token = ans.GetValue("access_token").ToString();

        }
    }
}