using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ModerationSystem.Webhook
{
    public class Http
    {
        public static void sendMessage(string message, string title)
        {
            var token = Plugin.Singleton.Config.WebHookURL;
            WebRequest wr = (HttpWebRequest)WebRequest.Create(token);
            wr.ContentType = "application/json";
            wr.Method = "POST";

            using (var sw = new StreamWriter(wr.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(new
                {
                    username = Plugin.Singleton.Config.WebHookName,
                    embeds = new[]
                    {
                        new
                        {
                            description = message,
                            title,
                            color = "8464285"
                        }
                    }
                });
                sw.Write(json);
            }

            var response = (HttpWebResponse)wr.GetResponse();
        }
    }
}