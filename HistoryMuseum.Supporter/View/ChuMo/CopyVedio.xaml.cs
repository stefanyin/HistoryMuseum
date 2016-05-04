using HistoryMuseum.Model;
using HistoryMuseum.Supporter.Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
    /// CopyVedio.xaml 的交互逻辑
    /// </summary>
    public partial class CopyVedio  : UserControl
   {
           Thread copythread;
           string savefilepath = "";
           string Id = "";
           MenuItemInfo _mi;
           public CopyVedio(MenuItemInfo mi,string id,ChuMoConInfo cmi )
          {
              InitializeComponent();
              _mi = mi;
              savefilepath = cmi.ChuMoSource;
              Id = id;
              if (!Directory.Exists(savefilepath+"\\"+Id+"\\vedio"))
              {
                  Directory.CreateDirectory(savefilepath + "\\" + Id + "\\vedio");
              }
             // this.displaytimebythread.Text = DateTime.Now.ToLocalTime().ToString("yyyy年mm月dd日 hh:mm:ss"); ;
              //timethread = new Thread(new ThreadStart(dispatcherthread));
          }
          //public void dispatcherthread()
          //{
          //    //可以通过循环条件来控制ui的更新
          //     while (true)
          //    {
          //        ///线程优先级，最长超时时间，方法委托（无参方法）
          //        // displaytimebythread.Dispatcher.BeginInvoke(
          //            DispatcherPriority.Normal, new Action(updatetime));
          //        Thread.Sleep(1000);                
          //    }
          //}
  
          
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
              savefile.Filter = "*.MP4|*.MP4|all files|*.*";
              savefile.FilterIndex = 0;
              
              bool? f= savefile.ShowDialog();
              if (f != null && f.Value)
              {
                  savefilepath = savefile.FileName;
              }
          }
  
          private void button4_click(object sender, RoutedEventArgs e)
          {
              string filename = this.srcfile.Text.Trim();//源文件
              string imgfileExp = filename.Substring(filename.LastIndexOf(".") + 1);

              string fileName = savefilepath + "\\" + _mi.Id.ToString() + "\\vedio\\" + Id + ".MP4"; ;                      //文件名称  
              //string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();      //文件的后缀名(小写)  

              if (imgfileExp.ToLower() == "flv" || imgfileExp.ToLower() == "mp4")
              {
                  //Response.Write("<br/>" + this.CatchImg("uploadfile/" + filename + "." + imgfileExp));//可以使用  
                  CatchImg1(filename,fileName);//可以使用经过整理  
              }  

              //string filename=this.srcfile.Text.Trim();
              string destpath = savefilepath + "\\" + _mi.Id.ToString() + "\\vedio\\" + Id + ".MP4";
              if(!File.Exists(filename))
              {
                  MessageBox.Show("源文件不存在");
                  return;
              }
              
              ///copy file and nodify ui that rate of progress of file copy          
              this.copyflag.Text = "开始复制。。。";
  
              //设置进度条最大值，这句代码写的有点郁闷
              this.copyprogress.Maximum = (new FileInfo(filename)).Length;
  
              //保存复制文件信息，以进行传递
              copyfileinfo c = new copyfileinfo() { sourcepath = filename, destpath = destpath };
              //线程异步调用复制文件
              copythread = new Thread(new ParameterizedThreadStart (copyfile));           
              copythread.Start(c);
              this.copyflag.Text = "复制完成。。。";
             
             
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
          /// <summary>  
        /// 上传视频生成缩略图  
        /// </summary>  
        /// <param name="vFileName"></param>  
        /// <param name="SmallPic"></param>  
        /// <returns></returns>  
        //public string CatchImg(string vFileName)  
        //{  
        //    try  
        //    {  
        //        string ffmpeg = "ffmpeg.exe";  
        //        ffmpeg = Server.MapPath(ffmpeg);  
        //        string aaa = System.Web.HttpContext.Current.Server.MapPath(vFileName);  
        //        if ((!System.IO.File.Exists(ffmpeg)) || (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(vFileName))))  
        //        {  
        //            return "";  
        //        }  
        //        //获得图片相对路径/最后存储到数据库的路径,如:/Web/FlvFile/User1/00001.jpg  
        //        string flv_img = System.IO.Path.ChangeExtension(vFileName, ".jpg");  
        //        //图片绝对路径,如:D:\Video\Web\FlvFile\User1\0001.jpg  
        //        string flv_img_p = Server.MapPath("/uploadfile/1.jpg");  
        //        //截图的尺寸大小,配置在Web.Config中,如:<add key="CatchFlvImgSize" value="140x110" />   
        //        string FlvImgSize = "140x110";  
        //        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);  
        //        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;  
        //        //此处组合成ffmpeg.exe文件需要的参数即可,此处命令在ffmpeg 0.4.9调试通过  
        //        startInfo.Arguments = " -i " + Server.MapPath(vFileName) + " -y -f image2 -t 0.1 -s " + FlvImgSize + " " + flv_img_p;  
        //        try  
        //        {  
        //            System.Diagnostics.Process.Start(startInfo);  
        //        }  
        //        catch  
        //        {  
        //            return "";  
        //        }  
        //        System.Threading.Thread.Sleep(4000);  
        //        /**/  
        //        ///注意:图片截取成功后,数据由内存缓存写到磁盘需要时间较长,大概在3,4秒甚至更长;  
        //        if (System.IO.File.Exists(flv_img_p))  
        //        {  
        //            return flv_img_p.Replace(Server.MapPath("~/"), ""); ;  
        //        }  
        //        return "";  
        //    }  
        //    catch  
        //    {  
        //        return "";  
        //    }  
        //}  
  
        /// <summary>  
        /// 上传视频生成缩略图  
        /// </summary>  
        /// <param name="vFileName">源文件</param>  
         /// <param name="sfilename">保存文件</param>  
        /// <returns></returns>  
         public string CatchImg1(string vFileName, string sfilename)  
        {  
            try  
            {  
                string ffmpeg = "ffmpeg.exe";  
                ffmpeg = System.AppDomain.CurrentDomain.BaseDirectory+ffmpeg;
                bool b = System.IO.File.Exists(ffmpeg);
                if (!(System.IO.File.Exists(ffmpeg)))  
                {  
                    return "";  
                }
                string flv_img = System.IO.Path.ChangeExtension(sfilename, ".jpg");
                string flv_img_p = flv_img;  
                string FlvImgSize = "1920*1080";
                
                //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);  
                //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;  
                //startInfo.Arguments = " -i " +vFileName + " -y -f image2 -t 0.1 -s " + FlvImgSize + " " + flv_img_p;  
                try  
                {
                    RunCmd2(ffmpeg, " -i " + vFileName + " -y -f image2 -t 0.1 -s " + FlvImgSize + " " + flv_img_p);
                
                    //System.Diagnostics.Process.Start(startInfo);  
                }  
                catch  
                {  
                    return "";  
                }  
                System.Threading.Thread.Sleep(4000);  
                if (System.IO.File.Exists(flv_img_p))  
                {  
                    return flv_img_p ;  
                }  
                return "";  
            }  
            catch  
            {  
                return "";  
            }  
        }
        bool RunCmd2(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    myPro.StartInfo.FileName = "cmd.exe";
                    myPro.StartInfo.UseShellExecute = false;
                    myPro.StartInfo.RedirectStandardInput = true;
                    myPro.StartInfo.RedirectStandardOutput = true;
                    myPro.StartInfo.RedirectStandardError = true;
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.Start();
                    //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                    string str = string.Format(@"""{0}"" {1} {2}", cmdExe, cmdStr, "&exit");

                    myPro.StandardInput.WriteLine(str);
                    myPro.StandardInput.AutoFlush = true;
                    myPro.WaitForExit();

                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }
    } 
}
