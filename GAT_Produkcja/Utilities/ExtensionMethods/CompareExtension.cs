using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja
{
    public static class CompareExtension
    {
        /// <summary>
        /// Method to compare objects. 
        /// Returns true when objects are equal.
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <returns></returns>
        public static bool Compare(this object object1, object object2)
        {
            //Get the type of the object
            Type type = object1.GetType();

            //Loop through each properties inside class and get values for the property from both the objects and compare
            var listOfProperties = type.GetProperties();
            foreach (var property in type.GetProperties())
            {
                if (property.Name != "ExtensionData"
                    && property.Name != "Item")
                {
                    if (property.Name == "IDZlecenieProdukcyjneMieszanka")
                    {

                    }
                    string Object1Value = "";
                    string Object2Value = string.Empty;
                    if (type.GetProperty(property.Name).GetValue(object1, null) != null)
                        Object1Value = type.GetProperty(property.Name).GetValue(object1, null).ToString();
                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                        Object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    if (Object1Value.Trim() != Object2Value.Trim())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Method to compare lists of the same type. 
        /// Returns true when list are equal comparing each item of list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool CompareWithList<T>(this IList<T> list1, IList<T> list2)
            where T: class
        {
            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i].Compare(list2[i]) == false)
                {
                    return false;
                };
            }
            return true;
        }


        #region Unused
        [Obsolete("Metoda Compare wykorzystywana jest do porownania", true)]
        private static bool CompareEx(this object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            //properties: int, double, DateTime, etc, not class
            if (!obj.GetType().IsClass) return obj.Equals(another);

            var result = true;
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.Name != "Item")
                {
                    var objValue = property.GetValue(obj);
                    var anotherValue = property.GetValue(another);
                    //Recursion
                    if (!objValue.Compare(anotherValue)) result = false;
                }
            }
            return result;
        }

        [Obsolete("Metoda Compare wykorzystywana jest do porownania", true)]
        private static bool DeepCompare(this object objectOne, object objectTwo)
        {

            if (ReferenceEquals(objectOne, objectTwo))
                return true;

            if ((objectOne == null) || (objectTwo == null))
                return false;

            //Compare two object's class, return false if they are difference
            if (objectOne.GetType() != objectTwo.GetType())
                return false;

            var result = true;
            //Get all properties of obj
            //And compare each other
            var listOfPropertiesToCompareObjectOne = objectOne.GetType().GetProperties();
            var listOfPropertiesToCompareObjectTwo = objectOne.GetType().GetProperties();

            var listDifferences = listOfPropertiesToCompareObjectOne.Except(listOfPropertiesToCompareObjectTwo);

            foreach (var property in listOfPropertiesToCompareObjectTwo)
            {
                if (property.Name != "Item")
                {
                    var objValue = property.GetValue(objectOne);
                    var anotherValue = property.GetValue(objectTwo);
                    if (objValue != null)
                    {
                        if (!objValue.Equals(anotherValue))
                            result = false;
                    }
                }
            }
            return result;
        }

        [Obsolete("Metoda Compare wykorzystywana jest do porownania", true)]
        private static bool JsonCompare(this object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            var objJson = JsonConvert.SerializeObject(obj);
            var anotherJson = JsonConvert.SerializeObject(another);

            return objJson == anotherJson;
        }

        #endregion
    }

}