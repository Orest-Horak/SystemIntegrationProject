using DTO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace DataRetriever
{
    public interface ICrawl
    {
        IEnumerable<NewsDTO> Crawl();
    }

    public class Crawlers : ICrawl
    {
        public IEnumerable<NewsDTO> Crawl()
        {
            string homeUrl = "https://www.theverge.com/tech";

            ChromeOptions chromeOptions = new ChromeOptions();
            IWebDriver driver = new ChromeDriver(chromeOptions);
            driver.Navigate().GoToUrl(homeUrl);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.CssSelector(".p-page-title ")));

            var elements = driver.FindElements(By.CssSelector(".c-entry-box--compact__title a"));

            List<NewsDTO> news = elements
                .Select(el => new NewsDTO
                {
                    Title = el.Text,
                    Url = el.GetAttribute("href")
                })
                .ToList();


            for (int i = 0; i < news.Count; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var n = news[i];
                try
                {
                    driver.Navigate().GoToUrl(n.Url);
                    wait.Until(d => d.FindElement(By.CssSelector(".c-page-title")));

                    n.Author = driver.FindElement(By.CssSelector("meta[property^=author]")).GetAttribute("content");
                    n.Description = driver.FindElement(By.CssSelector("meta[name^=description]")).GetAttribute("content");
                    n.DateOfPublication = DateTime.Parse(driver.FindElement(By.CssSelector("meta[property^=\"article:published_time\"]")).GetAttribute("content"));

                    var commentsBlock = driver.FindElement(By.CssSelector(".c-comments"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", commentsBlock);

                    wait.Until(d => d.FindElement(By.CssSelector(".c-comments__count")));

                    var comments = driver.FindElements(By.ClassName("c-comments__comment"));
                    foreach (var comment in comments)
                    {
                        n.Comments.Add(new CommentDTO
                        {
                            Author = comment.FindElement(By.CssSelector(".c-comments__header-author .c-comments__author")).Text,
                            Text = comment.FindElement(By.CssSelector(".c-comments__cbody")).Text,
                            DateOfPost = DateTime.ParseExact(comment.FindElement(By.CssSelector(".c-comments__date a")).Text, "MMM d, yyyy | h:mm tt", CultureInfo.InvariantCulture)
                        });
                    }

                }
                catch (Exception ex)
                {
                    //log exception
                }
                yield return n;
            }

            driver.Close();
        }
    }

    //public class UkrNetCrawler : ICrawl
    //{
    //    public IEnumerable<NewsDTO> Crawl()
    //    {
    //        string homeUrl = "https://www.ukr.net/news/proisshestvija.html";

    //        ChromeOptions chromeOptions = new ChromeOptions();
    //        IWebDriver driver = new ChromeDriver(chromeOptions);
    //        driver.Navigate().GoToUrl(homeUrl);
    //        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);


    //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
    //        //wait.Until(d => d.FindElement(By.CssSelector(".p-page-title ")));

    //        var elements = driver.FindElements(By.CssSelector(".im-tl a"));

    //        List<NewsDTO> news = elements
    //            .Select(el => new NewsDTO
    //            {
    //                Title = el.Text,
    //                Url = el.GetAttribute("href")
    //            })
    //            .Take(5).ToList();


    //        for (int i = 0; i < news.Count; i++)
    //        {
    //            Thread.Sleep(TimeSpan.FromSeconds(5));
    //            var n = news[i];
    //            //try
    //            //{
    //            //    driver.Navigate().GoToUrl(n.Url);
    //            //    wait.Until(d => d.FindElement(By.CssSelector(".c-page-title")));

    //            //    n.Author = driver.FindElement(By.CssSelector("meta[property^=author]")).GetAttribute("content");
    //            //    n.Description = driver.FindElement(By.CssSelector("meta[name^=description]")).GetAttribute("content");
    //            //    n.DateOfPublication = DateTime.Parse(driver.FindElement(By.CssSelector("meta[property^=\"article:published_time\"]")).GetAttribute("content"));

    //            //    var commentsBlock = driver.FindElement(By.CssSelector(".c-comments"));
    //            //    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", commentsBlock);

    //            //    wait.Until(d => d.FindElement(By.CssSelector(".c-comments__count")));

    //            //    var comments = driver.FindElements(By.ClassName("c-comments__comment"));
    //            //    foreach (var comment in comments)
    //            //    {
    //            //        n.Comments.Add(new CommentDTO
    //            //        {
    //            //            Author = comment.FindElement(By.CssSelector(".c-comments__header-author .c-comments__author")).Text,
    //            //            Text = comment.FindElement(By.CssSelector(".c-comments__cbody")).Text,
    //            //            DateOfPost = DateTime.ParseExact(comment.FindElement(By.CssSelector(".c-comments__date a")).Text, "MMM d, yyyy | h:mm tt", CultureInfo.InvariantCulture)
    //            //        });
    //            //    }

    //            //}
    //            //catch (Exception ex)
    //            //{
    //            //    //log exception
    //            //}
    //            yield return n;
    //        }

    //        driver.Close();
    //    }
    //}

    public class NYTimesCrawler : ICrawl
    {
        public IEnumerable<NewsDTO> Crawl()
        {
            string homeUrl = "https://www.nytimes.com/section/technology";

            ChromeOptions chromeOptions = new ChromeOptions();

            IWebDriver driver = new ChromeDriver(chromeOptions);
            driver.Navigate().GoToUrl(homeUrl);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            //wait.Until(d => d.FindElement(By.CssSelector(".p-page-title ")));

            //var elements = driver.FindElements(By.CssSelector(".entry-title td-module-title a"));
            var elements = driver.FindElements(By.XPath("//*[@id='app']/div[1]/div[2]/header/section[1]/div[3]/a"));

            List<NewsDTO> news = elements
                .Select(el => new NewsDTO
                {
                    Title = el.Text,
                    Url = el.GetAttribute("href")
                })
                .Take(5).ToList();
            Console.WriteLine("News count {0}", news.Count);


            for (int i = 0; i < news.Count; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var n = news[i];
                try
                {
                    driver.Navigate().GoToUrl(n.Url);
                    wait.Until(d => d.FindElement(By.CssSelector(".c-page-title")));

                    n.Author = driver.FindElement(By.CssSelector("meta[property^=author]")).GetAttribute("content");
                    n.Description = driver.FindElement(By.CssSelector("meta[name^=description]")).GetAttribute("content");
                    n.DateOfPublication = DateTime.Parse(driver.FindElement(By.CssSelector("meta[property^=\"article:published_time\"]")).GetAttribute("content"));

                    var commentsBlock = driver.FindElement(By.CssSelector(".c-comments"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", commentsBlock);

                    wait.Until(d => d.FindElement(By.CssSelector(".c-comments__count")));

                    var comments = driver.FindElements(By.ClassName("c-comments__comment"));
                    foreach (var comment in comments)
                    {
                        n.Comments.Add(new CommentDTO
                        {
                            Author = comment.FindElement(By.CssSelector(".c-comments__header-author .c-comments__author")).Text,
                            Text = comment.FindElement(By.CssSelector(".c-comments__cbody")).Text,
                            DateOfPost = DateTime.ParseExact(comment.FindElement(By.CssSelector(".c-comments__date a")).Text, "MMM d, yyyy | h:mm tt", CultureInfo.InvariantCulture)
                        });
                    }

                }
                catch (Exception ex)
                {
                    //log exception
                }
                yield return n;
            }

            driver.Close();
        }
    }


    public class NewsAggregationPage
    {
        protected IWebDriver driver;

        //private By messageBy = By.tagName("h1");


        private string pageUrl;

        private By newsBy;

        public NewsAggregationPage(IWebDriver driver, string pageUrl, By newsBy)
        {
            this.driver = driver;

            //if (!driver.getTitle().equals("Home Page of logged in user"))
            //{
            //    throw new IllegalStateException("This is not Home Page of logged in user," +
            //          " current page is: " + driver.getCurrentUrl());
            //}
        }

        //public String getMessageText()
        //{
        //    return driver.findElement(messageBy).getText();
        //}

        //public NewsAggregationPage manageProfile()
        //{
        //    // Page encapsulation to manage profile functionality
        //    return new HomePage(driver);
        //}

    }

      

    public class VoxCrawler : ICrawl
    {
        public IEnumerable<NewsDTO> Crawl()
        {
            string homeUrl = "https://www.vox.com/";

            ChromeOptions chromeOptions = new ChromeOptions();
            IWebDriver driver = new ChromeDriver(chromeOptions);


            driver.Navigate().GoToUrl(homeUrl);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            //wait.Until(d => d.FindElement(By.CssSelector(".p-page-title ")));

            //var elements = driver.FindElements(By.CssSelector(".entry-title td-module-title a"));
            var elements = driver.FindElements(By.XPath("h2.c-entry-box--compact__title > a"));

            List<NewsDTO> news = elements
                .Select(el => new NewsDTO
                {
                    Title = el.Text,
                    Url = el.GetAttribute("href")
                })
                .Take(5).ToList();
            Console.WriteLine("News count {0}", news.Count);


            for (int i = 0; i < news.Count; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var n = news[i];
                try
                {
                    driver.Navigate().GoToUrl(n.Url);
                    wait.Until(d => d.FindElement(By.CssSelector(".c-page-title")));

                    n.Author = driver.FindElement(By.CssSelector("meta[property^=author]")).GetAttribute("content");
                    n.Description = driver.FindElement(By.CssSelector("meta[name^=description]")).GetAttribute("content");
                    n.DateOfPublication = DateTime.Parse(driver.FindElement(By.CssSelector("meta[property^=\"article:published_time\"]")).GetAttribute("content"));

                    var commentsBlock = driver.FindElement(By.CssSelector(".c-comments"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", commentsBlock);

                    wait.Until(d => d.FindElement(By.CssSelector(".c-comments__count")));

                    var comments = driver.FindElements(By.ClassName("c-comments__comment"));
                    foreach (var comment in comments)
                    {
                        n.Comments.Add(new CommentDTO
                        {
                            Author = comment.FindElement(By.CssSelector(".c-comments__header-author .c-comments__author")).Text,
                            Text = comment.FindElement(By.CssSelector(".c-comments__cbody")).Text,
                            DateOfPost = DateTime.ParseExact(comment.FindElement(By.CssSelector(".c-comments__date a")).Text, "MMM d, yyyy | h:mm tt", CultureInfo.InvariantCulture)
                        });
                    }

                }
                catch (Exception ex)
                {
                    //log exception
                }
                yield return n;
            }

            driver.Close();
        }
    }


    public class Nature_Crawler : ICrawl
    {
        public IEnumerable<NewsDTO> Crawl()
        {
            string homeUrl = "https://www.nature.com/news";

            ChromeOptions chromeOptions = new ChromeOptions();
            IWebDriver driver = new ChromeDriver(chromeOptions);


            driver.Navigate().GoToUrl(homeUrl);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            //wait.Until(d => d.FindElement(By.CssSelector(".p-page-title ")));

            //var elements = driver.FindElements(By.CssSelector(".entry-title td-module-title a"));
            var elements = driver.FindElements(By.XPath("//*[@class='c-card__title u-serif u-text17 u-font-weight--regular']"));

            List<NewsDTO> news = elements
                .Select(el => new NewsDTO
                {
                    Title = el.Text,
                    Url = el.GetAttribute("href")
                })
                .Take(5).ToList();
            Console.WriteLine("News count {0}", news.Count);


            for (int i = 0; i < news.Count; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var n = news[i];
                //try
                //{
                //    driver.Navigate().GoToUrl(n.Url);
                //    wait.Until(d => d.FindElement(By.CssSelector(".c-page-title")));

                //    n.Author = driver.FindElement(By.CssSelector("meta[property^=author]")).GetAttribute("content");
                //    n.Description = driver.FindElement(By.CssSelector("meta[name^=description]")).GetAttribute("content");
                //    n.DateOfPublication = DateTime.Parse(driver.FindElement(By.CssSelector("meta[property^=\"article:published_time\"]")).GetAttribute("content"));

                //    var commentsBlock = driver.FindElement(By.CssSelector(".c-comments"));
                //    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", commentsBlock);

                //    wait.Until(d => d.FindElement(By.CssSelector(".c-comments__count")));

                //    var comments = driver.FindElements(By.ClassName("c-comments__comment"));
                //    foreach (var comment in comments)
                //    {
                //        n.Comments.Add(new CommentDTO
                //        {
                //            Author = comment.FindElement(By.CssSelector(".c-comments__header-author .c-comments__author")).Text,
                //            Text = comment.FindElement(By.CssSelector(".c-comments__cbody")).Text,
                //            DateOfPost = DateTime.ParseExact(comment.FindElement(By.CssSelector(".c-comments__date a")).Text, "MMM d, yyyy | h:mm tt", CultureInfo.InvariantCulture)
                //        });
                //    }

                //}
                //catch (Exception ex)
                //{
                //    //log exception
                //}
                yield return n;
            }

            driver.Close();
        }
    }

}
