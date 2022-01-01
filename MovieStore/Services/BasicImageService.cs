using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MovieStore.Services.Interfaces;

namespace MovieStore.Services
{
    public class BasicImageService : IImageService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BasicImageService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task<byte[]> EncodeImageAsync(IFormFile poster)
        {
            throw new System.NotImplementedException();
        }

        public Task<byte[]> EncodeImageUrlAsync(string imageUrl)
        {
            throw new System.NotImplementedException();
        }

        public string DecodeImage(byte[] poster, string contentType)
        {
            throw new System.NotImplementedException();
        }
    }
}
