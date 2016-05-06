using HistoryMuseum.Model;
using HistoryMuseum.Supporter.Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace HistoryMuseum.Supporter.View.ChuMo
{
    /// <summary>
    /// CopyPic.xaml 的交互逻辑
    /// </summary>
   
    public partial class CopyPic : UserControl
   {
           Thread copythread;
           string savefilepath = "";
           string Id = "";
           MenuItemInfo _mi;
           public CopyPic(MenuItemInfo mi,string id, string cmi)
          {
              InitializeComponent();
              _mi = mi;
              savefilepath = cmi;
              Id = id;
              if (!Directory.Exists(savefilepath+"\\"+_mi.MenuName+"\\"+Id))
              {
                  Directory.CreateDirectory(savefilepath + "\\" + _mi.MenuName + "\\"+Id);
              }
          }
          
          private void updatetime()
          {
             // this.displaytimebythread.Text = DateTime.Now.ToLocalTime().ToString("yyyy年mm月dd日 hh:mm:ss");      
          }
  
          private void window_closed(object sender, EventArgs e)
          {
              ///关闭所有启动的线程
              
              copythread.Abort();
              Application.Current.Shutdown();
          }
  
          private void button1_click(object sender, RoutedEventArgs e)
          {
               ///设定要复制的文件全路径
              OpenFileDialog openfile = new OpenFileDialog();
              openfile.AddExtension = true;
              openfile.CheckPathExists = true;
              openfile.Filter = "all files|*.*";
              openfile.FilterIndex = 0;
              openfile.Multiselect = false;
              bool? f=openfile.ShowDialog();
              if (f!=null && f.Value)
              {
                  this.srcfile.Text = openfile.FileName;
              }
          }
  
          private void button2_click(object sender, RoutedEventArgs e)
          {
              ///设定目标文件全路径
              SaveFileDialog savefile = new SaveFileDialog();
              savefile.AddExtension = true;
              savefile.Filter = "*.jpg|*.jgp|all files|*.*";
              savefile.FilterIndex = 0;
              
              bool? f= savefile.ShowDialog();
              if (f != null && f.Value)
              {
                  savefilepath = savefile.FileName;
              }
          }
  
          private void button4_click(object sender, RoutedEventArgs e)
          {
            try
            {
                string filePath = this.srcfile.Text.Trim();
                string filename = System.IO.Path.GetFileNameWithoutExtension(filePath);
                string destpath = savefilepath + "\\" + _mi.MenuName + "\\" +Id+"\\"+ Id + ".jpg";
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("源文件不存在");
                    return;
                }

                ///copy file and nodify ui that rate of progress of file copy          
                this.copyflag.Text = "开始复制。。。";

                //设置进度条最大值，这句代码写的有点郁闷
                this.copyprogress.Maximum = (new FileInfo(filePath)).Length;

                //保存复制文件信息，以进行传递
                copyfileinfo c = new copyfileinfo() { sourcepath = filePath, destpath = destpath };
                //线程异步调用复制文件
                copythread = new Thread(new ParameterizedThreadStart(copyfile));
                copythread.Start(c);

                this.copyflag.Text = "复制完成。。。";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
         }
         /// <summary>
         /// 复制文件的委托方法
         /// </summary>
         /// <param name="obj">复制文件的信息</param>
         private void copyfile(object obj)
         {
             copyfileinfo c = obj as copyfileinfo;
             copyfile(c.sourcepath, c.destpath);
         }
         /// <summary>
         /// 复制文件
         /// </summary>
         /// <param name="sourcepath"></param>
         /// <param name="destpath"></param>
         private void copyfile( string sourcepath,string destpath)
         {
             FileInfo f = new FileInfo(sourcepath);
             FileStream fsr = f.OpenRead();
             FileStream fsw = File.Create(destpath);
             long filelength = f.Length;
             byte[] buffer = new byte[1024];
             int n = 0;
             
             while (true)
             {
                 ///设定线程优先级
                 ///异步调用updatecopyprogress方法
                 ///并传递2个long类型参数filelength 与 fsr.position
                 this.displaycopyinfo.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle,
                     new Action<long, long>(updatecopyprogress), filelength, fsr.Position);
                 
                 //读写文件
                 n=fsr.Read(buffer, 0, 1024); 
                 if (n==0)
                 {
                     break;
                 }
                 fsw.Write(buffer, 0, n);
                 fsw.Flush();
                 Thread.Sleep(1);
             }
             fsr.Close();
             fsw.Close(); 
         }
 
         private void updatecopyprogress(long filelength,long currentlength)
         {
             this.displaycopyinfo.Text = string.Format("总大小：{0},已复制:{1}", filelength, currentlength);
             //刷新进度条            
             this.copyprogress.Value = currentlength;
         }
 
         private void window_loaded(object sender, RoutedEventArgs e)
         {
            
         }

         private void btnClose_Click(object sender, RoutedEventArgs e)
         {
             ChildWindowManager.Instance.CloseChildWindow();
         }
          
     }
     public class copyfileinfo
     {
         public string sourcepath { get; set; }
         public string destpath { get; set; }        
     }
}