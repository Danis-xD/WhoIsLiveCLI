using System;
using System.Collections.Generic;

namespace WhoIsLiveCLI
{
    static class Program
    {
        static void Main(string[] args2)
        {
            string[] args = { "--name", "danis_xd" };
            var request = new HttpRequest();
            MyID myID;
            if ((args.Length > 0) && (args[0] == "--name"))
            {
                myID = new MyID(args[1], request);
            }
            else
            {
                myID = new MyID();
            }
            string ID = myID.ID;
            dynamic response = request.GetData(ID, "").Result;
            List<string> channelsList = new List<string>();
            List<List<string>> channelsListFull = new List<List<string>>();
            for (int i = 0; i < Convert.ToInt32(response.total); i++)
            {
                channelsList.Add(response.data[i % 100].to_id);
                if (i % 100 == 99)
                {
                    response = request.GetData(ID, response.pagination.cursor).Result;
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
                        Console.WriteLine(String.Format("{0, 25} {1, 7} {2} ", responseLive.data[j].login, liveListFull[j][2], liveListFull[j][3]));
                    }
                }
            }
        }
    }
}


