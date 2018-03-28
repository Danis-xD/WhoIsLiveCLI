using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WhoIsLiveCLI
{
    static class Program
    {
        static void Main(string[] args)
        {
            var request = new HttpRequest();
            string MyID;

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\settings.json";

            if (args.Length > 0)
            {
                if (args[0] == "--name")
                {
                    try
                    {
                        dynamic responseName = request.GetUserID(args[1]).Result;
                        MyID = responseName.data[0].id;
                        MyIDjson myIDjson = new MyIDjson
                        {
                            MyID = MyID
                        };
                        File.WriteAllText(path, JsonConvert.SerializeObject(myIDjson));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Wrong username");
                        return;
                    }
                }
            }
            try
            {
                MyIDjson IDjson = JsonConvert.DeserializeObject<MyIDjson>(File.ReadAllText(path));
                MyID = IDjson.MyID;
            }
            catch
            {
                Console.WriteLine("Provide username with --name");
                return;
            }

            

        
            dynamic response = request.GetData(MyID, "").Result;
            List<string> channelsList = new List<string>();
            List<List<string>> channelsListFull = new List<List<string>>();
            for (int i = 0; i < Convert.ToInt32(response.total); i++)
            {
                channelsList.Add(response.data[i % 100].to_id);
                if (i % 100 == 99)
                {
                    response = request.GetData(MyID, response.pagination.cursor).Result;
                    channelsList = new List<string>();
                }
            }

            if (channelsList.Count != 0)
            {
                channelsListFull.Add(channelsList);
            }

            var liveListFull = new List<List<string>>();
            bool fl = false;
            foreach (List<string> item in channelsListFull)
            {

                var LiveRequest = String.Join("&user_id=", item.ToArray());
                dynamic responseChannel = request.GetUserData(LiveRequest).Result;
                var liveList = new List<List<string>>();

                for (int i = 0; i < item.Count; i++)
                {
                    try
                    {
                        var tempList = new List<string>(){ responseChannel.data[i].user_id, responseChannel.data[i].game_id, responseChannel.data[i].viewer_count.ToString(), responseChannel.data[i].title };
                       
                        liveList.Add(tempList);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        fl = true;
                        break;
                    }
                }

                liveListFull.AddRange(liveList);
                if (fl)
                {
                    break;
                }
            }

            List<string> ListNameRequest = new List<string>();
            for (var i = 0; i < liveListFull.Count; i++)
            {
                ListNameRequest.Add(liveListFull[i][0]);
                if ((i % 100 == 99) || (i == liveListFull.Count - 1))
                {
                    string NameRequest = String.Join("&id=", ListNameRequest.ToArray());
                    dynamic responseLive = request.GetUserName(NameRequest).Result;
                    for (int j = 0; j <= i; j++)
                    {
                        Console.WriteLine(String.Format("{0, 25} {1, 7} {2, 30} ", responseLive.data[j].login, liveListFull[j][2], liveListFull[j][3]));
                    }
                }
            }
        }
    }
}


