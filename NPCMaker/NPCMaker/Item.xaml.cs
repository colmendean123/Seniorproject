using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Item.xaml
    /// </summary>
    public partial class Item : Window
    {
        int tempSelector = 0;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        Slot obj = new Slot();
        String[] lines;
        int numSaves;
        String custom;
        String LoadFile;
        LinkedList<Slot> Roster;
        public Item()
        {
            
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var checker = new Regex("^[a-zA-Z0-9 ]*$");
            String mName = name.Text;
            
            if (!(checker.IsMatch(mName)))
            {
                MessageBox.Show("No Special Characters");
            }
            else
            {
                obj.setName(mName);
                empty.Content = mName;
            }
            
            if (!(slots.Text.All(char.IsDigit)))
            {
                MessageBox.Show("Please insert a number");
            }
            else
            {

                int mSlot = int.Parse(slots.Text);
                obj.setSlots(mSlot);
                empty1.Content = mSlot;
            }


        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            String saveFile = name.Text + "save" + ".txt";

            File.CreateText(saveFile).Close();
            this.lines = new string[4];
            if (obj.getName() == null)
            {
                MessageBox.Show("Please insert a value ");
            }
            else
            {
                lines[0] = string.Format(obj.getName());
                lines[1] = string.Format(obj.getSlots().ToString());
               

            }
            String file = obj.getName() + ".txt";
            File.CreateText(file).Close();
            using (StreamWriter sw = File.CreateText(file))
            {
                
                sw.WriteLine("slot = " + obj.getSlots().ToString());
                sw.WriteLine("name = " + obj.getName());
                
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
        private void Load_Click(object sender, RoutedEventArgs e)
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
                    LinkedList<String> NPCLIST = cutter(LoadFile, 2);
                    int i = NPCLIST.Count - 1;

                    Roster = new LinkedList<Slot>();
                    do
                    {
                        obj = new Slot();
                        obj.setName(NPCLIST.ElementAt(i));
                        i--;
                        obj.setSlots(int.Parse(NPCLIST.ElementAt(i)));
                        //MessageBox.Show(i.ToString());
                        i--;
                        obj.setCustom(NPCLIST.ElementAt(i));
                        i--;
                        Roster.AddLast(obj);
                    } while (i > 0);

                    if (this.tempSelector < Roster.Count - 1)
                    {
                        this.tempSelector++;
                        //MessageBox.Show(tempSelector.ToString());
                    }
                    else
                    {
                        this.tempSelector = 0;

                    }
                    empty.Content= Roster.ElementAt(this.tempSelector).getName();
                    name.Text= Roster.ElementAt(this.tempSelector).getName();
                    slots.Text = Roster.ElementAt(this.tempSelector).getSlots().ToString();
                    empty1.Content= Roster.ElementAt(this.tempSelector).getSlots();
                   
                }
                }
                }

        private void dialogueGetter_Click(object sender, RoutedEventArgs e)
        {
            if (name.Text == "")
            {
                MessageBox.Show("Please insert a name into name box");
            }
            else
            {
                String characterdialogue = name.Text + "script";
                this.custom = characterdialogue + ".txt";
                File.CreateText(this.custom);
                obj.setCustom(this.custom);
                System.Diagnostics.Process.Start(this.custom);
            }
        }
    }
}
