using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace QuickAccess
{

    public partial class MainWindow : Form
    {
        private HiddenForm hiddenForm;
        private SoundPlayer player;

        public MainWindow()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.QuickAccessLogoIco;
            RoundForm.RoundFormMethod(this, 20);

            hiddenForm = new HiddenForm(this);
            hiddenForm.QuickAccessLoad();

            //carrega os Items salvos no Json
            List<Registers> registers = StoreInJson.LoadItems();
            if (registers != null && registers.Count > 0)
            {
                foreach (Registers register in registers)
                {
                    Item newItem = new Item(register.Name, register.Url, register.Type);
                    bool context = register.Type == "Image" || register.Type == "Document";

                    AddItem(newItem, context);
                }
            }
            else
            { 
                //não faz nada ao encontrar a lista vazia
            }
        }       

        //declaração de variáveis
        bool move = false;
        Point initial;

        //métodos para mover a janela (inicio)
        private void FormMouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            initial = new Point(e.X, e.Y);
        }
        private void FormMouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                Point newLocation = PointToScreen(e.Location);
                Location = new Point(newLocation.X - initial.X, newLocation.Y - initial.Y);
            }
        }
        private void FormMouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }
        //métodos para mover a janela (fim)

        //fechar, minimizar janela e usar notifyIcon (inicio)
        private void FormClose(object sender, EventArgs e)
        {
            this.Close();
            hiddenForm.QuickAccessClose();
        }
        private void FormMinimize(object sender, EventArgs e) 
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }
        private void FormNormal(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }
        private void FormSizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.BalloonTipText = "running in background press Ctrl + Shift + A for restore";
                notifyIcon1.ShowBalloonTip(1);
                this.ShowInTaskbar = false;
            }
        }
        //fechar, minimizar janela e usar notifyIcon (fim)

        //chama a janela HowToUse
        private void HowToUse(object sender, EventArgs e)
        {
            HowToUse howToUse = new HowToUse();
            howToUse.Show();
        }

        //chama a janela Creation
        private void Creation(object sender, EventArgs e) 
        {
            Creation creation = new Creation(this);
            player = new SoundPlayer(Properties.Resources.clickSound);

            player.Play();
            creation.Show();
        }

        //método para adicionar Items
        public void AddItem(Item item, bool context) 
        {
            ContextMenuStrip options = new ContextMenuStrip();

            if (context)
            {
                options.Items.Add("Copy content");
                options.Items[0].Click += (sender, e) => CopyItemContent(item);
            }
            options.Items.Add("Delete item");
            options.Font = new Font("Microsoft Sans Serif", 10);
            options.Items[options.Items.Count - 1].Click += (sender, e) => DeleteItem(item);

            item.ContextMenuStrip = options;
            flowLayoutPanel1.Controls.Add(item);
        }

        //método que chama o método Copy da class Item
        private void CopyItemContent(Item item) 
        {
            string url = item.Url;
            string tipo = item.Tipo;
            item.CopyItemContent(url, tipo);
        }

        //método responsável por deletar um Item e seu resgitro
        private void DeleteItem(Item item)
        {
            player = new SoundPlayer(Properties.Resources.deleteSound);
            List<Registers> registers = StoreInJson.LoadItems();
            StoreInJson.DeleteRegister(registers, item.Nome);

            player.Play();
            flowLayoutPanel1.Controls.Remove(item);
        }

        //método para deixa a rolagem do flowLayoutPanel suave
        private void FlowLayoutPanelScroll(object sender, ScrollEventArgs e)
        {
            // Atualize a posição da visualização do FlowLayoutPanel
            flowLayoutPanel1.VerticalScroll.Value = e.NewValue;
            flowLayoutPanel1.PerformLayout();
        }
    }
}