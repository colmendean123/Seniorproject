using System;
using System.Collections.Generic;
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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.Win32;

namespace NPCMaker
{
    /// <summary>
    /// Interaction logic for Maker.xaml
    /// </summary>
    public partial class Maker : Window
    {
        int numSaves;
        string[] lines;
        string loader;
        LinkedList<NPC> Roster;
        NPC temp = new NPC();
        int tempSelector=0;
        string imageFile;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        public Maker()
        {
            InitializeComponent();
            
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            
            String mName = name.Text;
            temp.setName(mName);
            displayName.Content = mName;
            int mHP = int.Parse(hp.Text);
            temp.setHP(mHP);
            displayHP.Content = mHP;
            int mAttack = int.Parse(attack.Text);
            temp.setAttack(mAttack);
            displayAttack.Content = mAttack;
            int mDeffense = int.Parse(defense.Text);
            temp.setDefense(mDeffense);
            displaydefense.Content = mDeffense;
            int mMove = int.Parse(moves.Text);
            temp.setMove(mMove);
            displayMoves.Content = mMove;
            temp.setImage(this.imageFile);

        }
        public int getMonAttack()
        {
            return temp.getAttack();
        }
        public int getMonDefense()
        {
            return temp.getDefense();
        }
        public int getMonHP()
        {
            return temp.getHP();
        }
        public String getMonName()
        {
            return temp.getName();
        }
        public int getMonMoves()
        {
            return temp.getMove();
        }
        public string serializeO(object O)
        {
            if (!O.GetType().IsSerializable)
            {
                return null;
            }
            using(System.IO.MemoryStream stream=new System.IO.MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, temp);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
        public void save()
        {

            this.numSaves = System.IO.File.ReadLines(@"C:\Users\colme\source\repos\NPCMaker\save.txt").Count();
            
            numSaves++;
            
            MessageBox.Show(numSaves.ToString());
            this.lines = new string[numSaves];

            this.loader = serializeO(temp);
            MessageBox.Show(this.loader);
            for (int i = 0; i < this.numSaves; i++)
            {
                this.lines[i] = string.Format(loader);
            }
            if (numSaves <= 1)
            {
                System.IO.File.WriteAllLines(@"C:\Users\colme\source\repos\NPCMaker\save.txt", lines);
            }
            else
            {
                
               
                    System.IO.File.AppendAllText(@"C:\Users\colme\source\repos\NPCMaker\save.txt", this.lines[numSaves-1]+Environment.NewLine);
                    
                
            }
        }
        public object Deserialization(int index)
        {
            
            this.lines = File.ReadAllLines(@"C:\Users\colme\source\repos\NPCMaker\save.txt");
            /*for (int i = 0; i < this.lines.Length;i++) {
                MessageBox.Show(this.lines[i]);
            }*/
            this.loader = this.lines[index-1]; 
            byte[] bytes = Convert.FromBase64String(this.loader);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new BinaryFormatter().Deserialize(stream);
            }

        }
        
        public void load()
        {
            this.numSaves = System.IO.File.ReadLines(@"C:\Users\colme\source\repos\NPCMaker\save.txt").Count();
            if (this.tempSelector < numSaves-1)
            {
                this.tempSelector++;
                //MessageBox.Show(tempSelector.ToString());
            }
            else
            {
                this.tempSelector = 0;
            }
            int i = 0;
            this.Roster = new LinkedList<NPC>();
            do
            {
                i++;
                temp = (NPC)Deserialization(i);
                //MessageBox.Show(temp.getName());
                Roster.AddLast(temp);
               
            } while (i<numSaves);
            
            
            displayName.Content = Roster.ElementAt(this.tempSelector).getName();
            displayHP.Content = Roster.ElementAt(this.tempSelector).getHP();
            displayAttack.Content = Roster.ElementAt(this.tempSelector).getAttack();
            displaydefense.Content = Roster.ElementAt(this.tempSelector).getDefense();
            displayMoves.Content = Roster.ElementAt(this.tempSelector).getMove();
            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();
            bmImage.UriSource = new Uri(Roster.ElementAt(this.tempSelector).getImage(), UriKind.Absolute);
            bmImage.EndInit();
            Picture.Source = bmImage;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            load();
            
        }

        private void imageGetter_Click(object sender, RoutedEventArgs e)
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
                Picture.Source = bmImage;
            }
        }
    }
}
