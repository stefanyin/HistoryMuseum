using HistoryMuseum.Model;
using HistoryMuseum.MVVM.Common;
using HistoryMuseum.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
using System.Windows.Threading;

namespace HistoryMuseum.MVVM
{
    /// <summary>
    /// VedioShowUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class VedioShowUserControl : UserControl
    {
        MainWindow _mw;// = new PicControl();
        private int totelNum = 1;
        private int currentsize = 1;
        private const int Num = 6;  //表示每页显示12条记录  
        public DispatcherTimer _dTimer = new DispatcherTimer();
        Point _mos;
        public int i = 0;
        public VedioShowUserControl(MainWindow mw)
        {
            InitializeComponent();
            childWindow.DataContext = ChildWindowManager.Instance;
            _mw = mw;
            Init();
        }
        void Init()
        {
            _mos = Mouse.GetPosition(this);
            //定时器使用委托（代理）对象调用相关函数（方法）dTimer_Tick;
            //注：此处 Tick 为 dTimer 对象的事件（ 超过计时器间隔时发生）
            _dTimer.Tick += new EventHandler(_dTimer_Tick);

            //设置时间：TimeSpan（时, 分， 秒）
            _dTimer.Interval = new TimeSpan(0, 0, 2);
            if (!_dTimer.IsEnabled)
            {
                //启动 DispatcherTimer对象dTime。
                _dTimer.Start();
            }
            string url = ConfigurationManager.AppSettings["chumoMenu"];
            MenuSerice.Load(url);
            MenuInfo list = MenuSerice.GetInstance().MenuInfoList;
            int i = 0;
            foreach (MenuItemInfo obj in list.Items.Where(u => u.Stauts == false).OrderBy(u => u.OrderNum).ThenBy(u => u.Id))
            {
                Button b = new Button();
                b.FontFamily = new System.Windows.Media.FontFamily("楷体");
                b.Foreground = new SolidColorBrush(Colors.White);
                b.Opacity = 0.6;
                //ControlTemplate myTemplate = (ControlTemplate)this.FindResource("GlassButton");//TabItemStyl
                Style myStyle = (Style)this.FindResource("ListButtonStyle");
               // b.Template = myTemplate;
                b.Style = myStyle;
                //ImageBrush imageBrush = new ImageBrush();
                //imageBrush.ImageSource = new BitmapImage(new Uri("Images/01.png", UriKind.Relative));
                //b.Background = imageBrush;
                b.Margin = new Thickness(0, 5, 0, 0);
                b.MinWidth = 200;
                b.Height = 70;
                b.Name = "button" + obj.Id.ToString();
                if (obj.Meno == null)
                {
                    obj.Meno = "";
                }
                b.Tag = obj;
                b.Content = obj.MenuName;
                b.Click += new RoutedEventHandler(_button_Click);
                this.listBoxMenu.Items.Add(b);
                i++;
            }
        }
        MenuItemInfo mi = new MenuItemInfo();
        void _button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            mi = (MenuItemInfo)btn.Tag;
            Binding(Num, 1,mi);
        }
        private void ReturnMain_Click_1(object sender, RoutedEventArgs e)
        {
            if (_dTimer.IsEnabled)
            {
                _dTimer.Stop();
            }
            _mw.selTrans();
            _mw.mainp.Content = new Enter(_mw);
        }

        private void grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Grid objs = (Grid)sender;
            int i = -1;
           foreach(var obj in objs.Children)
            {
                FrameworkElement panel = (FrameworkElement)obj;
                if (panel.Name.StartsWith("Id"))
                {
                    TextBlock tb = panel as TextBlock;
                    i = int.Parse(tb.Text);
                 }

            }
           if (_dTimer.IsEnabled)
           {
               _dTimer.Stop();
           }
           ContentMenuItemsInfo cmi= ContentMenuService.GetInstance().GetById(i.ToString());
           string _url = System.AppDomain.CurrentDomain.BaseDirectory + "\\Source\\";
           _url = _url + cmi.MenuItemInfoId.ToString() + "\\vedio\\" + cmi.Id.ToString() + ".MP4";
           if (IsFileInUse(_url) || !File.Exists(_url))
           {
               MessageBox.Show("视频被占用或者不存在！");
           }
           else
           {
               ChildWindowManager.Instance.ShowChildWindow(new VedioDetailsUserControl(cmi, this));
           }

        }
        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;

            FileStream fs = null;
            try
            {

                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,

                FileShare.None);

                inUse = false;
            }
            catch
            {

            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用
        }
        private void LeftBtn_Click_1(object sender, RoutedEventArgs e)
        { //获取当前页数  
            if (currentsize > 1)
            {
                Binding(Num, currentsize - 1, mi);   //调用分页方法  
            }
            else
            {
                Binding(Num, totelNum, mi);
            }
        }

        private void RightBtn_Click_1(object sender, RoutedEventArgs e)
        {
            int total = totelNum; //总页数
            if (currentsize < total)
            {
                Binding(Num, currentsize + 1, mi);   //调用分页方法  
            }
            else
            {
                Binding(Num, 1, mi);
            }

        }
        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <returns></returns>
        private List<String> NoticeList(List<ContentMenuItemsInfo> Items)
        {
            List<String> noticelist = new List<String>();
            //string picUrl = System.AppDomain.CurrentDomain.BaseDirectory + "\\Source\\" + mi.Id.ToString();
            //foreach (string str in Directory.GetFiles(@picUrl))
            //    noticelist.Add(str);
            foreach (ContentMenuItemsInfo item in Items)
            {
                string picUrl = System.AppDomain.CurrentDomain.BaseDirectory + "Source\\" + item.MenuItemInfoId.ToString() + "\\vedio\\" + item.Id.ToString()+".jpg";
                noticelist.Add(picUrl);
            }
            return noticelist;
        }
        private void Binding(int number, int currentSize, MenuItemInfo mi)
        {
            string url = ConfigurationManager.AppSettings["chumoMenuChild"];
            ContentMenuService.Load(url);
            List<ContentMenuItemsInfo> Items = ContentMenuService.GetInstance().MenuInfoList.Items.Where(u=>u.MenuItemInfoId==mi.Id&&u.Stauts==true).ToList();
            
            int count = Items.Count;          //获取记录总数  
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
            Items = Items.Take(number * currentSize).Skip(number * (currentSize - 1)).ToList();   //刷选第currentSize页要显示的记录集  

            listboxVedio.DataContext = GetPictures(Items);        //重新绑定dataGrid1  
        }
        private IEnumerable<MyData> GetPictures(List<ContentMenuItemsInfo> Items)
        {
            foreach (ContentMenuItemsInfo item in Items)
            {
                string picUrl = System.AppDomain.CurrentDomain.BaseDirectory + "Source\\" + item.MenuItemInfoId.ToString() + "\\vedio\\" + item.Id.ToString() + ".jpg";
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
                yield return new MyData() {
                    Id=item.Id.ToString(),
                    ImageSource = b3,
                    Zhuti = item.ZhuTi,
                    Zuozhe = item.ZuoZhe,
                    Neirong = item.Meno
                };
            }
      }
        private void _dTimer_Tick(object sender, EventArgs e)
        {
            Point newmos = Mouse.GetPosition(this);
            if (_mos == newmos)
            {
                if (i < 120)
                {
                    i++;
                }
                else
                {
                    if (_dTimer.IsEnabled)
                    {
                        _dTimer.Stop();
                        _mw.selTrans();
                        ChildWindowManager.Instance.CloseChildWindow();
                        _mw.mainp.Content = new MainUserControl(_mw);
                    }
                }
            }
            else
            {
                _mos = newmos;
            }
        }
    }
   
}