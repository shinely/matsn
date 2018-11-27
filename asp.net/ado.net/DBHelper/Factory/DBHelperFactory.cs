using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper.Factory
{
    class DBHelperFactory
    {
        // 配置文件参考以下配置
        //<appSettings>
        //  <add key = "IDBHelper" value="Ruanmou.DB.MySql.MySqlHelper,Ruanmou.DB.MySql"/>
        //</appSettings>

        private static String TypeDll = ConfigurationManager.AppSettings["IDBHelper"];
        private static String DllName = TypeDll.Split(',')[1];
        private static String TypeName = TypeDll.Split(',')[0];

        public static IDBHelper CreateInstace()
        {
            Assembly assembly = Assembly.Load(DllName);
            Type typeDBHelper = assembly.GetType(TypeName);
            IDBHelper dBHelper = Activator.CreateInstance(typeDBHelper) as IDBHelper;
            return dBHelper;            
        }



    }
}
