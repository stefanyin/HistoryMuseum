using HistoryMuseum.Common;
using HistoryMuseum.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryMuseum.Service
{
    public class ChuMoConSerice
   {
        private static string _url = "ChuMo\\ChuMo.Xml";
        public static string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        private static ChuMoConSerice instance;
       private static object _lock=new object();
        public static void Load( string url)
        {
            _url = url;
            GetInstance()._chuMoConItemList = GetInstance().GetChuMo();
        }
        public static void Load()
        {
            GetInstance()._chuMoConItemList = GetInstance().GetChuMo();
        }
        private ChuMoConSerice()
       {

       }

        public static ChuMoConSerice GetInstance()
       {
               if(instance==null)
               {
                      lock(_lock)
                      {
                             if(instance==null)
                             {
                                 instance = new ChuMoConSerice();
                             }
                      }
               }
               return instance;
       }

       private ChuMoConItemList GetChuMo()
        {
            ChuMoConItemList list = new ChuMoConItemList();
            list = (ChuMoConItemList)SerializerUtil.LoadXml(_url, list);
            return list;
        }
        private ChuMoConItemList _chuMoConItemList = new ChuMoConItemList();
        public ChuMoConItemList ChuMoConItemList
        {
            get
            { return _chuMoConItemList; }


            set { _chuMoConItemList = value; }
        }
        public void Add(ChuMoConInfo miif)
        {
            ChuMoConItemList obj = GetInstance().ChuMoConItemList;
            obj.Items.Add(miif);
            GetInstance()._chuMoConItemList = obj;
            foreach (ChuMoConInfo i in obj.Items)
            {
                if (!GetInstance()._chuMoConItemList.Items.Contains(i))
                    GetInstance()._chuMoConItemList.Items.Add(i);
            }
        }
        public void UpDate()
        {
            SerializerUtil.SaveXml(Url, ChuMoConItemList);
        }
        //public void QiYong(string id)
        //{
        //    ChuMoConItemList obj = GetInstance().ChuMoConItemList;
        //    foreach (ChuMoConInfo mi in obj.Items)
        //    {
        //        if(mi.Id==id)
        //        {
        //            mi.Stauts= !mi.Stauts;
        //        }
        //    }
        //    UpDate();
        //}
        public void Del(string id)
        {
            ChuMoConItemList obj = GetInstance().ChuMoConItemList;
            for (int i = obj.Items.Count - 1; i >= 0; i--)
            {
                if (obj.Items[i].Id == id)
                {
                    obj.Items.Remove(obj.Items[i]);
                }
            }
            UpDate();
        }
        public ChuMoConInfo GetById(string id)
        {
            ChuMoConItemList obj = GetInstance().ChuMoConItemList;
            ChuMoConInfo m = new ChuMoConInfo();
            m = obj.Items.Find(u => u.Id == id);
            return m;
        }
        public void SaveModel(ChuMoConInfo model)
        {
            ChuMoConItemList obj = GetInstance().ChuMoConItemList;
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

