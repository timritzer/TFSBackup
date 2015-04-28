using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFS_Backup_Utility
{
    interface IApplicationLogger
    {
        public void LogException(Exception ex, String description);
        public void WriteToEventLog(string Entry, bool isError);
    }
}
