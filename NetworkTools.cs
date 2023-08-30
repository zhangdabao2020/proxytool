
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace myNetwork
{
    internal class NetworkTools
    {
        public NetworkTools()
        {

        }


        public void SetProxy(bool bOpen, string proxyServerAddress,string proxyServerPort)
        {
            //打开注册表
            var regKey = Registry.CurrentUser;
            const string subKeyPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
            var optionKey = regKey.OpenSubKey(subKeyPath, true);
            //更改健值，设置代理，
            if (optionKey != null)
            {
                optionKey.SetValue("ProxyEnable", bOpen ? 1 : 0);

                optionKey.SetValue("ProxyServer", (proxyServerAddress + ":" + proxyServerPort));
            }
            //激活代理设置
            InternetSetOption(0, 39, IntPtr.Zero, 0);
            InternetSetOption(0, 37, IntPtr.Zero, 0);
        }
        // DllImport：动态端口
        [DllImport(@"wininet",
        SetLastError = true,
        CharSet = CharSet.Auto,
        EntryPoint = "InternetSetOption",
        CallingConvention = CallingConvention.StdCall)]
        // InternetSetOption：互联网设置选项
        public static extern bool InternetSetOption
        (
            int hInternet,
            int dmOption,
            IntPtr lpBuffer,
            int dwBufferLength
         );
        // 获取代理
        public bool GetProxy()
        {
            //打开注册表CurrentUser
            var regKey = Registry.CurrentUser;

            const string subKeyPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
            var optionKey = regKey.OpenSubKey(subKeyPath, true);
            //更改健值，设置代理
            if (optionKey != null)
            {
                // 获取是否使用代理服务器
                string proxyEnable = optionKey.GetValue("ProxyEnable").ToString();
                // 获取地址跟端口
                string addressAndPort = optionKey.GetValue("ProxyServer").ToString();
                return proxyEnable == "1" ? true : false;
            }
            else
            {
                return false;
            }

        }

        //启动exe程序
        public ProcessStartInfo startmyexe(string filepath)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = filepath;
            info.Arguments = "";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardError = true;
            info.CreateNoWindow = true;
            return info;
            
            //pro.WaitForExit();  
        }
        public void closemyexe(string name)
        {
            Process[] p_arry = Process.GetProcesses();//得到系统所有进程
            for (int i = 0; i < p_arry.Length; i++)//遍历每个进程
            {
                if (p_arry[i].ProcessName == name)//
                {
                    p_arry[i].Kill();//就结束它。
                    return;
                }
            }
            System.GC.Collect();//垃圾回收
        }


    }
}
