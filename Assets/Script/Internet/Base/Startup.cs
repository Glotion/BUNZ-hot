using UnityEditor;
using UnityEngine;
/// <summary>
///     author:WenZeming
///     启动函数
/// </summary>
namespace Assets.Scripts.Internet.Base
{
    [InitializeOnLoad]
    class Startup
    {
        static bool CheckNetworkActive()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {//没有网络

                return false;
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {//234G网络

                return true;
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {//wifi网络
                return true;
            }
            return false;
        }
        //进入启动事件
        static Startup()
        {
            if (!CheckNetworkActive())
            {
                Application.Quit();

                //这个代码仅仅是测试开发用的，并非是打包后的退出，这个需要注意

                UnityEditor.EditorApplication.isPlaying = false;
                Debug.Log("游戏无网络，请检查连接或退出");
            }
            Debug.Log("游戏连接到网络");
        }
    }
}
