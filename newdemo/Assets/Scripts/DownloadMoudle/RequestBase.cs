using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DownloadMoudle
{
    public class RequestBase: IDisposable
    {
        UnityWebRequest webRequest;
        string url;
        string fileName;

        public UnityWebRequest WebRequest { get => webRequest; set => webRequest = value; }
        public string Url { get => url; set => url = value; }
        public string FileName { get => fileName; set => fileName = value; }

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

        public void Dispose()
        {
            WebRequest.Dispose();
        }
    }
}
