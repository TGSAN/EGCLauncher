using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EGCLauncher
{
    public partial class MenuForm : Form
    {
        public string token;
        public MenuForm(string inputToken)
        {
            token = inputToken;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Exec(string path, string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = path;
            startInfo.Arguments = args;
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = false;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }

        private void TryExecWithToken(string path, string appendArgs = "")
        {
            try
            {
                Exec(path, ("-t " + token + " " + appendArgs).Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../launcher/modules/updater.exe";
            TryExecWithToken(path);
        }

        private void checkGameButton_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../launcher/modules/updater.exe";
            TryExecWithToken(path, "-c");
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../launcher/modules/settings.exe";
            TryExecWithToken(path);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../game/modules/sv6c.exe";
            if (File.Exists(path))
            {
                TryExecWithToken(path);
                this.Close();
            }
            else
            {
                MessageBox.Show("游戏文件不存在，请检查游戏文件是否完整。", "SOUND VOLTEX EXCEED GEAR 启动器");
            }
        }

        private void launcherButton_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../launcher/modules/launcher.exe";
            try
            {
                Exec(path, ("konaste.sdvx://login/?tk=" + token).Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
