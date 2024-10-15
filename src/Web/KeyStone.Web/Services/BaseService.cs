using KeyStone.Web.Common;
using KeyStone.Web.Common.Models;
using KeyStone.Web.Extensions;
using System.Net.Http.Json;

namespace KeyStone.Web.Services
{
    public class BaseService
    {
        private readonly HttpClient _httpClient;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.ApiHttpClient);
        }

        public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(endpoint, request);
            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
            return ParseApiResponseForErrors(apiResponse);
        }

        public async Task<ApiResponse<TResponse>> PostAsync<TResponse>(string endpoint)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(endpoint, new { });
            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
            return ParseApiResponseForErrors(apiResponse);
        }

        public async Task<ApiResponse<TResponse>> PostMultipartAsync<TResponse>(string endpoint, MultipartFormDataContent content)
        {
            var httpResponse = await _httpClient.PostAsync(endpoint, content);
            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
            return ParseApiResponseForErrors(apiResponse);
        }

        public async Task<ApiResponse<TResponse>> GetAsync<TResponse>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint)!;
            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<TResponse>.OnFailure();
            }
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
            return apiResponse ?? ApiResponse<TResponse>.OnFailure();

        }
        public async Task<ApiResponse<byte[]>> GetByteArrayAsync(string endpoint)
        {
            //var byteArray = await _httpClient.GetByteArrayAsync(endpoint)!;
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<Byte[]>.OnFailure();
            }
            var byteArray = await response.Content.ReadAsByteArrayAsync();
            return ApiResponse<Byte[]>.OnSuccess(byteArray);
        }

        public async Task<ApiResponse<TResponse>> GetAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse<TResponse>>(endpoint)!;
            return ParseApiResponseForErrors(apiResponse);
        }

        public async Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string endpoint)
        {
            var apiResponse = await _httpClient.DeleteFromJsonAsync<ApiResponse<TResponse>>(endpoint);
            return ParseApiResponseForErrors(apiResponse);
        }

        public async Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync(endpoint, request);
            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
            return ParseApiResponseForErrors(apiResponse);
        }


        private static ApiResponse<T> ParseApiResponseForErrors<T>(ApiResponse<T>? apiResponse)
        {
            if (apiResponse is null)
                return ApiResponse<T>.OnFailure();

            if (!apiResponse.IsSuccess && apiResponse.ErrorResult is not null)
            {
                foreach (var error in apiResponse.ErrorResult)
                {
                    string objText = error.Value?.ToString() ?? "[]";
                    string[] errors = objText.ToObjectFromJson<string[]>() ?? [];
                    apiResponse.Errors = apiResponse.Errors.Concat(errors).ToArray();
                }
            }

            return apiResponse;
        }

    }
}
