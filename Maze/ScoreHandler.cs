using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Maze.Models;
using Newtonsoft.Json;
using Maze.Game.Models;

namespace Maze
{
    class ScoreHandler
    {
        private const string API_URL = "http://calumtomeny.apphb.com/";

        public SubmitScoreResult SubmitGameResult(GameResult gameResult)
        {
            return PostGameResult(gameResult);
        }

        public List<MazeRaceScore> GetTopTenScores()
        {
            WebRequest request = WebRequest.Create(API_URL + "API/MazeGameAPI/TopTen");
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            List<MazeRaceScore> scores = JsonConvert.DeserializeObject<List<MazeRaceScore>>(responseFromServer);
            return scores;
        }

        public static SubmitScoreResult PostGameResult(GameResult gameResult)
        {
            var httpWebRequest = WebRequest.Create(API_URL + "API/MazeGameAPI/submitscore");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            string result = null;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = gameResult.ToJSONString();

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            return result.FromJSONString<SubmitScoreResult>();
        }

        public MazeRaceScore GetScoreById(int Id)
        {
            WebRequest request = WebRequest.Create(API_URL + "API/MazeGameAPI/getscorebyid?Id=" + Id);
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            MazeRaceScore score = JsonConvert.DeserializeObject<MazeRaceScore>(responseFromServer);
            return score;
        }
    }
}
