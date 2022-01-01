using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MovieStore.Data;
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

        public async Task<byte[]> EncodeImageAsync(IFormFile poster)
        {
            if (poster is null)
                return null;

            await using var ms = new MemoryStream();
            await poster.CopyToAsync(ms);
            return ms.ToArray();
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
            if (poster is null) return null;
            var posterImage = Convert.ToBase64String(poster);
            return $"data:{contentType};base64,{posterImage}";
        }
    }
}