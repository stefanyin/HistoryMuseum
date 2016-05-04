using HistoryMuseum.Model;
using HistoryMuseum.MVVM.Common;
using HistoryMuseum.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Transitionals;

namespace HistoryMuseum.MVVM
{
    /// <summary>
    /// ShowUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class PicDdetailsControl : UserControl
    {
        MenuItemInfo _mi;
        private int totelNum = 1;
        private int currentsize = 1;
        private const int Num = 1;
        List<ContentMenuItemsInfo> _items;
        ContentMenuItemsInfo _cmi;
        private ObservableCollection<Type> transitionTypes = new ObservableCollection<Type>();
        PicShowUserControl _ps;
        public PicDdetailsControl(MenuItemInfo mi,ContentMenuItemsInfo cmi,PicShowUserControl ps)
        {
            InitializeComponent();
            Init(mi, cmi,ps);
        }
        void Init(MenuItemInfo mi, ContentMenuItemsInfo cmi,PicShowUserControl ps)
        {
            _ps = ps;
            if (!_ps._dTimer.IsEnabled)
            {
                _ps.time = 0;
                //启动 DispatcherTimer对象dTime。
                _ps._dTimer.Start();

            }
            _mi = mi;
            _cmi = cmi;
            string url = ConfigurationManager.AppSettings["chumoMenuChild"];
            ContentMenuService.Load(url);
            _items = ContentMenuService.GetInstance().MenuInfoList.Items.Where(u => u.MenuItemInfoId == mi.Id && u.Stauts == true).ToList();

            int size = _items.FindIndex(u => u.Id == cmi.Id && u.MenuItemInfoId == cmi.MenuItemInfoId);
            Binding(Num, size + 1, mi);
            
        }
        private List<String> NoticeList(List<ContentMenuItemsInfo> Items)
        {
            List<String> noticelist = new List<String>();
            //string picUrl = System.AppDomain.CurrentDomain.BaseDirectory + "\\Source\\" + mi.Id.ToString();
            //foreach (string str in Directory.GetFiles(@picUrl))
            //    noticelist.Add(str);
            foreach (ContentMenuItemsInfo item in Items)
            {
                string picUrl = System.AppDomain.CurrentDomain.BaseDirectory + "Source\\" + item.MenuItemInfoId.ToString() + "\\" + item.Id.ToString() + ".png";
                noticelist.Add(picUrl);
            }
            return noticelist;
        }
        private void Binding(int number, int currentSize, MenuItemInfo mi)
        {
            int count = _items.Count;          //获取记录总数  
            int pageSize = 0;            //pageSize表示总页数  
            if (count % number == 0)
            {
                pageSize = count / number;
            }
            else
            {
                pageSize = count / number + 1;
            }
            totelNum = pageSize;

            currentsize = currentSize;
            ContentMenuItemsInfo obj = _items.Take(number * currentSize).Skip(number * (currentSize - 1)).First();//刷选第currentSize页要显示的记录集  
            string picUrl = System.AppDomain.CurrentDomain.BaseDirectory + "Source\\" + obj.MenuItemInfoId.ToString() + "\\" + obj.Id.ToString() + ".png";
            ImageBrush b3 = new ImageBrush();
            if (File.Exists(picUrl))
            {
                b3.ImageSource = new BitmapImage(new Uri(picUrl, UriKind.RelativeOrAbsolute));
            }
            else
            {
                picUrl = System.AppDomain.CurrentDomain.BaseDirectory + "Images\\" + "Back.png";
                b3.ImageSource = new BitmapImage(new Uri(picUrl, UriKind.RelativeOrAbsolute));

            }
            Border bo = new Border();
            bo.CornerRadius = new CornerRadius(10);
            bo.Background = b3;
            LoadTransitions(Assembly.GetAssembly(typeof(Transition)));
            selTrans();
            try
            {
                mainpic.Content = bo;
            }
            catch (Exception e)
            {
                return;
            }
           // listboxPic.DataContext = GetPictures(Items);        //重新绑定dataGrid1  
        }
        private void LeftBtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (currentsize > 1)
            {
                Binding(Num, currentsize - 1, _mi);   //调用分页方法  
            }
            else
            {
                Binding(Num, totelNum, _mi);
            }
        }

        private void ReturnMain_Click_1(object sender, RoutedEventArgs e)
        {
            if (!_ps._dTimer.IsEnabled)
            {
                _ps._dTimer.Start();
                _ps.time = 0;
            }
            ChildWindowManager.Instance.CloseChildWindow();
        }

        private void RightBtn_Click_1(object sender, RoutedEventArgs e)
        {
            int total = totelNum; //总页数
            if (currentsize < total)
            {
                Binding(Num, currentsize + 1, _mi);   //调用分页方法  
            }
            else
            {
                Binding(Num, 1, _mi);
            }
        }
        public void LoadTransitions(Assembly assembly)
        {

            foreach (Type type in assembly.GetTypes())
            {
                // Must not already exist
                if (transitionTypes.Contains(type)) { continue; }
                if (type.FullName == "Transitionals.Transitions.PageTransition") { continue; };
                // Must not be abstract.
                if ((typeof(Transition).IsAssignableFrom(type)) && (!type.IsAbstract))
                {
                    transitionTypes.Add(type);
                }
            }
        }
        void tmp()
        {
            
            Transitionals.Transitions.RotateTransition t = new Transitionals.Transitions.RotateTransition();


            mainpic.Transition = t;
        }
        public void selTrans()
        {
            //tmp();
            // return;

            int m_nIndex = 0;
            m_nIndex = new Random().Next(0, transitionTypes.Count);

            Type transitionType = transitionTypes[m_nIndex];

            Transition t = (Transition)Activator.CreateInstance(transitionType);

            mainpic.Transition = t;
        }

        private void mainpic_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_ps._dTimer.IsEnabled)
            {
                _ps._dTimer.Start();
                _ps.time = 0;
            }
            ChildWindowManager.Instance.CloseChildWindow();
        }
    }
}
