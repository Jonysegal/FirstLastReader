using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstLastLine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool isSentanceEnd(char input)
        {
            if (input == '.' || input == '?' || input == '!')
            {
                return true;
            }
            return false;
        }
        List<paragraph> paragraphs = new List<paragraph>();
        void breakIntoParagraphs(string toBreak)
        {
            StringBuilder currentSentance = new StringBuilder();
            paragraph currentParagraph = new paragraph();
            paragraphs.Add(currentParagraph);
            for (int i = 0; i < toBreak.Length; i++)
            {
                char c = toBreak[i];
                currentSentance.Append(c);
                if (isSentanceEnd(c))
                {
                    currentParagraph.addString(currentSentance.ToString());
                    currentSentance.Clear();
                    for(int j= i+1; j < i + 4; j++) //scan 3 characters following end of sentance for a new line. this indicates new paragraph (also could just be line ended with paragraph but rip) 
                    {
                        if (j < toBreak.Length)
                        {
                            if(toBreak[j] == '\n')
                            {
                                currentParagraph = new paragraph();
                                paragraphs.Add(currentParagraph);
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "PDF files|*.pdf    ", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(ofd.FileName);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            richTextBox1.AppendText(PdfTextExtractor.GetTextFromPage(reader, i));
                        }
                        richTextBox1.AppendText("toas");
                        reader.Close();
                        string res = richTextBox1.Text;
                        breakIntoParagraphs(res);
                        richTextBox1.Clear();
                        for(int i=0; i < paragraphs.Count; i++)
                        {
                            richTextBox1.AppendText(paragraphs[i].mySentences[0]);
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
    class paragraph
    {
        public List<string> mySentences = new List<string>();
        public void addString(string toAdd)
        {
            mySentences.Add(toAdd);
        }

    }


}
