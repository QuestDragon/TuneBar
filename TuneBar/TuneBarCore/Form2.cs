using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace TuneBar
{
    public partial class Form2 : Form
    {

        private ListData list_type;
        private static ToolStripStatusLabel current_app_name;
        private static TextBox add_process_box;

        public Form2(ListData ld)
        {
            InitializeComponent();

            Debug.WriteLine("ListData: " + ld);

            current_app_name = toolStripStatusLabel1;
            add_process_box = textBox1;
            Text = "TuneBar - ";
            current_app_name.Text = "There is no window that satisfies the conditions for TuneBar to work.";

            switch (ld)
            {
                case ListData.Ignore: //除外リスト
                    Text += "Ignore list";
                    label1.Text = "Ignore list (TuneBar will not play music for the progress bar of the specified application.)";
                    break;
                case ListData.Target: //対象リスト
                    Text += "Target list";
                    label1.Text = "Target list (TuneBar will only play music for the progress bar of the specified application.)";
                    break;
                case ListData.IgnoreMPApp: //音楽再生による抑制無視リスト
                    Text += "Ignored sound playing apps list";
                    label1.Text = "Ignored SPA's list (TuneBar will play music even if the specified application is playing sound.)";
                    break;
            }

            //他のメソッドでも使えるようにしておく
            list_type = ld;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SettingsData settings = Serializer.Deserialize(); //Load

            button5.Text = "Current process name list";

            // ListDataによってわける。
            switch (list_type)
            {
                case ListData.Ignore: //除外リスト
                    listBox1.Items.AddRange(settings.IgnoreList.ToArray()); //ItemsのAddRangeでList型の中身をそのまま写せる。
                    break;
                case ListData.Target: //対象リスト
                    listBox1.Items.AddRange(settings.TargetList.ToArray()); //ItemsのAddRangeでList型の中身をそのまま写せる。
                    break;
                case ListData.IgnoreMPApp: //音楽再生による抑制無視リスト
                    listBox1.Items.AddRange(settings.IgnoreSPAList.ToArray()); //ItemsのAddRangeでList型の中身をそのまま写せる。
                    button5.Text = "Current sound playing app list";
                    break;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckAnyChange())
            {
                var conf = MessageBox.Show("Any changes you made will be discarded.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (conf == DialogResult.Cancel)
                {
                    e.Cancel = true; //Formがなくなってしまうのを防ぐ（「閉じる」の効果を無効にする）
                }
            }
        }

        /// <summary>
        /// Form2のウィンドウ下部に表示する内容である、ホストアプリケーション名の表示を更新します。
        /// </summary>
        /// <param name="override_text">ホストアプリケーション名の代わりに表示する文字列。</param>
        public static void Update_StripLabel(string override_text = "")
        {
            // フォームが起動後1度も開かれていないとNullなので実行しない
            if(current_app_name == null)
            {
                return;
            }

            // 再生中の場合はきっかけのウィンドウプロセス名を表示
            string host_window = GetWindow.GetHostWindowInfo();

            if (host_window == "")
            {
                host_window = "There is no window that satisfies the conditions for TuneBar to work.";
            }

            if(override_text != "")
            {
                host_window = override_text;
            }

            // コントロールにアクセスするスレッドが違う場合はInvokeを使用
            try
            {
                if (current_app_name.Owner.InvokeRequired)
                {
                    current_app_name.Owner.Invoke(new Action(() =>
                    {
                        current_app_name.Text = host_window;
                        add_process_box.Focus();
                    }));
                }
                else
                {
                    current_app_name.Text = host_window;
                    add_process_box.Focus();
                }
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Update_StripLabelは実行を拒否しました。");
            }
        }

        /// <summary>
        /// 変更内容が未保存であるかを確認します。
        /// </summary>
        /// <returns>未保存はTrueが返ります。</returns>
        private bool CheckAnyChange()
        {
            SettingsData settings = Serializer.Deserialize();
            List<string> current_list = listBox1.Items.Cast<string>().ToList();
            List<string> settings_list = new List<string>(); // ListTypeによって中身が変わる

            switch (list_type)
            {
                case ListData.Ignore: //除外リスト
                    settings_list = settings.IgnoreList;
                    break;
                case ListData.Target: //対象リスト
                    settings_list = settings.TargetList;
                    break;
                case ListData.IgnoreMPApp: //音楽再生による抑制無視リスト
                    settings_list = settings.IgnoreSPAList;
                    break;
            }

            if (current_list.Count == settings_list.Count) //どちらもデータが未登録の場合
            {
                return false; //内容にかかわらず未保存ではないと判定 ※そもそもデータどっちもないのに未保存も何もないし
            }

            Debug.WriteLine($"List: {current_list.Count} / Settings: {settings_list.Count}");

            return current_list != settings_list;
        }

        /// <summary>
        /// Addボタン
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            SettingsData settings = Serializer.Deserialize();

            if (textBox1.Text == "") //未入力時はメッセージボックス表示のみ
            {
                MessageBox.Show("Please enter the process name. The extension (.exe) is not necessary.", "Process name is empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string add_proc = textBox1.Text.ToLower(); //大文字小文字区別なしにするため小文字変換指定
            //大丈夫だと思うけど、exe拡張子チェック
            if (add_proc.EndsWith(".exe"))
            {
                add_proc = add_proc.Replace(".exe", ""); //exe削除
            }

            if (listBox1.Items.Contains(add_proc)) //すでにある項目は追加しない ※余談だけど曲目リストは重複OKにしておく。確率操作とかしたい人向け。
            {
                MessageBox.Show("This process name has already been added.", "Process name already added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if(list_type != ListData.IgnoreMPApp && settings.chrome_fix && add_proc == "chrome")
            {
                MessageBox.Show("This process name cannot be added when Fix Google Chrome issue is active.", "Unavailable process name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            listBox1.Items.Add(add_proc); //追加
            textBox1.Text = "";

            CheckAnyChange(); //変更の確認
        }

        /// <summary>
        /// Removeボタン
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift) //Shiftキーを一緒に押した場合
            {
                var conf = MessageBox.Show("Do you want to remove all items added to the list?", "Delete all items", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (conf == DialogResult.OK)
                {
                    listBox1.Items.Clear(); //全削除
                    Debug.WriteLine("list has been cleared!");
                    return;
                }
            }

            if (listBox1.SelectedItems.Count != 0) //選ばれていない状態でない時
            {
                var conf = MessageBox.Show("The selected item will be deleted. Are you sure?\n\n- " + listBox1.SelectedItem, "Delete selected item", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
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
            // 設定反映と保存
            List<string> new_list = listBox1.Items.Cast<string>().ToList();
            SettingsData settings = Serializer.Deserialize();
            SettingsData new_settings = settings;

            //以前の設定を上書き
            switch (list_type)
            {
                case ListData.Ignore: //除外リスト
                    new_settings = new SettingsData(settings.SongList, new_list,settings.TargetList,settings.IgnoreSPAList, settings.Fade, settings.Update, settings.dont_duplicate, settings.chrome_fix, settings.exp_only, settings.notify_active);
                    break;
                case ListData.Target: //対象リスト
                    new_settings = new SettingsData(settings.SongList,settings.IgnoreList , new_list, settings.IgnoreSPAList, settings.Fade, settings.Update, settings.dont_duplicate, settings.chrome_fix, settings.exp_only, settings.notify_active);
                    break;
                case ListData.IgnoreMPApp: //音楽再生による抑制無視リスト
                    new_settings = new SettingsData(settings.SongList, settings.IgnoreList, settings.TargetList, new_list, settings.Fade, settings.Update, settings.dont_duplicate, settings.chrome_fix, settings.exp_only, settings.notify_active);
                    break;
            }

            Serializer.Serialize(new_settings); //Save

            Close();
        }

        /// <summary>
        /// Cancelボタン
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Current process name list ボタン
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            List<string> proclist = new();
            string lists = "";

            if (list_type == ListData.IgnoreMPApp)
            {
                List<string> raw_list = new List<string>();

                raw_list = SoundCore.GetSPA();
                IEnumerable<string> result = raw_list.Distinct(StringComparer.InvariantCultureIgnoreCase); //重複削除

                proclist = result.ToList();
            }
            else
            {
                WindowHandle_Data wi = GetWindow.GetWindow_Datas();

                // まずは重複しないように情報収集
                foreach (int handleID in wi.WHD.Keys)
                {
                    string process = wi.WHD[handleID].ProcName;
                    if (!proclist.Contains(process + ".exe"))
                    {
                        proclist.Add(process + ".exe");
                    }
                }
            }


            // 表示準備
            foreach (string pr in proclist)
            {
                if (listBox1.Items.Contains(pr.ToLower().Replace(".exe", ""))) //ListBoxではexeの表示がないため、exeを抜きにしてチェックにかける。
                {
                    lists += pr + " (Added)\n"; //追加済みの場合は末尾にAdded追加
                }
                else
                {
                    lists += pr + "\n";
                }
            }

            // 何も登録されなかった場合（特に音楽再生中のアプリを取得するとき、どのアプリも再生していなかった場合）
            if(proclist.Count == 0)
            {
                lists = "There are no entries in this list.";
            }

            MessageBox.Show(lists,button5.Text,MessageBoxButtons.OK,MessageBoxIcon.Information);

        }
    }
}
