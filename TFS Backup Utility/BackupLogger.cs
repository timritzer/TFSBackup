using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFS_Backup_Utility
{
    public class BackupLogger
    {

        private IApplicationLogger AppLogger { get; set; }
        
        public BackupLogger(IApplicationLogger logger)
        {
            this.AppLogger = logger;
        }

        public void LogBackupFiles(Backup backup, List<string> fileNames)
        {
            try
            {
                using (TFSBackupEntities DB = new TFSBackupEntities())
                {
                    DB.Backups.Attach(backup);
                    //Add all of the files we shelved
                    foreach (var filename in fileNames)
                    {
                        BackupFile tmpBackupFile = new BackupFile();
                        tmpBackupFile.FilePath = filename;
                        backup.BackupFiles.Add(tmpBackupFile);
                    }

                    DB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Unable to access DB!");
            }
        }

        public void LogBackupDetails(Backup backup, string Logtext)
        {
            try
            {
                using (TFSBackupEntities DB = new TFSBackupEntities())
                {
                    DB.Backups.Attach(backup);

                    BackupLog log = new BackupLog();
                    log.LogText = Logtext;
                    backup.BackupLogs.Add(log);

                    DB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Unable to access DB!");
            }
        }

        public void LogBackupResult(Backup backup, bool SuccessfulResult)
        {
            try
            {
                using (TFSBackupEntities DB = new TFSBackupEntities())
                {
                    DB.Backups.Attach(backup);
                    backup.Successful = SuccessfulResult;

                    DB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Unable to access DB!");
            }
        }

        public Backup InitializeUserAndBackup(string backupName)
        {
            Backup currentBackup = default(Backup);

            using (TFSBackupEntities DB = new TFSBackupEntities())
            {
                BackupUser currentBackupUser = InitializeUser();
                DB.BackupUsers.Attach(currentBackupUser);

                //Now we need to make the backup record
                currentBackup = new Backup();
                currentBackup.BackupName = backupName;
                currentBackup.BackupDateTime = System.DateTime.UtcNow;

                //Now add the backup to the users backups
                currentBackupUser.Backups.Add(currentBackup);
                DB.SaveChanges();
            }

            return currentBackup;
        }

        public BackupUser InitializeUser()
        {
            using (TFSBackupEntities DB = new TFSBackupEntities()) {
                BackupUser currentBackupUser = default(BackupUser);
                string currentUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                var matchingBackupUsers = from user in DB.BackupUsers where (user.UserName == currentUserName) & (user.ComputerName == Environment.MachineName) select user;

                //Get or create our backupUser record
                if (matchingBackupUsers.Count() > 0) {
                    currentBackupUser = matchingBackupUsers.First();
                } else {
                    currentBackupUser = new BackupUser();
                    currentBackupUser.ComputerName = Environment.MachineName;
                    currentBackupUser.UserName = currentUserName;

                    DB.BackupUsers.Add(currentBackupUser);
                    DB.SaveChanges();
                }

                return currentBackupUser;
            }
        }

        public void LogApplicationDetails(string LogText)
        {
            try
            {
                using (TFSBackupEntities DB = new TFSBackupEntities())
                {
                    BackupUser currentBackupUser = InitializeUser();
                    DB.BackupUsers.Attach(currentBackupUser);

                    BackupApplicationLog log = new BackupApplicationLog();
                    log.LogText = LogText;
                    log.LogDateTime = System.DateTime.UtcNow;
                    currentBackupUser.BackupApplicationLogs.Add(log);
                    DB.SaveChanges();
                }

            }
            catch (Exception)
            {
            }
        }

    }
}
