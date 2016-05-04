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

                            }
                       ));
               return _btnUpLoad; 
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
                           Start();
                           Connect();
                           if (_client.Connected)
                           {
                               NetworkStream ns = _client.GetStream();
                               string downloadCommand = "GetData";
                               Byte[] sendBytes = Encoding.UTF8.GetBytes(downloadCommand);
                               ns.Write(sendBytes, 0, sendBytes.Length);
                               try
                               {
                                   if (_client.Connected)
                                   {
                                       FileStream fs_menu = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Menu\\Menu.xml", FileMode.Create, FileAccess.Write); 
                                       int size =0;
                                       byte[] buffer = new byte[_blockLength];
                                       while((size=_client.GetStream().Read(buffer,0,_blockLength))>0)
                                       {
                                           fs_menu.Write(buffer,0,size);
                                       }
                                       fs_menu.Flush();
                                       fs_menu.Close();            
                                   }
                                   else
                                   {
                                       Logger.Info("连接异常！");
                                   }
                               }
                               catch(Exception ex)
                               {
                                   Logger.Exception(ex);
                               }
                           }
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

                        }
                       ));
               return _btnSave; 
           }
       }
       #endregion

       TcpClient _client;
       string _hostname;
       int _port = 10003;
       int _blockLength = 1024;
    
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
   }
}
