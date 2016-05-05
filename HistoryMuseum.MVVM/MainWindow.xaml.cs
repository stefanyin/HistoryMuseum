using _3DTools;
using HistoryMuseum.Common;
using HistoryMuseum.Model;
using HistoryMuseum.MVVM.Common;
using HistoryMuseum.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Transitionals;
using System.ComponentModel;
using HistoryMuseum.MVVM.Service;
using SFLib;
namespace HistoryMuseum.MVVM
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        MainUserControl user;
        private ObservableCollection<Type> transitionTypes = new ObservableCollection<Type>();

        ClientReceiver _receiver = new ClientReceiver();

        public MainWindow()
        {
            InitializeComponent();
            Init();
            this.DataContext = this;
        }

        private void StartServers()
        {
            try
            {
                _receiver.Start();
                
            }
            catch(Exception ex)
            {

                Logger.Exception(ex.Message);
            }
        }

        void Init()
        {
            LoadTransitions(Assembly.GetAssembly(typeof(Transition)));
            selTrans();
            user = new MainUserControl(this);
            mainp.Content = user;
            BkVideo = "background_videos/bkgdvideo.avi";
        }
        private void EnterBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Enter win = new Enter(this);
            //win.Show();
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            StartServers();
        }
        public void LoadTransitions(Assembly assembly)
        {

            foreach (Type type in assembly.GetTypes())
            {
                // Must not already exist
                if (transitionTypes.Contains(type)) { continue; }
                if (type.FullName == "Transitionals.Transitions.PageTransition") { continue; };
                // Must not be abstract.
                if ((typeof(Transition).IsAssignableFrom(type)) && (!type.IsAbstract))
                {
                    transitionTypes.Add(type);
                }
            }
        }

        void tmp()
        {
            //所有的变换
            //Transitionals.Transitions.StarTransition        //星
            //Transitionals.Transitions.RotateTransition      //3d旋转
            //Transitionals.Transitions.VerticalWipeTransition//下拉
            //Transitionals.Transitions.PageTransition        //翻页
            //Transitionals.Transitions.RollTransition        //旋转出
            //Transitionals.Transitions.DiamondsTransition    //棋盒棱形
            //Transitionals.Transitions.VerticalBlindsTransition //垂直百叶窗
            //Transitionals.Transitions.HorizontalWipeTransition //左拉
            //Transitionals.Transitions.FadeAndBlurTransition    //淡入
            //Transitionals.Transitions.ExplosionTransition      //球形散开
            //Transitionals.Transitions.CheckerboardTransition   //棋盒方形
            //Transitionals.Transitions.TranslateTransition      //飞入
            //Transitionals.Transitions.RotateWipeTransition     //旋转擦除
            //Transitionals.Transitions.MeltTransition           //柱状
            //Transitionals.Transitions.DiagonalWipeTransition   //斜擦除
            //Transitionals.Transitions.FlipTransition           //单面翻书
            //Transitionals.Transitions.DotsTransition           //球状棋盒
            //Transitionals.Transitions.FadeAndGrowTransition    //淡入
            //Transitionals.Transitions.DoubleRotateWipeTransition //双线擦除
            //Transitionals.Transitions.DoorTransition             //门状
            //Transitionals.Transitions.HorizontalBlindsTransition //水平百叶窗
            //Transitionals.Transitions.FadeTransition             //溶解

            // Transition t;
            Transitionals.Transitions.RotateTransition t = new Transitionals.Transitions.RotateTransition();


            mainp.Transition = t;
        }
        public void selTrans()
        {
            //tmp();
            // return;

            int m_nIndex = 0;
            m_nIndex = new Random().Next(0, transitionTypes.Count);

            Type transitionType = transitionTypes[m_nIndex];

            Transition t = (Transition)Activator.CreateInstance(transitionType);

            mainp.Transition = t;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void miniSizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private string _bkVideo = "";
        public string BkVideo
        {
            get
            {
                return _bkVideo;
            }
            set
            {
                _bkVideo = value;
                RaisePropertyChanged("BkVideo");
            }
        }

        #region 实现接口
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected void RaisePropertyChanged(String propertyName)
        {
            //VerifyPropertyName(propertyName);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void Background_Video_MediaEnded(object sender, RoutedEventArgs e)
        {
            BkVideo = "background_videos/bkgdvideo.avi";
        }

        private void Background_Video_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _receiver.Stop();
        }
    }

}
