using HistoryMuseum.Model;
using HistoryMuseum.Service;
using HistoryMuseum.Supporter.Common;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HistoryMuseum.Supporter
{
    public class MainViewModel : BaseViewModel
    {
        private MainWindow _mYMainWindow;

        private UserControl _myContorl;
        public MenuSerice ms;
        

        public UserControl MyContorl
        {
            get { return _myContorl; }
            set { _myContorl = value; }
        }
       
        public MainWindow MYMainWindow
        {
            get { return _mYMainWindow; }
            set { _mYMainWindow = value; }
        }

        public MainViewModel(MainWindow obj)
        {
            this._mYMainWindow = obj;
            Init();
        }
        public MainViewModel()
        {
        }
        public List<ChuMoConInfo> ChuMoConInfos { get; set; }

        public ChuMoConInfo CurrentChuMoConInfo { get; set; }
        public DelegateCommand ShowSelectedChuMoConInfoCommand { get; set; }  
        void Init()
        {
            ChuMoConSerice.Load();
            ChuMoConInfos = ChuMoConSerice.GetInstance().ChuMoConItemList.Items;
            CurrentChuMoConInfo = ChuMoConInfos[0];
            this.ShowSelectedChuMoConInfoCommand = new DelegateCommand(this.ShowSelectedChuMoConInfoHandler); 
        }
        private void ShowSelectedChuMoConInfoHandler()
        {
            var childWindow = new ViewModel.ChuMo.List(CurrentChuMoConInfo);

            _mYMainWindow.ChildrenWinContent.Children.Clear();
            childWindow.ShowList(_mYMainWindow);
         }  
        

        

        
        
        private void ShowMyHomeWindow()
        {

        }
        private void ShowChuMoOneConfigWindow()
        {

        }
        private void ShowCommPortModelWindow()
        {

        }
        private void ShowDataBitModelWindow()
        {

        }
        private void ShowStopBitModelWindow()
        {

        }
        private void ShowParityCheckBitModelWindow()
        {

        }
        private void ShowControllerCenterWindow()
        {

        }
    }
}
