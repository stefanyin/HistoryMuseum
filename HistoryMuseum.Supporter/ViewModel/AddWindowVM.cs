using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMVVM;
using HistoryMuseum.Supporter.View;
using SFLib;
using HistoryMuseum.Model;
using HistoryMuseum.Service;
using HistoryMuseum.Supporter.Common;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel.DataAnnotations;
namespace HistoryMuseum.Supporter.ViewModel
{
   public class AddWindowVM :NotifyObject
    {
        private MenuSerice _ms;
        public MenuSerice Ms
        {
            get { return _ms; }
            set { _ms = value; }
        }

        #region Model
        private string _menuName;
        [Required(ErrorMessage = "必填选项")]
        [RegularExpression(@"^[\u4e00-\u9fa5]{1,7}$|^[\dA-Za-z_]{1,14}", ErrorMessage = "请输入正确名称格式!")]
        public string MenuName
        {
            get { return _menuName; }
            set
            {
                _menuName = value;
                RaisePropertyChanged("menuName");
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
                RaisePropertyChanged("OrderNum");
            }
        }

        private bool _stauts;
        public bool Stauts
        {
            get { return _stauts; }
            set
            {
                _stauts = value;
                RaisePropertyChanged("Stauts");
            }
        }

        private string _meno;
        public string Meno
        {
            get { return _meno; }
            set
            {
                _meno = value;
                RaisePropertyChanged("Meno");
            }
        }

        #endregion

        #region Command
        private MyCommand _addCommand;
        public MyCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new MyCommand(new Action<object>
                        (
                            o=>
                            {

                            }
                        ));
                return _addCommand;
            }
        }

        private MyCommand _closeCommand;
        public MyCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new MyCommand(new Action<object>
                        (
                            o=>
                            {

                            }
                        ));
                return _closeCommand;
            }
        }

        #endregion 



    }
}
