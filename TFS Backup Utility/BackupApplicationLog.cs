//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TFS_Backup_Utility
{
    using System;
    using System.Collections.Generic;
    
    public partial class BackupApplicationLog
    {
        public string LogText { get; set; }
        public int BackupUserSys { get; set; }
        public int BackupApplicationLogSys { get; set; }
        public System.DateTime LogDateTime { get; set; }
    
        public virtual BackupUser BackupUser { get; set; }
    }
}
