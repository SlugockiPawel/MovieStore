using System.IO;
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

        public async Task<byte[]> EncodeImageUrlAsync(string imageUrl)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(imageUrl);

            await using var stream = await response.Content.ReadAsStreamAsync();

            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            return ms.ToArray();
        }

        public string DecodeImage(byte[] poster, string contentType)
        {
            throw new System.NotImplementedException();
        }
    }
}