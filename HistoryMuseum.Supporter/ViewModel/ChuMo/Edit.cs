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
    public class Edit : BaseViewModel
    {
        public event Action Closed;
        private MenuSerice _ms;

        public MenuSerice Ms
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
        private MenuItemInfo _model;
        public MenuItemInfo Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged("Model");
            }
        }
        public Edit(ViewModel.ChuMo.List lvm, int id)
        {
            _myListViewModel = lvm;
            _model = new MenuItemInfo() { Id = id };
            _editCommand = new DelegateCommand(SaveModel);
            _closeCommand = new DelegateCommand(ChildWindow_Closed);
        }
        private void SaveModel()
        {
            if (Closed != null)
            {
                MenuSerice.GetInstance().SaveModel(_model);
                Closed();
            }
        }
        public void Show()
        {
            _model = MenuSerice.GetInstance().GetById(_model.Id.ToString());
            this.Closed += ChildWindow_Closed;
            ChildWindowManager.Instance.ShowChildWindow(new View.ChuMo.Edit() { DataContext = this });
        }

        public void ChildWindow_Closed()
        {
            ChildWindowManager.Instance.CloseChildWindow();
            _myListViewModel.BindDate();
        }
    }
}