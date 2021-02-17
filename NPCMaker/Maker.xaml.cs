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
        string dialogueFile;
        int dAdder = 0;
        int numSaves;
        string[] lines;
        int numWords = 0;
        string loader;
        LinkedList<NPC> Roster;
        NPC temp = new NPC();
        int tempSelector=0;
        string imageFile;
        String dialogue;
        String Moves;
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
            int mFaction = int.Parse(moves.Text);
            temp.setFaction(mFaction);
            displayMoves.Content = mFaction;
            temp.setImage(this.imageFile);
            temp.setDialogue(this.dialogueFile);
            temp.setMoves(this.Moves);
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
        public int getMonFaction()
        {
            return temp.getFaction();
        }
        public String getMonDialogue()
        {
            return temp.getDialogue();
        }
        
        public void save()
        {
            this.numSaves = System.IO.File.ReadLines(@"C:\Users\colme\source\repos\NPCMaker\save.txt").Count();

           
            this.lines = new string[7];
            //StreamWriter sw = new StreamWriter(@"C:\Users\colme\source\repos\NPCMaker\save.txt");
            //sw.WriteLine(temp.getName());
            /* sw.WriteLine(temp.getHP().ToString());
             sw.WriteLine(temp.getDefense().ToString());
             sw.WriteLine(temp.getAttack().ToString());
             sw.WriteLine(temp.getFaction().ToString());
             sw.WriteLine(temp.getImage());
             sw.WriteLine(temp.getDialogue());
             sw.WriteLine(temp.getMoves());*/
            lines[0] = string.Format(temp.getName());
            lines[1] = string.Format(temp.getDefense().ToString());
            lines[2] = string.Format(temp.getAttack().ToString());
            lines[3] = string.Format(temp.getDialogue());
            lines[4] = string.Format(temp.getImage().ToString());
            lines[5] = string.Format(temp.getFaction().ToString());
            lines[6] = string.Format(temp.getMoves().ToString());
            if (numSaves < 1)
            {
                System.IO.File.WriteAllLines(@"C:\Users\colme\source\repos\NPCMaker\save.txt", lines);
            }
            else
            {
                System.IO.File.AppendAllLines(@"C:\Users\colme\source\repos\NPCMaker\save.txt", lines);
            }
        }
       
        
        public void load()
        {
             LinkedList<String> NPCLIST=cutter(@"C:\Users\colme\source\repos\NPCMaker\save.txt", 7);
            int i = NPCLIST.Count-1;
            
            Roster = new LinkedList<NPC>();
            do
            {
                temp = new NPC();
                temp.setName(NPCLIST.ElementAt(i));
                i--;
         
                temp.setDefense(int.Parse(NPCLIST.ElementAt(i)));
                i--;             
                temp.setAttack(int.Parse(NPCLIST.ElementAt(i)));
                i--;
                temp.setDialogue(NPCLIST.ElementAt(i));
                i--;
                temp.setImage(NPCLIST.ElementAt(i));
                i--;
                temp.setFaction(int.Parse(NPCLIST.ElementAt(i)));
                i--;
                temp.setMoves(NPCLIST.ElementAt(i));
                i--;

                
                
                Roster.AddLast(temp);


            } while (i >= 0);
            
            
            if (this.tempSelector < Roster.Count-1)
            {
                this.tempSelector++;
                //MessageBox.Show(tempSelector.ToString());
            }
            else
            {
                this.tempSelector = 0;
            }

            displayName.Content = Roster.ElementAt(this.tempSelector).getName();
            displayHP.Content = Roster.ElementAt(this.tempSelector).getHP();
            displayAttack.Content = Roster.ElementAt(this.tempSelector).getAttack();
            displaydefense.Content = Roster.ElementAt(this.tempSelector).getDefense();

            displayMoves.Content = Roster.ElementAt(this.tempSelector).getFaction();
            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();
            bmImage.UriSource = new Uri(Roster.ElementAt(this.tempSelector).getImage(), UriKind.Absolute);
            bmImage.EndInit();
            Picture.Source = bmImage;
        }
        public static LinkedList<String> cutter(string file, int Len)
        {
            LinkedList<String> listOfLines = new LinkedList<String>();
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    
                    for(int i=0; i<Len && !reader.EndOfStream; i++)
                    {
                        listOfLines.AddFirst(reader.ReadLine());
                    }
                    
                }
            }
            return listOfLines;
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

        private void dialogueGetter_Click(object sender, RoutedEventArgs e)
        {
            //Will create a file for incrementing number of scripts
            if ((File.Exists(@"C:\Users\colme\source\repos\NPCMaker\mydialogue" + dAdder + ".txt")))
            {
                numWords = System.IO.File.ReadLines(@"C:\Users\colme\source\repos\NPCMaker\save.txt").Count();
                numWords++;
                this.dialogue = @"C:\Users\colme\source\repos\NPCMaker\mydialogue" + numWords + ".txt";
                dAdder = numWords;
                File.CreateText(this.dialogue);
                this.dialogueFile = this.dialogue;
            }
            else
            {   
                this.dialogue = @"C:\Users\colme\source\repos\NPCMaker\mydialogue" + numWords + ".txt";
                dAdder = numWords;
                File.CreateText(this.dialogue);
                this.dialogueFile = this.dialogue;
            }
            


        }

        private void moveGetter_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.ShowDialog();
            openFileDialog.DefaultExt = ".txt";
            if (File.Exists(openFileDialog.FileName))
            {
                Moves = openFileDialog.FileName;
            }
        }
    }
}
