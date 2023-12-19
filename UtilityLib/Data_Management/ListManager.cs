// Author: Hedda Eriksson
// Date: 2023-28-09 
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: Listmanager to manage List<T> collections

using System;
using System.Linq;
using System.Collections.Generic;
using UtilityLib.Interfaces;

namespace UtilityLib.Data_Management
{
    [Serializable]
    public class ListManager<T> : IListManager<T>
    {
        private List<T> m_list;
        private object value;

        public ListManager()
        {
            m_list = new List<T>();
        }
        public ListManager(IEnumerable<T> collection)
        {
            m_list = new List<T>(collection);
        }

        public int Count
        {
            get { return m_list.Count; }
        }

        public List<T> List { get => m_list; set => m_list = value; }

        public bool Add(T aType)
        {
            bool ok = true;
            if (aType != null)
            {
                m_list.Add(aType);
            }
            else
            {
                ok = false;
            }
            return ok;
        }


        public bool ChangeAt(T aType, int anIndex)
        {
            m_list[anIndex] = aType;
            return m_list.Contains(aType); //OBS not sure this is correct
        }

        public bool CheckIndex(int index)
        {
            bool ok = false;
            if (index >= 0 && index < Count)
            {
                ok = true;
            }
            return ok;
        }

        public void DeleteAll()
        {
            m_list.Clear();
        }

        public bool DeleteAt(int anIndex)
        {
            bool ok = false;
            int countBefore = Count;
            m_list.RemoveAt(anIndex);

            if (Count < countBefore)
            {
                ok = true;
            }
            return ok;
        }

        public T GetAt(int anIndex)
        {
            if (CheckIndex(anIndex))
            {
                return m_list[anIndex];
            }
            else
            {
                return default;
            }

        }

        public string[] ToStringArray()
        {
            string[] strOut = new string[m_list.Count];
            int i = 0;
            foreach (T t in m_list)
            {
                strOut[i] += t.ToString();
            }
            return strOut;
        }

        public List<string> ToStringList()
        {
            List<string> list = new List<string>();
            foreach (T t in m_list)
            {
                list.Add(t.ToString());
            }
            return list;
        }

        public int GetIndex(T aType)
        {
            return List.IndexOf(aType);
        }
    }
}
