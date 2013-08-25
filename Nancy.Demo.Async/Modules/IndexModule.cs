namespace Nancy.Demo.Async.Modules
{
    using System.Net.Http;
    using ServiceStack.Text;
    using System.Threading.Tasks;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters => View["Index"];

            Post["/", true] = async (x, ctx) =>
            {
                var link = await GetQrCode();
                var model = new { QrPath = link };
                return View["Index", model];
            };
        }

        private async Task<string> GetQrCode()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Mashape-Authorization", "oEzDRdFudTpsuLtmgewrIGcuj08tK7PI");
            var response = await client.GetAsync(
                    "https://mutationevent-qr-code-generator.p.mashape.com/generate.php?content=http://www.nancyfx.org&type=url");

            var stringContent = await response.Content.ReadAsStringAsync();

            dynamic model = JsonObject.Parse(stringContent); 

            return model["image_url"];
        }
    }
}