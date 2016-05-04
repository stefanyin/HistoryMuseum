using HistoryMuseum.Model;
using HistoryMuseum.MVVM.Common;
using HistoryMuseum.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HistoryMuseum.MVVM
{
    /// <summary>
    /// Enter.xaml 的交互逻辑
    /// </summary>
    public partial class Enter : UserControl
    {
        MainWindow _mw;// = new PicControl();
        DispatcherTimer _dTimer = new DispatcherTimer();
        Point _mos ;
        int i = 0;
        public Enter(MainWindow mw )
        {
            InitializeComponent();
            _mw = mw;
            _mos = Mouse.GetPosition(this);
            //定时器使用委托（代理）对象调用相关函数（方法）dTimer_Tick;
            //注：此处 Tick 为 dTimer 对象的事件（ 超过计时器间隔时发生）
            _dTimer.Tick += new EventHandler(_dTimer_Tick);

            //设置时间：TimeSpan（时, 分， 秒）
            _dTimer.Interval = new TimeSpan(0, 0, 2);
            if (!_dTimer.IsEnabled)
            {
                //启动 DispatcherTimer对象dTime。
                _dTimer.Start();
            }
        }

        private void EnterPic_Click_1(object sender, RoutedEventArgs e)
        {
            if (_dTimer.IsEnabled)
            {
                _dTimer.Stop();
            }
            _mw.selTrans();
            _mw.mainp.Content = new PicShowUserControl(_mw);
        }

        private void ReturnMain_Click_1(object sender, RoutedEventArgs e)
        {
            if (_dTimer.IsEnabled)
            {
                _dTimer.Stop();
            }
            _mw.selTrans();
            _mw.mainp.Content = new MainUserControl(_mw);
        }

        private void EnterVedio_Click_1(object sender, RoutedEventArgs e)
        {
            if (_dTimer.IsEnabled)
            {
                _dTimer.Stop();
            }
            _mw.selTrans();
            _mw.mainp.Content = new VedioShowUserControl(_mw);
        }
        private void _dTimer_Tick(object sender, EventArgs e)
        {
            Point newmos = Mouse.GetPosition(this);
            if (_mos == newmos)
            {
                if (i < 5)
                {
                    i++;
                }
                else
                {
                    if (_dTimer.IsEnabled)
                    {
                        _dTimer.Stop();
                        _mw.selTrans();
                        _mw.mainp.Content = new MainUserControl(_mw);
                    }
                }
            }
            else
            {
                _mos = newmos;
            }
        }
    }

}

