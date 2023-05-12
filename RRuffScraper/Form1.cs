using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
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
        string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Download");
        string errorsFilePath = null;
        public Form1()
        {
            InitializeComponent();
            FormClosing += Form1_FormClosing;
            Directory.CreateDirectory(downloadPath);
            errorsFilePath = Path.Combine(downloadPath, "errors.txt");
            File.Create(errorsFilePath);
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            driver.Quit();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                ChromeOptions options = new ChromeOptions();
                ChromeDriver driver = new ChromeDriver(options);
                this.driver = driver;
                driver.Navigate().GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                WebDriverWait wait10s = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                Invoke(new Action(() =>
                {
                    try
                    {
                        lblTotalDocument.Text = wait10s.Until(driver => js.ExecuteScript("return document.getElementsByTagName(\"tr\")[document.getElementsByTagName(\"tr\").length-8].children[2].innerText.substring(0,4)")).ToString();
                        downloadCount = Directory.GetFiles(downloadPath).Count()-1;
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

                List<string> errorList = File.ReadAllLines(errorsFilePath).Where(line => !string.IsNullOrEmpty(line)).ToList();

                int _downloadCounter = downloadCount;

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                IWebElement allRecordsTable = driver.FindElement(By.XPath("//body/form[@id='frm_search_results']/table[1]"));
                ReadOnlyCollection<IWebElement> allRecordsItems = allRecordsTable.FindElements(By.TagName("tr"));

                int startInt = downloadCount <= 3 ? 3 : downloadCount + 3;

                for (int i = startInt; i < allRecordsItems.Count - 3; i++)
                {
                    if (i != downloadCount + 3)
                    {
                        driver.Navigate().GoToUrl(this.url);
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
                    }
                    catch (Exception ex)
                    {
                        string _header = driver.FindElement(By.XPath("//body//h1")).Text.Trim().ToLower();
                        if (!errorList.Contains(_header))
                        {
                            using (StreamWriter writer = new StreamWriter(errorsFilePath, true))
                            {
                                writer.WriteLine(errorList);
                            }
                        }
                        continue;
                    }

                    Thread.Sleep(500);
                    string url = wait.Until(driver => js.ExecuteScript("return document.getElementsByClassName(\"info_box\")[0].getElementsByTagName(\"tbody\")[0].getElementsByTagName(\"a\")[0].getAttribute(\"href\");")).ToString();
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