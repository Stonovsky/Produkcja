using Newtonsoft.Json;
//using Json.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GAT_Produkcja
{
    public static class CopyPropertyExtension
    {
        public static void CopyPropertiesTo<T>(this T source, T destination)
            where T : class
        {
            if (destination is null) return;

            // Iterate the Properties of the destination instance and  
            // populate them from their source counterparts  
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();
            foreach (PropertyInfo destinationPi in destinationProperties.Where(p => p.CanWrite))
            {
                PropertyInfo sourcePi = source.GetType().GetProperty(destinationPi.Name);
                destinationPi.SetValue(destination, sourcePi.GetValue(source, null), null);
            }
        }

        public static TTarget CopyPropertiesTo<TSource, TTarget>(this TSource source)
                where TSource : class
                where TTarget : class, new()
        {
            var destination = new TTarget();

            // Iterate the Properties of the destination instance and  
            // populate them from their source counterparts  
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();
            foreach (PropertyInfo destinationPi in destinationProperties.Where(p => p.CanWrite)
                                                                        .Where(p => !p.Name.ToLower().Contains("tbl")))
            {
                PropertyInfo sourcePi = source.GetType().GetProperty(destinationPi.Name);
                destinationPi.SetValue(destination, sourcePi.GetValue(source, null), null);
            }

            return destination;
        }


        public static void CopyListPropertiesTo<T>(this IEnumerable<T> source, IEnumerable<T> destination)
             where T : class
        {
            var source2 = source.ToList();
            var destinantion2 = destination.ToList();

            if (source2.Count != destinantion2.Count) return;

            for (int i = 0; i < source2.Count; i++)
            {
                source2[i].CopyPropertiesTo(destinantion2[i]);
            }
        }

        public static IEnumerable<T> CopyList<T>(this IEnumerable<T> list)
            where T : class, new()
        {
            var newList = new List<T>();

            foreach (var item in list)
            {
                var newItem = new T();
                item.CopyPropertiesTo(newItem);

                newList.Add(newItem);
            }

            return newList;
        }

    }





}
