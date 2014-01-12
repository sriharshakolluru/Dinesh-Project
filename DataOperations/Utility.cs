using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.IO;
namespace DataOperations
{
   public class Utility
    {
        public static void WriteLog(string Message)
        {
            try
            {
                Logger.Write(Message, "Log");
                StreamWriter writer=new StreamWriter(@"D:\Dinesh Project\Logs\log.txt",true);
                writer.WriteLine(string.Format("Date: {0} {1}{2}",DateTime.Now.ToString(),Environment.NewLine,Message));
                writer.Close();
            }
            catch (Exception ex)
            {

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

                }
            }
            catch (Exception ex)
            {

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
                    if (start.CompareTo(targetDate) < 0)
                    {
                        if (end.CompareTo(targetDate) > 0)
                            return true;
                    }

                }
            }
            catch (Exception  ex)
            {

                Utility.WriteLogError("Exception Occurred in verification of Dates" + ex.ToString());
            }
            return false;
        }

    }
}
