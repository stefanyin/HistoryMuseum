﻿using HistoryMuseum.Model;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HistoryMuseum.Supporter.View.ChuMo
{
    /// <summary>
    /// List.xaml 的交互逻辑
    /// </summary>
    public partial class List : UserControl
    {
        public List()
        {
            InitializeComponent();
            
        }
        public ViewModel.ChuMo.List ViewModel
        {
            get
            {
                return this.DataContext as ViewModel.ChuMo.List;
            }
            set
            {
                this.DataContext = value;
            }
        }
    }
}
