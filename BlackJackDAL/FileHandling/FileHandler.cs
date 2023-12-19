// Author: Hedda Eriksson
// Date: 2023-10-11
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: Handle logic that concerns file handeling.

using System;
using System.Xml.Serialization;

namespace BlackJackDAL.FileHandling
{
    public partial class FileHandler<T> : IFileHandler<T>
    {
        static XmlSerializer? xmlSerializer;
    }
}
