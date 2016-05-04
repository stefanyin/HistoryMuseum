using HistoryMuseum.Common;
using HistoryMuseum.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HistoryMuseum.Service
{
    public  class MenuSerice
    {
        private static string _url = "Menu\\Menu.Xml";
        public static string Url
        {
            get { return _url; }
            set { _url = value; }
        }
       private  static MenuSerice instance;
       private static object _lock=new object();
        public static void Load( string url)
        {
            _url = url;
           GetInstance()._menuInfoList = GetInstance().GetMune();
        }
        public static void Load()
        {
            GetInstance()._menuInfoList = GetInstance().GetMune();
        }
       private MenuSerice()
       {

       }

       public static MenuSerice GetInstance()
       {
               if(instance==null)
               {
                      lock(_lock)
                      {
                             if(instance==null)
                             {
                                     instance=new MenuSerice();
                             }
                      }
               }
               return instance;
       }

        private  MenuInfo GetMune()
        {
            MenuInfo list = new MenuInfo();
            list = (MenuInfo)SerializerUtil.LoadXml(_url, list);
            return list;
        }
        private  MenuInfo _menuInfoList= new MenuInfo();
        public  MenuInfo MenuInfoList
        {
            get 
            { return _menuInfoList ; }


            set { _menuInfoList = value ; }
        }
        public  void Add( MenuItemInfo miif)
        {
            MenuInfo obj = GetInstance().MenuInfoList;
            obj.Items.Add(miif);
            GetInstance()._menuInfoList = obj;
            foreach( MenuItemInfo i in obj.Items)
            {
                if (!GetInstance()._menuInfoList.Items.Contains(i))
                    GetInstance()._menuInfoList.Items.Add(i);
            }
        }
        public void UpDate()
        {
            SerializerUtil.SaveXml(Url,MenuInfoList);
        }
        public void QiYong(string id)
        {
            MenuInfo obj = GetInstance().MenuInfoList;
            foreach(MenuItemInfo mi in obj.Items)
            {
                if(mi.Id==Convert.ToInt32(id))
                {
                    mi.Stauts= !mi.Stauts;
                }
            }
            UpDate();
        }
        public void Del(string id)
        {
            MenuInfo obj = GetInstance().MenuInfoList;
            for (int i = obj.Items.Count - 1; i >= 0; i--)
            {
                if (obj.Items[i].Id == Convert.ToInt32(id))
                {
                    obj.Items.Remove(obj.Items[i]);
                }
            }
            UpDate();
        }
        public MenuItemInfo GetById(string id)
        {
            MenuInfo obj = GetInstance().MenuInfoList;
            MenuItemInfo m = new MenuItemInfo();
            m = obj.Items.Find(u => u.Id == Convert.ToInt32(id));
            return m;
        }
        public void SaveModel(MenuItemInfo model)
        {
            MenuInfo obj = GetInstance().MenuInfoList;
            for (int i = obj.Items.Count - 1; i >= 0; i--)
            {
                if (obj.Items[i].Id == model.Id)
                {
                    obj.Items[i] = model;
                }
            }
            UpDate();
        }
    }
}
