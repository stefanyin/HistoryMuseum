using HistoryMuseum.Model;
using HistoryMuseum.Service;
using HistoryMuseum.Supporter.Common;
using HistoryMuseum.Supporter.View.ChuMo;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HistoryMuseum.Supporter.View;
using HistoryMuseum.Supporter.Utility;
namespace HistoryMuseum.Supporter.ViewModel.ChuMo
{
    public class List : BaseViewModel
    {
        #region 一级
        private ICommand detailsCommand;
        private ICommand addCommand;
        private ICommand editCommand;
        private ICommand delCommand;
        private ICommand addCopyPicCommand;
        private ICommand addCopyVedioCommand;
        private ICommand upDateCommand;
        private ICommand qiYongCommand;
        #endregion

        #region 二级
        private ICommand addChildCommand;
        private ICommand editChildCommand;
        private ICommand delChildCommand;
        private ICommand addChildCopyPicCommand;
        private ICommand addChildCopyVedioCommand;
        private ICommand upDateChildCommand;
        private ICommand qiYongChildCommand;
        #endregion

        private ObservableCollection<MenuItemInfo> _ipModelDataList;

        private ObservableCollection<ContentMenuItemsInfo> _childModelDataList;
        public View.ChuMo.List View { get; set; }
        #region 一级
        public ICommand DetailsCommand
        {
            get { return detailsCommand; }
            set { detailsCommand = value; }
        }
        public ICommand EditCommand
        {
            get { return editCommand; }
            set { editCommand = value; }
        }
        public ICommand DelCommand
        {
            get { return delCommand; }
            set { delCommand = value; }
        }
        public ICommand AddCopyPicCommand
        {
            get { return addCopyPicCommand; }
            set { addCopyPicCommand = value; }
        }
        public ICommand AddCopyVedioCommand
        {
            get { return addCopyVedioCommand; }
            set { addCopyVedioCommand = value; }
        }
        public ICommand AddCommand
        {
            get { return addCommand; }
            set { addCommand = value; }
        }
        public ICommand UpDateCommand
        {
            get { return upDateCommand; }
            set { upDateCommand = value; }
        }
        public ICommand QiYongCommand
        {
            get { return qiYongCommand; }
            set { qiYongCommand = value; }
        }
        #endregion

        #region 二级
        public ICommand EditChildCommand
        {
            get { return editChildCommand; }
            set { editChildCommand = value; }
        }
        public ICommand DelChildCommand
        {
            get { return delChildCommand; }
            set { delChildCommand = value; }
        }
        public ICommand AddChildCopyPicCommand
        {
            get { return addChildCopyPicCommand; }
            set { addChildCopyPicCommand = value; }
        }
        public ICommand AddChildCopyVedioCommand
        {
            get { return addChildCopyVedioCommand; }
            set { addChildCopyVedioCommand = value; }
        }
        public ICommand AddChildCommand
        {
            get { return addChildCommand; }
            set { addChildCommand = value; }
        }
        public ICommand UpDateChildCommand
        {
            get { return upDateChildCommand; }
            set { upDateChildCommand = value; }
        }
        public ICommand QiYongChildCommand
        {
            get { return qiYongChildCommand; }
            set { qiYongChildCommand = value; }
        }
        #endregion
        public ObservableCollection<MenuItemInfo> MenuInfoList
        {
            get { return this._ipModelDataList; }
            set
            {
                this._ipModelDataList = value;
                OnPropertyChanged("MenuInfoList");
            }
        }

        private MenuItemInfo _selectedData;
        public MenuItemInfo SelectedData
        {
            get { return _selectedData; }
            set
            {
                if(_selectedData !=value)
                {
                    _selectedData = value;
                    OnPropertyChanged("SelectedData");
                }
            }
        }

        public ObservableCollection<ContentMenuItemsInfo> ChildMenuInfoList
        {
            get { return this._childModelDataList; }
            set
            {
                this._childModelDataList = value;
                OnPropertyChanged("ChildMenuInfoList");
            }
        }


        private MenuItemInfo menuItemInfoModel;
        public MenuItemInfo MenuItemInfoModel
        {
            get { return menuItemInfoModel; }
            set
            {
                this.MenuItemInfoModel = value;
                OnPropertyChanged("MenuItemInfoModel");
            }
        }
        
        public ChuMoConInfo ChuMoCon { get; set; }

        public string ChildMenuPath { get; set; }
        private Visibility vedioVisibility;
        public Visibility VedioVisibility
        {
            get { return vedioVisibility; }
            set
            { vedioVisibility = value; }
        }
        private Visibility picVisibility;
        public Visibility PicVisibility
        {
            get { return picVisibility; }
            set
            { PicVisibility = value; }
        }

        private Visibility showBtnVisibility;
        public Visibility ShowBtnVisibility
        {
            get { return showBtnVisibility; }
            set
            {
                this.showBtnVisibility = value;
                OnPropertyChanged("ShowBtnVisibility");
            }
        }
        private string showTitle;
        public string ShowTitle
        {
            get { return showTitle; }
            set
            {
                this.showTitle = value;
                OnPropertyChanged("ShowTitle");
            }
        }
        public List(ChuMoConInfo cmi)
        {
            
            ChuMoCon = cmi;
            string url = ChuMoCon.MuneXml;
            MenuSerice.Load(@url);
            if (cmi.YangShi == "N")
            {
                vedioVisibility = Visibility.Collapsed;
            }
            showBtnVisibility = Visibility.Collapsed;
            showTitle = "内容配置：";
            BindDate();
            #region 一级
            detailsCommand = new DelegateCommand<object>(ShowDetailsWindow, arg => true);
            addCommand = new DelegateCommand(ShowAddWindow);
            editCommand = new DelegateCommand<object>(ShowEditWindow, arg => true);
            delCommand = new DelegateCommand<object>(ShowDelWindow, arg => true);
            addCopyPicCommand = new DelegateCommand<object>(ShowAddCopyPicWindow, arg => true);
            addCopyVedioCommand = new DelegateCommand<object>(ShowAddCopyVedioWindow, arg => true);
            upDateCommand = new DelegateCommand<object>(ShowUpDateWindow, arg => true);
            qiYongCommand = new DelegateCommand<object>(ShowQiYongWindow, arg => true);
            #endregion
            #region 二级
            addChildCommand = new DelegateCommand(ShowAddChildWindow);
            editChildCommand = new DelegateCommand<object>(ShowEditChildWindow, arg => true);
            delChildCommand = new DelegateCommand<object>(ShowDelChildWindow, arg => true);
            addChildCopyPicCommand = new DelegateCommand<object>(ShowAddCopyPicWindow, arg => true);
            addChildCopyVedioCommand = new DelegateCommand<object>(ShowAddCopyVedioWindow, arg => true);
            upDateChildCommand = new DelegateCommand<object>(ShowUpDateChildWindow, arg => true);
            qiYongChildCommand = new DelegateCommand<object>(ShowQiYongChildWindow, arg => true);
            #endregion
        }

        public List(string _menu_xml,string _child_menu_xml)
        {
            ChildMenuPath = _child_menu_xml;
            MenuSerice.Load(@_menu_xml);
           // showBtnVisibility = Visibility.Collapsed;
            showTitle = "内容配置：";
            BindDate();
            #region 一级
            detailsCommand = new DelegateCommand<object>(ShowDetailsWindow, arg => true);
            addCommand = new DelegateCommand(ShowAddWindow);
            editCommand = new DelegateCommand<object>(ShowEditWindow, arg => true);
            delCommand = new DelegateCommand<object>(ShowDelWindow, arg => true);
            addCopyPicCommand = new DelegateCommand<object>(ShowAddCopyPicWindow, arg => true);
            addCopyVedioCommand = new DelegateCommand<object>(ShowAddCopyVedioWindow, arg => true);
            upDateCommand = new DelegateCommand<object>(ShowUpDateWindow, arg => true);
            qiYongCommand = new DelegateCommand<object>(ShowQiYongWindow, arg => true);
            #endregion
            #region 二级
            addChildCommand = new DelegateCommand(ShowAddChildWindow);
            editChildCommand = new DelegateCommand<object>(ShowEditChildWindow, arg => true);
            delChildCommand = new DelegateCommand<object>(ShowDelChildWindow, arg => true);
            addChildCopyPicCommand = new DelegateCommand<object>(ShowAddCopyPicWindow, arg => true);
            addChildCopyVedioCommand = new DelegateCommand<object>(ShowAddCopyVedioWindow, arg => true);
            upDateChildCommand = new DelegateCommand<object>(ShowUpDateChildWindow, arg => true);
            qiYongChildCommand = new DelegateCommand<object>(ShowQiYongChildWindow, arg => true);
            #endregion
        }
        public void ShowList(MainWindow mw)
        {
            View.ChuMo.List list = new Supporter.View.ChuMo.List();
            list.ViewModel = this;
            mw.ChildrenWinContent.Children.Add(list);
        }

        public void ShowList(SettingWindow sw)
        {
            HistoryMuseum.Supporter.View.ChuMo.List list = new Supporter.View.ChuMo.List();
            list.ViewModel = this;
            sw.ChildrenWinContent.Children.Add(list);
        }

        private void ShowDetailsWindow(object sender)
        {
            string s = sender.ToString();
            string url = ChildMenuPath;
            ContentMenuService.Load(@url);
            ChildBindDate(int.Parse(s));
        }
        private void ShowEditWindow(object sender)
        {
            string s = sender.ToString();
            var childWindow = new ViewModel.ChuMo.Edit(this, int.Parse(s));
            childWindow.Closed += BindDate;
            childWindow.Show();
        }
        private void ShowEditChildWindow(object sender)
        {
            string s = sender.ToString();
            var childWindow = new ViewModel.ChuMo.EditChild(this, int.Parse(s));
            childWindow.Show();
        }
        private void ShowUpDateWindow(object sender)
        {
            MenuSerice.GetInstance().UpDate();
        }

        private void ShowUpDateChildWindow(object sender)
        {
            ContentMenuService.GetInstance().UpDate();
        }

        private string _sourcePath = "\\\\"+Data.GetInstance().GetIP()+"\\Source\\";

        private void ShowAddCopyPicWindow(object sender)
        {
            string s = sender.ToString();
            Console.WriteLine(_selectedData.Id);
            ChildWindowManager.Instance.ShowChildWindow(new CopyPic(menuItemInfoModel,s, _sourcePath));
        }
        private void ShowAddCopyVedioWindow(object sender)
        {
            string s = sender.ToString();
            ChildWindowManager.Instance.ShowChildWindow(new CopyVedio(menuItemInfoModel,s, ChuMoCon));
        } 
        private void ShowDelWindow(object sender)
        {
            string s = sender.ToString();
            MenuSerice.GetInstance().Del(s);
            BindDate();

        }
        private void ShowDelChildWindow(object sender)
        {
            string s = sender.ToString();
            ContentMenuService.GetInstance().Del(s);
            ChildBindDate(menuItemInfoModel.Id);

        }
        private void ShowQiYongWindow(object sender)
        {
            string s = sender.ToString();
            MenuSerice.GetInstance().QiYong(s);
            BindDate();

        }
        private void ShowQiYongChildWindow(object sender)
        {
            string s = sender.ToString();
            ContentMenuService.GetInstance().QiYong(s);
            ChildBindDate(menuItemInfoModel.Id);

        } 
        private void ShowAddWindow()
        {
            var childWindow = new ViewModel.ChuMo.Add(this);
            childWindow.Show();
            //var addwindow = new AddWindow();
            //addwindow.Show();
        }
        private void ShowAddChildWindow()
        {
            if (menuItemInfoModel==null|| menuItemInfoModel.Id==0)
            {
                MessageBox.Show("选择菜单！");
                return;
            }
            var childWindow = new ViewModel.ChuMo.AddChild(this);
            childWindow.Show();
        }
        public void BindDate()
        {
            IList<MenuItemInfo> gbList = MenuSerice.GetInstance().MenuInfoList.Items;
            this.MenuInfoList = ConvertIListToList<MenuItemInfo>(gbList);
        }

        public void ChildBindDate(int id)
        {
            MenuItemInfo mii = MenuSerice.GetInstance().MenuInfoList.Items.Find(u => u.Id == id);
            menuItemInfoModel = MenuSerice.GetInstance().MenuInfoList.Items.Find(u => u.Id == id);
            showTitle = "内容配置:" + menuItemInfoModel.MenuName;
            if (mii.Stauts)
            {
                picVisibility = Visibility.Visible;
                vedioVisibility = Visibility.Collapsed;
            }
            else
            {
                picVisibility = Visibility.Collapsed;
                vedioVisibility = Visibility.Visible;
            }
            var obj = ContentMenuService.GetInstance().MenuInfoList.Items;
            IList<ContentMenuItemsInfo> gbChildList = new List<ContentMenuItemsInfo>();
            if (obj != null)
            {
                gbChildList = obj.Where(u=>u.MenuItemInfoId==id).OrderBy(u=>u.OrderNum).ThenBy(u=>u.Id).ToList();
            }
            this.ChildMenuInfoList = ConvertIListToList<ContentMenuItemsInfo>(gbChildList);
        }


        // **//// <summary>
        /// 转换IList<T>为List<T>      
        /// //将IList接口泛型转为List泛型类型
        /// </summary>
        /// <typeparam name="T">指定的集合中泛型的类型</typeparam>
        /// <param name="gbList">需要转换的IList</param>
        /// <returns></returns>
        private ObservableCollection<T> ConvertIListToList<T>(IList<T> gbList) where T : class   //静态方法，泛型转换，
        {
            if (gbList != null && gbList.Count >= 1)
            {
                ObservableCollection<T> list = new ObservableCollection<T>();
                for (int i = 0; i < gbList.Count; i++)  //将IList中的元素复制到List中
                {
                    T temp = gbList[i] as T;
                    if (temp != null)
                        list.Add(temp);
                }
                return list;
            }
            return null;
        }
    }
}

