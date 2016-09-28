using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace DotaAPI.Models
{
    public class Match
    {
        public long MatchId { get; set; }
        public string Radiant_win { get; set; }
        public string Duration { get; set; }
        public static Match GetMatch(long MatchId)
        {
            var client = new RestClient("https://api.steampowered.com/IDOTA2Match_570/");
            var request = new RestRequest("GetMatchDetails/V001/?key=2C1D1B2E76EE4826FE71B8E9D878303C&match_id=" + MatchId + "&?", Method.GET);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var jsonResults = JsonConvert.DeserializeObject<Match>(jsonResponse["result"].ToString());
            return jsonResults;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task; 
        }
    }
}
