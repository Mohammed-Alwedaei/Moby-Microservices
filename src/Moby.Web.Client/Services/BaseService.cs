using Moby.Web.Client.Models;
using Moby.Web.Client.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace Moby.Web.Client.Services
{
    public class BaseService : IBaseService
    {
        public HttpRequestModel HttpRequest { get; set; }

        private IHttpClientFactory HttpClientFactory { get; set; }

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;

            HttpRequest = new();
        }

        public async Task<T> SendAsync<T>(HttpRequestModel httpRequest)
        {
            try
            {
                var client = HttpClientFactory.CreateClient("Moby.Client");
                HttpRequestMessage message = new();

                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(httpRequest.Url);

                client.DefaultRequestHeaders.Clear();

                if (httpRequest.Data is not null)
                    message.Content = new StringContent(JsonConvert.SerializeObject(httpRequest.Data),
                        encoding: Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                switch (httpRequest.HttpMethodTypes)
                {
                    case SD.HttpMethodTypes.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case SD.HttpMethodTypes.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case SD.HttpMethodTypes.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                response = await client.SendAsync(message);

                var content = await response.Content.ReadAsStringAsync();

                var responseDto = JsonConvert.DeserializeObject<T>(content);

                return responseDto;
            }
            catch (Exception exception)
            {
                var dto = new ResponseDto()
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        exception.Message
                    },
                    Message = "Error"
                };

                var response = JsonConvert.SerializeObject(dto);
                var responseDto = JsonConvert.DeserializeObject<T>(response);

                return responseDto;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}