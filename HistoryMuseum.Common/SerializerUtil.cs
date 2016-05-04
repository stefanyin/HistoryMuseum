using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace HistoryMuseum.Common
{
    public static class SerializerUtil
    {

        #region 序列化位二进制

        ///   <summary>   
        ///   序列化位二进制   
        ///   </summary>   
        ///   <param   name="request">要序列化的对象</param>   
        ///   <returns>字节数组</returns>   
        public static byte[] SerializeBinary(object request)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            using (MemoryStream memStream = new MemoryStream())
            {
                serializer.Serialize(memStream, request);
                return memStream.GetBuffer();
            }
        }
        #endregion

        #region 二进制反序列化

        ///   <summary>   
        ///   二进制反序列化   
        ///   </summary>   
        ///   <param   name="buf">字节数组</param>   
        ///   <returns>得到的对象</returns>   
        public static object DeserializeBinary(byte[] buf)
        {
            if (buf == null)
            {
                return null;
            }
            using (MemoryStream memStream = new MemoryStream(buf))
            {
                memStream.Position = 0;
                BinaryFormatter deserializer = new BinaryFormatter();
                object info = (object)deserializer.Deserialize(memStream);
                memStream.Close();
                return info;
            }
        }
        #endregion

        #region 序列化为xml文件
        /// <summary>
        /// 序列化为xml
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        public static void SaveXml(string filePath, object obj)
        {
            CreateDir(filePath);
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath,false, Encoding.Default))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                xs.Serialize(writer, obj);
                writer.Close();
            }
        }

        /// <summary>
        /// 根据文件路径，或文件夹路径，创建文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void CreateDir(string dir)
        {
            if (dir.Contains("."))
            {
                dir = dir.Substring(0, dir.LastIndexOf("\\"));
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static string SaveXml(object obj)
        {
            MemoryStream ms = new MemoryStream();
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(ms, Encoding.Default))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                xs.Serialize(writer, obj);
                writer.Close();

                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        #endregion

        #region xml文件反序列化
        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object LoadXml(string filePath, object type)
        {
            object result = null;
            if (!File.Exists(filePath))
            {
                CreateDir(filePath);
            }
            FileInfo CreateFile = new FileInfo(filePath); //创建文件 
            if (!CreateFile.Exists)
            {
                FileStream FS = CreateFile.Create();
                FS.Close();
            }
            try
            {
                using (StreamReader reader = new StreamReader(filePath, Encoding.Default))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type.GetType());
                    result = xmlSerializer.Deserialize(reader);
                }
            }
            catch(Exception e)
            {
                result = new object() ;
            }
            return result;
        }
        public static T Deserialize<T>(T t, string s)
        {
            using (StringReader sr = new StringReader(s ))
            {
                XmlSerializer xz = new XmlSerializer(t.GetType());

                return (T)xz.Deserialize(sr);
            }
        }
        #endregion

        #region 将C#数据实体转化为xml数据
        /// <summary>
        /// 将C#数据实体转化为xml数据
        /// </summary>
        /// <param name="obj">要转化的数据实体</param>
        /// <returns>xml格式字符串</returns>
        public static string XmlSerialize<T>(T obj)
        {
            //添加引用 using System.Runtime.Serialization;
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Position = 0;

            StreamReader sr = new StreamReader(stream, Encoding.Default);
            string resultStr = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return resultStr;
        }
        #endregion

        #region 将xml数据转化为C#数据实体
        /// <summary>
        /// 将xml数据转化为C#数据实体
        /// </summary>
        /// <param name="json">符合xml格式的字符串</param>
        /// <returns>T类型的对象</returns>
        public static object XmlDeserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(System.Text.Encoding.Default.GetBytes(xml.ToCharArray()));
            T obj = (T)serializer.ReadObject(ms);
            ms.Close();

            return obj;
        }
        #endregion


        #region Json序列化

        /// <summary>
        /// Json序列化
        /// </summary>
        public static string ToJson(this object item)
        {
            if (item == null)
            {
                return null;
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());

            using (MemoryStream ms = new MemoryStream())
            {

                serializer.WriteObject(ms, item);

                StringBuilder sb = new StringBuilder();

                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));

                return sb.ToString();

            }

        }
        #endregion

        #region Json反序列化
        /// <summary>
        /// Json反序列化
        /// </summary>
        public static T FromJsonTo<T>(this string jsonString)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T jsonObject = (T)ser.ReadObject(ms);
                return jsonObject;
            }
        }
        #endregion

        #region Json序列化

        ///// <summary>
        ///// Json序列化,解决json日期格式问题
        ///// </summary>
        //public static string ToJson2(this object item)
        //{
        //    //需要使用Json.net    Newtonsoft.Json.dll文件
        //    IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();//这里使用自定义日期格式，默认是ISO8601格式 

        //    timeConverter.DateTimeFormat = "yyyy-MM-dd";//设置时间格式
        //    return JsonConvert.SerializeObject(item, timeConverter);
        //}
        #endregion


    }
}
