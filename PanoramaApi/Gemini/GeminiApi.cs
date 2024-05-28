using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using PanoramaApi.Gemini.Models;
using System.Text;

namespace PanoramaApi.Gemini
{
    public class GeminiApi
    {
        public Uri Url { get; }
        public string Key { get; }
        public GeminiModel Model { get; set; }

        public GeminiApi(Uri url, string key, GeminiModel model = GeminiModel.GeminiPro)
        {
            Url = url;
            Key = key;
            Model = model;
        }

        public async Task<GenerateContentResponse> GenerateContent(string prompt)
        {
            return await GenerateContent(new GenerateContentRequest
            {
                Contents = new List<Content>
                {
                    new Content
                    {
                        Parts = new List<ContentPart>
                        {
                            new ContentPart
                            {
                                Text = prompt
                            }
                        }
                    }
                }
            });
        }

        public async Task<GenerateContentResponse> GenerateContent(GenerateContentRequest request)
        {
            return await MakeRequest<GenerateContentResponse>("generateContent", async client =>
            {
                var content = new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }), Encoding.UTF8, "application/json");
                return await client.PostAsync("", content);
            }); 
        }

        protected async Task<T> MakeRequest<T>(string path, Func<HttpClient, Task<HttpResponseMessage>> callback, Dictionary<string, object>? query = null)
        {
            var queryItems = query is null ? new List<string>() : query.Select(pair => $"{pair.Key}={pair.Value}");

            using (var client = new HttpClient())
            {
                var modelPath = GetModelPath(Model);

                client.BaseAddress = new Uri($"{Url}models/{modelPath}:{path}?{string.Join('&', queryItems)}&key={Key}");

                var result = await callback(client);
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    var obj = JsonConvert.DeserializeObject<T>(content);

                    if (obj is null)
                    {
                        throw new Exception("Request returned nothing.");
                    }

                    return obj;
                }

                throw new HttpRequestException(content, null, result.StatusCode);
            }
        }

        public static string GetModelPath(GeminiModel model)
        {
            switch (model)
            {
                case GeminiModel.GeminiPro:
                    return "gemini-pro";

                default:
                    throw new Exception($"Path not found for model {model}");
            }
        }
    }
}
