using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace HistoryMuseum.MVVM.Common
{
    public class ChildWindowManager  : BaseViewModel
    {
        public ChildWindowManager()
        {
            WindowVisibility = Visibility.Collapsed;
            XmlContent = null;
        }

        //Singleton pattern implementation
        private static ChildWindowManager instance;
        public static ChildWindowManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChildWindowManager();
                }
                return instance;
            }
        }

        #region Public Properties

        private Visibility windowVisibility;
        public Visibility WindowVisibility
        {
            get { return windowVisibility; }
            set
            {
                windowVisibility = value;
                OnPropertyChanged("WindowVisibility");
            }
        }

        private FrameworkElement xmlContent;
        public FrameworkElement XmlContent
        {
            get { return xmlContent; }
            set
            {
                xmlContent = value;
                OnPropertyChanged("XmlContent");
            }
        }

        #endregion

        #region Public Methods

        public void ShowChildWindow(FrameworkElement content)
        {
            XmlContent = content;
            OnPropertyChanged("XmlContent");
            WindowVisibility = Visibility.Visible;
            OnPropertyChanged("WindowVisibility");
        }

        public void CloseChildWindow()
        {
            WindowVisibility = Visibility.Collapsed;
            OnPropertyChanged("WindowVisibility");
            XmlContent = null;
            OnPropertyChanged("XmlContent");
        }

        #endregion
    }
}

