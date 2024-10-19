using System.Diagnostics;

namespace TuneBar
{
    public partial class Form1 : Form
    {
        SettingsData settings;
        private static ToolStripLabel songname_label;
        public static Form1 this_form;
        public static SoundCore sc;

        // �R���e�L�X�g���j���[��Ԃ̕ۑ�
        private bool isForce = false;
        private bool isPause = false;

        public Form1()
        {
            InitializeComponent();

            this_form = this;
            songname_label = toolStripStatusLabel1;
            songname_label.Text = "It's not currently playing music.";

            sc = new SoundCore(); //SoundCore������

            string app_path = AppDomain.CurrentDomain.BaseDirectory;

            string fileName = app_path + "data.bin";
            if (File.Exists(fileName))
            {
                Debug.WriteLine("'" + fileName + "'�͑��݂��܂��B");
                try
                {
                    settings = Serializer.Deserialize();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Err: {ex.Message}");
                    Debug.WriteLine("�ݒ�f�[�^���č쐬���܂��c�B");
                    settings = new SettingsData(new List<string>(), new List<string>(), new List<string>(), new List<string>(), 500, 500, false, false, false,false); // new List<string>()������List��New���Ƃ��Đ錾�ł���B
                    Serializer.Serialize(settings); //Save

                    MessageBox.Show("A problem occurred with the configuration data, so it has been reset. \nPlease set it again.", "TuneBar - Your settings have been reset.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                Debug.WriteLine("'" + fileName + "'�͑��݂��܂���B");
                settings = new SettingsData(new List<string>(), new List<string>(), new List<string>(), new List<string>(), 500, 500, false, false, false,false); // new List<string>()������List��New���Ƃ��Đ錾�ł���B
                Serializer.Serialize(settings); //Save
            }

            form_state(false);
            // timer2.Enabled = true;

            // SoundCore.init(); //SoundCore������(�G���[�̂��߈ꎞ�I�Ɏg�p�֎~�j
            // Thread.Sleep(1000);
            Update_Start(); //�쓮�J�n
        }

        /// <summary>
        /// �t�H�[���̓��e���ĕ`�悵�܂��B
        /// </summary>
        private void repaint()
        {
            listBox1.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";

            // �ݒ���e�𔽉f
            listBox1.Items.AddRange(settings.SongList.ToArray()); //Items��AddRange��List�^�̒��g�����̂܂܎ʂ���B
            textBox1.Text = settings.Fade.ToString();
            textBox2.Text = settings.Update.ToString();
            checkBox1.Checked = settings.dont_duplicate;
            checkBox2.Checked = settings.chrome_fix;
            checkBox3.Checked = settings.exp_only;
            checkBox4.Checked = settings.notify_active;

            checkBox2.Enabled = !checkBox3.Checked; //Exp only�̎���Fix Chrome�g�p�s��
            button5.Enabled = !checkBox3.Checked; //Exp only�̎���List�g�p�s��
            button6.Enabled = !checkBox3.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            repaint();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; //Form���Ȃ��Ȃ��Ă��܂��̂�h���i�u����v�̌��ʂ𖳌��ɂ���j
            form_state(false);
        }

        /// <summary>
        /// �t�H�[���̕\����Ԃ�؂�ւ��܂��B
        /// </summary>
        /// <param name="is_show">�\���L���̑I��</param>
        public void form_state(bool is_show)
        {
            // �t�H�[���܂��̓R���g���[���ɑ΂���InvokeRequired���`�F�b�N
            if (InvokeRequired) //�g�[�X�g�ʒm�Ȃǂ���A�N�Z�X�����ƕʃX���b�h�����ɂȂ���InvalidOperationException�ɂȂ�̂ŁAInvoke���K�v���ǂ����𔻒f����B
            {
                Invoke(new Action<bool>((val) => form_state(val)), is_show); //Invoke���g�p����K�v������ꍇ�͍ċA�����Ď��s�i�ċA�Fform_state���\�b�h�̒���form_state���\�b�h���Ăяo�����Ɓj
                return; //����ȉ���Invoke��K�v�Ƃ��Ȃ��ꍇ�̏����Ȃ̂Ŏ��s���Ȃ�
            }

            // Invoke���g�p���Ȃ��ꍇ�iForm���璼�ڂȂǁj�͒��ڂ����ɗ���BInvoke���g���ꍇ��Invoke�̒��ŌĂ΂�Ă���Ă���B

            if (is_show) //�\���Ɠ����ɕ`�悵����
            {
                repaint();
                this_form.WindowState = FormWindowState.Normal;
            }
            else
            {
                this_form.WindowState = FormWindowState.Minimized;
            }


            // this_form.Visible = is_show;
            this_form.ShowInTaskbar = is_show;

            Debug.WriteLine("�\����ԁF" + is_show);
        }

        /// <summary>
        /// Add�{�^��
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            #region �t�@�C�����J���_�C�A���O�ݒ�
            //OpenFileDialog�N���X�̃C���X�^���X���쐬
            OpenFileDialog ofd = new OpenFileDialog();

            //�͂��߂̃t�@�C�������w�肷��
            //�͂��߂Ɂu�t�@�C�����v�ŕ\������镶������w�肷��
            ofd.FileName = "";
            //�͂��߂ɕ\�������t�H���_���w�肷��
            //�w�肵�Ȃ��i��̕�����j�̎��́A���݂̃f�B���N�g�����\�������
            //����̓~���[�W�b�N�t�H���_�B
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            //[�t�@�C���̎��]�ɕ\�������I�������w�肷��
            //�w�肵�Ȃ��Ƃ��ׂẴt�@�C�����\�������
            string music_files = "Music files(*.mp3;*.wav;)|*.mp3;*.wav";
            string mp3_files = "MP3 files(*.mp3)|*.mp3";
            string wave_files = "Wave files(*.wav)|*.wav";

            ofd.Filter = music_files + "|" + mp3_files + "|" + wave_files;
            //[�t�@�C���̎��]�ł͂��߂ɑI���������̂��w�肷��
            //1�Ԗڂ́umusic_files�v���I������Ă���悤�ɂ���
            ofd.FilterIndex = 1;
            //�^�C�g����ݒ肷��
            ofd.Title = "Selecting a music file";
            //�_�C�A���O�{�b�N�X�����O�Ɍ��݂̃f�B���N�g���𕜌�����悤�ɂ���
            ofd.RestoreDirectory = true;
            //���݂��Ȃ��t�@�C���̖��O���w�肳�ꂽ�Ƃ��x����\������
            //�f�t�H���g��True�Ȃ̂Ŏw�肷��K�v�͂Ȃ�
            ofd.CheckFileExists = true;
            //���݂��Ȃ��p�X���w�肳�ꂽ�Ƃ��x����\������
            //�f�t�H���g��True�Ȃ̂Ŏw�肷��K�v�͂Ȃ�
            ofd.CheckPathExists = true;
            //�����̃t�@�C����I���ł���悤�ɂ���
            ofd.Multiselect = true;

            #endregion

            //�_�C�A���O��\������
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OK�{�^�����N���b�N���ꂽ�Ƃ��A�I�����ꂽ�t�@�C�����i�p�X�j�����ׂĕ\������
                foreach (string fn in ofd.FileNames)
                {
                    Debug.WriteLine(fn);
                    listBox1.Items.Add(fn);
                }
            }

        }

        /// <summary>
        /// Remove�{�^��
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift) //Shift�L�[���ꏏ�ɉ������ꍇ
            {
                var conf = MessageBox.Show("Do you want to delete all the songs you added?\n (This will only clear the list, not the original files)", "Delete all songs", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (conf == DialogResult.OK)
                {
                    listBox1.Items.Clear(); //�S�폜
                    Debug.WriteLine("Song list has been cleared!");
                    return;
                }
            }

            if (listBox1.SelectedItems.Count != 0) //�I�΂�Ă��Ȃ���ԂłȂ���
            {
                var conf = MessageBox.Show("Delete the selected song. Are you sure? \n(It will only be removed from the list, the file will not be deleted.)\n\n- " + listBox1.SelectedItem, "Delete selected songs", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (conf == DialogResult.OK)
                {
                    Debug.WriteLine($"Removed: {listBox1.SelectedItem}");
                    listBox1.Items.Remove(listBox1.SelectedItem);
                }

            }
        }

        /// <summary>
        /// OK����������
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            List<string> new_song_list = listBox1.Items.Cast<string>().ToList();

            // �ݒ蔽�f�ƕۑ�
            SettingsData new_settings = new SettingsData(new_song_list, settings.IgnoreList, settings.TargetList, settings.IgnoreSPAList, int.Parse(textBox1.Text), int.Parse(textBox2.Text), checkBox1.Checked, checkBox2.Checked, checkBox3.Checked,checkBox4.Checked);
            Serializer.Serialize(new_settings); //Save
            settings = new_settings;

            form_state(false);
            Update_Start(); //�C���^�[�o���ݒ�K�p
        }

        /// <summary>
        /// Cancel�{�^��
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            form_state(false);
        }

        /// <summary>
        /// Ignore List�{�^��
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {

            Form2 f2 = new Form2(ListData.Ignore); //Form�쐬���ɂ��̏��𑀍삷��悤�ɁA�Ƃ�������n��
            f2.ShowDialog();
            settings = Serializer.Deserialize(); //�ŐV�̃Z�[�u�f�[�^���󂯎��
        }

        /// <summary>
        /// Target List�{�^��
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {

            Form2 f2 = new Form2(ListData.Target); //Form�쐬���ɂ��̏��𑀍삷��悤�ɁA�Ƃ�������n��
            f2.ShowDialog();
            settings = Serializer.Deserialize(); //�ŐV�̃Z�[�u�f�[�^���󂯎��
        }

        /// <summary>
        /// Ignore List�{�^���i���A�v���P�[�V�����̃I�[�f�B�I�Đ����͓����~�̕����j
        /// </summary>
        private void button7_Click(object sender, EventArgs e)
        {

            Form2 f2 = new Form2(ListData.IgnoreMPApp); //Form�쐬���ɂ��̏��𑀍삷��悤�ɁA�Ƃ�������n��
            f2.ShowDialog();
            settings = Serializer.Deserialize(); //�ŐV�̃Z�[�u�f�[�^���󂯎��
        }

        // Explorer Only�ݒ��ς����ꍇ
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Enabled = !checkBox3.Checked; //�G�N�X�v���[���[��p���I���̎���ChromeFix�͖����ɂ���
        }

        // ----------Update----------
        /// <summary>
        /// �������Ńv���O���X�o�[�̊Ď��Ɖ��y�Đ���S���܂��B
        /// </summary>
        private void Update_Start()
        {
            if (timer1.Enabled) //�쓮���̏ꍇ
            {
                Update_Stop(); //��~���ăC���^�[�o���ݒ��A�Ďn��
                Thread.Sleep(1000); //1�b�ҋ@
            }

            // �^�C�}�[�̊Ԋu(�~���b)
            timer1.Interval = settings.Update;

            timer1.Start(); //�Ď��X�^�[�g
            GetNewWindow.StartMonitoring();
            Debug.WriteLine("TuneBar Started.");
            if (settings.notify_active)
            {
                Notify.show_notify("Information", "Activated!");
            }
        }

        private void Update_Stop()
        {
            timer1.Stop(); //�Ď���~
            GetNewWindow.StopMonitoring();
            Debug.WriteLine("TuneBar Stopped.");
            if (settings.notify_active)
            {
                Notify.show_notify("Information", "Deactivated!");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                // �����Ԃ����鏈��

                if (notifyIcon1.Text == "TuneBar") //�ʏ펞
                {
                    GetWindow.Window_Handler(); //���ꂪ���y�Đ��𔻒f���郁�\�b�h�ƂȂ�B
                }
                else
                {
                    sc.AudioPlay(true); //�����Đ�����i�v���O���X�o�[����A���A�v���P�[�V�����̃I�[�f�B�I�Đ��𖳎��j
                }
            });
        }

        /// <summary>
        /// �f�o�b�O�p
        /// </summary>
        private void timer2_Tick(object sender, EventArgs e)
        {
        }

        public static void update_current_song(string songname)
        {
            songname_label.Text = songname;
        }

        // ---------------NotifyIcon---------------

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            form_state(true);
        }

        /// <summary>
        /// �R���e�L�X�g���j���[��Ԃ�ύX���܂��B
        /// </summary>
        /// <param name="Force">Force Play���[�h�̕ύX���s���܂��B</param>
        /// <param name="Pause">Pause���[�h�̕ύX���s���܂��B</param>
        private void Change_QuickState(bool Force,bool Pause)
        {
            if (Pause != isPause) //�ύX����Ă���ꍇ
            {
                if (Pause)
                {
                    Update_Stop(); //�Ď���~
                    sc.AudioStop(); //�Đ���~
                    PauseToolStripMenuItem.Text = "Unpause";
                    notifyIcon1.Text = "TuneBar (Paused)";
                    if (settings.notify_active)
                    {
                        Notify.show_notify("Information", "Playback has been paused.");
                    }
                }
                else //�ʏ탂�[�h�֖߂��ꍇ
                {
                    Update_Start();
                    PauseToolStripMenuItem.Text = "Pause";
                    notifyIcon1.Text = "TuneBar";
                    if (settings.notify_active)
                    {
                        Notify.show_notify("Information", "Playback has been unpaused.");
                    }
                }
            }

            if(Force != isForce)
            {
                if (Force)
                {
                    forcePlayToolStripMenuItem.Text = "Return to normal mode";
                    notifyIcon1.Text = "TuneBar (Force playing)";
                    if (settings.notify_active)
                    {
                        Notify.show_notify("Information", "Force music to play.");
                    }
                }
                else
                {
                    forcePlayToolStripMenuItem.Text = "Force play";
                    notifyIcon1.Text = "TuneBar";
                    if (settings.notify_active)
                    {
                        Notify.show_notify("Information", "Return to normal operation.");
                    }
                }
            }

            Debug.WriteLine($"QuickState changed: ForcePlay: {Force} / Pause: {Pause}");
        }

        // �R���e�L�X�g���j���[��Force Play��I�񂾏ꍇ
        private void forcePlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Change_QuickState(!isForce, isPause); //T/F���]
        }

        // �R���e�L�X�g���j���[��Pause��I�񂾏ꍇ
        private void PauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Change_QuickState(isForce, !isPause);
        }

        // �R���e�L�X�g���j���[��Next track��I�񂾏ꍇ
        private void nextTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sc.AudioStop(true); //���̃g���b�N�������Đ�
        }

        // �R���e�L�X�g���j���[��Help��I�񂾏ꍇ
        private void helpGitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = "https://github.com/QuestDragon/TuneBar", //GitHub�T�C�g
                UseShellExecute = true,
            };

            Process.Start(pi);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form_state(true); //�t�H�[���\��
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        // �R���e�L�X�g���j���[��Quit App��I�񂾏ꍇ
        private void quitAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settings.notify_active)
            {
                Notify.show_notify("TuneBar was successfully closed!", "See you next time!");
            }
            Application.Exit(); //�A�v���P�[�V�����̏I��
        }
    }

    /// <summary>
    /// ���ꃊ�X�g�̎�ށB
    /// Ignore�͎w�肳�ꂽ�A�v�����v���O���X�o�[��ۗL���Ă��Ă�����̑ΏۊO�Ƃ���B
    /// Target�͋t�Ɏw�肳�ꂽ�A�v���P�[�V�����ɂ̂ݓ��삷��B
    /// IgnoreMPApp�͎w�肵���A�v�������y���Đ����Ă��Ă�����}���̑ΏۊO�Ƃ���B
    /// </summary>
    public enum ListData
    {
        Ignore,
        Target,
        IgnoreMPApp
    }
}
