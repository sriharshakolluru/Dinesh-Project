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
                //Logger.Write(Message, "Important");
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
    }
}
