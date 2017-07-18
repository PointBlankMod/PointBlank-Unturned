namespace Launcher
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Child Node");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Test Node", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.conServers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabConsole = new System.Windows.Forms.TabPage();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.btnRunCommand = new System.Windows.Forms.Button();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.tabInformation = new System.Windows.Forms.TabPage();
            this.tabConfiguration = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabTranslation = new System.Windows.Forms.TabPage();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.tabModules = new System.Windows.Forms.TabPage();
            this.tabPlugins = new System.Windows.Forms.TabPage();
            this.tabPlayers = new System.Windows.Forms.TabPage();
            this.listView2 = new System.Windows.Forms.ListView();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.conServers.SuspendLayout();
            this.tabConsole.SuspendLayout();
            this.tabConfiguration.SuspendLayout();
            this.tabTranslation.SuspendLayout();
            this.tabPlayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabConsole);
            this.tabControl1.Controls.Add(this.tabInformation);
            this.tabControl1.Controls.Add(this.tabConfiguration);
            this.tabControl1.Controls.Add(this.tabTranslation);
            this.tabControl1.Controls.Add(this.tabModules);
            this.tabControl1.Controls.Add(this.tabPlugins);
            this.tabControl1.Controls.Add(this.tabPlayers);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(641, 391);
            this.tabControl1.TabIndex = 0;
            // 
            // conServers
            // 
            this.conServers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addServerToolStripMenuItem,
            this.deleteServerToolStripMenuItem,
            this.startServerToolStripMenuItem,
            this.stopServerToolStripMenuItem});
            this.conServers.Name = "conServers";
            this.conServers.Size = new System.Drawing.Size(143, 92);
            // 
            // addServerToolStripMenuItem
            // 
            this.addServerToolStripMenuItem.Name = "addServerToolStripMenuItem";
            this.addServerToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.addServerToolStripMenuItem.Text = "Add Server";
            // 
            // deleteServerToolStripMenuItem
            // 
            this.deleteServerToolStripMenuItem.Name = "deleteServerToolStripMenuItem";
            this.deleteServerToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.deleteServerToolStripMenuItem.Text = "Delete Server";
            // 
            // startServerToolStripMenuItem
            // 
            this.startServerToolStripMenuItem.Enabled = false;
            this.startServerToolStripMenuItem.Name = "startServerToolStripMenuItem";
            this.startServerToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.startServerToolStripMenuItem.Text = "Start Server";
            // 
            // stopServerToolStripMenuItem
            // 
            this.stopServerToolStripMenuItem.Enabled = false;
            this.stopServerToolStripMenuItem.Name = "stopServerToolStripMenuItem";
            this.stopServerToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.stopServerToolStripMenuItem.Text = "Stop Server";
            // 
            // tabConsole
            // 
            this.tabConsole.Controls.Add(this.txtConsole);
            this.tabConsole.Controls.Add(this.btnRunCommand);
            this.tabConsole.Controls.Add(this.txtCommand);
            this.tabConsole.Location = new System.Drawing.Point(4, 22);
            this.tabConsole.Name = "tabConsole";
            this.tabConsole.Size = new System.Drawing.Size(633, 365);
            this.tabConsole.TabIndex = 5;
            this.tabConsole.Text = "Console";
            this.tabConsole.UseVisualStyleBackColor = true;
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.txtConsole.Location = new System.Drawing.Point(8, 3);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.Size = new System.Drawing.Size(617, 325);
            this.txtConsole.TabIndex = 2;
            // 
            // btnRunCommand
            // 
            this.btnRunCommand.Location = new System.Drawing.Point(544, 337);
            this.btnRunCommand.Name = "btnRunCommand";
            this.btnRunCommand.Size = new System.Drawing.Size(81, 23);
            this.btnRunCommand.TabIndex = 1;
            this.btnRunCommand.Text = "Run";
            this.btnRunCommand.UseVisualStyleBackColor = true;
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(8, 337);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(530, 20);
            this.txtCommand.TabIndex = 0;
            // 
            // tabInformation
            // 
            this.tabInformation.Location = new System.Drawing.Point(4, 22);
            this.tabInformation.Name = "tabInformation";
            this.tabInformation.Size = new System.Drawing.Size(633, 365);
            this.tabInformation.TabIndex = 1;
            this.tabInformation.Text = "Information";
            this.tabInformation.UseVisualStyleBackColor = true;
            // 
            // tabConfiguration
            // 
            this.tabConfiguration.Controls.Add(this.treeView1);
            this.tabConfiguration.Location = new System.Drawing.Point(4, 22);
            this.tabConfiguration.Name = "tabConfiguration";
            this.tabConfiguration.Size = new System.Drawing.Size(633, 365);
            this.tabConfiguration.TabIndex = 2;
            this.tabConfiguration.Text = "Configuration";
            this.tabConfiguration.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Test Child";
            treeNode1.Text = "Child Node";
            treeNode2.Name = "Test";
            treeNode2.Text = "Test Node";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeView1.Size = new System.Drawing.Size(633, 365);
            this.treeView1.TabIndex = 0;
            // 
            // tabTranslation
            // 
            this.tabTranslation.Controls.Add(this.treeView2);
            this.tabTranslation.Location = new System.Drawing.Point(4, 22);
            this.tabTranslation.Name = "tabTranslation";
            this.tabTranslation.Size = new System.Drawing.Size(633, 365);
            this.tabTranslation.TabIndex = 8;
            this.tabTranslation.Text = "Translation";
            this.tabTranslation.UseVisualStyleBackColor = true;
            // 
            // treeView2
            // 
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.Location = new System.Drawing.Point(0, 0);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(633, 365);
            this.treeView2.TabIndex = 0;
            // 
            // tabModules
            // 
            this.tabModules.Location = new System.Drawing.Point(4, 22);
            this.tabModules.Name = "tabModules";
            this.tabModules.Size = new System.Drawing.Size(633, 365);
            this.tabModules.TabIndex = 3;
            this.tabModules.Text = "Modules";
            this.tabModules.UseVisualStyleBackColor = true;
            // 
            // tabPlugins
            // 
            this.tabPlugins.Location = new System.Drawing.Point(4, 22);
            this.tabPlugins.Name = "tabPlugins";
            this.tabPlugins.Size = new System.Drawing.Size(633, 365);
            this.tabPlugins.TabIndex = 4;
            this.tabPlugins.Text = "Plugins";
            this.tabPlugins.UseVisualStyleBackColor = true;
            // 
            // tabPlayers
            // 
            this.tabPlayers.Controls.Add(this.listView2);
            this.tabPlayers.Location = new System.Drawing.Point(4, 22);
            this.tabPlayers.Name = "tabPlayers";
            this.tabPlayers.Size = new System.Drawing.Size(633, 365);
            this.tabPlayers.TabIndex = 6;
            this.tabPlayers.Text = "Players";
            this.tabPlayers.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(633, 365);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // tabSettings
            // 
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(633, 365);
            this.tabSettings.TabIndex = 7;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 391);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PointBlank Launcher";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabControl1.ResumeLayout(false);
            this.conServers.ResumeLayout(false);
            this.tabConsole.ResumeLayout(false);
            this.tabConsole.PerformLayout();
            this.tabConfiguration.ResumeLayout(false);
            this.tabTranslation.ResumeLayout(false);
            this.tabPlayers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInformation;
        private System.Windows.Forms.TabPage tabConsole;
        private System.Windows.Forms.TabPage tabConfiguration;
        private System.Windows.Forms.TabPage tabTranslation;
        private System.Windows.Forms.TabPage tabModules;
        private System.Windows.Forms.TabPage tabPlugins;
        private System.Windows.Forms.TabPage tabPlayers;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.ContextMenuStrip conServers;
        private System.Windows.Forms.ToolStripMenuItem addServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopServerToolStripMenuItem;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Button btnRunCommand;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.ListView listView2;
    }
}

