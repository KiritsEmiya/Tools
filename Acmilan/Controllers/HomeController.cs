using System.Net.Http;
using Acmilan.Models;
using Acmilan.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HtmlAgilityPack;

namespace Acmilan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICompanyService _companyService;

        public HomeController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<IEnumerable<CompanyServices>> GetServices()
        {
            var result = await _companyService.GetServiceAsync();
            return result;
        }

        public async Task<string> Capture(string str)
        {
            var handler = new HttpClientHandler()
            {
                //UseDefaultCredentials = true,
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true
            };
            handler.UseCookies = true;
            string code = "";
            HttpClient client = new HttpClient(handler, true);
            client.DefaultRequestHeaders.Add("User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests","1");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //获取我们当前网站的dom
            try
            {
                code = await client.GetStringAsync(str);
            }
            catch (Exception ex)
            {
                code = ex.ToString();
                //code = "Failure to parse the website";
            }
            //await _companyService.AddSiteView(code);
            //string webSite = string.Empty;
            //webSite = await _companyService.GetSiteView(4);
            return code;
        }

        public void AnalysisHtml(HtmlDocument doc,string tag,string attr,string value,string insertHtml)
        {
            var nodes = doc.DocumentNode.SelectNodes(tag);
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    //i++;
                    try
                    {
                        string attrValue = item.Attributes[attr].Value;
                        if (!attrValue.Contains("http"))
                        {
                            string webSite = value.Split('/')[2];
                            webSite = "http://" + webSite;
                            string itemHtml = item.WriteTo();
                            insertHtml = itemHtml.Insert(itemHtml.IndexOf(attrValue), webSite);
                            HtmlNode newChild = HtmlNode.CreateNode(insertHtml);
                            string str2 = item.XPath;
                            var htmlBody = doc.DocumentNode.SelectSingleNode(str2);
                            HtmlNode parent = htmlBody.ParentNode;
                            HtmlNode html = parent.ReplaceChild(newChild, htmlBody);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(i);
                        Console.WriteLine(ex);
                        continue;
                    }
                }
            }
        }

        public async Task<string> Analysis(string value)
        {
            HtmlWeb web = new HtmlWeb();
            string insertHtml = "";
            string docHtml = "";
            //int i = 0;
            try { 
                var doc = web.Load(value);
                AnalysisHtml(doc, "*//link", "href", value,insertHtml);
                AnalysisHtml(doc, "*//script", "src", value,insertHtml);
                AnalysisHtml(doc, "*//img", "src", value,insertHtml);
                docHtml = doc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                docHtml = await Capture(value);
            }
            return docHtml;
        }

        public async Task<IActionResult> SiteView(string value)
        {
            //string code = await Capture(value);
            string code = await Analysis(value);
            return View("SiteView", code);
        }

        public async Task<IActionResult> Index()
        {
            //_companyService.AddItem();
            ViewBag.Title = "Tools";
            return View(await _companyService.GetCompanyAsync());
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var result = await _companyService.GetCompanyAsync();
            return result;
        }

        public async Task<IEnumerable<Company>> GetCompany(int value)
        {
            var result = await _companyService.GetCompanyAsync(value);
            return result;
        }

        [HttpGet]
        public IEnumerable<Category> Select(int id)
        {
            //ModelItem modelList;
            //string str = string.Empty;
            var modelList = _companyService.SelectList();
            return modelList;
        }

        [HttpPost]
        public async Task<IActionResult> AddServiceList([FromBody]ViewServiceList lists)
        {
            if (ModelState.IsValid)
            {
                await _companyService.AddServiceList(lists);
            }
            return RedirectToAction("Index");
        }

        public async Task<ViewServiceList> GetServiceList(int company_id)
        {
            var lists = await _companyService.GetServiceList(company_id);
            return lists;
        }

        [HttpPost]
        public async Task<IActionResult> EditServiceList([FromBody]ViewServiceList lists)
        {
            if (ModelState.IsValid)
            {
                await _companyService.EditServiceList(lists);
            }
            return RedirectToAction("Index");
        }
    }
}
