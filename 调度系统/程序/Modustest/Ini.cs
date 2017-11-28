﻿using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Modustest
{
    public class Ini
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static string Filename = string.Empty;

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="Section">配置项目</param>
        /// <param name="Key">配置关键字</param>
        /// <param name="Data">参数内容</param>
        public static void Write(string Section, string Key, string Data)
        {
            try
            {
                WritePrivateProfileString(Section, Key, Data, System.AppDomain.CurrentDomain.BaseDirectory + Filename);
            }
            catch
            {
                FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + Filename, FileMode.Create);
                fs.Close();
                WritePrivateProfileString(Section, Key, Data, System.AppDomain.CurrentDomain.BaseDirectory + Filename);
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="Section"></param>
        public static void delete(string Section)
        {
            Write(Section,null,null);
        }



        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="Section">配置项目</param>
        /// <param name="Key">配置关键字</param>
        /// <returns>返回读取内容</returns>
        public static string Read(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "无法读取！", temp, 255, System.AppDomain.CurrentDomain.BaseDirectory + Filename);
            return temp.ToString();
        }
    }
}
