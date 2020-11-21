using Assets.Scripts.Internet;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
/// <summary>
///     author：WenZeming
///     下载关键流程：
///         返回：0未下载；
///         返回：1下载失败;
///         返回：2下载成功写入失败或者校验失败;
///         返回：3下载成功
/// </summary>
namespace Assets.Scripts.Internet
{
    public class DownloadProgress
    {
        //临时下载目录
        private string tempFilePath;
        //获取地址
        private string uri;
        //保存文件名
        private string fileName;
        //构造函数
        public DownloadProgress(string url)
        {
            this.uri = url;
            char[] delimiterChars = { '/', ':' };
            fileName = url.Split(delimiterChars)[4];
            this.tempFilePath = $"C:\\LenovoBox\\temp\\{fileName}";
        }
        ////临时文件块大小,range
        //private int readLen;
        //服务端校验MD5
        private string getTotalMD5()
        {
            return "";
        }
        //客户端校验MD5
        public bool CheckMD5(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            using (FileStream file = File.Open(fileName, FileMode.Open))
            {
                using (System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    byte[] retVal = md5.ComputeHash(file);
                    foreach (var i in retVal)
                    {
                        sb.Append(i.ToString("x2"));
                    }
                }
                //文件校验逻辑，对比文件名
                if (sb.ToString().CompareTo(fileName) != 0) { return false; }
                else { return true; }
            }
        }
        //获取文件长度
        public long GetLength()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "HEAD";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.ContentLength;
        }
        //获取文件名
        public string GetName()
        {
            return "";
        }
        //下载流程
        public async Task<int> Request()
        {
            long totalLength = GetLength();
            HttpWebRequest myReq;
            //如果临时文件不存在
            if (!File.Exists(tempFilePath))
            {
                File.Create(tempFilePath).Close();
                FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.ReadWrite);
                myReq = (HttpWebRequest)WebRequest.Create(uri);
                myReq.AddRange(0);//请求范围，从数字到文件长度(0~ContengLength)
                myReq.Timeout = 5 * 1000;//请求超时等待时间(ms)
                myReq.ReadWriteTimeout = 10 * 1000; //获取写入等待时间(ms)
                //接收响应
                using (HttpWebResponse response = (HttpWebResponse)myReq.GetResponse())
                {
                    Stream inputStream = response.GetResponseStream();
                    await inputStream.CopyToAsync(fileStream);//异步写入
                    await fileStream.FlushAsync();//异步缓冲
                }
                fileStream.Close();
            }
            //如果临时文件存在
            else
            {
                FileInfo fileInfo = new FileInfo(tempFilePath);
                long nowLength = fileInfo.Length;
                if (totalLength > nowLength)
                {
                    myReq = (HttpWebRequest)WebRequest.Create(uri);
                    myReq.AddRange(nowLength);
                    myReq.Timeout = 5 * 1000;//请求超时等待时间(ms)
                    myReq.ReadWriteTimeout = 10 * 1000; //获取写入等待时间(ms)
                    FileStream fileStream = new FileStream(tempFilePath, FileMode.Append, FileAccess.Write);
                    fileStream.Seek(nowLength, SeekOrigin.Begin);
                    using (HttpWebResponse response = (HttpWebResponse)myReq.GetResponse())
                    {
                        Stream inputStream = response.GetResponseStream();
                        await inputStream.CopyToAsync(fileStream);
                        await fileStream.FlushAsync();
                    }
                    fileStream.Close();
                }
            }
            //下载完后校验MD5
            //if (!CheckMD5(fileName))
            //{
            //    throw new System.ArgumentException("MD5ERROR");
            //}
            return 3;//下载成功
        }
    }
}
