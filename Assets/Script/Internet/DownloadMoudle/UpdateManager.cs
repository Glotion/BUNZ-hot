using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Internet.DownloadMoudle
{
    public class UpdateManager : MonoBehaviour
    {
        // 判断版本号
        public string url = "http://localhost:5000";
        void Start()
        {
            url += "/Version.ini";
            //直接开始协程
            StartCoroutine(Upgrade(url));
        }
        IEnumerator Upgrade(string uri)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(uri: uri);

            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                string version = webRequest.downloadHandler.text;
                Debug.Log(pages[page] + ":\nReceived: " + version);
                //存储版本数据
                PlayerPrefs.SetString("Version", version);
                string url = $"http://localhost/data/{version}";
                webRequest = UnityWebRequest.Get(url);
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    //获取更新的文件列表
                    Dictionary<string, string> TaskList = new Dictionary<string, string>();
                    string[] TaskSting = version.Trim().Split('\n');
                    foreach (var t in TaskSting)
                    {
                        TaskList.Add(t.Split('\t')[1], t.Split('\t')[0]);
                    }
                    SendMessage("StartDownload", TaskList);
                }
            }
            webRequest.Dispose();
        }
        void Update()
        {

        }
    }
}
