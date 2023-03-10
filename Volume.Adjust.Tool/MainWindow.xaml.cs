using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;


namespace Volume.Adjust.Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon? _notifyIcon = null;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        const uint WM_APPCOMMAND = 0x319;
        const uint APPCOMMAND_VOLUME_UP = 0x0a;
        const uint APPCOMMAND_VOLUME_DOWN = 0x09;
        const uint APPCOMMAND_VOLUME_MUTE = 0x08;
        public MainWindow()
        {
            InitializeComponent();
            InitNotifyIcon();
            this.KeyDown += keyDownEvent;
            this.MouseWheel += mouseWheelEvent;
            // 设置app初始位置在右下角
            this.Left = System.Windows.SystemParameters.WorkArea.Width - Width;
            this.Top = System.Windows.SystemParameters.WorkArea.Height - Height;
        }

        private void InitNotifyIcon()
        {
            // this.Hide();
            if (this._notifyIcon == null)
            {
                this._notifyIcon = new NotifyIcon()
                {
                    Text = "音量控制工具",
                    Icon = new System.Drawing.Icon("Assets/sound.ico"),
                };
            }
            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("关闭").MouseUp += notifyIcon_Close_MouseUp;
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add("显示").MouseUp += notifyIcon_Show_MouseUp;
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            this._notifyIcon.ContextMenuStrip = contextMenuStrip;
            this._notifyIcon.Visible = true;
            this._notifyIcon.MouseClick += _notifyIcon_MouseClick;
        }

        private void notifyIcon_Show_MouseUp(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show();
        }

        private void notifyIcon_Close_MouseUp(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Close();
        }

        private void _notifyIcon_MouseClick(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.Show();
        }

        // 调节音量
        private void mouseWheelEvent(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                SendMessage(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_UP * 0x10000);
            else
                SendMessage(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_DOWN * 0x10000);
        }
        // esc 退出
        private void keyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        // 拖拽图片
        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
