using System;
using System.IO;
using Newtonsoft.Json;

namespace WhoIsLiveCLI
{
    public class MyID
    {
        private string path = AppDomain.CurrentDomain.BaseDirectory + "\\settings.json";

        public MyID(string name, HttpRequest request)
        {
            try
            {
                dynamic responseName = request.GetUserID(name).Result;
                ID = responseName.data[0].id;
                File.WriteAllText(path, JsonConvert.SerializeObject(this));
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Wrong username");
                return;
            }
        }

        public MyID()
        {
            try
            {
                ID = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(path)).ID;
            }
            catch
            {
                Console.WriteLine("Provide username with --name");
                return;
            }
        }

        public string ID { get; set; }
    }
}
