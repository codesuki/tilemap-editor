using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections;

namespace MapEditor
{
    public partial class TextureForm : Form
    {
        private Device m_d3dDevice = null;
        private ArrayList m_tilesets;
        private Sprite m_sprite = null;
        private VScrollBar m_vscrollbar;
        private HScrollBar m_hscrollbar;
        public int m_selected_tile = -1;
        public int m_selected_tileset = 1000;
        public Size m_tilemap_size;
        public Hashtable m_tileset_hash;

        public void SetTileset(String name)
        {
            IDictionaryEnumerator enumerator = m_tileset_hash.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Key.Equals(name))
                {
                    m_selected_tileset = (int)enumerator.Value;
                    ImageInformation img = TextureLoader.ImageInformationFromFile((String)enumerator.Key);
                    m_tilemap_size.Width = img.Width;
                    m_tilemap_size.Height = img.Height;
                    m_hscrollbar.Maximum = m_tilemap_size.Width;
                    m_vscrollbar.Maximum = m_tilemap_size.Height;
                    break;
                }
            }
        }

        public void AddTileset(String name)
        {
            ImageInformation img = TextureLoader.ImageInformationFromFile(name);
            m_tilesets.Add(TextureLoader.FromFile(m_d3dDevice, name, img.Width, img.Height, img.MipLevels, 0, img.Format, Pool.Managed, Filter.None, Filter.None, Color.FromArgb(255, 0, 255).ToArgb()));
            m_tileset_hash.Add(name, m_tilesets.Count - 1);
        }

        public TextureForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            m_vscrollbar = new VScrollBar();
            m_hscrollbar = new HScrollBar();
            m_vscrollbar.Dock = DockStyle.Right;
            m_hscrollbar.Dock = DockStyle.Bottom;
            Controls.Add(m_vscrollbar);
            Controls.Add(m_hscrollbar);

            m_tilesets = new ArrayList();
            m_tileset_hash = new Hashtable();
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
                    //this.Dispose();
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
            d3dpp.PresentationInterval = PresentInterval.One;

            m_d3dDevice = new Device(0, DeviceType.Hardware, this, flags, d3dpp);

            // Register an event-handler for DeviceReset and call it to continue
            // our setup.
            m_d3dDevice.DeviceReset += new System.EventHandler(this.OnResetDevice);
            OnResetDevice(m_d3dDevice, null);

            m_sprite = new Sprite(m_d3dDevice);

            //m_tilemap_size.Height = img.Height;
            //m_tilemap_size.Width = img.Width;
            //hScrollbar.Minimum = vScrollbar.Minimum = 0;
            //hScrollbar.Maximum = m_tilemap_size.Width;
            //vScrollbar.Maximum = m_tilemap_size.Height;
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
        }

        private void Render()
        {
            if (m_d3dDevice == null) return;
            m_d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.FromArgb(255, 0, 0, 0), 1.0f, 0);

            m_d3dDevice.BeginScene();

            Matrix matView = Matrix.LookAtLH(new Vector3(m_hscrollbar.Value, m_vscrollbar.Value, -1.0f), new Vector3(m_hscrollbar.Value, m_vscrollbar.Value, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            Matrix matProj = Matrix.OrthoOffCenterLH(0.0f, (float)ClientSize.Width, (float)ClientSize.Height, 0.0f, 0.0f, 10.0f);
            Matrix matWorld = Matrix.Identity;

            m_d3dDevice.SetTransform(TransformType.World, matWorld);
            m_d3dDevice.SetTransform(TransformType.View, matView);
            m_d3dDevice.SetTransform(TransformType.Projection, matProj);

            if (m_selected_tileset < m_tilesets.Count) {
                m_sprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.ObjectSpace);
                m_sprite.Draw((Texture)m_tilesets[m_selected_tileset], new Rectangle(0, 0, m_tilemap_size.Width, m_tilemap_size.Height), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), Color.White.ToArgb());
                m_sprite.End();
            }
            m_d3dDevice.EndScene();

            m_d3dDevice.Present();
        }

        private void OnShown(object sender, EventArgs e)
        {
           //this.Init();
        }

        private void TextureForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_vscrollbar.Value + e.Y > m_tilemap_size.Height || m_hscrollbar.Value + e.X > m_tilemap_size.Width) return;
            int y = (int)Math.Floor((double)((m_vscrollbar.Value+e.Y) / 32));
            int x = (int)Math.Floor((double)((m_hscrollbar.Value+e.X) / 32));
            m_selected_tile = y * 8 + x;
        }

        private void TextureForm_Layout(object sender, LayoutEventArgs e)
        {
            if (MdiParent != null)
            {
                MdiParent.Invalidate(true);
                MdiParent.Update();
            }
        }
    }
}