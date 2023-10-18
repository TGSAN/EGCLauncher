namespace EGCLauncher
{
    partial class LoginForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.loginWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.loginWebView)).BeginInit();
            this.SuspendLayout();
            // 
            // loginWebView
            // 
            this.loginWebView.AllowExternalDrop = true;
            this.loginWebView.CreationProperties = null;
            this.loginWebView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.loginWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loginWebView.Location = new System.Drawing.Point(0, 0);
            this.loginWebView.Name = "loginWebView";
            this.loginWebView.Size = new System.Drawing.Size(800, 450);
            this.loginWebView.TabIndex = 0;
            this.loginWebView.ZoomFactor = 1D;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.loginWebView);
            this.Name = "Form1";
            this.Text = "SOUND VOLTEX EXCEED GEAR";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.loginWebView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private Microsoft.Web.WebView2.WinForms.WebView2 loginWebView;
    }
}

