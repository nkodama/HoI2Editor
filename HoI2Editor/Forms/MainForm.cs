using System;
using System.Windows.Forms;

namespace HoI2Editor.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OnMinisterButtonClick(object sender, EventArgs e)
        {
            var form = new MinisterEditorForm();
            form.Show();
        }
    }
}