using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryMuseum.Model
{
    public class MenuItemInfo
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public int OrderNum { get; set; }
        public string QiYong 
        {
            get
            {
                string s = "图片";
                if (Stauts == true)
                {
                    s = "图片";
                }
                else
                {
                    s = "视频";
                }
                return s;
            }
        }
        public bool Stauts { get; set; }
        public String Meno { get; set; }
    }
}
