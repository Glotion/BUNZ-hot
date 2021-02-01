using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.DownloadMoudle
{
    public class Download : MonoBehaviour
    {
        public string TempFilePath { get; private set; }
        public GameObject progressbarObject;

        void Start()
        {
            TempFilePath = "C:\\Users\\erica\\temp\\" + PlayerPrefs.GetString("Version") + "\\";//这是存放临时文件的路径
        }
        void StartDownload(Dictionary<string, string> TaskList)
        {
            foreach (var k in TaskList.Keys)
            {
                string fPath = TempFilePath + k;
                if (!System.IO.File.Exists(fPath))
                {
                    using (System.IO.File.Create(fPath)) { };
                }
            }

            foreach (KeyValuePair<string, string> kvp in TaskList)
            {
                //StartCoroutine(GetRequestFileLength(kvp));
                StartCoroutine(GetRequest(kvp));
            }

        }

        IEnumerator GetRequest(KeyValuePair<string, string> kvp)
        {
            using (RequestBase requestHead = new RequestBase(kvp.Value, kvp.Key, true))
            {
                yield return requestHead.WebRequest.SendWebRequest();
                long totallength = long.Parse(requestHead.WebRequest.GetResponseHeader("content-length"));
                long length = new System.IO.FileInfo(TempFilePath + kvp.Key).Length;
                if (length >= totallength)
                {
                    yield return null;
                }
                else
                {
                    RequestBase request = new RequestBase(kvp.Value, kvp.Key);
                    request.WebRequest.SetRequestHeader("Range", "bytes=" + length.ToString() + "-");
                    request.WebRequest.downloadHandler = new DownloadHandlerFile(TempFilePath + request.FileName, true);
                    StartCoroutine(ShowDownloadProgress(request));
                    yield return request.WebRequest.SendWebRequest();
                    if (request.WebRequest.isHttpError)
                    {
                        Debug.Log(request.WebRequest.error);
                    }
                    else
                    {
                        Debug.Log("File successfully downloaded and saved to " + TempFilePath + request.FileName);
                    }
                }
            }
        }

        private bool CheckMD5(string fileName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            using (System.IO.FileStream file = System.IO.File.Open(fileName, System.IO.FileMode.Open))
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

        IEnumerator ShowDownloadProgress(RequestBase request)
        {
            while (!request.WebRequest.isDone)
            {
                yield return null;
                if (request.WebRequest.downloadProgress == 1.0f)
                {
                    progressbarObject.GetComponent<Slider>().value += 1;
                }
            }
            request.Dispose();
        }
    }
}