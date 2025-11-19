using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace QuickAccess
{
    public partial class Creation : Form
    {
        private MainWindow main;
        private SoundPlayer player;

        public Creation(MainWindow main)
        {
            InitializeComponent();
            RoundForm.RoundFormMethod(this, 20);
            this.Icon = Properties.Resources.QuickAccessLogoIco;
            this.main = main;
        }

        //declaração de variáveis
        bool move = false;
        Point initial;

        //métodos para mover e fechar janela (inicio)
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
        private void FormClose(object sender, EventArgs e)
        {
            this.Close();
        }
        //métodos para mover e fechar janela (fim)

        //método para identificar o tipo de item do evento drag
        private void TextBoxDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; // Permite a cópia do arquivo
            }
            else
            { 
                e.Effect = DragDropEffects.None; // Não permite a cópia se não for um arquivo
            }
        }

        //método que identifica que um evento drag aconteceu
        private void TextBoxDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Verifica se foi arrastado apenas um arquivo
            if (files.Length == 1)
            {
                urlBox.Text = files[0];
            }
            else
            {
                MessageBox.Show("Please drag only one file at a time.");
            }
        }

        //método responsável por garantir a criação do Item e seu registro
        private void CreateItem(object sender, EventArgs e) 
        {
            //garante que não há inputs vazios
            if (nameBox.Text == "" || urlBox.Text == "")
            {
                MessageBox.Show("You did not fill in all the information.");
                return;
            }
            
            if (comboBox1.Text != "App" && comboBox1.Text != "Website"
                && comboBox1.Text != "Document" && comboBox1.Text != "Image"
                && comboBox1.Text != "Folder")
            {
                MessageBox.Show("Check the selected item type.");
                return;
            }

            //verifica se o Tipo do Item é Image ou Document
            bool context = comboBox1.Text == "Image" || comboBox1.Text == "Document";
            
            //cria um novo Item com os valores dos inputs
            Item item = new Item(nameBox.Text, urlBox.Text, comboBox1.Text);
            
            //cria um arquivo de áudio
            player = new SoundPlayer(Properties.Resources.clickSound);

            //registra um novo Item nos Registros e no Json
            List<Registers> registers = StoreInJson.LoadItems();
            Registers newRegister = new Registers
            {
                Name = nameBox.Text,
                Url = urlBox.Text,
                Type = comboBox1.Text
            };
            registers.Add(newRegister);
            StoreInJson.SaveItems(registers);
            main.AddItem(item, context);

            player.Play();
            this.Close();
        }
    }
}
