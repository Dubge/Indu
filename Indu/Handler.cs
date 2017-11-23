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
    class Handler
    {

        private HttpClient client = new HttpClient();
        public bool IDset = false;


        private int iD { get; set; }


        public int GetCharacterID(string token)
        {
            if (token == null)
            {
                return 1;
            }
            else if(!IDset)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var answer = client.GetAsync("https://login.eveonline.com/oauth/verify").Result;

                JObject liste = JObject.Parse(answer.Content.ReadAsStringAsync().Result);

                iD = int.Parse(liste.GetValue("CharacterID").ToString());
                IDset = true;
                return iD;
            }
            else if (IDset)
            {
                return iD;
            }
            return 1;
        }

        public string getName(string token)
        {
            if (IDset) {

                var answer = client.GetAsync("https://esi.tech.ccp.is/latest/characters/" + iD + "/?datasource=tranquility").Result;

                JObject liste = JObject.Parse(answer.Content.ReadAsStringAsync().Result);

                string name = liste.GetValue("name").ToString();

                return name;
            }
            else
            {
                GetCharacterID(token);
                getName(token);
            }
           
            return "log in noob";
        }

       




    }
}
