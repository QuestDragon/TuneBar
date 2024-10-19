namespace TuneBar
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            listBox1 = new ListBox();
            button1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button3 = new Button();
            button4 = new Button();
            label1 = new Label();
            label2 = new Label();
            checkBox1 = new CheckBox();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            forcePlayToolStripMenuItem = new ToolStripMenuItem();
            PauseToolStripMenuItem = new ToolStripMenuItem();
            nextTrackToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            helpGitHubToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            quitAppToolStripMenuItem = new ToolStripMenuItem();
            timer1 = new System.Windows.Forms.Timer(components);
            checkBox2 = new CheckBox();
            checkBox3 = new CheckBox();
            button5 = new Button();
            timer2 = new System.Windows.Forms.Timer(components);
            label3 = new Label();
            button6 = new Button();
            button7 = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            checkBox4 = new CheckBox();
            label4 = new Label();
            contextMenuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.HorizontalScrollbar = true;
            listBox1.ItemHeight = 22;
            listBox1.Location = new Point(12, 34);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(399, 334);
            listBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(434, 22);
            button1.Name = "button1";
            button1.Size = new Size(180, 51);
            button1.TabIndex = 1;
            button1.Text = "Add...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(434, 79);
            button2.Name = "button2";
            button2.Size = new Size(180, 51);
            button2.TabIndex = 1;
            button2.Text = "Remove...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(620, 157);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(344, 27);
            textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(620, 206);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(344, 27);
            textBox2.TabIndex = 2;
            // 
            // button3
            // 
            button3.Location = new Point(439, 400);
            button3.Name = "button3";
            button3.Size = new Size(255, 51);
            button3.TabIndex = 1;
            button3.Text = "OK";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(716, 400);
            button4.Name = "button4";
            button4.Size = new Size(255, 51);
            button4.TabIndex = 1;
            button4.Text = "Cancel";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(434, 162);
            label1.Name = "label1";
            label1.Size = new Size(119, 22);
            label1.TabIndex = 3;
            label1.Text = "Fade time (ms)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(434, 209);
            label2.Name = "label2";
            label2.Size = new Size(180, 22);
            label2.TabIndex = 3;
            label2.Text = "Update cycle time (ms)";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(439, 254);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(382, 26);
            checkBox1.TabIndex = 4;
            checkBox1.Text = "Other apps will not work while music is playing";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "TuneBar";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { forcePlayToolStripMenuItem, PauseToolStripMenuItem, nextTrackToolStripMenuItem, toolStripMenuItem1, helpGitHubToolStripMenuItem, settingsToolStripMenuItem, aboutToolStripMenuItem, toolStripMenuItem2, quitAppToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(205, 198);
            // 
            // forcePlayToolStripMenuItem
            // 
            forcePlayToolStripMenuItem.Name = "forcePlayToolStripMenuItem";
            forcePlayToolStripMenuItem.Size = new Size(204, 26);
            forcePlayToolStripMenuItem.Text = "Force play";
            forcePlayToolStripMenuItem.Click += forcePlayToolStripMenuItem_Click;
            // 
            // PauseToolStripMenuItem
            // 
            PauseToolStripMenuItem.Name = "PauseToolStripMenuItem";
            PauseToolStripMenuItem.Size = new Size(204, 26);
            PauseToolStripMenuItem.Text = "Pause";
            PauseToolStripMenuItem.Click += PauseToolStripMenuItem_Click;
            // 
            // nextTrackToolStripMenuItem
            // 
            nextTrackToolStripMenuItem.Name = "nextTrackToolStripMenuItem";
            nextTrackToolStripMenuItem.Size = new Size(204, 26);
            nextTrackToolStripMenuItem.Text = "Next track";
            nextTrackToolStripMenuItem.Click += nextTrackToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(201, 6);
            // 
            // helpGitHubToolStripMenuItem
            // 
            helpGitHubToolStripMenuItem.Name = "helpGitHubToolStripMenuItem";
            helpGitHubToolStripMenuItem.Size = new Size(204, 26);
            helpGitHubToolStripMenuItem.Text = "Help （GitHub）";
            helpGitHubToolStripMenuItem.Click += helpGitHubToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(204, 26);
            settingsToolStripMenuItem.Text = "Settings...";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(204, 26);
            aboutToolStripMenuItem.Text = "About...";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(201, 6);
            // 
            // quitAppToolStripMenuItem
            // 
            quitAppToolStripMenuItem.Name = "quitAppToolStripMenuItem";
            quitAppToolStripMenuItem.Size = new Size(204, 26);
            quitAppToolStripMenuItem.Text = "Quit app";
            quitAppToolStripMenuItem.Click += quitAppToolStripMenuItem_Click;
            // 
            // timer1
            // 
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(439, 286);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(212, 26);
            checkBox2.TabIndex = 4;
            checkBox2.Text = "Fix Google Chrome issue";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(439, 318);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(212, 26);
            checkBox3.TabIndex = 4;
            checkBox3.Text = "Only work with Explorer";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // button5
            // 
            button5.Location = new Point(12, 400);
            button5.Name = "button5";
            button5.Size = new Size(188, 51);
            button5.TabIndex = 1;
            button5.Text = "Ignore list...";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // timer2
            // 
            timer2.Tick += timer2_Tick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(620, 93);
            label3.Name = "label3";
            label3.Size = new Size(255, 22);
            label3.TabIndex = 3;
            label3.Text = "<<< Press Shift to delete all songs";
            // 
            // button6
            // 
            button6.Location = new Point(223, 400);
            button6.Name = "button6";
            button6.Size = new Size(188, 51);
            button6.TabIndex = 1;
            button6.Text = "Target list...";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(827, 248);
            button7.Name = "button7";
            button7.Size = new Size(137, 36);
            button7.TabIndex = 1;
            button7.Text = "Ignore list...";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 464);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(983, 28);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(152, 22);
            toolStripStatusLabel1.Text = "SONG_NAME_PATH";
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(439, 350);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(225, 26);
            checkBox4.TabIndex = 4;
            checkBox4.Text = "Notifying of state changes";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 9);
            label4.Name = "label4";
            label4.Size = new Size(73, 22);
            label4.TabIndex = 3;
            label4.Text = "Song list";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 22F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(983, 492);
            Controls.Add(statusStrip1);
            Controls.Add(checkBox4);
            Controls.Add(checkBox3);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(label2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button4);
            Controls.Add(button2);
            Controls.Add(button3);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button1);
            Controls.Add(listBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            ShowInTaskbar = false;
            Text = "TuneBar";
            WindowState = FormWindowState.Minimized;
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button3;
        private Button button4;
        private Label label1;
        private Label label2;
        private CheckBox checkBox1;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem forcePlayToolStripMenuItem;
        private ToolStripMenuItem PauseToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem helpGitHubToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem quitAppToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private Button button5;
        private System.Windows.Forms.Timer timer2;
        private Label label3;
        private Button button6;
        private Button button7;
        private ToolStripMenuItem nextTrackToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private CheckBox checkBox4;
        private Label label4;
    }
}
