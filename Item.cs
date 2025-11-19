using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Media;

namespace QuickAccess
{
    public class Item : Panel
    {
        public string Nome { get; private set; }
        public string Url { get; private set; }
        public string Tipo { get; private set; }

        private SoundPlayer player;

        public Item(string nome, string url, string tipo)
        {
            Create(nome, tipo);
            this.Nome = nome;
            this.Url = url;
            this.Tipo = tipo;
        }

        //método para iniciar iniciar uma Url especificada
        private void OpenUrl(string url)
        {
            ItemType itemType = GetItemType(url);
            
            if (itemType == ItemType.File || itemType == ItemType.Url ||
                itemType == ItemType.Directory || itemType == ItemType.App)
            {
                try
                {
                    Process.Start(url);
                    PlaySound(Properties.Resources.clickSound);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("The specified item was not found. Check the Url.");
            }
        }
        //método para copiar conteúdo de um Item do tipo Imagem ou Documento
        public void CopyItemContent(string url, string tipo) 
        {
            ItemType itemType = GetItemType(url);

            if (itemType == ItemType.File)
            {
                if (tipo == "Image")
                {
                    try
                    {
                        Bitmap bitmap = new Bitmap(url);
                        Clipboard.SetImage(bitmap);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
                else
                {
                    string text = File.ReadAllText(url);
                    Clipboard.SetText(text);
                }
                PlaySound(Properties.Resources.copySound);
            }
            else
            {
                MessageBox.Show("The specified item was not found. Check the Url.");
            }
        }

        //cria um panel com controles dentro
        private void Create(string nome, string tipo)
        {
            //define o tamanho da janela e cria os controles internos
            this.Width = 280;
            this.Height = 60;
            this.BackColor = Color.Black;
            PictureBox picture = new PictureBox();
            Label label = new Label();

            //define as propriedades do pictureBox
            picture.Width = 60;
            picture.Height = 50;
            switch (tipo) 
            {
                case "App":
                    picture.Image = Properties.Resources.appIcon;
                    break;
                case "Website":
                    picture.Image = Properties.Resources.websiteIcon;
                    break;
                case "Image":
                    picture.Image = Properties.Resources.imageIcon;
                    break;
                case "Document":
                    picture.Image = Properties.Resources.docIcon;
                    break;
                case "Folder":
                    picture.Image = Properties.Resources.folderIcon;
                    break;
            }
            picture.Location = new Point(6, (this.Height - picture.Height) / 2);
            picture.Cursor = Cursors.Hand;
            picture.Click += PictureClick;

            //define as propriedades do label
            label.Text = " - " + nome;
            label.AutoSize = true;
            label.ForeColor = Color.White;
            label.Font = new Font("Microsoft Sans Serif", 10);
            label.Location = new Point(picture.Width, (this.Height - label.Height) / 2);

            //adiciona os dois controles ao Item (panel)
            this.Controls.Add(picture);
            this.Controls.Add(label);
        }

        //método para verificar Url's de sites
        private bool VerifyUrl(string url)
        {
            //expressão regular para verificar o formato do URL
            string pattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return regex.IsMatch(url);
        }
        private bool IsApp(string appName) 
        {
            // Adiciona a extensão .exe se não estiver presente
            if (!appName.ToLower().EndsWith(".exe"))
            {
                appName += ".exe";
            }

            // Tenta iniciar o aplicativo e verifica se não há exceção
            try
            {
                //descarta os recursos corretamente após verificar o app
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = appName;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    return true; 
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //cria uma enumeração de valores possíveis para ItemType
        private enum ItemType
        {
            File,
            Directory,
            Url,
            App,
            NotFound
        }
        //método que verifica o tipo de ItemType que a url representa
        private ItemType GetItemType(string url) 
        {
            if (File.Exists(url))
            {
                return ItemType.File;
            }
            else if (Directory.Exists(url))
            {
                return ItemType.Directory;
            }
            else if (VerifyUrl(url))
            {
                return ItemType.Url;
            }
            else if (IsApp(url))
            {
                return ItemType.App;
            }
            else 
            {
                return ItemType.NotFound;
            }
        }

        //ação de clique sobre o pictureBox
        private void PictureClick(object sender, EventArgs e)
        {
            OpenUrl(Url);
        }

        private void PlaySound(Stream soundStream) 
        {
            player = new SoundPlayer(soundStream);
            player.Play();
        }
    }
}