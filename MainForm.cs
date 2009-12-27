using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace MapEditor
{
    public partial class MainForm : Form
    {
        private MapForm mapform;
        private TextureForm textureform;
        private int m_selectedTool;

        public int selectedTool
        {
            get 
            {
                return m_selectedTool;
            }
            set
            {
                m_selectedTool = value;
            }
        }


        public MainForm()
        {
            InitializeComponent();
        }

        private void eraseButton_Click(object sender, EventArgs e)
        {
            selectedTool = 0;
            textureform.m_selected_tile = -1;
        }

        private void layerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mapform.m_current_layer = (int)layerComboBox.SelectedItem;
        }

        private void addTilesetButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            ofd.ShowDialog();
            FileInfo file = new FileInfo(ofd.FileName);
            AddTileset(file.Name);
        }

        private void AddTileset(String filename) 
        {
            tilemapComboBox.Items.Add(filename);
            mapform.AddTileset(filename);
            textureform.AddTileset(filename);
        }

        private void tilemapComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mapform.SetTileset((String)tilemapComboBox.SelectedItem);
            textureform.SetTileset((String)tilemapComboBox.SelectedItem);
        }

        private void newmapButton_Click(object sender, EventArgs e)
        {
            NewMapDialog newmapdialog = new NewMapDialog();
            newmapdialog.ShowDialog(this);

            if (newmapdialog.m_ok) 
            {
                textureform = new TextureForm();
                textureform.MdiParent = this;
                textureform.Show();
                textureform.Init();

                mapform = new MapForm(newmapdialog.m_tilesize, newmapdialog.m_layers, newmapdialog.m_height, newmapdialog.m_width);
                mapform.MdiParent = this;
                mapform.Show();
                mapform.SetTextureForm(ref textureform);
                mapform.SetMainForm(this);
                mapform.Init();

                for (int i = 0; i < newmapdialog.m_layers; ++i)
                {
                    layerComboBox.Items.Add(i);
                }
                layerComboBox.SelectedIndex = 0;
            }
        }

        private void loadmapButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Mapfile|*.map";
            ofd.ShowDialog();

            if (ofd.FileName.Length > 0)
            {
                Tilemap tilemap = new Tilemap(ofd.FileName);

                textureform = new TextureForm();
                textureform.MdiParent = this;
                textureform.Show();
                textureform.Init();

                mapform = new MapForm(tilemap);
                mapform.MdiParent = this;
                mapform.Show();
                mapform.SetTextureForm(ref textureform);
                mapform.SetMainForm(this);
                mapform.Init();

                for (int i = 0; i < tilemap.m_num_layers; ++i)
                {
                    layerComboBox.Items.Add(i);
                }
                layerComboBox.SelectedIndex = 0;

                for (int i = 0; i < tilemap.m_tilesets.Count; ++i)
                {
                    AddTileset((String)tilemap.m_tilesets[i]);
                }
                layerComboBox.SelectedIndex = 0;
            }
        }

        private void savemapButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Mapfile|*.map";
            sfd.ShowDialog();
            if (sfd.FileName.Length != 0)
                mapform.m_tilemap.Save(sfd.FileName);
        }

        private void collisionToolButton_Click(object sender, EventArgs e)
        {
            selectedTool = 1;
        }
    }
}