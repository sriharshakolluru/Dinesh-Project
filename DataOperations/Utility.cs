using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.IO;
using System.Security.Cryptography;
namespace DataOperations
{
   public class Utility
    {
        public static void WriteLog(string Message)
        {
            try
            {
                //Logger.Write(Message, "Log");
                StreamWriter writer=new StreamWriter(@"D:\Dinesh Project\Logs\log.txt",true);
                writer.WriteLine(string.Format("Date: {0} {1}{2}",DateTime.Now.ToString(),Environment.NewLine,Message));
                writer.Close();
            }
            catch (Exception ex)
            {
                //File.AppendAllText(@"C:\ProjLogs\Failure.txt", "Failure is writing Log" + ex.ToString());
            }
        }

        public static void WriteLogError(string Message)
        {
            try
            {
                //Logger.Write(Message, "LogError");
                try
                {
                    //Logger.Write(Message, "Important");
                    StreamWriter writer = new StreamWriter(@"D:\Dinesh Project\Logs\logError.txt", true);
                    writer.WriteLine(string.Format("Date: {0} {1}{2}", DateTime.Now.ToString(), Environment.NewLine, Message));
                    writer.Close();
                }
                catch (Exception ex)
                {
                    File.AppendAllText(@"C:\ProjLogs\Failure.txt", "Failure is writing Log" + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(@"C:\ProjLogs\Failure.txt", "Failure is writing Log" + ex.ToString());
            }
        }

        public static string CreateRandomID(string start)
        {
            int count = 4;
            StringBuilder ID = new StringBuilder();
            for (int i=0;i<start.Length&&count-->0;i++)
            {
                ID.Append(start[i]);
            }
            var rng = new Random();
            int first = rng.Next(10);
            int second = rng.Next(10);
            int third = rng.Next(10);
            int fourth = rng.Next(10);

            ID.Append(first).Append(second).Append(third).Append(fourth);
            return ID.ToString();
        }

        public static bool DateInBetween(DateTime targetDate, DateTime start, DateTime end)
        {

            try
            {
                if (targetDate != null && targetDate != default(DateTime))
                {
                    if (start.Equals(default(DateTime)))
                    {
                        return true;
                    }
                    if (start.CompareTo(targetDate) < 0)
                    {
                        if (end.Equals(default(DateTime)))
                            return true;
                        else if (end != default(DateTime) && end.CompareTo(targetDate) > 0)
                            return true;
                        else
                            return false;

                    }

                }
            }
            catch (Exception  ex)
            {

                Utility.WriteLogError("Exception Occurred in verification of Dates" + ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// take any string and encrypt it using MD5 then
        /// return the encrypted data 
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        private string GetMD5HashData(string data)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }

        /// <summary>
        /// encrypt input text using MD5 and compare it with
        /// the stored encrypted text
        /// </summary>
        /// <param name="inputData">input text you will enterd to encrypt it</param>
        /// <param name="storedHashData">the encrypted text
        ///         stored on file or database ... etc</param>
        /// <returns>true or false depending on input validation</returns>
        private bool ValidateMD5HashData(string inputData, string storedHashData)
        {
            //hash input text and save it string variable
            string getHashInputData = GetMD5HashData(inputData);

            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       

    }
}
