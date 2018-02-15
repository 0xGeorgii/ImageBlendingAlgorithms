using SixLabors.ImageSharp;
using SourceProvider.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SourceProvider.Network
{
    public class SrcLoader
    {
        public static async Task<byte[]> DownloadSrcAsync(uint x, uint y)
        {
            return await DownloadSrcAsync(x, y, CancellationToken.None);
        }

        public static async Task<byte[]> DownloadSrcAsync(uint x, uint y, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, UrlProvider.GetRandomUrl(x, y)))
            {
                return await (await client.SendAsync(request, cancellationToken)).Content.ReadAsByteArrayAsync();
            }
        }

        public static async Task<Image<Rgba32>> DownloadImageAsync(uint x, uint y)
        {
            return await DownloadImageAsync(x, y, CancellationToken.None);
        }

        public static async Task<Image<Rgba32>> DownloadImageAsync(uint x, uint y, CancellationToken cancellationToken)
        {
            var data = await DownloadSrcAsync(x, y, cancellationToken);
            return Image.Load(data);
        }

        public static async Task<string> DownloadAndSaveSrc(uint x, uint y, string ext = ".jpg")
        {
            var fileName = $"./{Guid.NewGuid().ToString()}{ext}";
            using (var stream = new FileStream(fileName, FileMode.CreateNew))
            {
                var data = await DownloadSrcAsync(x, y);
                await stream.WriteAsync(data, 0, data.Length);
            }
            return fileName;
        }
    }
}
