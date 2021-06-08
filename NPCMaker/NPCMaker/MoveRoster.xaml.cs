using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MoveRoster.xaml
    /// </summary>
    public partial class MoveRoster : Window
    {
        String file;
        String Move;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        public MoveRoster(String move)
        {
            InitializeComponent();
            this.Move = move;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.ShowDialog();
            openFileDialog.DefaultExt = ".txt";
            //if (File.Exists(openFileDialog.FileName))
            //{
            empty1.Content = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            //}
            /*string p = System.IO.Path.GetFullPath("moves.txt");
            String[] lines;

            lines=System.IO.File.ReadAllLines(p);
            //MessageBox.Show(lines[1]);
            if (selector1 < lines.Length)
            {
                empty1.Content = lines[selector1];
                selector1++;
            }
            else
            {
                selector1 = 0;
                empty1.Content = lines[selector1];

            }*/

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

            openFileDialog.ShowDialog();
            openFileDialog.DefaultExt = ".txt";
            //if (File.Exists(openFileDialog.FileName))
            //{
            empty2.Content = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            /* string p = System.IO.Path.GetFullPath("moves.txt");
             String[] lines;

             lines = System.IO.File.ReadAllLines(p);
             //MessageBox.Show(lines[1]);
             if (selector2 < lines.Length)
             {
                 empty2.Content = lines[selector2];
                 selector2++;
             }
             else
             {
                 selector2 = 0;
                 empty2.Content = lines[selector2];

             }*/
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

            openFileDialog.ShowDialog();
            openFileDialog.DefaultExt = ".txt";
            //if (File.Exists(openFileDialog.FileName))
            //{
            empty3.Content = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            /*string p = System.IO.Path.GetFullPath("moves.txt");
            String[] lines;

            lines = System.IO.File.ReadAllLines(p);
            //MessageBox.Show(lines[1]);
            if (selector3 < lines.Length)
            {
                empty3.Content = lines[selector3];
                selector3++;
            }
            else
            {
                selector3 = 0;
                empty3.Content = lines[selector3];

            }*/
        }
        public String getFile()
        {
            return this.file;

        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
             file = this.Move+"moveSet" + ".txt";
            File.CreateText(file).Close();
            using (StreamWriter sw = File.CreateText(file))
            {
                sw.WriteLine("move=[" + empty1.Content + "," + empty2.Content + "," + empty3.Content + "]");
                MessageBox.Show("Move file created");

            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
