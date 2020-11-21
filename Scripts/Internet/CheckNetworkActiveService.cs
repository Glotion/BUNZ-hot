using UnityEngine;
/// <summary>
///     检测网路状态
/// </summary>
namespace Assets.Scripts.Internet
{
    //检测网络状态文件
    static class CheckNetworkActiveService
    {
        public static bool CheckNetworkActive()
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
    }
}
