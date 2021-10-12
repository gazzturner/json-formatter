using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Formatter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var inputText = txtInput.Text;
            if (!ValidJson(inputText))
            {
                return;
            }

            if(inputText.StartsWith("["))
            {
                inputText = inputText.Remove(0, 1);
            }

            if (inputText.EndsWith("]"))
            {
                inputText = inputText.Remove(inputText.Length-1, 1);
            }
           
            var pattern = @"\},";
            var data = Regex.Split(inputText, pattern);

            var outputText = "";
            for (var i = 0; i < data.Length; i++)
            {
                var modifiedRecord = "";
                modifiedRecord += data[i];
                modifiedRecord = modifiedRecord.Replace(System.Environment.NewLine, "");
                modifiedRecord = modifiedRecord.Replace(" ", "");
                if (i != data.Length - 1)
                {
                    modifiedRecord += "}," + Environment.NewLine;
                }

                outputText += modifiedRecord;
            }

            outputText = $"[{outputText}]";
            txtOutput.Text = outputText;
            btnCopy.Enabled = !string.IsNullOrWhiteSpace(txtOutput.Text);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtOutput.Text))
            {
                Clipboard.SetText(txtOutput.Text);
            }
        }

        public bool ValidJson(string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException ex)
            {
                Trace.WriteLine(ex);
                return false;
            }
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            btnConvert.Enabled = !string.IsNullOrWhiteSpace(txtInput.Text);
        }
    }
}
