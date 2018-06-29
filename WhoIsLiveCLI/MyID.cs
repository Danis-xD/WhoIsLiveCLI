using System;
using System.IO;
using Newtonsoft.Json;

namespace WhoIsLiveCLI
{
    public class MyID
    {
        private string path = Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public MyID(string name, HttpRequest request)
        {
                dynamic responseName = request.GetUserID(name).Result;
                ID = responseName.data[0].id;
                File.WriteAllText(path, JsonConvert.SerializeObject(this));
        }

        public MyID()
        {
            ID = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(path)).ID;
        }

        public string ID { get; set; }
    }
}
