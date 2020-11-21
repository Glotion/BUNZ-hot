using Assets.Scripts.Internet;
using UnityEditor;
using UnityEngine;
/// <summary>
///     author:WenZeming
///     启动函数
/// </summary>
namespace Assets.Scripts.Base
{
    [InitializeOnLoad]
    class Startup
    {
        //进入启动事件
        static Startup()
        {
            if (!CheckNetworkActiveService.CheckNetworkActive())
            {
                Debug.Log("游戏无网络，请检查连接或退出");
            }
            Debug.Log("Up and running");
            DownloadManager manager = new DownloadManager();
        }

    }
}
