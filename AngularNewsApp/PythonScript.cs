using IronPython.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RunPythonScript
{
    public class RecommenderSystem
    {
        public List<int> GetRecommendations(int idUser)
        {

            string filePythonScript = @"main.py";

            string outputText = string.Empty;
            string standardError = string.Empty;
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo()
                    {
                        FileName = "py",
                        Arguments = filePythonScript + " " + idUser,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };
                    process.Start();
                    outputText = process.StandardOutput.ReadToEnd();
                    outputText = outputText.Replace(Environment.NewLine, string.Empty);
                    standardError = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    JsonReader reader = new JsonTextReader(new StringReader(outputText));

                    var datas = new JsonSerializer().Deserialize(reader, typeof(List<List<ArticleRecommended>>)) as List<List<ArticleRecommended>>;

                    var articles = datas.SelectMany(x => x);

                    return articles.Where(x=>x.concordance > 0.25).Select(x => x.idArticle).ToList();
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;

                return new List<int>();
            }

        }
    }

    public class ArticleRecommended
    {
        public int idArticle { get; set; }
        public double concordance { get; set; }
    }

}


