using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using FM.TR_FMSystemDLL.BLL;
using System.IO;

namespace WmsMobileSystem.Utilities
{
    public class ReadINIFile
    {
        #region kernel dll imported
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(int Section, string Key,
               string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result,
               int Size, string FileName);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, int Key,
               string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result,
               int Size, string FileName);
        #endregion      

       


        public static string[] GetSectionNames(string iniPath)
        {
            string Selected = "";
            try
            {
                if (File.Exists(iniPath))
                {
                    for (int maxsize = 500; true; maxsize *= 2)
                    {
                        byte[] bytes = new byte[maxsize];
                        int size = GetPrivateProfileString(0, "", "", bytes, maxsize, iniPath);
                        if (size < maxsize - 2)
                        {
                            Selected = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }

            return Selected.Split(new char[] { '\0' });
        }
        public static string[] GetKeyNames(string iniPath, string section)
        {
            string key = "";
            try
            {
                if (File.Exists(iniPath))
                {
                    for (int maxsize = 500; true; maxsize *= 2)
                    {
                        byte[] bytes = new byte[maxsize];
                        int size = GetPrivateProfileString(section, 0, "", bytes, maxsize, iniPath);
                        if (size < maxsize - 2)
                        {
                            key = Encoding.ASCII.GetString(bytes, 0,
                                                      size - (size > 0 ? 1 : 0));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return key.Split(new char[] { '\0' });
        }
        public static object GetEntryValue(string iniPath, string section, string key)
        {
            StringBuilder result = null;
            try
            {
                if (File.Exists(iniPath))
                {
                    for (int maxsize = 250; true; maxsize *= 2)
                    {
                        result = new StringBuilder(maxsize);
                        int size = GetPrivateProfileString(section, key, "",
                                                           result, maxsize, iniPath);
                        if (size < maxsize - 1)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return result.ToString();
        }
    }
}
