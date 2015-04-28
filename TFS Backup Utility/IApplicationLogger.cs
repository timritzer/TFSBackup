using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFS_Backup_Utility
{
    public interface IApplicationLogger
    {
        void LogException(Exception ex, String description);
        void WriteToEventLog(string Entry, bool isError);
    }
}
