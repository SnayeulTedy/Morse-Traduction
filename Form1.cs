using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOS
{
    public partial class Form1 : Form
    {
        private Dictionary<char, string> morseAlphabet;
        public Form1()
        {
            InitializeComponent();
            InitialiseMorseAlphabet();
        }

        private void InitialiseMorseAlphabet()
        {
            morseAlphabet = new Dictionary<char, string>()
            {
               {'a', ".-"}, {'b', "-..."}, {'c', "-.-."}, {'d', "-.."}, {'e', "."}, {'f', "..-."},
                {'g', "--."}, {'h', "...."}, {'i', ".."}, {'j', ".---"}, {'k', "-.-"}, {'l', ".-.."},
                {'m', "--"}, {'n', "-."}, {'o', "---"}, {'p', ".--."}, {'q', "--.-"}, {'r', ".-."},
                {'s', "..."}, {'t', "-"}, {'u', "..-"}, {'v', "...-"}, {'w', ".--"}, {'x', "-..-"},
                {'y', "-.--"}, {'z', "--.."}, {'0', "-----"}, {'1', ".----"}, {'2', "..---"},
                {'3', "...--"}, {'4', "....-"}, {'5', "....."}, {'6', "-...."}, {'7', "--..."},
                {'8', "---.."}, {'9', "----."}
            };
        }

        private string TranslationToMorse(string input)
        {
            StringBuilder morseCode = new StringBuilder();
            foreach(char character in input.ToLower())
            {
                if(morseAlphabet.ContainsKey(character))
                {
                    morseCode.Append(morseAlphabet[character] + " ");
                }
                else if(character == ' ')
                {
                    morseCode.Append("/ ");
                }
                else
                {
                    morseCode.Append(character + " ");
                }
            }
            return morseCode.ToString();
        }

        private void PlayMorseAudio(string morseCode)
        {
            System.Threading.Thread audioThread = new System.Threading.Thread(() =>
            {
                foreach (char symbol in morseCode)
                {
                    if (symbol == '.')
                    {
                        PlaySound("dot.wav");
                    }
                    else if (symbol == '_')
                    {
                        PlaySound("dash.wav");
                    }
                    else if (symbol == ' ')
                    {
                        System.Threading.Thread.Sleep(100); // la pause entre les characteres
                    }
                }
            });
            audioThread.Start();
        }

        private void PlaySound(string soundFile)
        {
            try
            {
                string filePath = Application.StartupPath + "\\" + soundFile;
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(filePath);
                player.Play();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur lors de la lecture du son :" + ex.Message);
            }
        }

        private void DisplayMorseVisual(string morseCode)
        {
            Bitmap image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.White);
                int x = 10;
                int y = 20;
                int symbolWidth = 20;

                foreach(char symbol in morseCode)
                {
                    if(symbol == '.')
                    {
                        g.FillEllipse(Brushes.Black, x, y, symbolWidth, symbolWidth);
                    }
                    else if(symbol == '_')
                    {
                        g.FillEllipse(Brushes.Black, x, y, symbolWidth = 3, symbolWidth);
                    }

                    x += symbolWidth * 4;
                }
            }

            pictureBox1.Invoke((MethodInvoker)delegate
            {
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = image;
            });
        }




        private void btnTranslate_Click(object sender, EventArgs e)
        {
            string userInput = txtInput.Text;
            string morseTranslation = TranslationToMorse(userInput);
            txtOutput.Text = morseTranslation;

            PlayMorseAudio(morseTranslation);

            DisplayMorseVisual(morseTranslation);
        }
    }
}
