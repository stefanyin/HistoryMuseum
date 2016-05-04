using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFLib;
namespace HistoryMuseum.Supporter.Utility
{
   public class Data
    {

       public void SaveIP(string ip)
       {
           try
           {
               IniFile _iniFile = new IniFile(_iniFilePath);
               _ip = ip;
               _iniFile.WriteString("Config", "IP", _ip);
           }
           catch (Exception ex)
           {
               Logger.Exception(ex);
           }
       }

       public string GetIP()
       {
           IniFile _iniFile = new IniFile(_iniFilePath);
           return _iniFile.ReadString("Config", "IP", "127.0.0.1");
       }

       public static Data GetInstance()
       {
           if (_instance == null)
               _instance = new Data();
           return _instance;
       }
       static Data _instance;
       string _ip = "192.168.1.1";
       string _iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
    }
}
