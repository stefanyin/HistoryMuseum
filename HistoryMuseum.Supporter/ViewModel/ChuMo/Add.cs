using HistoryMuseum.Model;
using HistoryMuseum.Service;
using HistoryMuseum.Supporter.Common;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryMuseum.Supporter.ViewModel.ChuMo
{
    public class Add: BaseViewModel
    {
        private MenuSerice _ms;

        public MenuSerice Ms
        {
            get { return _ms; }
            set { _ms = value; }
        }
        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand
        {
            get { return _closeCommand; }
        }
        private DelegateCommand _addCommand;
        public DelegateCommand AddCommand
        {
            get { return _addCommand; }
        }
        private ViewModel.ChuMo.List  _myListViewModel;
        public ViewModel.ChuMo.List MyListViewModel
        {
            get { return _myListViewModel; }
            set { _myListViewModel =value; }
        }
        private string _menuName;

        [Required(ErrorMessage = "必填选项")]
        [RegularExpression(@"^[\u4e00-\u9fa5]{1,7}$|^[\dA-Za-z_]{1,14}", ErrorMessage = "请输入正确名称格式!")]
        public string MenuName
        {
            get { return _menuName; }
            set
            {
                _menuName = value;
                OnPropertyChanged("menuName"); 
            }
        }
        private string _orderNum;

        [Required(ErrorMessage = "必填选项")]
        [RegularExpression(@"\b(?:[0-9]|[1-9][0-9]|1[0-4][0-9]|150)\b", ErrorMessage = "请输入正确排序号格式!")]
        public string OrderNum
        {
            get { return _orderNum; }
            set
            {
                _orderNum = value;
                OnPropertyChanged("OrderNum");
            }
        }

        private bool _stauts;
        public bool Stauts
        {
            get { return _stauts; }
            set
            {
                _stauts = value;
                OnPropertyChanged("Stauts");
            }
        }
        private string _meno;
        public string Meno
        {
            get { return _meno; }
            set
            {
                _meno = value;
                OnPropertyChanged("Meno");
            }
        }
        public Add()
        {
        }
        public Add(ViewModel.ChuMo.List lvm)
        {
            _myListViewModel = lvm;
            _addCommand = new DelegateCommand(SaveModel);
            _closeCommand = new DelegateCommand(ChildWindow_Closed);
        }
        private void SaveModel()
        {
            if (_closeCommand != null&&this.Validate())
            {
                int i = 1;
                if (MenuSerice.GetInstance().MenuInfoList.Items.Count > 0)
                {
                    i = MenuSerice.GetInstance().MenuInfoList.Items.Max(u => u.Id) + 1;
                }
                var obj = new MenuItemInfo()
                {
                    MenuName = _menuName,Id=i,OrderNum= Convert.ToInt32(_orderNum),Stauts= true,Meno=_meno
                };

                MenuSerice.GetInstance().Add(obj);
                ChildWindow_Closed();
            }
        }
        public void Show()
        {
            ChildWindowManager.Instance.ShowChildWindow(new  View.ChuMo.Add() { DataContext = this });
        }

        public void ChildWindow_Closed()
        {
            ChildWindowManager.Instance.CloseChildWindow();
            _myListViewModel.BindDate();
        }
    }
}

