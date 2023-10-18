using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EGCLauncher
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Bridge
    {
        private LoginForm ctxForm;

        public Bridge(LoginForm form)
        {
            ctxForm = form;
        }

        public void LoginGame(string bootUrl, string moveUrl)
        {
            new Thread(() =>
            {
                ctxForm.ProcessLogin(bootUrl);
            }).Start();
        }
    }
}
