using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickAccess
{
    public class HiddenForm : Form
    {
        //importando suporte para atalhos globais (inicio)
        private const int WM_HOTKEY = 0x0312;

        private Form formToRestore;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        //importando suporte para atalhos globais (fim)

        //construtor do Form
        public HiddenForm(Form form)
        {
            formToRestore = form;
        }

        //ouvir atalhos globais
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY)
            {
                formToRestore.Show();
                formToRestore.WindowState = FormWindowState.Normal;
                formToRestore.ShowInTaskbar = true;
            }
        }

        //registrar atalhos globais
        public void QuickAccessLoad()
        {
            RegisterHotKey(this.Handle, 1, 2 | 4, (int)Keys.A);
        }

        //remover atalhos globais
        public void QuickAccessClose()
        {
            UnregisterHotKey(this.Handle, 1);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // HiddenForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 254);
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "HiddenForm";
            this.ResumeLayout(false);

        }
    }
}
