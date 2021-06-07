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
using System.Drawing;
using System.Text.RegularExpressions;

namespace NPCMaker
{
    /// <summary>
    /// Interaction logic for Maker.xaml
    /// </summary>
    public partial class Maker : Window
    {


        int numSaves;
        string[] lines;


        LinkedList<NPC> Roster;
        NPC temp = new NPC();
        int tempSelector = 0;
        string imageFile;
        String dialogue;
        String Moves;
        String LoadFile;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        public Maker()
        {
            InitializeComponent();

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            String mName = name.Text;
            var checker = new Regex("^[a-zA-Z0-9 ]*$");
            if (name.Text == "" || attack.Text == "" || defense.Text == "" || moves.Text == "")
            {
                MessageBox.Show("please fill in the rest of the data");
            }
            else if (!(checker.IsMatch(mName)))
            {
                MessageBox.Show("No Special Characters");
            }

            else
            {
                if (Checky.IsChecked == true)
                {
                    temp.setIsPlayable("yes");
                    //MessageBox.Show(temp.getIsPlayable());
                }
                else
                {
                    temp.setIsPlayable("no");
                    //MessageBox.Show(temp.getIsPlayable());

                }

                Save.IsEnabled = true;
                mName = name.Text;

                temp.setName(mName);

                displayName.Content = mName;

                if (!(hp.Text.All(char.IsDigit)))
                {
                    MessageBox.Show("Please insert a number for hp");

                }
                else
                {
                    int mHP = int.Parse(hp.Text);
                    temp.setHP(mHP);
                    displayHP.Content = mHP;
                }
                if (!(attack.Text.All(char.IsDigit)))
                {
                    MessageBox.Show("Please make the value of Attack a number");
                }
                else
                {
                    int mAttack = int.Parse(attack.Text);
                    temp.setAttack(mAttack);
                    displayAttack.Content = mAttack;
                }
                if (!(defense.Text.All(char.IsDigit)))
                {
                    MessageBox.Show("Please insert a number for Defense");
                }
                else
                {
                    int mDeffense = int.Parse(defense.Text);
                    temp.setDefense(mDeffense);
                    displaydefense.Content = mDeffense;
                }
                if (!(defense.Text.All(char.IsDigit)))
                {
                    MessageBox.Show("Please insert a number for moves");
                }
                else
                {
                    int mFaction = int.Parse(moves.Text);
                    temp.setFaction(mFaction);
                    displayMoves.Content = mFaction;
                }
                temp.setImage(this.imageFile);
                temp.setDialogue(this.dialogue);
                temp.setMoves(this.Moves);
                temp.setSpeed(int.Parse(Speed.Text));
                empty6.Content = int.Parse(Speed.Text);
            }
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
            /* string p = System.IO.Path.GetFullPath("save.txt");
             this.numSaves = System.IO.File.ReadLines(p).Count();*/
            //MessageBox.Show(numSaves.ToString());
            String saveFile = name.Text + "save" + ".txt";
            //MessageBox.Show(temp.getImage());
            File.CreateText(saveFile).Close();
            this.lines = new string[9];

            if (temp.getName() == null)
            {
                MessageBox.Show("NPCs need a name");
            }
            else
            {
                lines[0] = string.Format(temp.getName());
                lines[1] = string.Format(temp.getHP().ToString());
                lines[2] = string.Format(temp.getDefense().ToString());
                lines[3] = string.Format(temp.getAttack().ToString());
                lines[4] = string.Format(temp.getFaction().ToString());
                //lines[5] = string.Format(temp.getDialogue());
                lines[5] = string.Format(temp.getImage());
                //lines[7] = string.Format(temp.getMoves().ToString());
                lines[6] = string.Format(temp.getIsPlayable());
                lines[7] = string.Format(temp.getSpeed().ToString());
            }

            String file = temp.getName() + ".txt";
            File.CreateText(file).Close();
            using (StreamWriter sw = File.CreateText(file))
            {
                sw.WriteLine("name = " + temp.getName());
                sw.WriteLine("hp = " + temp.getHP().ToString());
                string sprite = temp.getImage().Substring(temp.getImage().LastIndexOf('\\') + 1);
                sw.WriteLine("sprite = " + sprite);
                System.IO.File.Copy(temp.getImage(), sprite, true);
                sw.WriteLine("defense = " + temp.getDefense().ToString());
                sw.WriteLine("attack = " + temp.getAttack().ToString());
                sw.WriteLine("faction = " + temp.getFaction().ToString());
                sw.WriteLine("isPlayable = " + temp.getIsPlayable());
                sw.WriteLine("speed = " + temp.getSpeed());


            }

            if (numSaves < 1)
            {

                System.IO.File.WriteAllLines(saveFile, lines);

            }
            else
            {
                System.IO.File.WriteAllLines(saveFile, lines);
                //System.IO.File.AppendAllLines(saveFile, System.IO.File.ReadAllLines(name + "script.txt"));
            }
        }


        public void load()
        {
            //string p = System.IO.Path.GetFullPath("save.txt");
            openFileDialog.ShowDialog();
            openFileDialog.DefaultExt = ".txt";
            if (File.Exists(openFileDialog.FileName))
            {
                this.LoadFile = openFileDialog.FileName;
                this.numSaves = System.IO.File.ReadLines(this.LoadFile).Count();

                //string p = System.IO.Path.GetFullPath("save.txt");
                //MessageBox.Show(p);

                if (this.numSaves == 0)
                {
                    MessageBox.Show("Please create a character");
                }
                else
                {
                    LinkedList<String> NPCLIST = cutter(LoadFile, 9);
                    int i = NPCLIST.Count - 1;

                    Roster = new LinkedList<NPC>();
                    do
                    {
                        temp = new NPC();
                        temp.setName(NPCLIST.ElementAt(i));
                        i--;
                        temp.setHP(int.Parse(NPCLIST.ElementAt(i)));
                        i--;
                        temp.setDefense(int.Parse(NPCLIST.ElementAt(i)));
                        i--;
                        temp.setAttack(int.Parse(NPCLIST.ElementAt(i)));
                        i--;
                        temp.setFaction(int.Parse(NPCLIST.ElementAt(i)));
                        i--;
                        //temp.setDialogue(NPCLIST.ElementAt(i));
                        //i--;
                        temp.setImage(NPCLIST.ElementAt(i));
                        i--;
                        this.imageFile = temp.getImage();
                        MessageBox.Show(temp.getImage());

                        //temp.setMoves(NPCLIST.ElementAt(i));
                        //i--;
                     
                        temp.setIsPlayable(NPCLIST.ElementAt(i));
                        if (NPCLIST.ElementAt(i) == "yes")
                        {
                            Checky.IsChecked = true;
                        }
                        i--;
                        temp.setSpeed(int.Parse(NPCLIST.ElementAt(i)));
                        i--;

                        Roster.AddLast(temp);


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

                    displayName.Content = Roster.ElementAt(this.tempSelector).getName();
                    name.Text= Roster.ElementAt(this.tempSelector).getName();
                    displayHP.Content = Roster.ElementAt(this.tempSelector).getHP();
                    hp.Text= Roster.ElementAt(this.tempSelector).getHP().ToString();
                    displayAttack.Content = Roster.ElementAt(this.tempSelector).getAttack();
                    attack.Text = Roster.ElementAt(this.tempSelector).getAttack().ToString();
                    displaydefense.Content = Roster.ElementAt(this.tempSelector).getDefense();

                    defense.Text = Roster.ElementAt(this.tempSelector).getDefense().ToString();
                    displayMoves.Content = Roster.ElementAt(this.tempSelector).getFaction().ToString();
                    moves.Text= Roster.ElementAt(this.tempSelector).getFaction().ToString();
                    empty6.Content = Roster.ElementAt(this.tempSelector).getSpeed();
                    Speed.Text = Roster.ElementAt(this.tempSelector).getSpeed().ToString();

                    //displayMoves.Content = Roster.ElementAt(this.tempSelector).getFaction();
                    BitmapImage bmImage = new BitmapImage();
                    bmImage.BeginInit();
                    bmImage.UriSource = new Uri(Roster.ElementAt(this.tempSelector).getImage(), UriKind.Absolute);
                    bmImage.EndInit();
                    /*  Bitmap temp1 = newBitmap(bmImage);
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
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            load();

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
                //Bitmap temp1 = newBitmap(bmImage);

                //Bitmap resized = new Bitmap(temp1, new System.Drawing.Size(32, 32));
                //BitmapImage bm = bitmapImage(temp1);
                Picture.Source = bmImage;
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
                this.dialogue = characterdialogue + ".txt";
                File.CreateText(this.dialogue);
                temp.setDialogue(this.dialogue);
                System.Diagnostics.Process.Start(this.dialogue);
            }



        }

        private void moveGetter_Click(object sender, RoutedEventArgs e)
        {

            if (name.Text == "")
            {
                MessageBox.Show("please insert a name ");
            }
            else
            {
                MoveRoster obj = new MoveRoster(this.name.Text);
                obj.ShowDialog();
                temp.setMoves(obj.getFile());
                this.Moves = temp.getMoves();
            }
            /*openFileDialog.ShowDialog();
            openFileDialog.DefaultExt = ".txt";
            if (File.Exists(openFileDialog.FileName))
            {
                Moves = openFileDialog.FileName;
            }*/
            //MessageBox.Show(temp.getMoves());
        }
    }
}
