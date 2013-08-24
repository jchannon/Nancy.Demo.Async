namespace Nancy.Demo.Async.Modules
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading.Tasks;

    public class IndexModule : NancyModule
    {
        private readonly IRootPathProvider rootPathProvider;

        public IndexModule(IRootPathProvider rootPathProvider)
        {
            this.rootPathProvider = rootPathProvider;

            Get["/"] = parameters => View["Index"];

            Post["/", true] = async (x, ctx) =>
            {
                var link = await ResizeImage();
                var model = new { ResizePath = link };
                return View["Index", model];
            };
        }

        private async Task<string> ResizeImage()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var newWidth = 225;
                var newHeight = 255;
                var srcImage = Image.FromFile(rootPathProvider.GetRootPath() + "Content/nancy-logo.png");

                var newImage = new Bitmap(newWidth, newHeight);
                using (var gr = Graphics.FromImage(newImage))
                {
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                }

                newImage.Save(rootPathProvider.GetRootPath() + "Content/nancy-logo-resized.png");
                return "/Content/nancy-logo-resized.png";
            });

            return await task;
        }
    }
}