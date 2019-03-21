using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Heavy.Web.ViewComponents
{
    public class ViewHtml: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string code = "";
            HttpClient client = new HttpClient(new HttpClientHandler());
            //获取我们当前网站的dom
            try
            {
                code = await client.GetStringAsync("https://www.baidu.com");
            }
            catch
            {
                code = "Failure to parse the website";
            }
            return View("Default", code);
        }
    }
}
