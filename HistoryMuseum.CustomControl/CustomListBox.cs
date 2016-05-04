using System;
using System.Collections.Generic;
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
using System.Threading;
using System.Windows.Media.Animation;

namespace HistoryMuseum.CustomControl
{
    public class CustomListBox : ListBox
    {
        #region 变量
        Grid _mainScrollCanvas = null;
        Canvas _mainCanvas = null;

        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        private bool _mouseDown = false;
        /// <summary>
        /// 鼠标按下时容器的Y坐标
        /// </summary>
        private double _mouseDownY = 0;
        /// <summary>
        /// 鼠标按下时的坐标
        /// </summary>
        private Point _mouseDownPoint = new Point();
        private Point _lastDownPoint = new Point();
        /// <summary>
        /// 鼠标按下和抬起期间移动的距离
        /// </summary>
        private double _moveDelta = 0;
        /// <summary>
        /// 弹性运动参数
        /// </summary>
        private static double SPEED_SPRINGNESS = 0.05;
        private static double BOUNCING_SPRINGESS = 0.1;
        #endregion

        static CustomListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomListBox), new FrameworkPropertyMetadata(typeof(CustomListBox)));
        }

        public override void OnApplyTemplate()
        {
            _mainCanvas = this.GetTemplateChild("layoutRoot") as Canvas;
            _mainScrollCanvas = this.GetTemplateChild("scrollCanvas") as Grid;
            if (_mainCanvas != null && _mainScrollCanvas != null)
            {
                _mainCanvas.PreviewMouseLeftButtonDown += MList_MouseLeftButtonDown;
                _mainCanvas.PreviewMouseMove += MList_MouseMove;
                _mainCanvas.PreviewMouseLeftButtonUp += MList_MouseLeftButtonUp;
                this.MouseLeave += (s, e) => { MList_MouseLeftButtonUp(null, null); };
                //this.Loaded += (s, e) => { _mainScrollCanvas.Width = this.ActualWidth; };
                this.Loaded += (s, e) => { _mainScrollCanvas.Height = this.ActualHeight; };
                CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            }
            base.OnApplyTemplate();
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!_mouseDown)
            {
                _moveDelta += (-_moveDelta) * SPEED_SPRINGNESS;
                double bouncing = 0;
                // double canvasHeight = _mainCanvas.ActualHeight;
                double canvasHeight = _mainCanvas.ActualWidth;
                //double listHeight = _mainScrollCanvas.ActualHeight;
                double listHeight = _mainScrollCanvas.ActualWidth;
                // double y = Canvas.GetTop(_mainScrollCanvas);
                double y = Canvas.GetLeft(_mainScrollCanvas);
                if (y > 0)
                {
                    bouncing = -y * BOUNCING_SPRINGESS;
                }
                else if (y + listHeight < canvasHeight)
                {
                    bouncing = (canvasHeight - listHeight - y) * BOUNCING_SPRINGESS;
                }
                //Canvas.SetTop(_mainScrollCanvas, y + bouncing + _moveDelta);
                Canvas.SetLeft(_mainScrollCanvas, y + bouncing + _moveDelta);
            }
        }
        #region << 列表操作方法

        //鼠标按下
        void MList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDownPoint = e.GetPosition(_mainCanvas);
            _lastDownPoint = _mouseDownPoint;
            _mouseDown = true;
            _moveDelta = 0;
            //_mouseDownY = Canvas.GetTop(_mainScrollCanvas);
            _mouseDownY = Canvas.GetLeft(_mainScrollCanvas);
            //_mainScrollCanvas.BeginAnimation(Canvas.TopProperty, null);
            _mainScrollCanvas.BeginAnimation(Canvas.LeftProperty, null);
        }

        //鼠标移动
        void MList_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                // 更新元素位置
                Point point = e.GetPosition(_mainCanvas);
                //Canvas.SetTop(_mainScrollCanvas, _mouseDownY + (point.Y - _mouseDownPoint.Y));
                Canvas.SetLeft(_mainScrollCanvas, _mouseDownY + (point.X - _mouseDownPoint.X));

                //_moveDelta =  point.Y - _lastDownPoint.Y;
                _moveDelta = point.X - _lastDownPoint.X;
                _lastDownPoint = point;
            }
        }


        //鼠标抬起
        void MList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = false;
        }
        #endregion

    }

}
