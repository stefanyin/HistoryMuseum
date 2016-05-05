using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMVVM;
using HistoryMuseum.Supporter.View;
using SFLib;
using System.IO;
using System.Net;
using System.Net.Sockets;
using HistoryMuseum.Supporter.Utility;
using System.Windows.Threading;
using System.Threading;
using System.Windows;

namespace HistoryMuseum.Supporter.ViewModel
{
   public class SettingWindowVM :NotifyObject
   {
       #region Mode
       private string _serverIP=Data.GetInstance().GetIP();
       public string ServerIP
       {
           get { return _serverIP; }
           set 
           {
               if (_serverIP != value)
               {
                   _serverIP = value;
                   RaisePropertyChanged("ServerIP");
               }
               
           } 
       }

        private SettingWindow _settingWindow;
       #endregion

       #region Command
       private MyCommand _btnUpLoad;
       public MyCommand BtnUpLoad
       {
           get 
           {
               if (_btnUpLoad == null)
                   _btnUpLoad = new MyCommand(new Action<object>
                       (
                            o=>
                            {
                                uploadMenuFile();
                                uploadChildMenuFile();
                            }
                       ));
               return _btnUpLoad; 
           }
       }

       private void uploadMenuFile()
        {
            Start();
            Connect();
            if (_client.Connected)
            {
                NetworkStream ns = _client.GetStream();
                string uploadCommand = "SendMenuData";
                Byte[] sendBytes = Encoding.UTF8.GetBytes(uploadCommand);
                ns.Write(sendBytes, 0, sendBytes.Length);
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("确定上传配置，并更新服务器？", "操作确认") == System.Windows.Forms.DialogResult.OK)
                    {
                        if (_client.Connected)
                        {
                            FileStream fs_menu = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Menu\\Menu.xml", FileMode.Open);
                            int size = 0;
                            byte[] buffer = new byte[_blockLength];
                            while ((size = fs_menu.Read(buffer, 0, _blockLength)) > 0)
                            {
                                ns.Write(buffer, 0, size);
                            }
                            fs_menu.Flush();
                            fs_menu.Close();
                            ns.Close();
                            MessageBox.Show("文件上传成功");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Stop();
                    Logger.Exception(ex.Message);
                    MessageBox.Show("上传失败！");
                }
            }
        }

        private void uploadChildMenuFile()
        {
            Start();
            Connect();
            if (_client.Connected)
            {
                NetworkStream ns = _client.GetStream();
                string uploadCommand = "SendChildMenuData";
                Byte[] sendBytes = Encoding.UTF8.GetBytes(uploadCommand);
                ns.Write(sendBytes, 0, sendBytes.Length);
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("确定上传配置，并更新服务器？", "操作确认") == System.Windows.Forms.DialogResult.OK)
                    {
                        if (_client.Connected)
                        {
                            FileStream fs_childmenu = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Menu\\ChildMenu.xml", FileMode.Open);

                            int size = 0;
                            byte[] buffer = new byte[_blockLength];
                            while ((size = fs_childmenu.Read(buffer, 0, _blockLength)) > 0)
                            {
                                ns.Write(buffer, 0, size);
                            }
                            fs_childmenu.Flush();
                            fs_childmenu.Close();
                            ns.Close();
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    Stop();
                    Logger.Exception(ex.Message);
                    MessageBox.Show("上传失败！");
                }
            }
        }

        private void DownloadMenuFile()
        {
            Start();
            Connect();
            if (_client.Connected)
            {
                NetworkStream ns = _client.GetStream();
                string downloadCommand = "GetMenuData";
                Byte[] sendBytes = Encoding.UTF8.GetBytes(downloadCommand);
                ns.Write(sendBytes, 0, sendBytes.Length);
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("确定下载配置，并覆盖本地配置吗？", "操作确认") == System.Windows.Forms.DialogResult.OK)
                    {
                        if (_client.Connected)
                        {
                            FileStream fs_menu = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Menu\\Menu.xml", FileMode.Create, FileAccess.Write);
                            int size = 0;
                            byte[] buffer = new byte[_blockLength];
                            while ((size = _client.GetStream().Read(buffer, 0, _blockLength)) > 0)
                            {
                                fs_menu.Write(buffer, 0, size);
                            }
                            fs_menu.Flush();
                            fs_menu.Close();
                            MessageBox.Show("下载完毕！");
                        }
                        else
                        {
                            Logger.Info("连接异常！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }
            }
        }

        private void DownloadChildMenuFile()
        {
            Start();
            Connect();
            if (_client.Connected)
            {
                NetworkStream ns = _client.GetStream();
                string downloadCommand = "GetChildMenuData";
                Byte[] sendBytes = Encoding.UTF8.GetBytes(downloadCommand);
                ns.Write(sendBytes, 0, sendBytes.Length);
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("确定下载配置，并覆盖本地配置吗？", "操作确认") == System.Windows.Forms.DialogResult.OK)
                    {
                        if (_client.Connected)
                        {
                            FileStream fs_childMenu = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Menu\\ChildMenu.xml", FileMode.Create, FileAccess.Write);
                            int size = 0;
                            byte[] buffer = new byte[_blockLength];
                            while ((size = _client.GetStream().Read(buffer, 0, _blockLength)) > 0)
                            {
                                fs_childMenu.Write(buffer, 0, size);
                            }
                            fs_childMenu.Flush();
                            fs_childMenu.Close();
                            MessageBox.Show("下载完毕！");
                        }
                        else
                        {
                            Logger.Info("连接异常！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }
            }
        }

       private MyCommand _btnDownLoad;
       public MyCommand BtnDownLoad
       {
           get 
           {
               if (_btnDownLoad == null)
                   _btnDownLoad = new MyCommand(new Action<object>
                       (
                       o=>
                       {
                           DownloadMenuFile();
                           DownloadChildMenuFile();
                       }
                       ));
               return _btnDownLoad; 
           }
       }

       private MyCommand _btnSave;
       public MyCommand BtnSave
       {
           get 
           {
               if (_btnSave == null)
                   _btnSave = new MyCommand(new Action<object>
                       (
                        o =>
                        {
                            Start();
                            Connect();
                            if (_client.Connected)
                            {
                                NetworkStream ns = _client.GetStream();
                                string uploadCommand = "SendChildMenuData";
                                Byte[] sendBytes = Encoding.UTF8.GetBytes(uploadCommand);
                                ns.Write(sendBytes, 0, sendBytes.Length);
                                try
                                {
                                    if (System.Windows.Forms.MessageBox.Show("确定上传配置，并更新服务器？", "操作确认") == System.Windows.Forms.DialogResult.OK)
                                    {
                                        if (_client.Connected)
                                        {
                                            FileStream fs_childmenu = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Menu\\ChildMenu.xml", FileMode.Open);

                                            int size = 0;
                                            byte[] buffer = new byte[_blockLength];
                                            while ((size = fs_childmenu.Read(buffer, 0, _blockLength)) > 0)
                                            {
                                                ns.Write(buffer, 0, size);
                                            }
                                            fs_childmenu.Flush();
                                            fs_childmenu.Close();
                                            ns.Close();
                                            MessageBox.Show("文件2上传成功");
                                        }
                                    }
                                }
                                catch(Exception ex)
                                {
                                    Stop();
                                    Logger.Exception(ex.Message);
                                    MessageBox.Show("上传失败！");
                                }
                            }
                        }
                       ));
               return _btnSave; 
           }
       }

       private MyCommand _windowLoaded;
       public MyCommand WindowLoaded
        {
            get
            {
                if (_windowLoaded == null)
                    _windowLoaded = new MyCommand(new Action<object>
                        (
                            o=>
                            {
                                Console.WriteLine("表格");
                              
                            }
                        ));
                return _windowLoaded;
            }
        }

        #endregion

        TcpClient _client;
       string _hostname;
       int _port = 10003;
        int _blockLength = 1024;
        private static string _menu_url = AppDomain.CurrentDomain.BaseDirectory+"Menu\\Menu.Xml";
        private static string _child_menu_url = AppDomain.CurrentDomain.BaseDirectory + "Menu\\ChildMenu.xml";
        private void Start()
       {
           _client = new TcpClient();
           _client.ReceiveTimeout = 1000 * 10;
       }

       private void Connect()
       {
           try
           {
               _hostname = _serverIP;
               _client.Connect(IPAddress.Parse(_hostname), _port);
               Data.GetInstance().SaveIP(_hostname);
           }
           catch(Exception ex)
           {
               Logger.Exception(ex);
           }
       }

       private void Stop()
       {
           _client.Close();
       }

        public SettingWindowVM(SettingWindow obj)
        {
            this._settingWindow = obj;
            var chidwindow = new ViewModel.ChuMo.List(_menu_url,_child_menu_url);
            _settingWindow.ChildrenWinContent.Children.Clear();
            chidwindow.ShowList(_settingWindow);
        }
   }
}
