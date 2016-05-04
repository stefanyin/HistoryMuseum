using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryMuseum.Model
{
    public class ContentMenuItemsInfo
    {
        public int Id { get; set; }
        public int MenuItemInfoId { get; set; }
        public string MenuName { get; set; }
        public int OrderNum { get; set; }
        /// <summary>
        /// 是否是视频
        /// </summary>
        public string QiYong
        {
            get
            {
                string s = "启用中";
                if (Stauts == true)
                {
                    s = "启用中";
                }
                else
                {
                    s = "禁止中";
                }
                return s;
            }
        }
        public bool Stauts { get; set; }
        public string ZhuTi { get; set; }
        public string ZuoZhe { get; set; }
        public string Meno { get; set; }
    }
}
