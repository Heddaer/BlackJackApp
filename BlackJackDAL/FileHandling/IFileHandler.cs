// Author: Hedda Eriksson
// Date: 2023-10-11
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: Interface. 

namespace BlackJackDAL.FileHandling
{
    public interface IFileHandler <T>
    {
        bool XMLSerialize(T obj, string filePath);
        T XMLDeserialize<T>(string filePath);
        
    }
}
