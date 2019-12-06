using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DotNetFramework.Infrastructure.Utilities
{
    public class FileActionResult : IHttpActionResult
    {
        public FileActionResult(string filePath)
        {
            this.FilePath = filePath;
        }

        public FileActionResult(string fileName, Stream stream)
        {
            this.FilePath = fileName;
            this.Stream = stream;
            this.Stream.Seek(0, SeekOrigin.Begin);
        }

        public string FilePath { get; }

        public Stream Stream { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content = new StreamContent(this.Stream ?? File.OpenRead(this.FilePath))
            };

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(this.FilePath)
            };

            var contentType = "application/octet-stream";
            if (!string.IsNullOrEmpty(this.FilePath) && this.FilePath.EndsWith(".xlsx"))
            {
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return Task.FromResult(response);
        }
    }
}
