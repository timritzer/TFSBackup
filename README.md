# TFSBackup
Backup application for automatically creating shelvesets in TFS to use as a regular backup mechanism.


Creates shelvesets every 30 Minutes, and if the option is selected makes a Daily shelveset for each day of the week.

Shelvesets are automatically overwritten when the next backup of the same type kicks off.
By default this leaves you with up to a week of rolling daily backups to fall back on if needed.

Restoring from backups can be done by simply unshelving the shelveset in Visual Studio, no additional tools needed.

<b>Note:</b> this utility allows logging of the backups, users and any conflicts/errrors to a DB store.
The functionality is disabled by default to make it easier to run, but can be re-enabled by uncommenting the following line in the Load event:
<pre>    //Uncomment to support Database logging
    DBLogger = new BackupLogger(AppLogger);
</pre>

ToDo:
  <ul>
  <li>Finish porting over features from original source</li>
    <ul>
    	<li>Automatic update functionality from fileshare</li>
    	<li>Watchdog process to make sure backup application stays alive and is not killed.</li>
    </ul>
</ul>
