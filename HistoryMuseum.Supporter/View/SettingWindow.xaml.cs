﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HistoryMuseum.Supporter.ViewModel;
using HistoryMuseum.Service;
using HistoryMuseum.Supporter.Common;
namespace HistoryMuseum.Supporter.View
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            childWindow.DataContext = ChildWindowManager.Instance;
            this.DataContext = new SettingWindowVM(this);
        }
    }
}
