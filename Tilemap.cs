using System;
using System.Collections;
using System.Text;
using System.IO;

namespace MapEditor
{
    public class Tile
    {
        public int m_tile;
        public int m_tileset;
    }

    public class Tilemap
    {
        public ArrayList m_tilesets;

        public Tile[, ,] m_tilemap;
        public Boolean[,] m_collisionMap;

        public int m_num_rows;
        public int m_num_cols;
        public int m_num_layers;
        public int m_tilesize;

        public Tilemap(int tilesize, int num_layers, int num_rows, int num_cols) 
        {
            m_tilesize = tilesize;
            m_num_layers = num_layers;
            m_num_rows = num_rows;
            m_num_cols = num_cols;

            m_tilesets = new ArrayList();

            m_tilemap = new Tile[num_layers, num_rows, num_cols];

            for (int layer = 0; layer < m_num_layers; ++layer)
            {
                for (int row = 0; row < m_num_rows; ++row)
                {
                    for (int col = 0; col < m_num_cols; ++col)
                    {
                        m_tilemap[layer, row, col] = new Tile();
                        m_tilemap[layer, row, col].m_tile = -1;
                        m_tilemap[layer, row, col].m_tileset = -1;
                    }
                }
            }

            m_collisionMap = new Boolean[num_rows, num_cols];
            for (int row = 0; row < m_num_rows; ++row)
            {
                for (int col = 0; col < m_num_cols; ++col)
                {
                    m_collisionMap[row, col] = false;
                }
            }
        }

        public Tilemap(String filename) 
        {
            FileStream fs = File.OpenRead(filename);
            BinaryReader br = new BinaryReader(fs);

            m_num_layers = br.ReadInt32();
            m_num_rows = br.ReadInt32();
            m_num_cols = br.ReadInt32();
            m_tilesize = br.ReadInt32();

            int tilesets = br.ReadInt32();

            m_tilesets = new ArrayList();

            for (int i = 0; i < tilesets; ++i)
            {
                String tmp = new String(br.ReadChars(255));
                m_tilesets.Add(tmp);
            }

            m_tilemap = new Tile[m_num_layers, m_num_rows, m_num_cols];

            for (int layer = 0; layer < m_num_layers; ++layer)
            {
                for (int row = 0; row < m_num_rows; ++row)
                {
                    for (int col = 0; col < m_num_cols; ++col)
                    {
                        m_tilemap[layer, row, col] = new Tile();
                        m_tilemap[layer, row, col].m_tile = br.ReadInt32();
                        m_tilemap[layer, row, col].m_tileset = br.ReadInt32();
                    }
                }
            }

            m_collisionMap = new Boolean[m_num_rows, m_num_cols];
            for (int row = 0; row < m_num_rows; ++row)
            {
                for (int col = 0; col < m_num_cols; ++col)
                {
                    try
                    {
                        m_collisionMap[row, col] = br.ReadBoolean();
                    }
                    catch (System.Exception e)
                    {
                        return;
                    }
                }
            }
        }

        public void AddTilemap(String name) 
        {
            if (m_tilesets.Contains(name)) return;
            m_tilesets.Add(name);
        }

        public void Save(String filename) 
        {
            FileStream fs = File.OpenWrite(filename);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(m_num_layers);
            bw.Write(m_num_rows);
            bw.Write(m_num_cols);
            bw.Write(m_tilesize);
            bw.Write(m_tilesets.Count);
            
            for (int i = 0; i < m_tilesets.Count; ++i) 
            {
                char[] tmp = new char[255];
                for (int j = 0; j < 255; ++j)
                {
                    tmp[j] = '\0';
                }
                for (int j = 0; j < ((String)(m_tilesets[i])).ToCharArray().Length; ++j)
                {
                    tmp[j] = ((String)(m_tilesets[i])).ToCharArray()[j];
                }
                bw.Write(tmp, 0, 255);
            }

            for (int layer = 0; layer < m_num_layers; ++layer)
            {
                for (int row = 0; row < m_num_rows; ++row)
                {
                    for (int col = 0; col < m_num_cols; ++col)
                    {
                        bw.Write(m_tilemap[layer, row, col].m_tile);
                        bw.Write(m_tilemap[layer, row, col].m_tileset);
                    }
                }
            }

            for (int row = 0; row < m_num_rows; ++row)
            {
                for (int col = 0; col < m_num_cols; ++col)
                {
                    bw.Write(m_collisionMap[row, col]);
                }
            }
        }
    }
}
