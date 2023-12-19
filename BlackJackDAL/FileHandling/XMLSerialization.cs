// Author: Hedda Eriksson
// Date: 2023-10-11
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: 
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BlackJackDAL.FileHandling
{
    public partial class FileHandler<T>
    {
        public bool XMLSerialize(T obj, string filePath)
        {
            bool success = true;
            xmlSerializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                try
                {
                    xmlSerializer.Serialize(writer, obj);
                }
                catch (Exception)
                {
                    success = false;
                    throw;
                }
            }
            return success;
        }

        public bool XMLSerializeList(string filePath, List<T> m_list)
        {
            bool ok = true;

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter writer = new StreamWriter(filePath);
            try
            {
                serializer.Serialize(writer, m_list);
            }
            catch (Exception)
            {
                ok = false;
                throw;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }


            return ok;
        }

        public T XMLDeserialize<T>(string filePath)
        {
            T obj = default;
            xmlSerializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filePath))
            {
                try
                {
                    obj = (T)xmlSerializer.Deserialize(reader);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return obj;
        }
    }
}
