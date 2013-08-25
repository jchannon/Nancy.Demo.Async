namespace Nancy.Demo.Async.Modules
{
    using System.Net.Http;
    using ServiceStack.Text;
    using System.Threading.Tasks;
    using System.Threading;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters => View["Index"];

            Post["/", true] = async (x, ct) =>
            {
                var link = await GetQrCode(ct);
                var model = new { QrPath = link };
                return View["Index", model];
            };
        }

        private async Task<string> GetQrCode(CancellationToken ct)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Mashape-Authorization", "oEzDRdFudTpsuLtmgewrIGcuj08tK7PI");
            var response = await client.GetAsync(
                    "https://mutationevent-qr-code-generator.p.mashape.com/generate.php?content=http://www.nancyfx.org&type=url", ct);

            var stringContent = await response.Content.ReadAsStringAsync();

            dynamic model = JsonObject.Parse(stringContent);

            return model["image_url"];
        }
    }
}