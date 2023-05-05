using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;

namespace OnurScraping
{
    public partial class Form1 : Form
    {
        IWebDriver driver;
        string url = "https://rruff.info/all/display=default/";
        int downloadCount = 0;
        string downloadPath = @"..\..\..\bin\Download";

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                string driverPath = @"..\..\..\bin\Debug";
                ChromeOptions options = new ChromeOptions();
                ChromeDriver driver = new ChromeDriver(driverPath, options);
                this.driver = driver;
                driver.Navigate().GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                IWebElement totalRecord = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//body[1]/form[1]/table[1]/tbody[1]/tr[4146]/th[1]/table[1]/tbody[1]/tr[1]/td[3]")));

                Invoke(new Action(() =>
                {
                    try
                    {
                        lblTotalDocument.Text = totalRecord.Text.Substring(0, 4);
                        downloadCount = Directory.GetFiles(downloadPath).Count();
                        lblInforming.Text = $"{downloadCount} document has been downloaded before.";
                        btnDownload.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Code: 01\n\nError Message: " + ex.Message);
                    }
                }));
            });
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                Invoke(new Action(() =>
                {
                    lblInforming.Visible = false;
                    btnDownload.Enabled = false;
                }));
                
                int _downloadCounter = downloadCount;

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                IWebElement allRecordsTable = driver.FindElement(By.XPath("//body/form[@id='frm_search_results']/table[1]"));
                ReadOnlyCollection<IWebElement> allRecordsItems = allRecordsTable.FindElements(By.TagName("tr"));

                int startInt = downloadCount <= 3 ? 3 : downloadCount+3;

                for (int i = startInt; i < allRecordsItems.Count - 3; i++)
                {
                    if (i != downloadCount+3)
                    {
                        driver.Navigate().GoToUrl(this.url);
                        //wait.Until(driver => js.ExecuteScript("return ready.state").Equals("complete"));
                        allRecordsTable = driver.FindElement(By.XPath("//body/form[@id='frm_search_results']/table[1]"));
                        allRecordsItems = allRecordsTable.FindElements(By.TagName("tr"));
                    }

                    try
                    {
                        var rowLink = allRecordsItems[i].FindElement(By.TagName("a")).GetAttribute("href");
                        driver.Navigate().GoToUrl(rowLink);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Code: 10\n\nError Message: " + ex.Message);
                    }

                    try
                    {
                        wait.Until(driver => js.ExecuteScript("return document.getElementsByTagName('select')[1].selectedIndex = 0;"));
                        js.ExecuteScript("return document.getElementsByTagName(\"select\")[1].dispatchEvent(new Event('change'));");


                        //var dropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//body[1]/table[3]/tbody[1]/tr[2]/td[1]/div[1]/table[1]/tbody[1]/tr[3]/td[2]/table[1]/tbody[1]/tr[1]/td[1]/form[1]/select[1]")));
                        //dropdown.Click();

                        //var selectDropdown = dropdown.FindElement(By.XPath("//option[contains(text(),'unoriented (532 nm)')]"));
                        //selectDropdown.Click();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Error Code: 11\n\nError Message: " + ex.Message);
                        string _header = driver.FindElement(By.XPath("//body//h1")).Text.Trim();
                        string errorContents;
                        using (StreamReader reader = new StreamReader(downloadPath + "\\errors.txt"))
                        {
                            errorContents = reader.ReadToEnd();
                        }
                        if (!errorContents.Contains(_header))
                        {
                            using (StreamWriter writer = new StreamWriter(downloadPath + "\\errors.txt", true))
                            {
                                writer.WriteLine(errorContents);
                            }
                        }
                        continue;
                    }


                    //wait.Until(driver => js.ExecuteScript("return document.readystate").Equals("complete"));
                    //Actions action = new Actions(driver.SwitchTo().Window(driver.WindowHandles.Last()));
                    //action.SendKeys(System.Windows.Forms.Keys.Control + "s").Perform();
                    //action.SendKeys(System.Windows.Forms.Keys.Enter.ToString()).Perform();

                    //var secondPage = driver.SwitchTo().Window(driver.WindowHandles.Last());

                    Thread.Sleep(500);
                    string url = wait.Until(driver => js.ExecuteScript("return document.getElementsByClassName(\"info_box\")[0].getElementsByTagName(\"tbody\")[0].getElementsByTagName(\"a\")[0].getAttribute(\"href\");")).ToString();
                    //var url = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(),'Raman Data (Processed)')]"))).GetAttribute("href");
                    string header = driver.FindElement(By.XPath("//body//h1")).Text.Trim();
                    string filePath = downloadPath + "\\" + header + ".txt";

                    if (!IsExistFile(filePath))
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(url, filePath);
                        }

                        _downloadCounter++;
                        Invoke(new Action(() => { lblDownloadCount.Text = _downloadCounter.ToString(); }));
                    }
                }
            });
            btnDownload.Enabled = true;
            Process.Start(downloadPath);
        }

        /// <summary>
        /// Find specified file. If it is in the directory returns true, else false.
        /// </summary>
        /// <param name="path">Full path(also file extension)</param>
        /// <returns></returns>
        private bool IsExistFile(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            else { return false; }
        }
    }
}