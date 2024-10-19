using System.Diagnostics;

namespace TuneBar
{
    public partial class Form1 : Form
    {
        SettingsData settings;
        private static ToolStripLabel songname_label;
        public static Form1 this_form;
        public static SoundCore sc;

        // コンテキストメニュー状態の保存
        private bool isForce = false;
        private bool isPause = false;

        public Form1()
        {
            InitializeComponent();

            this_form = this;
            songname_label = toolStripStatusLabel1;
            songname_label.Text = "It's not currently playing music.";

            sc = new SoundCore(); //SoundCore初期化

            string app_path = AppDomain.CurrentDomain.BaseDirectory;

            string fileName = app_path + "data.bin";
            if (File.Exists(fileName))
            {
                Debug.WriteLine("'" + fileName + "'は存在します。");
                try
                {
                    settings = Serializer.Deserialize();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Err: {ex.Message}");
                    Debug.WriteLine("設定データを再作成します…。");
                    settings = new SettingsData(new List<string>(), new List<string>(), new List<string>(), new List<string>(), 500, 500, false, false, false,false); // new List<string>()だけでListをNew式として宣言できる。
                    Serializer.Serialize(settings); //Save

                    MessageBox.Show("A problem occurred with the configuration data, so it has been reset. \nPlease set it again.", "TuneBar - Your settings have been reset.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                Debug.WriteLine("'" + fileName + "'は存在しません。");
                settings = new SettingsData(new List<string>(), new List<string>(), new List<string>(), new List<string>(), 500, 500, false, false, false,false); // new List<string>()だけでListをNew式として宣言できる。
                Serializer.Serialize(settings); //Save
            }

            form_state(false);
            // timer2.Enabled = true;

            // SoundCore.init(); //SoundCore初期化(エラーのため一時的に使用禁止）
            // Thread.Sleep(1000);
            Update_Start(); //作動開始
        }

        /// <summary>
        /// フォームの内容を再描画します。
        /// </summary>
        private void repaint()
        {
            listBox1.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";

            // 設定内容を反映
            listBox1.Items.AddRange(settings.SongList.ToArray()); //ItemsのAddRangeでList型の中身をそのまま写せる。
            textBox1.Text = settings.Fade.ToString();
            textBox2.Text = settings.Update.ToString();
            checkBox1.Checked = settings.dont_duplicate;
            checkBox2.Checked = settings.chrome_fix;
            checkBox3.Checked = settings.exp_only;
            checkBox4.Checked = settings.notify_active;

            checkBox2.Enabled = !checkBox3.Checked; //Exp onlyの時はFix Chrome使用不可
            button5.Enabled = !checkBox3.Checked; //Exp onlyの時はList使用不可
            button6.Enabled = !checkBox3.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            repaint();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; //Formがなくなってしまうのを防ぐ（「閉じる」の効果を無効にする）
            form_state(false);
        }

        /// <summary>
        /// フォームの表示状態を切り替えます。
        /// </summary>
        /// <param name="is_show">表示有無の選択</param>
        public void form_state(bool is_show)
        {
            // フォームまたはコントロールに対してInvokeRequiredをチェック
            if (InvokeRequired) //トースト通知などからアクセスされると別スレッド扱いになってInvalidOperationExceptionになるので、Invokeが必要かどうかを判断する。
            {
                Invoke(new Action<bool>((val) => form_state(val)), is_show); //Invokeを使用する必要がある場合は再帰させて実行（再帰：form_stateメソッドの中でform_stateメソッドを呼び出すこと）
                return; //これ以下はInvokeを必要としない場合の処理なので実行しない
            }

            // Invokeを使用しない場合（Formから直接など）は直接ここに来る。Invokeを使う場合はInvokeの中で呼ばれてやってくる。

            if (is_show) //表示と同時に描画し直し
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

            Debug.WriteLine("表示状態：" + is_show);
        }

        /// <summary>
        /// Addボタン
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            #region ファイルを開くダイアログ設定
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            ofd.FileName = "";
            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            //今回はミュージックフォルダ。
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            string music_files = "Music files(*.mp3;*.wav;)|*.mp3;*.wav";
            string mp3_files = "MP3 files(*.mp3)|*.mp3";
            string wave_files = "Wave files(*.wav)|*.wav";

            ofd.Filter = music_files + "|" + mp3_files + "|" + wave_files;
            //[ファイルの種類]ではじめに選択されるものを指定する
            //1番目の「music_files」が選択されているようにする
            ofd.FilterIndex = 1;
            //タイトルを設定する
            ofd.Title = "Selecting a music file";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;
            //複数のファイルを選択できるようにする
            ofd.Multiselect = true;

            #endregion

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名（パス）をすべて表示する
                foreach (string fn in ofd.FileNames)
                {
                    Debug.WriteLine(fn);
                    listBox1.Items.Add(fn);
                }
            }

        }

        /// <summary>
        /// Removeボタン
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift) //Shiftキーを一緒に押した場合
            {
                var conf = MessageBox.Show("Do you want to delete all the songs you added?\n (This will only clear the list, not the original files)", "Delete all songs", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (conf == DialogResult.OK)
                {
                    listBox1.Items.Clear(); //全削除
                    Debug.WriteLine("Song list has been cleared!");
                    return;
                }
            }

            if (listBox1.SelectedItems.Count != 0) //選ばれていない状態でない時
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
        /// OKを押したら
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            List<string> new_song_list = listBox1.Items.Cast<string>().ToList();

            // 設定反映と保存
            SettingsData new_settings = new SettingsData(new_song_list, settings.IgnoreList, settings.TargetList, settings.IgnoreSPAList, int.Parse(textBox1.Text), int.Parse(textBox2.Text), checkBox1.Checked, checkBox2.Checked, checkBox3.Checked,checkBox4.Checked);
            Serializer.Serialize(new_settings); //Save
            settings = new_settings;

            form_state(false);
            Update_Start(); //インターバル設定適用
        }

        /// <summary>
        /// Cancelボタン
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            form_state(false);
        }

        /// <summary>
        /// Ignore Listボタン
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {

            Form2 f2 = new Form2(ListData.Ignore); //Form作成時にこの情報を操作するように、という情報を渡す
            f2.ShowDialog();
            settings = Serializer.Deserialize(); //最新のセーブデータを受け取る
        }

        /// <summary>
        /// Target Listボタン
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {

            Form2 f2 = new Form2(ListData.Target); //Form作成時にこの情報を操作するように、という情報を渡す
            f2.ShowDialog();
            settings = Serializer.Deserialize(); //最新のセーブデータを受け取る
        }

        /// <summary>
        /// Ignore Listボタン（他アプリケーションのオーディオ再生中は動作停止の部分）
        /// </summary>
        private void button7_Click(object sender, EventArgs e)
        {

            Form2 f2 = new Form2(ListData.IgnoreMPApp); //Form作成時にこの情報を操作するように、という情報を渡す
            f2.ShowDialog();
            settings = Serializer.Deserialize(); //最新のセーブデータを受け取る
        }

        // Explorer Only設定を変えた場合
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Enabled = !checkBox3.Checked; //エクスプローラー専用がオンの時はChromeFixは無効にする
        }

        // ----------Update----------
        /// <summary>
        /// 一定周期でプログレスバーの監視と音楽再生を担います。
        /// </summary>
        private void Update_Start()
        {
            if (timer1.Enabled) //作動中の場合
            {
                Update_Stop(); //停止してインターバル設定後、再始動
                Thread.Sleep(1000); //1秒待機
            }

            // タイマーの間隔(ミリ秒)
            timer1.Interval = settings.Update;

            timer1.Start(); //監視スタート
            GetNewWindow.StartMonitoring();
            Debug.WriteLine("TuneBar Started.");
            if (settings.notify_active)
            {
                Notify.show_notify("Information", "Activated!");
            }
        }

        private void Update_Stop()
        {
            timer1.Stop(); //監視停止
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
                // 長時間かかる処理

                if (notifyIcon1.Text == "TuneBar") //通常時
                {
                    GetWindow.Window_Handler(); //これが音楽再生を判断するメソッドとなる。
                }
                else
                {
                    sc.AudioPlay(true); //強制再生する（プログレスバー判定、他アプリケーションのオーディオ再生を無視）
                }
            });
        }

        /// <summary>
        /// デバッグ用
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
        /// コンテキストメニュー状態を変更します。
        /// </summary>
        /// <param name="Force">Force Playモードの変更を行います。</param>
        /// <param name="Pause">Pauseモードの変更を行います。</param>
        private void Change_QuickState(bool Force,bool Pause)
        {
            if (Pause != isPause) //変更されている場合
            {
                if (Pause)
                {
                    Update_Stop(); //監視停止
                    sc.AudioStop(); //再生停止
                    PauseToolStripMenuItem.Text = "Unpause";
                    notifyIcon1.Text = "TuneBar (Paused)";
                    if (settings.notify_active)
                    {
                        Notify.show_notify("Information", "Playback has been paused.");
                    }
                }
                else //通常モードへ戻す場合
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

        // コンテキストメニューのForce Playを選んだ場合
        private void forcePlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Change_QuickState(!isForce, isPause); //T/F反転
        }

        // コンテキストメニューのPauseを選んだ場合
        private void PauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Change_QuickState(isForce, !isPause);
        }

        // コンテキストメニューのNext trackを選んだ場合
        private void nextTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sc.AudioStop(true); //次のトラックを強制再生
        }

        // コンテキストメニューのHelpを選んだ場合
        private void helpGitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = "https://github.com/QuestDragon/TuneBar", //GitHubサイト
                UseShellExecute = true,
            };

            Process.Start(pi);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form_state(true); //フォーム表示
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        // コンテキストメニューのQuit Appを選んだ場合
        private void quitAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settings.notify_active)
            {
                Notify.show_notify("TuneBar was successfully closed!", "See you next time!");
            }
            Application.Exit(); //アプリケーションの終了
        }
    }

    /// <summary>
    /// 特殊リストの種類。
    /// Ignoreは指定されたアプリがプログレスバーを保有していても動作の対象外とする。
    /// Targetは逆に指定されたアプリケーションにのみ動作する。
    /// IgnoreMPAppは指定したアプリが音楽を再生していても動作抑制の対象外とする。
    /// </summary>
    public enum ListData
    {
        Ignore,
        Target,
        IgnoreMPApp
    }
}
