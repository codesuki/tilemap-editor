namespace MapEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.layerComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tilemapComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.newmapButton = new System.Windows.Forms.ToolStripButton();
            this.loadmapButton = new System.Windows.Forms.ToolStripButton();
            this.savemapButton = new System.Windows.Forms.ToolStripButton();
            this.eraseButton = new System.Windows.Forms.ToolStripButton();
            this.collisionToolButton = new System.Windows.Forms.ToolStripButton();
            this.addTilesetButton = new System.Windows.Forms.ToolStripButton();
            this.characterToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newmapButton,
            this.loadmapButton,
            this.savemapButton,
            this.eraseButton,
            this.collisionToolButton,
            this.toolStripLabel1,
            this.layerComboBox,
            this.toolStripLabel2,
            this.tilemapComboBox,
            this.addTilesetButton,
            this.characterToolButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(835, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel1.Text = "layer";
            // 
            // layerComboBox
            // 
            this.layerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.layerComboBox.Name = "layerComboBox";
            this.layerComboBox.Size = new System.Drawing.Size(121, 25);
            this.layerComboBox.SelectedIndexChanged += new System.EventHandler(this.layerComboBox_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(47, 22);
            this.toolStripLabel2.Text = "tilemap";
            // 
            // tilemapComboBox
            // 
            this.tilemapComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tilemapComboBox.Name = "tilemapComboBox";
            this.tilemapComboBox.Size = new System.Drawing.Size(121, 25);
            this.tilemapComboBox.SelectedIndexChanged += new System.EventHandler(this.tilemapComboBox_SelectedIndexChanged);
            // 
            // newmapButton
            // 
            this.newmapButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newmapButton.Image = global::MapEditor.Properties.Resources.newMap;
            this.newmapButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newmapButton.Name = "newmapButton";
            this.newmapButton.Size = new System.Drawing.Size(23, 22);
            this.newmapButton.Text = "toolStripButton1";
            this.newmapButton.Click += new System.EventHandler(this.newmapButton_Click);
            // 
            // loadmapButton
            // 
            this.loadmapButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadmapButton.Image = global::MapEditor.Properties.Resources.load;
            this.loadmapButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadmapButton.Name = "loadmapButton";
            this.loadmapButton.Size = new System.Drawing.Size(23, 22);
            this.loadmapButton.Text = "toolStripButton1";
            this.loadmapButton.Click += new System.EventHandler(this.loadmapButton_Click);
            // 
            // savemapButton
            // 
            this.savemapButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.savemapButton.Image = global::MapEditor.Properties.Resources.save;
            this.savemapButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.savemapButton.Name = "savemapButton";
            this.savemapButton.Size = new System.Drawing.Size(23, 22);
            this.savemapButton.Text = "toolStripButton2";
            this.savemapButton.Click += new System.EventHandler(this.savemapButton_Click);
            // 
            // eraseButton
            // 
            this.eraseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.eraseButton.Image = global::MapEditor.Properties.Resources.Eraser;
            this.eraseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.eraseButton.Name = "eraseButton";
            this.eraseButton.Size = new System.Drawing.Size(23, 22);
            this.eraseButton.Text = "toolStripButton1";
            this.eraseButton.Click += new System.EventHandler(this.eraseButton_Click);
            // 
            // collisionToolButton
            // 
            this.collisionToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.collisionToolButton.Image = global::MapEditor.Properties.Resources.gitter;
            this.collisionToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collisionToolButton.Name = "collisionToolButton";
            this.collisionToolButton.Size = new System.Drawing.Size(23, 22);
            this.collisionToolButton.Text = "toolStripButton1";
            this.collisionToolButton.Click += new System.EventHandler(this.collisionToolButton_Click);
            // 
            // addTilesetButton
            // 
            this.addTilesetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addTilesetButton.Image = global::MapEditor.Properties.Resources.addTileset;
            this.addTilesetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addTilesetButton.Name = "addTilesetButton";
            this.addTilesetButton.Size = new System.Drawing.Size(23, 22);
            this.addTilesetButton.Text = "toolStripButton1";
            this.addTilesetButton.Click += new System.EventHandler(this.addTilesetButton_Click);
            // 
            // characterToolButton
            // 
            this.characterToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.characterToolButton.Image = global::MapEditor.Properties.Resources.character;
            this.characterToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.characterToolButton.Name = "characterToolButton";
            this.characterToolButton.Size = new System.Drawing.Size(23, 22);
            this.characterToolButton.Text = "toolStripButton1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 534);
            this.Controls.Add(this.toolStrip1);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "MapEditor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton eraseButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox layerComboBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox tilemapComboBox;
        private System.Windows.Forms.ToolStripButton addTilesetButton;
        private System.Windows.Forms.ToolStripButton newmapButton;
        private System.Windows.Forms.ToolStripButton loadmapButton;
        private System.Windows.Forms.ToolStripButton savemapButton;
        private System.Windows.Forms.ToolStripButton collisionToolButton;
        private System.Windows.Forms.ToolStripButton characterToolButton;
    }
}

