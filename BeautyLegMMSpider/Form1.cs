using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeautyLegMMSpider.Cus;
using System.Linq;
using System.IO;
using System.Net;

namespace BeautyLegMMSpider
{
    public partial class Form1 : Form
    {
        private readonly string _homePageUrl = "http://www.beautylegmm.com/";
        private readonly FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnGet_ClickAsync(object sender, EventArgs e)
        {
            var tuJis=await BeautylehMMHeler.GetTuJiCollectionAsync(_homePageUrl);
            var controls = gbTuJis.Controls;
            controls.Clear();
            var index = 0;
            foreach (var tuJi in tuJis)
            {
                controls.Add(new ResourcesItem(tuJi.Item1,tuJi, index));
                index++;
            } 
        }

        private async void btnDownload_ClickAsync(object sender, EventArgs e)
        {
            var dialogResult = _folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            var baseDir = _folderBrowserDialog.SelectedPath;
            foreach (ResourcesItem c in gbTuJis.Controls)
            {
                if (!c.IsCheck())
                {
                    continue;
                }
                var tag = c.Tag as Tuple<string, string>;
                await HandlerTuJi(tag.Item1, tag.Item2, baseDir);
            }
            MessageBox.Show("下载完毕");
        }

        private async Task HandlerTuJi(string title, string url,string baseDir)
        {
            var dir = Path.Combine(baseDir,title);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var imgs= await BeautylehMMHeler.GetImgs(url, _homePageUrl,title);
            var lsTask = new List<Task>(imgs.Count);
            foreach (var img in imgs)
            {
                await BeautylehMMHeler.DownloadImgAsync(Path.Combine(dir, Path.GetFileName(img)),img);
            }
        }
    }

    public static class BeautylehMMHeler
    {
        private static HttpClient _httpClient = new HttpClient();
        private static Regex _getTuJiRegex = new Regex("<div align=\"center\" class=\"post_weidaopic_title\"><a href=\"(.*?.html)\" target=\"_blank\">(.*?)</a></div>", RegexOptions.Compiled); 

        public static async Task<IEnumerable<Tuple<string, string>>> GetTuJiCollectionAsync(string homePageUrl)
        {
            var raw = await _httpClient.GetStringAsync(homePageUrl);
            var ms=_getTuJiRegex.Matches(raw);
            List<Tuple<string,string>> ls = new List<Tuple<string, string>>();
            foreach (Match m in ms)
            {
               ls.Add(new Tuple<string, string>(m.Groups[2].Value, m.Groups[1].Value));
            }
            return ls;
        }
        public static void DownloadImg(string fileName, string url)
        {
            var buf = new byte[1024 * 512];
            int bufLen = buf.Length;
            var req = WebRequest.CreateHttp(url);
            using (var resp = req.GetResponse())
            using (var stream = resp.GetResponseStream())
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                int readLen = 0;
                do
                {
                    readLen = stream.Read(buf, 0, bufLen);
                    if (readLen != 0)
                    {
                        fs.Write(buf, 0, readLen);
                    }
                } while (readLen != 0);
            }
        }
        public static async Task DownloadImgAsync(string fileName,string url)
        {
            var buf = new byte[1024*512];
            int bufLen = buf.Length;
            using (var stream = await _httpClient.GetStreamAsync(url))
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                int readLen = 0;
                do
                {
                    readLen = await stream.ReadAsync(buf, 0, bufLen);
                    if(readLen!=0)
                    {
                        fs.Write(buf, 0, readLen);
                    }
                } while (readLen!=0);
            }
        }

        public static async Task<List<string>> GetImgs(string url,string baseUrl,string tujiTitle)
        {
            var raw = await _httpClient.GetStringAsync(url);
            int totalImgs = GetTotalNumber(tujiTitle);
            Regex regex = new Regex(string.Format("<a href=\"(.*?[.]jpg)\" target=\"_blank\" title=\".*?第1张图\">",tujiTitle));
            var m = regex.Match(raw).Groups [1];
            Regex regexImg = new Regex("(?<=-00)([0-9]{2})(?=[.]jpg)");
            var template=regexImg.Replace(m.Value,"{0}");
            var ls = new List<string>(totalImgs);
            for (int i = 0; i < totalImgs; i++)
            {
                ls.Add(string.Format(baseUrl.TrimEnd('/')+template, i.ToString("00")));
            }
            return ls;
        }

        private static int GetTotalNumber(string tujiTitle)
        { 
            Regex regex = new Regex("\\[([0-9]{1,2})P\\]");
            var m =regex.Match(tujiTitle);
            return int.Parse( m.Groups[1].Value);
        }
    }
}
