using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LicenseWindowKiller
{
    public class Killer
    {
        bool isKilled { get; set; }

        public Killer()
        {
        }

        public void Kill()
        {
            while (!isKilled)
            {
                var process = Process.GetProcesses();

                if (process.Length > 0)
                {
                    foreach (var proc in process)
                    {
                        if (proc.MainWindowTitle == "DlhSoft Component Licensing Warning")
                        {
                            var handle = proc.CloseMainWindow();
                            isKilled = true;
                            return;
                        }
                    }
                }
                Thread.Sleep(250);
            }
        }
    }
}
