using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SFLib;
using System.Timers;
using System.Net;
using System.Threading;
using System.IO;
namespace HistoryMuseum.MVVM
{
    /// <summary>
    /// MainUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        Enter et;
        MainWindow _mw;
        public MainUserControl(MainWindow mw)
        {
            InitializeComponent();
            _mw = mw;
        }

        private void EnterBtn_Click_1(object sender, RoutedEventArgs e)
        {
            et = new Enter(_mw);
            _mw.selTrans();
            _mw.mainp.Content = et;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void miniSizeButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
