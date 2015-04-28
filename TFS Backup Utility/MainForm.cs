
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.Client;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows;


namespace TFS_Backup_Utility
{

    public partial class MainForm : Form
    {

        private System.Threading.Timer timer;
        private int backupInterval;
        private Task waitForCloseTask = null;
        private EventWaitHandle wh = null;
        private Properties.Settings settings = new Properties.Settings();


        private IApplicationLogger AppLogger { get; private set; }
        private BackupLogger DBLogger { get; private set; }

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern short GetAsyncKeyState(Keys vKey);

        #region "Form Events"

        private void ConfigureToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            UpdateAndShowConfigure();
        }

        private void UpdateAndShowConfigure()
        {
            VersionLabel.Text = "Version " + Application.ProductVersion;
            LastBackupDate.Text = settings.LastBackup.ToString();

            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void SaveBtn_Click(System.Object sender, System.EventArgs e)
        {
            SaveSettings();
            this.Hide();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            InitializeComponent();
            
            //If we don't exit properly, consider it an error.
            Environment.ExitCode = 1;

            AppLogger = new ApplicationLogger();

            //Uncomment to support Database logging
            //DBLogger = new BackupLogger(AppLogger);

            if (settings.BackupInterval == 0)
            {
                settings.BackupInterval = 30;
                //Default
            }

            LoadSettings();
            StartTimer();

            wh = new EventWaitHandle(false, EventResetMode.ManualReset, "CloseTFSBackup");
            //Make sure we aren't set
            wh.Reset();

            waitForCloseTask = Task.Factory.StartNew(() => WaitForCloseSignal());
        }

        private void MainForm_Shown(System.Object sender, System.EventArgs e)
        {
            if (!String.IsNullOrEmpty(settings.TFS_Server))
            {
                this.Hide();
            }
            else
            {
                UpdateAndShowConfigure();
            }

        }

        private void CloseToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            DialogResult continueClose =
              MessageBox.Show(
                "Do you wish to close the backup software?" + Environment.NewLine +
                "No backups will run until you restart the backup client.", "Close?", MessageBoxButtons.YesNo);
            if (continueClose == DialogResult.Yes)
            {
                try
                {
                    AppLogger.WriteToEventLog("TFS Backup manually closed by user.", false);
                    if (DBLogger != null) DBLogger.LogApplicationDetails("TFS Backup manually closed by user.");
                }
                catch
                {
                    //Can't do anything, we are exiting
                }

                //Successful exit
                Environment.ExitCode = 0;
                this.Close();
            }
        }

        private void CloseUntilRebootToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            DialogResult continueClose =
              MessageBox.Show(
                "Do you wish to close the backup software?" + Environment.NewLine +
                "The software will not be restarted automatically and you will be vulnerable to data loss until reboot.",
                "Close?", MessageBoxButtons.YesNo);
            if (continueClose == DialogResult.Yes)
            {
                try
                {
                    AppLogger.WriteToEventLog("TFS Backup manually closed until reboot by user.", false);
                    if (DBLogger != null) DBLogger.LogApplicationDetails("TFS Backup manually closed until reboot by user.");
                }
                catch
                {
                    //Can't do anything, we are exiting
                }

                //Successful exit, no restart
                Environment.ExitCode = 2;
                this.Close();
            }
        }

        private void BackupNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DBLogger != null) DBLogger.LogApplicationDetails("TFS Backup manually requested by user.");

            CreateBackup("Manual_Backup");
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowBaloon("Please wait while we check for updates and restart.");
            if (DBLogger != null) DBLogger.LogApplicationDetails("Update manually requested by user.");

            //Request update and restart
            Environment.ExitCode = 3;
            this.Close();
        }

        private void ContextMenuStrip1_Opening(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (Convert.ToBoolean(GetAsyncKeyState(Keys.LControlKey)) && Convert.ToBoolean(GetAsyncKeyState(Keys.LShiftKey)))
            //{
            //    CloseUntilRebootToolStripMenuItem.Visible = true;
            //}
            //else
            //{
            CloseUntilRebootToolStripMenuItem.Visible = false;
            //}
        }

        #endregion

        #region "Settings Methods"

        private void SaveSettings()
        {
            if (settings.PerformDailyBackup != DailyChk.Checked)
            {
                settings.PerformDailyBackup = DailyChk.Checked;
                if (DBLogger != null) DBLogger.LogApplicationDetails("Backup Daily Setting Changed. New Setting: " + DailyChk.Checked);
            }

            if (settings.TFS_Server != TFSServerTxt.Text)
            {
                settings.TFS_Server = TFSServerTxt.Text;
                if (DBLogger != null) DBLogger.LogApplicationDetails("TFS Server Setting Changed. New Setting: " + TFSServerTxt.Text);
            }

            settings.Save();
        }

        private void LoadSettings()
        {
            DailyChk.Checked = settings.PerformDailyBackup;
            TFSServerTxt.Text = settings.TFS_Server;
        }

        #endregion

        #region "Backup Methods"

        private void StartTimer()
        {
            const int msInSecond = 1000;
            const int secondsInMinute = 60;
            backupInterval = settings.BackupInterval * msInSecond * secondsInMinute;
            timer = new System.Threading.Timer(new TimerCallback(TimerCallback), "Latest_Backup", 0, backupInterval);
        }

        public void TimerCallback(object state)
        {
            if (!String.IsNullOrWhiteSpace(settings.TFS_Server) &&
                Uri.IsWellFormedUriString(settings.TFS_Server, UriKind.RelativeOrAbsolute))
            {
                Uri uri = new Uri(settings.TFS_Server);

                if (HostIsReachable(uri.Host))
                {
                    if (settings.LastDailyBackup.Date != System.DateTime.Now.Date && settings.PerformDailyBackup)
                    {
                        if (CreateBackup("Daily_Backup " + System.DateTime.Now.DayOfWeek.ToString().Substring(0, 2)))
                        {
                            //Only update if we were successful. Otherwise retry.
                            MarkSuccessfulBackup(true);
                        }
                    }
                    else if ((settings.LastBackup).AddMinutes(settings.BackupInterval) <= System.DateTime.Now)
                    {
                        //Prevent back to back backups when coming out of sleep.
                        if (CreateBackup("Latest_Backup"))
                        {
                            //Track that we succeeded.
                            MarkSuccessfulBackup();
                        }
                    }
                }
                else
                {
                    AppLogger.WriteToEventLog("Backup skipped due to lack of network connectivity.", false);
                }
            }
            else
            {
                AppLogger.WriteToEventLog("Backup skipped because the TFS server is not configured.", false);
            }
        }

        private void MarkSuccessfulBackup(bool isDaily = false)
        {
            if (isDaily)
            {
                settings.LastDailyBackup = System.DateTime.Now;
            }
            settings.LastBackup = System.DateTime.Now;
            settings.Save();
        }

        public Boolean HostIsReachable(string host)
        {
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply pingReply = ping.Send(host);
                return pingReply.Status == System.Net.NetworkInformation.IPStatus.Success;
            }
            catch (System.Net.NetworkInformation.PingException)
            {
                return false;
            }
        }

        private bool CreateBackup(string ShelvesetName)
        {
            TfsTeamProjectCollection tfs = null;
            VersionControlServer vcs = null;
            string serverName = settings.TFS_Server;

            Backup currentBackup = null;

            bool shelvesetCreated = false;
            try
            {
                if (DBLogger != null) currentBackup = DBLogger.InitializeUserAndBackup(ShelvesetName);

                tfs = new TfsTeamProjectCollection(new Uri(serverName));
                vcs = tfs.GetService<VersionControlServer>();

                Workspace workspace = vcs.GetWorkspace(Environment.MachineName, Environment.UserName);

                PendingSet[] pendingSets = vcs.QueryPendingSets(new string[] { "$/" }, RecursionType.Full, workspace.Name,
                  Environment.UserName);

                if (DBLogger != null) DBLogger.LogBackupDetails(currentBackup, "Starting Backup process. TFS Backup version: " + Application.ProductVersion);
                ShowBaloon("Starting Backup process.", "Backup Status (" + ShelvesetName + ")", ToolTipIcon.Info, 1000);

                //Group all changes into one shelve set
                List<PendingChange> tempChanges = new List<PendingChange>();
                foreach (PendingSet set in pendingSets)
                {
                    tempChanges.AddRange(set.PendingChanges);
                }
                PendingChange[] latestPendingChanges = tempChanges.ToArray();

                if (latestPendingChanges.Length > 0)
                {
                    Shelveset shelveset = new Shelveset(vcs, ShelvesetName, workspace.OwnerName);
                    shelveset.Comment = "Backup Shelveset";
                    workspace.Shelve(shelveset, latestPendingChanges, ShelvingOptions.Replace);

                    List<string> changeList = new List<string>();
                    foreach (PendingChange Change in latestPendingChanges)
                    {
                        changeList.Add(Change.ServerItem);
                    }

                    if (DBLogger != null) DBLogger.LogBackupFiles(currentBackup, changeList);

                    ShowBaloon("Backup Completed Successfully. Shelved " + changeList.Count + " files.",
                      "Backup Status (" + ShelvesetName + ")");

                    if (DBLogger != null) DBLogger.LogBackupDetails(currentBackup, "Backup Completed Successfully.");
                    if (DBLogger != null) DBLogger.LogBackupResult(currentBackup, true);
                }
                else
                {
                    ShowBaloon("No changes to shelve.", "Backup Status (" + ShelvesetName + ")");

                    if (DBLogger != null) DBLogger.LogBackupDetails(currentBackup, "No changes to shelve.");
                    if (DBLogger != null) DBLogger.LogBackupResult(currentBackup, true);
                }

                shelvesetCreated = true;
            }
            catch (Exception ex)
            {
                ShowBaloon("Backup Failed! Please check Event Log for details!", "Backup Status (" + ShelvesetName + ")",
                  ToolTipIcon.Error, 5000);
                AppLogger.LogException(ex, "Error Shelving changes!");
            }
            finally
            {
                if ((tfs != null))
                    tfs.Dispose();
                tfs = null;
                vcs = null;
                currentBackup = null;
            }

            return shelvesetCreated;
        }

        #endregion

        #region "Other Methods"

        private void ShowBaloon(string Message, string Title = "", ToolTipIcon Icon = ToolTipIcon.Info, int Duration = 2000)
        {
            try
            {
                TrayIcon.BalloonTipIcon = Icon;
                TrayIcon.BalloonTipText = Message;
                TrayIcon.BalloonTipTitle = Title;
                TrayIcon.ShowBalloonTip(Duration);
            }
            catch
            {
                //Fail gracefully
            }
        }

        private void WaitForCloseSignal()
        {
            wh.WaitOne();
            wh.Reset();

            try
            {
                AppLogger.WriteToEventLog("TFS Backup closed for update.", false);
                if (DBLogger != null) DBLogger.LogApplicationDetails("TFS Backup closed for update.");
            }
            catch
            {
                //Can't do anything, we are exiting
            }

            Environment.ExitCode = 0;
            this.Close();
        }

        public MainForm()
        {
            Shown += MainForm_Shown;
            Load += MainForm_Load;
        }

        #endregion

    }
}