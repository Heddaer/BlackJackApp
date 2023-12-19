// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: Log messages to file.
using System;
using System.IO;
using System.Text.RegularExpressions;
using BlackJackDAL.FileHandling;
using UtilityLib.Data_Management;

namespace BlackJackDAL.Logging
{
    public class FileLogger
    {
        private string? LogFilePath { get; set; }
        private FileHandler<string> fileHandler;
        private ListManager<string> loggers;
        private const string XMLExtension = ".xml";

        public FileLogger(string filePath)
        {

            LogFilePath = GetUniqueFilePath(filePath);
            fileHandler = new FileHandler<string>();
            loggers = new ListManager<string>();
        }
        public void Log(string message)
        {
            bool gameOver = Regex.IsMatch(message, @"\b" + "GameOver" + @"\b", RegexOptions.IgnoreCase); // Check if game over by searching the word in the message
            DateTime now = DateTime.Now; // Add timestamp to every logged action
            string timeNow = now.ToString();
            message += "    " + timeNow;

            loggers.Add(message);

            if (gameOver)
            {
                LogToFile();
            }

        }

        /// <summary>
        /// Calls on class fileHandler to process log into a XML file.
        /// </summary>
        private void LogToFile()
        {
            if (fileHandler != null && LogFilePath != null)
            {
                fileHandler.XMLSerializeList(LogFilePath, loggers.List);

            }
        }

        /// <summary>
        /// Change filePath to another FilePath if current FilePath exist.
        /// </summary>
        /// <param name="filePath">A string</param>
        /// <returns>A string with new filePath.</returns>
        public string GetUniqueFilePath(string filePath)
        {
            int number = 0;
            string baseFilePath = filePath;

            while (File.Exists(filePath))
            {
                number++;
                string tempPath = Path.ChangeExtension(baseFilePath, null);
                tempPath +=  (number + XMLExtension);
                filePath = tempPath;
            }
            return filePath;
        }

    }
}
