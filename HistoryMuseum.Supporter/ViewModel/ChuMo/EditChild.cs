using HistoryMuseum.Model;
using HistoryMuseum.Service;
using HistoryMuseum.Supporter.Common;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryMuseum.Supporter.ViewModel.ChuMo
{
    public class EditChild: BaseViewModel
    {
        public event Action Closed;
        private ContentMenuService _ms;

        public ContentMenuService Ms
        {
            get { return _ms; }
            set { _ms = value; }
        }
        private DelegateCommand _editCommand;
        public DelegateCommand EditCommand
        {
            get { return _editCommand; }
        }
        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand
        {
            get { return _closeCommand; }
        }
        private ViewModel.ChuMo.List _myListViewModel;
        public ViewModel.ChuMo.List MyListViewModel
        {
            get { return _myListViewModel; }
            set { _myListViewModel = value; }
        }
        private ContentMenuItemsInfo _model;
        public ContentMenuItemsInfo Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged("Model");
            }
        }
        public EditChild(ViewModel.ChuMo.List lvm, int id)
        {
            _myListViewModel = lvm;
            _model = ContentMenuService.GetInstance().GetById(id.ToString());
            _editCommand = new DelegateCommand(SaveModel);
            _closeCommand = new DelegateCommand(ChildWindow_Closed);
        }
        private void SaveModel()
        {
            if (Closed != null)
            {
                ContentMenuService.GetInstance().SaveModel(_model);
                Closed();
            }
        }
        public void Show()
        {
            this.Closed += ChildWindow_Closed;
            ChildWindowManager.Instance.ShowChildWindow(new View.ChuMo.EditChild() { DataContext = this });
        }

        public void ChildWindow_Closed()
        {
            ChildWindowManager.Instance.CloseChildWindow();
            _myListViewModel.ChildBindDate(_myListViewModel.MenuItemInfoModel.Id);
        }
    }
}
