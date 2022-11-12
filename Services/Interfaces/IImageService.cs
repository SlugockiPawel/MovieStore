using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MovieStore.Services.Interfaces
{
    public interface IImageService
    {
        Task<byte[]> EncodeImageAsync(IFormFile poster);
        Task<byte[]> EncodeImageUrlAsync(string imageUrl);
        string DecodeImage(byte[] poster, string contentType);
    }
}
