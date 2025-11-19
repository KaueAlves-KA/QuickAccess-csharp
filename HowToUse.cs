using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickAccess
{
    public partial class HowToUse : Form
    {
        public HowToUse()
        {
            InitializeComponent();
            RoundForm.RoundFormMethod(this, 20);
            this.Icon = Properties.Resources.QuickAccessLogoIco;
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
        private void CloseForm(object sender, EventArgs e) 
        {
            this.Close();
        }
        //métodos para mover a janela (fim)
    }
}
