using HistoryMuseum.Common;
using HistoryMuseum.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryMuseum.Service
{
    public class ContentMenuService
    {
        private static string _url = "Menu\\ContentMenu.Xml";
        public static string Url
        {
            get { return _url; }
            set { _url = value; }
        }
       private  static ContentMenuService instance;
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
        private ContentMenuService()
       {

       }

        public static ContentMenuService GetInstance()
       {
               if(instance==null)
               {
                      lock(_lock)
                      {
                             if(instance==null)
                             {
                                 instance = new ContentMenuService();
                             }
                      }
               }
               return instance;
       }

        private ContentMenuInfo GetMune()
        {
            ContentMenuInfo list = new ContentMenuInfo();
            list = (ContentMenuInfo)SerializerUtil.LoadXml(_url, list);
            if (list == null)
            {
                return new ContentMenuInfo() { Items = new List<ContentMenuItemsInfo>() };
            }
            return list;
        }
        private ContentMenuInfo _menuInfoList = new ContentMenuInfo() { Items=new List<ContentMenuItemsInfo>()};
        public ContentMenuInfo MenuInfoList
        {
            get 
            { return _menuInfoList ; }


            set { _menuInfoList = value ; }
        }
        public void Add(ContentMenuItemsInfo miif)
        {
            ContentMenuInfo obj = GetInstance().MenuInfoList;
            obj.Items.Add(miif);
            GetInstance()._menuInfoList = obj;
            foreach (ContentMenuItemsInfo i in obj.Items)
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
            ContentMenuInfo obj = GetInstance().MenuInfoList;
            foreach (ContentMenuItemsInfo mi in obj.Items)
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
            ContentMenuInfo obj = GetInstance().MenuInfoList;
            for (int i = obj.Items.Count - 1; i >= 0; i--)
            {
                if (obj.Items[i].Id == Convert.ToInt32(id))
                {
                    obj.Items.Remove(obj.Items[i]);
                }
            }
            UpDate();
        }
        public ContentMenuItemsInfo GetById(string id)
        {
            ContentMenuInfo obj = GetInstance().MenuInfoList;
            ContentMenuItemsInfo m = new ContentMenuItemsInfo();
            m = obj.Items.Find(u => u.Id == Convert.ToInt32(id));
            return m;
        }
        public void SaveModel(ContentMenuItemsInfo model)
        {
            ContentMenuInfo obj = GetInstance().MenuInfoList;
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
