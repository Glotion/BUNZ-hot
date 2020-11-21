using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
///     author:WenZeming
///     下载管理器
/// </summary>
namespace Assets.Scripts.Internet
{
    //下载管理器单例
    public class DownloadManager
    {
        //直接进入函数
        public DownloadManager()
        {
            Download();
        }
        //并行下载逻辑
        public void Download()
        {
            int retry = 0;
            int fail = 0;
            while (true)
            {
                ConcurrentDictionary<string, int> dictionary = new ConcurrentDictionary<string, int>();//创建一个线程安全的字典

                //数据源
                for (int i = 4; i <= 11; i++)
                {
                    string url = $"http://10.16.95.194/456({i}).txt";
                    dictionary.TryAdd(url, 0);
                }
                //并行循环直接取列表全部下载
                Parallel.ForEach(dictionary, async (keyValuePair) =>
                {
                    try
                    {
                        if (keyValuePair.Value == 0)
                        {
                            int v = await new DownloadProgress(keyValuePair.Key).Request();
                            dictionary[keyValuePair.Key] = v;
                            Debug.Log(dictionary[keyValuePair.Key]);
                        }
                    }
                    catch
                    {
                        dictionary[keyValuePair.Key] = 1;
                        Debug.Log(dictionary[keyValuePair.Key]);
                    }
                });
                //标记
                foreach (var d in dictionary)
                {
                    if (d.Value == 1)
                    {
                        int v;
                        dictionary.TryRemove(d.Key, out v);
                        fail += v;
                    }
                }
                //报告
                Debug.Log($"本次下载失败结果：{fail}次，尝试次数{retry}");
                retry++;
                if(fail == 0)
                {
                    Debug.Log("下载成功");
                    return;
                }
                if (retry > 4)
                {
                    Debug.Log("下载太多次了，退出，我要退出!!!!");
                    return;
                }
            }
        }
        //下载进度
    }
}

