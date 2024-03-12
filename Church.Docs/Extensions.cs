using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Church.Docs
{
    public static class Extensions
    {
        public static string GetFullPath(this Assembly Assembly, string Root, string Folder)
        {
            string FullRoot = Directory.GetParent(Path.GetDirectoryName(Root)).FullName;
            return string.Format("{0}\\{1}\\{2}", FullRoot, Assembly.GetName(true).Name, Folder);
        }

        public static Type GetType(this Assembly Assembly, string FileNameWE, string Folder)
        {
            var FileName = Path.GetFileNameWithoutExtension(FileNameWE);
            string FullClassName = string.Format(@"{0}{1}.{2}", Assembly.GetName().Name, string.IsNullOrEmpty(Folder) ? string.Empty : "." + Folder, FileName);
            return Assembly.GetType(FullClassName);
        }
        public static List<MethodInfo> GetCurrentMethods(this Type Type)
        {
            List<MethodInfo> Methods = new List<MethodInfo>();
            Methods = (from method in Type.GetMethods(
                         BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                       select method).Distinct().ToList();

            //Get DefaultMethods
            List<MethodInfo> lstDefaultMethods = Type.GetMethods().Where(m => m.IsHideBySig && !m.IsSpecialName && !m.IsVirtual && !m.IsGenericMethod && !m.IsSecuritySafeCritical).Distinct().ToList();

            foreach (MethodInfo MI in lstDefaultMethods)
            {
                if (lstDefaultMethods.Where(a => a.Name == MI.Name).Count() == 0)
                {
                    Methods.Add(MI);
                }
            }

            return Methods;
        }

        public static string GetFormattedParameter(this ParameterInfo PInfo)
        {
            string PName = PInfo.ParameterType.Name;
            string CustomName = PName.Contains("`1") ? string.Format("{0} {1}", PName.Replace("`1", ""), PInfo.ParameterType.GetGenericArguments().FirstOrDefault().Name) : PName;

            return string.Format("{0} {1}", CustomName, PInfo.Name);
        }
    }
}
