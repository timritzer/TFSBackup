
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace TFS_Backup_Utility
{

    partial class MainForm : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer

        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ConfigureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BackupNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseUntilRebootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.DailyChk = new System.Windows.Forms.CheckBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.HourlyBkupTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.Label2 = new System.Windows.Forms.Label();
            this.TFSServerTxt = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.LastBackupDate = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.ContextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenuStrip = this.ContextMenuStrip1;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "TFS Backup Utility";
            this.TrayIcon.Visible = true;
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigureToolStripMenuItem,
            this.BackupNowToolStripMenuItem,
            this.CheckForUpdatesToolStripMenuItem,
            this.CloseToolStripMenuItem,
            this.CloseUntilRebootToolStripMenuItem});
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(181, 114);
            // 
            // ConfigureToolStripMenuItem
            // 
            this.ConfigureToolStripMenuItem.Name = "ConfigureToolStripMenuItem";
            this.ConfigureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ConfigureToolStripMenuItem.Text = "Configure";
            this.ConfigureToolStripMenuItem.Click += new System.EventHandler(this.ConfigureToolStripMenuItem_Click);
            // 
            // BackupNowToolStripMenuItem
            // 
            this.BackupNowToolStripMenuItem.Name = "BackupNowToolStripMenuItem";
            this.BackupNowToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.BackupNowToolStripMenuItem.Text = "Backup Now";
            this.BackupNowToolStripMenuItem.Click += new System.EventHandler(this.BackupNowToolStripMenuItem_Click);
            // 
            // CheckForUpdatesToolStripMenuItem
            // 
            this.CheckForUpdatesToolStripMenuItem.Name = "CheckForUpdatesToolStripMenuItem";
            this.CheckForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.CheckForUpdatesToolStripMenuItem.Text = "Check For Updates";
            this.CheckForUpdatesToolStripMenuItem.Visible = false;
            this.CheckForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.CheckForUpdatesToolStripMenuItem_Click);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.CloseToolStripMenuItem.Text = "Close";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // CloseUntilRebootToolStripMenuItem
            // 
            this.CloseUntilRebootToolStripMenuItem.Name = "CloseUntilRebootToolStripMenuItem";
            this.CloseUntilRebootToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.CloseUntilRebootToolStripMenuItem.Text = "Close (Until Reboot)";
            this.CloseUntilRebootToolStripMenuItem.Visible = false;
            this.CloseUntilRebootToolStripMenuItem.Click += new System.EventHandler(this.CloseUntilRebootToolStripMenuItem_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveBtn.Location = new System.Drawing.Point(393, 94);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 4;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // DailyChk
            // 
            this.DailyChk.AutoSize = true;
            this.DailyChk.Checked = true;
            this.DailyChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DailyChk.Location = new System.Drawing.Point(15, 62);
            this.DailyChk.Name = "DailyChk";
            this.DailyChk.Size = new System.Drawing.Size(94, 17);
            this.DailyChk.TabIndex = 5;
            this.DailyChk.Text = "Daily Backups";
            this.DailyChk.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(12, 17);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(121, 13);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "Backups to perform:";
            // 
            // HourlyBkupTooltip
            // 
            this.HourlyBkupTooltip.ToolTipTitle = "Hourly backups are required.";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 39);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(89, 13);
            this.Label2.TabIndex = 8;
            this.Label2.Text = "Regular Backups";
            // 
            // TFSServerTxt
            // 
            this.TFSServerTxt.Location = new System.Drawing.Point(79, 97);
            this.TFSServerTxt.Name = "TFSServerTxt";
            this.TFSServerTxt.Size = new System.Drawing.Size(194, 20);
            this.TFSServerTxt.TabIndex = 9;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(12, 100);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(61, 13);
            this.Label3.TabIndex = 11;
            this.Label3.Text = "TFS Server";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(195, 62);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(145, 13);
            this.Label4.TabIndex = 12;
            this.Label4.Text = "Last successful backup:";
            // 
            // LastBackupDate
            // 
            this.LastBackupDate.AutoSize = true;
            this.LastBackupDate.Location = new System.Drawing.Point(346, 62);
            this.LastBackupDate.Name = "LastBackupDate";
            this.LastBackupDate.Size = new System.Drawing.Size(53, 13);
            this.LastBackupDate.TabIndex = 13;
            this.LastBackupDate.Text = "Unknown";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(400, 9);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.VersionLabel.Size = new System.Drawing.Size(45, 13);
            this.VersionLabel.TabIndex = 14;
            this.VersionLabel.Text = "Version:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 122);
            this.ControlBox = false;
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.LastBackupDate);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.TFSServerTxt);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.DailyChk);
            this.Controls.Add(this.SaveBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 165);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 165);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TFS Backup Utility Settings";
            this.TopMost = true;
            this.ContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.NotifyIcon TrayIcon;
        internal System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem ConfigureToolStripMenuItem;
        internal System.Windows.Forms.Button SaveBtn;
        internal System.Windows.Forms.CheckBox DailyChk;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ToolTip HourlyBkupTooltip;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox TFSServerTxt;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CloseUntilRebootToolStripMenuItem;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label LastBackupDate;
        internal System.Windows.Forms.Label VersionLabel;
        internal System.Windows.Forms.ToolStripMenuItem BackupNowToolStripMenuItem;

        internal System.Windows.Forms.ToolStripMenuItem CheckForUpdatesToolStripMenuItem;
    }
}