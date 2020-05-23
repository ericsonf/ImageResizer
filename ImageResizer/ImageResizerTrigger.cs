using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer
{
    public static class ImageResizerTrigger
    {
        [FunctionName("ImageResizerTrigger")]
        public static void Run([BlobTrigger("images/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, 
            [Blob("thumbnails/{name}", FileAccess.Write)] Stream outputBlob, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using (var image = Image.Load(myBlob))
            {
                var resizeOptions = new ResizeOptions
                {
                    Size = new Size(640, 480),
                    Compand = true,
                    Mode = ResizeMode.Max
                };

                image.Mutate(ctx => ctx.Resize(resizeOptions));
                image.Save(outputBlob, new JpegEncoder { Quality = 80 });
            }
        }
    }
}
