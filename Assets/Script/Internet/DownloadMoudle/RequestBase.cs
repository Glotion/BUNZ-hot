using System;
using UnityEngine.Networking;

namespace Assets.Scripts.Internet.DownloadMoudle
{
    public class RequestBase : IDisposable
    {
        UnityWebRequest webRequest;//封装的webrequest
        string url;//网络路径
        string fileName;//文件名

        public UnityWebRequest WebRequest { get => webRequest; set => webRequest = value; }
        public string Url { get => url; set => url = value; }
        public string FileName { get => fileName; set => fileName = value; }

        //发送Get请求
        public RequestBase(string _url, string _fileName)
        {
            Url = _url;
            WebRequest = UnityWebRequest.Get(Url);
            FileName = _fileName;
        }
        //发送头请求
        public RequestBase(string _url, string _fileName, bool isHead)
        {
            Url = _url;
            WebRequest = UnityWebRequest.Head(Url);
            FileName = _fileName;
        }
        //GC的时候回收Webrequest
        public void Dispose()
        {
            WebRequest.Dispose();
        }
    }
}
