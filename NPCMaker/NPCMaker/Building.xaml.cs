using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace NPCMaker
{
    /// <summary>
    /// Interaction logic for Building.xaml
    /// </summary>
    public partial class Building : Window
    {
        Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
        int tempSelector = 0;
        LinkedList<Structure> Roster;
        String LoadFile;
        int numSaves;
        string imageFile;
        String dialogue;
        Structure temp = new Structure();
        String file;
        String[] lines;
        String Move;
        OpenFileDialog openFileDialog2 = new OpenFileDialog();
        public Building()
        {
            InitializeComponent();
        }
        public Bitmap newBitmap(BitmapImage bm)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bm));
                enc.Save(memoryStream);
                System.Drawing.Bitmap nbm = new System.Drawing.Bitmap(memoryStream);
                return nbm;
            }
        }
        public BitmapImage bitmapImage(Bitmap bm)
        {

            MemoryStream memoryStream = new MemoryStream();
            ((System.Drawing.Bitmap)bm).Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage temp = new BitmapImage();
            temp.BeginInit();
            memoryStream.Seek(0, SeekOrigin.Begin);
            temp.StreamSource = memoryStream;
            temp.EndInit();
            return temp;
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.ShowDialog();
            openFileDialog.DefaultExt = ".png";
            if (File.Exists(openFileDialog.FileName))
            {
                this.imageFile = openFileDialog.FileName;
                BitmapImage bmImage = new BitmapImage();
                bmImage.BeginInit();
                bmImage.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                bmImage.EndInit();
                //Bitmap temp1 = newBitmap(bmImage);

                //  Bitmap resized = new Bitmap(temp1, new System.Drawing.Size(32, 32));
               // BitmapImage bm = bitmapImage(temp1);
                Picture.Source = bmImage;
                temp.setImage(this.imageFile);
            }
        }

        private void Dialogue_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "")
            {
                MessageBox.Show("Please insert a name into name box");
            }
            else
            {
                String characterdialogue = Name.Text + "dialogue";
                this.dialogue = characterdialogue + ".txt";
                File.CreateText(this.dialogue);
                temp.setDialogue(this.dialogue);
                System.Diagnostics.Process.Start(this.dialogue);
                temp.setDialogue(this.dialogue);
            }
        }

        private void Moves_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog2.ShowDialog();
            openFileDialog2.DefaultExt = ".txt";
            //if (File.Exists(openFileDialog.FileName))
            //{
            empty1.Content = System.IO.Path.GetFileNameWithoutExtension(openFileDialog2.FileName);
            temp.setMoves(openFileDialog2.FileName);
        }
        public void save()
        {
            String saveFile = Name.Text + "save" + ".txt";

            File.CreateText(saveFile).Close();
            this.lines = new string[5];
            if (temp.getName() == null || temp.getMoves() == null || temp.getImage() == null || temp.getDialogue() == null)
            {
                MessageBox.Show("Please check name, moves, images and dialogue");
            }
            else
            {
                lines[0] = string.Format(temp.getName());
                
                lines[1] = string.Format(temp.getDialogue());
                lines[2] = string.Format(temp.getImage().ToString());
                lines[3] = string.Format(temp.getMoves().ToString());
            
            }

            String file = temp.getName() + ".txt";
            File.CreateText(file).Close();
            using (StreamWriter sw = File.CreateText(file))
            {
                sw.WriteLine("name = " + temp.getName());
               

            }

            if (numSaves < 1)
            {
                System.IO.File.WriteAllLines(saveFile, lines);
            }
            else
            {
                System.IO.File.AppendAllLines(saveFile, lines);
            }
        }
        private void load()
        {
            openFileDialog.ShowDialog();
            openFileDialog.DefaultExt = ".txt";
            if (File.Exists(openFileDialog.FileName))
            {
                this.LoadFile = openFileDialog.FileName;
                this.numSaves = System.IO.File.ReadLines(this.LoadFile).Count();
                if (this.numSaves == 0)
                {
                    MessageBox.Show("Please create a character");
                }
                else
                {
                    LinkedList<String> SLIST = cutter(LoadFile, 4);
                    int i = SLIST.Count - 1;
                    
                    Roster = new LinkedList<Structure>();
                    do
                    {
                        temp = new Structure();
                        temp.setName(SLIST.ElementAt(i));
                        i--;
                        temp.setDialogue(SLIST.ElementAt(i));
                        i--;
                        temp.setImage(SLIST.ElementAt(i));
                        i--;
                        temp.setMoves(SLIST.ElementAt(i));
                        i--;
                        //MessageBox.Show(i.ToString());
                        Roster.AddLast(temp);
                    } while (i > 0);
                    if(this.tempSelector < Roster.Count - 1)
                    {
                        this.tempSelector++;
                    }
                    else
                    {
                        this.tempSelector = 0;
                    }
                    empty2.Content = Roster.ElementAt(this.tempSelector).getName();
                    empty1.Content = Roster.ElementAt(this.tempSelector).getMoves();
                    BitmapImage bmImage = new BitmapImage();
                    bmImage.BeginInit();
                    bmImage.UriSource = new Uri(Roster.ElementAt(this.tempSelector).getImage(), UriKind.Absolute);
                    bmImage.EndInit();
                    /*Bitmap temp1 = newBitmap(bmImage);
                    Bitmap resized = new Bitmap(temp1, new System.Drawing.Size(32, 32));
                    BitmapImage bm = bitmapImage(resized);*/
                    Picture.Source = bmImage;


                }

                }

        }
        public static LinkedList<String> cutter(string file, int Len)
        {
            //MessageBox.Show(file);
            LinkedList<String> listOfLines = new LinkedList<String>();
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {

                    for (int i = 0; i < Len && !reader.EndOfStream; i++)
                    {
                        listOfLines.AddFirst(reader.ReadLine());
                    }

                }
            }
            return listOfLines;
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            empty2.Content = Name.Text;
            temp.setName(Name.Text);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            load();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            save();
        }
    }
}
