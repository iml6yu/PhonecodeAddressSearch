using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace 号码归属地查询
{
    class Program
    {
        static void Main(string[] args)
        {


            List<string> ps = new List<string>() {
   "17134833483",
"17168033456",
"17134822345",
"16724780123",
"17178044440",
"17045711110",
"17113722221",
"17045899992",
"17178033337",
"17178033335",
"17178033338",
"17134799995",
"17113722226",
"17134798765",
"17045711116",
"17178044448",
"17071844441"};


            Console.WriteLine("Hello World!");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("GB2312");

            HttpClient c = new HttpClient();
            var total = 0;
            foreach (var item in ps)
            {
                var request = c.GetAsync("https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?resource_name=guishudi&query=" + item).Result;
                //request.Headers.Add("Content-Type", "application/x-javascript;charset=gbk");
                var result = request.Content.ReadAsStringAsync().Result;
                //result = result.Replace("void(\"", "").Replace("\")", "");// "void("黑龙江哈尔滨 中国电信");"
                var r = JsonConvert.DeserializeObject<R>(result);

                if (r.status == "0")
                {
                    total++;
                    WritFile($"'{item},{(string.IsNullOrEmpty(r.data.FirstOrDefault()?.prov) ? r.data.FirstOrDefault()?.city : r.data.FirstOrDefault()?.prov)},{r.data.FirstOrDefault()?.city}");
                }
                else
                {
                    Console.WriteLine($"\r\n--------------号码{item}没查询到---------------\r\n");
                }
            }


            Console.WriteLine($"一共{ps.Count},成功{total}");
            Console.ReadLine();
        }
        public static void WritFile(string s)
        {
            using (StreamWriter outputFile = new StreamWriter(Environment.CurrentDirectory + "\\phone.txt", true))
            {
                outputFile.WriteLine(s);
            }
        }

        /// <summary>
        /// 使用get方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> GetAsync(string url, Dictionary<string, string> header = null, bool Gzip = false)
        {

            HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();//用来抛异常的 
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

    }

    public class R
    {
        public string status { get; set; }
        public List<Data> data { get; set; }
    }

    public class Data
    {
        public string city { get; set; }
        public string prov { get; set; }
        public string type { get; set; }
        public string phoneno { get; set; }
    }
}