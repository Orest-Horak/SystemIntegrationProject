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


    public class TheVergeCrawler : ICrawl
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



    //public class NYTimesCrawler : ICrawl
    //{
    //    public IEnumerable<NewsDTO> Crawl()
    //    {
    //        string homeUrl = "https://www.nytimes.com/section/technology";

    //        ChromeOptions chromeOptions = new ChromeOptions();

    //        IWebDriver driver = new ChromeDriver(chromeOptions);
    //        driver.Navigate().GoToUrl(homeUrl);
    //        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);


    //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
    //        //wait.Until(d => d.FindElement(By.CssSelector(".p-page-title ")));

    //        //var elements = driver.FindElements(By.CssSelector(".entry-title td-module-title a"));
    //        var elements = driver.FindElements(By.CssSelector("h3.entry-title.td-module-title > a"));

    //        List<NewsDTO> news = elements
    //            .Select(el => new NewsDTO
    //            {
    //                Title = el.Text,
    //                Url = el.GetAttribute("href")
    //            })
    //            .Take(5).ToList();
    //        Console.WriteLine("News count {0}", news.Count);


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









    public interface ICrawl
    {
        IEnumerable<NewsDTO> Crawl();
    }

    public class Navigator
    {
        public static void Navigate(IWebDriver driver, string url, double secondsTimeout)
        {
            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }
    }

    public class ChromeDriverInitializer
    {
        public static IWebDriver getChromeDriver(ChromeOptions chromeOptions)
        {
            return new ChromeDriver(chromeOptions);
        }
    }

    public class NewsAggregationPage
    {
        protected IWebDriver driver;

        private By newsBy;

        public NewsAggregationPage(IWebDriver driver, By newsBy)
        {
            this.driver = driver;
            this.newsBy = newsBy;
        }

        public List<NewsDTO> getNews()
        {
            var pageElements = driver.FindElements(newsBy);
            return pageElements.Select(el => new NewsDTO
            {
                Title = el.Text,
                Url = el.GetAttribute("href")
            })
            .Take(5).ToList();
        }

    }

    public class NewsPage
    {
        protected IWebDriver driver;

        public NewsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string getAuthor(By authorBy, string attributeName)
        {
            if (attributeName == "text")
            {
                return driver.FindElement(authorBy).Text;
            }
            return driver.FindElement(authorBy).GetAttribute(attributeName);
        }
        public string getDescription(By descriptionBy, string attributeName)
        {
            if (attributeName == "text")
            {
                return driver.FindElement(descriptionBy).Text;
            }
            return driver.FindElement(descriptionBy).GetAttribute(attributeName);
        }
        public DateTime getDateOfPublication(By dateOfPublicationBy, string attributeName)
        {
            if (attributeName == "text")
            {
                return DateTime.Parse(driver.FindElement(dateOfPublicationBy).Text);
            }
            return DateTime.Parse(driver.FindElement(dateOfPublicationBy).GetAttribute(attributeName));
        }
    }


    public class ITechuaCrawler : ICrawl
    {
        public IEnumerable<NewsDTO> Crawl()
        {
            string newsAggregationUrl = "https://itechua.com/";

            IWebDriver driver = ChromeDriverInitializer.getChromeDriver(new ChromeOptions());
            Navigator.Navigate(driver, newsAggregationUrl, 20);

            var newsAggregationPage = new NewsAggregationPage(driver, By.CssSelector("h3.entry-title.td-module-title > a"));

            List<NewsDTO> news = newsAggregationPage.getNews();
            Console.WriteLine("News count {0}", news.Count);


            for (int i = 0; i < news.Count; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var n = news[i];

                try
                {
                    Navigator.Navigate(driver, n.Url, 5);
                    var newsPage = new NewsPage(driver);

                    n.Author = newsPage.getAuthor(By.CssSelector("div.td-author-by"), "text");
                    //n.Description = driver.FindElement(By.CssSelector("meta[name^=description]")).GetAttribute("content");
                    n.DateOfPublication = newsPage.getDateOfPublication(
                                                     By.CssSelector("span.td-post-date > time.entry-date.updated.td-module-date"), "text");
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

    public class ViceCrawler : ICrawl
    {
        public IEnumerable<NewsDTO> Crawl()
        {
            string newsAggregationUrl = "https://www.vice.com/en/section/tech";

            IWebDriver driver = ChromeDriverInitializer.getChromeDriver(new ChromeOptions());
            Navigator.Navigate(driver, newsAggregationUrl, 20);

            var newsAggregationPage = new NewsAggregationPage(driver, By.CssSelector("a.vice-card-hed__link"));

            List<NewsDTO> news = newsAggregationPage.getNews();
            Console.WriteLine("News count {0}", news.Count);


            for (int i = 0; i < news.Count; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var n = news[i];

                try
                {
                    Navigator.Navigate(driver, n.Url, 10);
                    var newsPage = new NewsPage(driver);

                    n.Author = newsPage.getAuthor(By.CssSelector("div.contributor__meta > div > a "), "text");
                    //n.Description = driver.FindElement(By.CssSelector("meta[name^=description]")).GetAttribute("content");
                    n.DateOfPublication = newsPage.getDateOfPublication(
                                                     By.CssSelector("div.article__header__datebar__date--original"), "text");
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

}
