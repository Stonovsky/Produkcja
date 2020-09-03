using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja
{
    public static class RemoveAllChildObjectsExtension
    {
        public static T RemoveChildObjects<T>(this T obj, IEnumerable<string> childObjectsNotToRemove = null)
        {
            Type type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.Contains("tbl"))
                {
                    if (ShouldRemove(childObjectsNotToRemove, prop))
                        prop.SetValue(obj, null);
                }
            }

            return obj;
        }

        private static bool ShouldRemove(IEnumerable<string> childObjectsNotToRemove, PropertyInfo prop)
        {
            return childObjectsNotToRemove is null
                || childObjectsNotToRemove.Where(s => s.Contains(prop.Name)).Count()==0;
        }
    }
}
