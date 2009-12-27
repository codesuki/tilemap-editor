using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapEditor
{
    public partial class NewMapDialog : Form
    {
        public int m_tilesize;
        public int m_width;
        public int m_height;
        public int m_layers;
        public bool m_ok = false;

        public NewMapDialog()
        {
            InitializeComponent();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            m_tilesize = int.Parse(tilesizeBox.Text);
            m_width = int.Parse(widthBox.Text);
            m_height = int.Parse(heightBox.Text);
            m_layers = int.Parse(layersBox.Text);
            m_ok = true;
            Close();
        }
    }
}
