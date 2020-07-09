using PuppeteerSharp;
using System;

namespace BrowserFetcher
{
    class Program
    {
        static async void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions());
            var page = await browser.NewPageAsync();

            await page.GoToAsync("https://read.amazon.com/notebook");

            var x = 1;

        }
    }
}
