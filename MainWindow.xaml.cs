using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Diagnostics;

namespace myNetwork
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty OpenCommandProperty =
         DependencyProperty.Register("OpenCommand", typeof(RoutedCommand), typeof(MainWindow), new PropertyMetadata(null));
        public static string proxyServerAddress = "127.0.0.1";
        public static string proxyServerPort = "10809";
        private Process proIpv4 = null;
        private Process proIpv6 = null;
        private string netmodle = "ipv4";
        public RoutedCommand OpenCommand
        {
            get { return (RoutedCommand)GetValue(OpenCommandProperty); }
            set { SetValue(OpenCommandProperty, value); }
        }
         //static NetworkTools mynetworkTools = new NetworkTools();

        
        public MainWindow()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// 选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BulletCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //打开代理
            NetworkTools myNetWorkTools = new NetworkTools();

            myNetWorkTools.SetProxy(true, proxyServerAddress, proxyServerPort);
            if (netmodle == "ipv4")
            {
                if (proIpv4 == null)
                {
                    ProcessStartInfo info = new ProcessStartInfo();
                    string filenme = Directory.GetCurrentDirectory() + @"\IPv4\xrayIpv4.exe";
                    info = myNetWorkTools.startmyexe(filenme);
                    proIpv4 = Process.Start(info);
                }
            }
            else
            {
                if (proIpv6 == null)
                {
                    ProcessStartInfo info = new ProcessStartInfo();
                    string filenme = Directory.GetCurrentDirectory() + @"\IPv6\xrayIpv6.exe";
                    info = myNetWorkTools.startmyexe(filenme);
                    proIpv6 = Process.Start(info);
                }
            }
        }
        /// <summary>
        /// 取消选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BulletCheckBox_unChecked(object sender, RoutedEventArgs e)
        {
            //关闭代理
            NetworkTools myNetWorkTools = new NetworkTools();

            myNetWorkTools.SetProxy(false, proxyServerAddress, proxyServerPort);
            if (proIpv4 != null)
            {
                proIpv4.Kill();
                proIpv4.WaitForExit();
                proIpv4 = null;
            }

            if(proIpv6 != null)
            {
                proIpv6.Kill();
                proIpv6.WaitForExit();
                proIpv6 = null;
            }

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn == null)
                return;
            NetworkTools myNetWorkTools = new NetworkTools();
            string filepath = "";
            bool proxyFlag = myNetWorkTools.GetProxy();
            if (!proxyFlag){
                return;
            }
            myNetWorkTools.SetProxy(true, proxyServerAddress, proxyServerPort);
            string rootpath = Directory.GetCurrentDirectory();
            string cloasepath = "";
            if (btn.Name == "IPv4")
            {
                this.netmodle = "ipv4";
                if (proIpv6 != null)
                {
                    proIpv6.Kill();
                    proIpv6.WaitForExit();
                    proIpv6 = null;
                }
                if (proIpv4 == null)
                {
                    ProcessStartInfo info = new ProcessStartInfo();
                    string filenme = Directory.GetCurrentDirectory() + @"\IPv4\xrayIpv4.exe";
                    info = myNetWorkTools.startmyexe(filenme);
                    proIpv4 = Process.Start(info);
                }

            }
            if (btn.Name == "IPv6")
            {
                this.netmodle = "ipv6";
                
                if (proIpv4 != null)
                {
                    proIpv4.Kill();
                    proIpv4.WaitForExit();
                    proIpv4 = null;
                }
                if (proIpv6 == null)
                {
                    string filenme = Directory.GetCurrentDirectory() + @"\IPv6\xrayIpv6.exe";
                    ProcessStartInfo info6 = myNetWorkTools.startmyexe(filenme);
                    //info = myNetWorkTools.startmyexe(filenme);
                    proIpv6 = Process.Start(info6);
                }
            }

            /*  Task task = Task.Run(() =>
              {
                  myNetWorkTools.closemyexe(cloasepath);
                  myNetWorkTools.startmyexe(filepath);
              });*/

        }
        /// <summary>
        /// 窗口移动事件
        /// </summary>
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        /// <summary>
        /// 窗口关闭
        /// </summary>
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            //关闭代理
            NetworkTools myNetWorkTools = new NetworkTools();
            myNetWorkTools.SetProxy(false, proxyServerAddress, proxyServerPort);
            if (proIpv4 != null)
            {
                proIpv4.Kill();
                proIpv4.WaitForExit();
                proIpv4 = null;
            }

            if (proIpv6 != null)
            {
                proIpv6.Kill();
                proIpv6.WaitForExit();
                proIpv6 = null;
            }
            this.Close();
        }

        /// <summary>
        /// 窗口最小化
        /// </summary>
        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //设置窗口最小化
        }
    }
}
