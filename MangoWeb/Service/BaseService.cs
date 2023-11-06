using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MangoWeb.Models;
using MangoWeb.Service.IService;
using Newtonsoft.Json;
using static MangoWeb.Utillity.SD;

namespace MangoWeb.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDTO?> SendAsync(RequestDTO requestDTO)
        {
            HttpClient client = _httpClientFactory.CreateClient("MangoAPI");

            HttpRequestMessage message = new HttpRequestMessage
            {
                RequestUri = new Uri(requestDTO.Url),
                Method = GetHttpMethod(requestDTO.ApiType)
            };

            message.Headers.Add("Accept", "application/json");

            if (requestDTO.Data != null)
            {
                var jsonData = JsonConvert.SerializeObject(requestDTO.Data);
                message.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage apiResponse = await client.SendAsync(message);

            return await HandleApiResponse(apiResponse);
        }

        private HttpMethod GetHttpMethod(ApiType apiType)
        {
            return apiType switch
            {
                ApiType.POST => HttpMethod.Post,
                ApiType.DELETE => HttpMethod.Delete,
                ApiType.PUT => HttpMethod.Put,
                _ => HttpMethod.Get,
            };
        }

        private async Task<ResponseDTO?> HandleApiResponse(HttpResponseMessage apiResponse)
        {
            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new ResponseDTO { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.Forbidden:
                    return new ResponseDTO { IsSuccess = false, Message = "Access Denied" };
                case HttpStatusCode.Unauthorized:
                    return new ResponseDTO { IsSuccess = false, Message = "Unauthorized" };
                case HttpStatusCode.InternalServerError:
                    return new ResponseDTO { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                    return result;
            }
        }
    }
}
