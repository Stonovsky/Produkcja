using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja
{
    public static  class CastExtension
    {
        /// <summary>
        /// Cast only the same properties of Two different object 
        /// </summary>
        /// <typeparam name="T">type that we cast to</typeparam>
        /// <param name="myobj">given object that we cast from</param>
        /// <returns></returns>
        public static T Cast<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type targetType = typeof(T);
            var target = Activator.CreateInstance(targetType, false);

            var objProp = objectType.GetProperties().Select(s => s.Name);
            var targetProp = targetType.GetProperties().Select(s => s.Name);

            var commonProperties = targetProp.Intersect(objProp).ToList();

            foreach (var prop in commonProperties)
            {
                var value = myobj.GetType().GetProperty(prop).GetValue(myobj);
                targetType.GetProperty(prop).SetValue(target, value, null);
            }

            return (T)target;
        }
    }
}
