using EGCLauncher.Properties;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EGCLauncher
{
    public struct WebView2RuntimeInfo 
    {
        public string Version;
        public string Path;
    }

    public partial class LoginForm : Form
    {
        public string webview2StartupArgs = "";
        WebView2RuntimeInfo? webview2RuntimeInfo = null;
        private CoreWebView2Environment coreWebView2Environment;

        public LoginForm(string[] args)
        {
            string[] runtimePaths = {
                // Fixed Version 固定版本
                AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "runtime",
                // Evergreen 长青版
                null
            };
            
            foreach (string runtimePath in runtimePaths)
            {
                webview2RuntimeInfo = readRuntime(runtimePath);
                if (webview2RuntimeInfo != null)
                {
                    break;
                }
            }

            if (webview2RuntimeInfo != null)
            {
#if DEBUG
                var availableBrowserVersionString = CoreWebView2Environment.GetAvailableBrowserVersionString();
                MessageBox.Show("当前 WebView2 Runtime:\n" + (webview2RuntimeInfo.Value.Path == null ? "Evergreen Runtime" : "Fixed Version Runtime: " + webview2RuntimeInfo.Value.Path) + "\nVersion: " + availableBrowserVersionString, "SOUND VOLTEX EXCEED GEAR 启动器");
#endif
            }
            else
            {
                MessageBox.Show("缺少 WebView2 Runtime，无法运行。\n可以通过以下任意一种方式安装：\n\n1. 安装任意非稳定通道 Microsoft Edge (Chromium) 浏览器。\n2. 安装 WebView2 Runtime Evergreen 版本。\n3. 将 WebView2 Runtime Fixed Version 版本放入「SOUND VOLTEX EXCEED GEAR 启动器」的 Runtime 文件夹下。", "SOUND VOLTEX EXCEED GEAR 启动器");
                Close();
                Application.Exit();
                return;
            }

            StringBuilder webview2StartupArgsBuilder = new StringBuilder();

            foreach (var arg in args)
            {
                switch (arg)
                {
                    //case "--allow-endscreen":
                    //    {
                    //        allowEndscreen = true;
                    //    }
                    //    break;
                    default:
                        {
                            webview2StartupArgsBuilder.Append(arg + " ");
                        }
                        break;
                }

            }

            webview2StartupArgs = webview2StartupArgsBuilder.ToString();
            
            // Create CoreWebView2Environment

            var customSchemes = new List<CoreWebView2CustomSchemeRegistration>
            {
                new CoreWebView2CustomSchemeRegistration("konaste.sdvx")
            };
            var userDataDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "User Data";
            var ua = "CloudMoeWebBrowser/1.0 (DISKTOP; Windows NT " + Environment.OSVersion.Version.ToString() + "; Wired) Edge/" + webview2RuntimeInfo.Value.Version + " (unlike Gecko)";
            var options = new CoreWebView2EnvironmentOptions(webview2StartupArgs + "--allow-failed-policy-fetch-for-test --allow-running-insecure-content --disable-web-security --user-agent=\"" + ua + "\"", null, null, false, customSchemes);
            coreWebView2Environment = CoreWebView2Environment.CreateAsync(webview2RuntimeInfo.Value.Path, userDataDir, options).Result;

            InitializeComponent();
        }

        private WebView2RuntimeInfo? readRuntime(string path)
        {
            try
            {
                var availableBrowserVersionString = CoreWebView2Environment.GetAvailableBrowserVersionString(path);
                if (availableBrowserVersionString != null)
                {
                    WebView2RuntimeInfo info = new WebView2RuntimeInfo()
                    {
                        Version = availableBrowserVersionString,
                        Path = path
                    };
                    return info;
                }
            }
            catch { }
            return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeWebView2Async();
        }

        private async void InitializeWebView2Async()
        {
            await loginWebView.EnsureCoreWebView2Async(coreWebView2Environment);
            await NativeBridgeRegister(loginWebView);
            loginWebView.CoreWebView2.Navigate("http://eagate.573.jp/game/konasteapp/API/login/login.html?game_id=sdvx");
            loginWebView.CoreWebView2.AddWebResourceRequestedFilter("konaste.sdvx:*", CoreWebView2WebResourceContext.All);
            //loginWebView.CoreWebView2.AddWebResourceRequestedFilter("*/boot.html", CoreWebView2WebResourceContext.All);
            loginWebView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
        }

        private async Task NativeBridgeRegister(WebView2 webView2)
        {
            webView2.CoreWebView2.AddHostObjectToScript("NativeBridge", new Bridge(this));
            // 简化 NativeBridge
            await webView2.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.NativeBridge = window?.chrome?.webview?.hostObjects?.NativeBridge;");
            // 替换 gamBootAndHref
            await webView2.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.addEventListener(\"load\", function () { window.gamBootAndHref = window?.NativeBridge?.LoginGame });");
        }

        public void ProcessLogin(string url)
        {
            Console.WriteLine("拦截：" + url);
            var pattern = @"tk\=(.+?)(?=&|$)";

            var matches = Regex.Matches(url, pattern);
            if (matches.Count > 0)
            {
                var match = matches[0];
                if (match.Groups.Count > 1)
                {
                    var token = match.Groups[1].Value;
                    Console.WriteLine("Token：" + token);
                    this.Invoke(new Action(() =>
                    {
                        new Thread(() =>
                        {
                            Application.Run(new MenuForm(token));
                        }).Start();
                        this.Close();
                    }));
                }
            }
        }

        private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            if (e.Request.Uri.StartsWith("konaste.sdvx"))
            {
                ProcessLogin(e.Request.Uri);
            }
            else if (e.Request.Uri.IndexOf("/boot.html") > -1)
            {
                e.Response = loginWebView.CoreWebView2.Environment.CreateWebResourceResponse(null, 204, "No Content", null);
            }
        }
    }
}
