using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KindleScrapeConsole
{
    class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions() { Headless = false });


            var page = await browser.NewPageAsync();

           
            Console.WriteLine("awaiting page load");

            await page.GoToAsync("https://read.amazon.com/notebook");

            var cookies = await page.GetCookiesAsync(new string[] { "https://read.amazon.com" });

            string[] bookNames = await page.EvaluateFunctionAsync<string[]>(" function(){ var elements = []; $('h2').each(function() " +
                                                                               "{ var ele = this; elements.push(ele.innerText) }); return elements;}");

            foreach(var bookName in bookNames)
            {
                string searchBookFunctionJson = @"function(){
                                                                            $('#kp-notebook-search-input')[0].value = '';
                                                                            $('#kp-notebook-search-input')[0].value = ""@BOOKNAME@"";
                                                                            $('#kp-notebook-search-input').submit();

                                                                            
                                                                        }
            ".Replace("@BOOKNAME@", bookName);
                await page.EvaluateFunctionAsync(searchBookFunctionJson);

                //First thing that shows up on the page, even if there are 0 highlights
                await page.WaitForSelectorAsync("#kp-notebook-highlights-count", new WaitForSelectorOptions() { Visible = true, Timeout = 5000 });

                string getHighlightsFunction = @"function(){ 
                                                            var highlights = $(""[id ^= 'highlight-']"").toArray().map(ele => ele.innerText);
                                                            return highlights;
                                                            }";
                var highlights = await page.EvaluateFunctionAsync<string[]>(getHighlightsFunction);
                Console.WriteLine($"Total Highlights for {bookName} : {highlights.Length}");
            }

            var x = 1;

        }
    }
}
