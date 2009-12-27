using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MapEditor
{
    public partial class MapForm : Form
    {
        private Device m_d3dDevice = null;
        private CustomVertex.PositionColored[] m_grid_vertexes = null;
        private Sprite m_sprite = null;

        public int m_current_layer = 0;

        private ArrayList m_tilesets;
        private Hashtable m_tileset_hash;
        private int m_selected_tileset;
        
        private TextureForm m_textureForm;

        private VScrollBar m_vscrollbar;
        private HScrollBar m_hscrollbar;

        public Tilemap m_tilemap;

        private Texture m_collisionTexture;
        private MainForm m_mainForm;

        public void SetTileset(String name)
        {
            IDictionaryEnumerator enumerator = m_tileset_hash.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Key.Equals(name))
                {
                    m_selected_tileset = (int)enumerator.Value;
                }
            }
        }

        public void LoadTilesets()
        {
            m_tileset_hash = new Hashtable();
            IEnumerator enumerator = m_tilemap.m_tilesets.GetEnumerator();
            while (enumerator.MoveNext())
            {
                AddTileset((String)enumerator.Current);
            }
        }

        public void AddTileset(String name) 
        {
            ImageInformation img = TextureLoader.ImageInformationFromFile(name);
            m_tilesets.Add(TextureLoader.FromFile(m_d3dDevice, name, img.Width, img.Height, img.MipLevels, 0, img.Format, Pool.Managed, Filter.None, Filter.None, Color.FromArgb(255, 0, 255).ToArgb()));
            m_tileset_hash.Add(name, m_tilesets.Count - 1);
            m_tilemap.AddTilemap(name);
        }

        public void SetTextureForm(ref TextureForm form)
        {
            m_textureForm = form;
        }

        public void SetMainForm(MainForm form) 
        {
            m_mainForm = form;
        }

        private void Initialize() 
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            m_vscrollbar = new VScrollBar();
            m_hscrollbar = new HScrollBar();
            m_vscrollbar.Dock = DockStyle.Right;
            m_hscrollbar.Dock = DockStyle.Bottom;
            Controls.Add(m_vscrollbar);
            Controls.Add(m_hscrollbar);

            //HScroll = true;
            //VScroll = true;

            m_tilesets = new ArrayList();
            m_tileset_hash = new Hashtable();
        }

        public MapForm(int tilesize, int layers, int height, int width)
        {
            InitializeComponent();

            Initialize();

            m_tilemap = new Tilemap(tilesize, layers, height, width);
        }

        public MapForm(Tilemap tilemap) 
        {
            InitializeComponent();

            Initialize();

            m_tilemap = tilemap;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            this.Render();
            this.Invalidate();
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Escape:
                    this.Dispose();
                    break;
            }
        }

        public void Init()
        {
            // Does the hardware support a 16-bit z-buffer?
            if (!Manager.CheckDeviceFormat(Manager.Adapters.Default.Adapter,
                                           DeviceType.Hardware,
                                           Manager.Adapters.Default.CurrentDisplayMode.Format,
                                           Usage.DepthStencil,
                                           ResourceType.Surface,
                                           DepthFormat.D16))
            {
                // POTENTIAL PROBLEM: We need at least a 16-bit z-buffer!
                return;
            }

            //
            // Do we support hardware vertex processing? if so, use it. 
            // If not, downgrade to software.
            //

            Caps caps = Manager.GetDeviceCaps(Manager.Adapters.Default.Adapter, DeviceType.Hardware);
            CreateFlags flags;

            if (caps.DeviceCaps.SupportsHardwareTransformAndLight)
                flags = CreateFlags.HardwareVertexProcessing;
            else
                flags = CreateFlags.SoftwareVertexProcessing;

            //
            // Everything checks out - create a simple, windowed device.
            //

            PresentParameters d3dpp = new PresentParameters();

            d3dpp.BackBufferFormat = Format.Unknown;
            d3dpp.SwapEffect = SwapEffect.Discard;
            d3dpp.Windowed = true;
            d3dpp.EnableAutoDepthStencil = true;
            d3dpp.AutoDepthStencilFormat = DepthFormat.D16;
            d3dpp.PresentationInterval = PresentInterval.Immediate;

            m_d3dDevice = new Device(0, DeviceType.Hardware, this, flags, d3dpp);
            // Register an event-handler for DeviceReset and call it to continue
            // our setup.
            m_d3dDevice.DeviceReset += new System.EventHandler(this.OnResetDevice);
            OnResetDevice(m_d3dDevice, null);

            m_grid_vertexes = new CustomVertex.PositionColored[1000];

            for (int i = 0; i < m_tilemap.m_num_cols; i++)
            {
                m_grid_vertexes[i * 4].X = i * m_tilemap.m_tilesize;
                m_grid_vertexes[i * 4].Y = 0;
                m_grid_vertexes[i * 4].Z = 2.0f;
                m_grid_vertexes[i * 4].Color = Color.FromArgb(255, 255, 255).ToArgb();

                m_grid_vertexes[i * 4 + 1].X = i * m_tilemap.m_tilesize;
                m_grid_vertexes[i * 4 + 1].Y = m_tilemap.m_tilesize * m_tilemap.m_num_rows;
                m_grid_vertexes[i * 4 + 1].Z = 2.0f;
                m_grid_vertexes[i * 4 + 1].Color = Color.FromArgb(255, 255, 255).ToArgb();

                m_grid_vertexes[i * 4 + 2].X = 0;
                m_grid_vertexes[i * 4 + 2].Y = i * m_tilemap.m_tilesize;
                m_grid_vertexes[i * 4 + 2].Z = 2.0f;
                m_grid_vertexes[i * 4 + 2].Color = Color.FromArgb(255, 255, 255).ToArgb();

                m_grid_vertexes[i * 4 + 3].X = m_tilemap.m_tilesize * m_tilemap.m_num_cols;
                m_grid_vertexes[i * 4 + 3].Y = i * m_tilemap.m_tilesize;
                m_grid_vertexes[i * 4 + 3].Z = 2.0f;
                m_grid_vertexes[i * 4 + 3].Color = Color.FromArgb(255, 255, 255).ToArgb();
            }

            m_sprite = new Sprite(m_d3dDevice);

            m_hscrollbar.Maximum = m_tilemap.m_num_cols * m_tilemap.m_tilesize;
            m_vscrollbar.Maximum = m_tilemap.m_num_rows * m_tilemap.m_tilesize;

            ImageInformation img = TextureLoader.ImageInformationFromFile("gitter.bmp");
            m_collisionTexture = TextureLoader.FromFile(m_d3dDevice, "gitter.bmp", img.Width, img.Height, img.MipLevels, 0, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, Color.FromArgb(255, 0, 255).ToArgb());
        }

        /// <summary>
        /// This event-handler is a good place to create and initialize any 
        /// Direct3D related objects, which may become invalid during a 
        /// device reset.
        /// </summary>
        public void OnResetDevice(object sender, EventArgs e)
        {
            // This sample doens't create anything that requires recreation 
            // after the DeviceReset event.

            m_d3dDevice.RenderState.Lighting = false;
            //d3dDevice.RenderState.CullMode = Cull.None;
            //d3dDevice.RenderState.Ambient = Color.FromArgb(255, 50, 50, 50); 
        }

        private void Render()
        {
            if (m_d3dDevice == null || m_tilesets.Count == 0) return;
            m_d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.FromArgb(255, 200, 200, 200), 1.0f, 0);

            m_d3dDevice.BeginScene();
            m_d3dDevice.VertexFormat = CustomVertex.PositionColored.Format;

            Matrix matView = Matrix.LookAtLH(new Vector3(m_hscrollbar.Value, m_vscrollbar.Value, -1.0f), new Vector3(m_hscrollbar.Value, m_vscrollbar.Value, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            Matrix matProj = Matrix.OrthoOffCenterLH(0.0f, (float)ClientSize.Width, (float)ClientSize.Height, 0.0f, 0.0f, 10.0f);
            Matrix matWorld = Matrix.Identity;

            m_d3dDevice.SetTransform(TransformType.World, matWorld);
            m_d3dDevice.SetTransform(TransformType.View, matView);
            m_d3dDevice.SetTransform(TransformType.Projection, matProj);

            m_d3dDevice.DrawUserPrimitives(PrimitiveType.LineList, 100, m_grid_vertexes);

            m_sprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.ObjectSpace);
            for (int layer = 0; layer < m_tilemap.m_num_layers; ++layer)
            {
                for (int row = 0; row < m_tilemap.m_num_rows; ++row)
                {
                    for (int col = 0; col < m_tilemap.m_num_cols; ++col)
                    {
                        if (m_tilemap.m_tilemap[layer, row, col].m_tile == -1) continue;
                        int y = (int)(m_tilemap.m_tilemap[layer, row, col].m_tile / 8) * m_tilemap.m_tilesize;
                        int x = (int)(m_tilemap.m_tilemap[layer, row, col].m_tile % 8) * m_tilemap.m_tilesize;
                        m_sprite.Draw((Texture)m_tilesets[m_tilemap.m_tilemap[layer, row, col].m_tileset], new Rectangle(x, y, m_tilemap.m_tilesize, m_tilemap.m_tilesize), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(col * m_tilemap.m_tilesize, row * m_tilemap.m_tilesize, 0.0f), Color.White.ToArgb());
                    }
                }
            }

            if (m_mainForm.selectedTool == 1)
            {
                for (int row = 0; row < m_tilemap.m_num_rows; ++row)
                {
                    for (int col = 0; col < m_tilemap.m_num_cols; ++col)
                    {
                        if (m_tilemap.m_collisionMap[row, col] == false) continue;
                        m_sprite.Draw(m_collisionTexture, new Rectangle(0, 0, m_tilemap.m_tilesize, m_tilemap.m_tilesize), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(col * m_tilemap.m_tilesize, row * m_tilemap.m_tilesize, 0.0f), Color.White.ToArgb());
                    }
                }
            }
            m_sprite.End();

            m_d3dDevice.EndScene();

            m_d3dDevice.Present();
        }

        private void OnShown(object sender, EventArgs e)
        {
            //this.Init();
        }

        private void MapForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_vscrollbar.Value + e.Y > m_tilemap.m_tilesize * m_tilemap.m_num_rows || m_hscrollbar.Value + e.X > m_tilemap.m_tilesize * m_tilemap.m_num_cols) return;
           
            if (m_mainForm.selectedTool == 1) {
                int row = (int)Math.Floor((double)((m_vscrollbar.Value + e.Y) / m_tilemap.m_tilesize));
                int col = (int)Math.Floor((double)((m_hscrollbar.Value + e.X) / m_tilemap.m_tilesize));
                m_tilemap.m_collisionMap[row, col] = !m_tilemap.m_collisionMap[row, col];
            } else {
                int row = (int)Math.Floor((double)((m_vscrollbar.Value + e.Y) / m_tilemap.m_tilesize));
                int col = (int)Math.Floor((double)((m_hscrollbar.Value + e.X) / m_tilemap.m_tilesize));
                m_tilemap.m_tilemap[m_current_layer, row, col].m_tile = m_textureForm.m_selected_tile;
                m_tilemap.m_tilemap[m_current_layer, row, col].m_tileset = m_selected_tileset;
            }
        }

        private void MapForm_Layout(object sender, LayoutEventArgs e)
        {
            if (MdiParent != null)
            {
                MdiParent.Invalidate(true);
                MdiParent.Update();
            }
        }
    }
}