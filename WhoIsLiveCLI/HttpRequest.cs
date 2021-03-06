﻿using System.Threading.Tasks;
using Flurl.Http;

namespace WhoIsLiveCLI
{
    public class HttpRequest
    {
        private string oauth = "b4ju86p4tb4nv6hn2ubbdsqox6iczd";
        private string channelOauth = "";
        public async Task<dynamic> GetData(string channel, string pagination)
        {
            string requestString = "https://api.twitch.tv/helix/users/follows?from_id=" + channel +
                                   "&first=100&after=" + pagination;
            var responseString = await requestString.WithHeader("Client-ID", oauth).GetJsonAsync();
            return responseString;
        }

        public async Task<dynamic> GetUserData(string channel)
        {
            string requestString = "https://api.twitch.tv/helix/streams?user_id=" + channel;
            var responseString = await requestString.WithHeader("Client-ID", oauth).GetJsonAsync();
            return responseString;
        }

        public async Task<dynamic> GetUserName(string channel)
        {
            string requestString = "https://api.twitch.tv/helix/users/?&id=" + channel;
            var responseString = await requestString.WithHeader("Client-ID", oauth).GetJsonAsync();
            return responseString;
        }

        public async Task<dynamic> GetUserID(string channel)
        {
            string requestString = "https://api.twitch.tv/helix/users?login=" + channel;
            var responseString = await requestString.WithHeader("Client-ID", oauth).GetJsonAsync();
            return responseString;
        }
        //public async Task<dynamic> CreateClip(string channel)
        //{
        //    try
        //    {
        //        dynamic responseName = GetUserID(channel).Result;
        //        string requestString = "https://api.twitch.tv/helix/clips?scope=clips:edit&broadcaster_id=" + responseName.data[0].id;
        //        var responseString = await requestString.WithHeader("Authorization", "Bearer " + channelOauth).PostAsync(null);
        //        return responseString;
        //    }
        //    catch (System.ArgumentOutOfRangeException)
        //    {
        //       System.Console.WriteLine("Wrong channel name");
        //       return;
        //    }
        //}
    }
}
