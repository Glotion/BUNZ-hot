using Assets.Scripts.Base;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DownloadMoudle
{
    public class UpdateManager: MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //WhoYouAre w = new WhoYouAre();
            string url = "http://localhost/Version.ini";
            // A correct website page.
            StartCoroutine(Upgrade(url));
        }
        IEnumerator Upgrade(string uri)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                //存储版本数据
                PlayerPrefs.SetString("Version", webRequest.downloadHandler.text);


                string url = $"http://localhost/data/{webRequest.downloadHandler.text}";
                webRequest.Dispose();
                webRequest = UnityWebRequest.Get(url);
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    Dictionary<string, string> TaskList = new Dictionary<string, string>();
                    // A correct website page.
                    string[] TaskSting = webRequest.downloadHandler.text.Trim().Split('\n');
                    foreach(var t in TaskSting)
                    {
                        TaskList.Add(t.Split('\t')[1], t.Split('\t')[0]);
                    }
                    webRequest.Dispose();
                    SendMessage("StartDownload", TaskList);
                }
            }
        }
        void DIE(int controller)
        {
            StopAllCoroutines();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
