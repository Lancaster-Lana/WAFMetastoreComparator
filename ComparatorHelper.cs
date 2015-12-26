using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace  WAFMetastoreComparator
{
    public enum CompareItemsType { attr = 0, field = 1, form = 2 };
    public enum CustomizationNumber { first = 1, second = 2 };

    public enum ComparatorView { tree = 0, analyze = 1 }
    public enum TableAnalyzingType { attributes, fields, actions, forms, searches}
		public enum FormAnalyzingType { wholeform, attributes, security, fields, actions, linkedforms }

    public delegate void CustomizationComparatorHandler();

    /// <summary>
    /// Class to store pair of values ( of certain type)
    /// </summary>
    public class CompareItem
    {
        object _secondvalue;

        public object Firstvalue { get; set; }
        public object Secondvalue { get; set; }

        public CompareItem(string firstvalue, string secondvalue)
        {
            Firstvalue = firstvalue;
            Secondvalue = secondvalue;
        }

        public CompareItem(object firstvalue, object secondvalue)
        {
            Firstvalue = firstvalue;
            Secondvalue = secondvalue;
        }
    }

    /// <summary>
    /// Class to store pairs of properties differ by value for two
    /// </summary>
    public class DifferPropertiesDictionary
    {
        Dictionary<string, CompareItem> _items;

        public CompareItem this[string propertyName]
        {
            get { return _items[propertyName]; }
            set { _items[propertyName] = value; }
        }

        public bool IsEmpty
        {
            get
            {
                if ((_items != null) || (_items.Count == 0))
                    return true;
                return false;
            }
        }

        public Dictionary<string, CompareItem> Items
        {
            get
            {
                return _items;
            }
        }

        /// <summary>
        /// Differ Properties of two elements are being compared
        /// </summary>
        public List<string> Properties
        {
            get
            {
                List<string> cmpProps = new List<string>();

                IEnumerator ien = _items.Keys.GetEnumerator();
                while (ien.MoveNext())
                {
                    string propname = ien.Current.ToString();
                    cmpProps.Add(propname);
                }
                return cmpProps;
            }
        }

        public DifferPropertiesDictionary()
        {
            _items = new Dictionary<string, CompareItem>();
        }

        public DifferPropertiesDictionary(Dictionary<string, CompareItem> items)
        {
            _items = items;
        }

        /// <summary>
        /// GENERATE List of differ props of first and second objects
        /// </summary>
        public DifferPropertiesDictionary(object cmp1, object cmp2)
        {
            _items = new Dictionary<string, CompareItem>();

            Type itemsType = null;

            if (cmp1 != null) itemsType = cmp1.GetType();
            else if (cmp2 != null) itemsType = cmp2.GetType();

            if (itemsType != null)
            {
                PropertyInfo[] props = itemsType.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    object cmpProp1 = (cmp1 != null) ? prop.GetValue(cmp1, null) : null;
                    object cmpProp2 = (cmp2 != null) ? prop.GetValue(cmp2, null) : null;
                    CompareItem cmpProp = new CompareItem(cmpProp1, cmpProp2);
                    _items.Add(prop.Name, cmpProp);
                }
            }
        }

        public void Add(string propertyName, CompareItem cmpItem)
        {
            Items[propertyName] = cmpItem;
        }

        public void Remove(string propertyName)
        {
            Items.Remove(propertyName);
        }
    }

    public class CompareHelper
    {
        public static void DetectCommonAndOriginalNames(IEnumerable firstIEn, IEnumerable secondIEn,
                                                        out List<string> firstOriginalList, out List<string> secondOriginalList,
                                                        out List<string> commonList)
        {
            List<string> lst1 = new List<string>();
            List<string> lst2 = new List<string>();

            foreach (var item in firstIEn)
            {
                string name = firstIEn.ToString();
                lst1.Add(name);
            }

            foreach (var item in secondIEn)
            {
                string name = secondIEn.ToString();
                lst2.Add(name);
            }

            DetectCommonAndOriginalNames(lst1, lst2, out firstOriginalList, out secondOriginalList, out commonList);
        }

        public static void DetectCommonAndOriginalNames(IEnumerator firstIEn, IEnumerator secondIEn,
                                                        out List<string> firstOriginalList, out List<string> secondOriginalList,
                                                        out List<string> commonList)
        {
            List<string> lst1 = new List<string>();
            List<string> lst2 = new List<string>();

            while (firstIEn.MoveNext())
            {
                string name = firstIEn.Current.ToString();
                lst1.Add(name);
            }

            while (secondIEn.MoveNext())
            {
                string name = secondIEn.Current.ToString();
                lst2.Add(name);
            }
            DetectCommonAndOriginalNames(lst1, lst2, out firstOriginalList, out secondOriginalList, out commonList);
        }

        public static void DetectCommonAndOriginalNames(List<string> firstLst, List<string> secondLst,
            out List<string> firstOriginalLst, out List<string> secondOriginalLst,
            out List<string> commonLst)
        {
            firstOriginalLst = firstLst.FindAll((name) => !secondLst.Contains(name));
            secondOriginalLst = secondLst.FindAll((name) => !firstLst.Contains(name));
            commonLst = firstLst.FindAll(secondLst.Contains);
        }

        public static bool Compare<T>(T data1, T data2, out DifferPropertiesDictionary diffPropsList)
        {
            return GetDifferProperties(data1, data2, typeof(T), out diffPropsList);
        }

        public static bool CompareOptionalProperties(string diffPropName, IList lst1, IList lst2, ref DifferPropertiesDictionary diffList)
        {
            bool isEqual = true;

            // Compare options of first and second lists
            if ((lst1 != null) && (lst2 != null))
            {
                if (lst1.Count == lst2.Count)
                {
                    if (lst1.Count == 0)
                        isEqual = true;
                    else
                    {/*
                        if ((lst1[0] is DisplayName) || (lst1[0] is Option) || (lst1[0] is State) || (lst1[0] is Status)
                                || (lst1[0] is FormEvent) || (lst1[0] is Dependency))
                        {
                            //Compare items of first and second lists
                            foreach (object item in lst1)
                            {
                                if (!lst2.Contains(item))
                                    isEqual = false;
                            }
                        }
                        else// No way to compare items of unknown type
                        {
                            //Implement 
                            isEqual = false;
                        }*/
                    }
                }
            }
            else if ((lst1 == null) && (lst2 == null))
            {
                isEqual = true;
            }
            else
                isEqual = false;

            if (!isEqual)
                diffList.Add(diffPropName, new CompareItem(lst1, lst2));

            return isEqual;
        }

        public static bool GetDifferProperties(object firstElem, object secondElem, Type elemType, out DifferPropertiesDictionary diffList)
        {
            diffList = new DifferPropertiesDictionary();

            PropertyInfo[] props = elemType.GetProperties();

            foreach (PropertyInfo fieldProperty in props)
            {
                string diffPropName = fieldProperty.Name;

                object firstPropValue = null;
                try { firstPropValue = fieldProperty.GetValue(firstElem, null); }
                catch { firstPropValue = null; }
                object secondPropValue = null;
                try { secondPropValue = fieldProperty.GetValue(secondElem, null); }
                catch { secondPropValue = null; }
  
                bool isArray = firstPropValue is ICollection;

                if (!isArray)
                {
                    if (!object.Equals(firstPropValue, secondPropValue))
                        diffList.Add(diffPropName, new CompareItem(firstPropValue, secondPropValue));
                }
                else if (((ICollection)firstPropValue).Count > 0)
                {
                    if (firstPropValue is IDictionary)
                    {
                        //CompareCustomAttrsPropsLists((IDictionary)firstPropValue, (IDictionary)secondPropValue, ref diffList);
                    }
                    else if (firstPropValue is IList)// compound (optional) properties
                    {
                        //2. Compare subitems of  property of the element(attribute|field)                    
                        CompareOptionalProperties(diffPropName, (IList)firstPropValue, (IList)secondPropValue, ref diffList);
                    }
                    else
                    {
                        //TODO:
                    }
                }
            }

            if (diffList.Items.Count > 0)
                return true;

            return false;
        }

        public static string ToString(object obj)
        {
            string convertedObj = string.Empty;
            if ((obj is IList) || (obj is IDictionary))
            {
                IEnumerator ien = (obj is IList) ? ((IList)obj).GetEnumerator() : ((IDictionary)obj).Values.GetEnumerator();
                while (ien.MoveNext())
                    convertedObj += ien.Current + "\r\n";
            }
            else
            {
                if (obj != null)
                    convertedObj = obj.ToString();
            }
            return convertedObj;
        }
    }


    public static class ListExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> iList, string tableName)
        {
            DataTable dataTable = new DataTable(tableName);
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);

                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
