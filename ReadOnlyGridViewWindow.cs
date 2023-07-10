using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtelRezervasyon
{
    public partial class ReadOnlyGridViewWindow : Form
    {
        public ReadOnlyGridViewWindow()
        {
            InitializeComponent();
        }

        private void ReadOnlyGridViewWindow_Load(object sender, EventArgs e)
        {

        }

        private void BackButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
