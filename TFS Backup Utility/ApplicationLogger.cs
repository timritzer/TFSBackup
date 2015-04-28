using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFS_Backup_Utility
{
    public class ApplicationLogger : IApplicationLogger
    {
        public void LogException(Exception ex, string errorMessage = "")
        {
            try
            {
                string logText = string.Empty;

                logText += errorMessage + Environment.NewLine;

                logText += AddLogLine(ex.Message);
                logText += AddLogLine(ex.Source);
                logText += AddLogLine(ex.StackTrace);

                //If we have an inner exception
                if ((ex.InnerException != null))
                {
                    logText += Environment.NewLine + Environment.NewLine;
                    logText += "Inner exception: " + Environment.NewLine;

                    logText += AddLogLine(ex.InnerException.Message);
                    logText += AddLogLine(ex.InnerException.Source);
                    logText += AddLogLine(ex.InnerException.StackTrace);
                }

                WriteToEventLog(logText, true);
            }
            catch
            {
            }
        }

        private string AddLogLine(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                return Text + Environment.NewLine;
            }
            else
            {
                return string.Empty;
            }
        }

        public void WriteToEventLog(string Entry, bool isError = true)
        {
            try
            {
                EventLog objEventLog = new EventLog();

                string LogName = "Application";
                EventLogEntryType EventType = EventLogEntryType.Information;
                string AppName = "TFS Backup Utility";

                if (isError)
                    EventType = EventLogEntryType.Error;

                //Register the App as an Event Source
                if (!EventLog.SourceExists(AppName))
                {
                    EventLog.CreateEventSource(AppName, LogName);
                }

                objEventLog.Source = AppName;
                objEventLog.WriteEntry(Entry, EventType);
            }
            catch
            {
                //Nothing we can do.
            }
        }
    }
}
