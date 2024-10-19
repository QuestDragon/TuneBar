using MessagePack;
using System.Diagnostics;
using System.Threading;

namespace TuneBar
{
    internal class Serializer
    {
        static string app_path = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 設定データを保存します。
        /// </summary>
        /// <param name="settings">保存する設定データ。</param>
        public static void Serialize(SettingsData settings)
        {
            // オブジェクトをMessagePack形式でシリアライズ
            var bytes = MessagePackSerializer.Serialize(settings);

            File.WriteAllBytes(app_path + "data.bin", bytes); // Save

            // シリアライズされたデータをコンソールに出力（バイナリなので、読みやすい形に変換）
            Debug.WriteLine("バイナリとして下記のように保存しました：");
            Debug.WriteLine(BitConverter.ToString(bytes));

            if (settings.notify_active)
            {
                Notify.show_notify("Information", "The setting data was saved successfully.");
            }
        }
        
        /// <summary>
        /// 設定データを読み込みます。
        /// </summary>
        /// <returns>読み込んだ設定データ。</returns>
        public static SettingsData Deserialize()
        {
            byte[] bytes = Array.Empty<byte>(); //バイト配列初期化

            while(true)
            {
                bool occured_ex = false;

                try
                {
                    bytes = File.ReadAllBytes(app_path + "data.bin");
                }
                catch (IOException)
                {
                    occured_ex=true; //例外発生
                    Thread.Sleep(1000); // 遅延を挿入
                }

                if (!occured_ex)
                {
                    break;
                }
            }
            
            Debug.WriteLine("セーブデータを読み込みました。");

            return MessagePackSerializer.Deserialize<SettingsData>(bytes);
        }
    }

    /// <summary>
    /// 設定データ。
    /// </summary>
    [MessagePackObject]
    public class SettingsData
    {
        [Key(0)]
        public List<string> SongList { get; set; }
        [Key(1)]
        public List<string> IgnoreList { get; set; }
        [Key(2)]
        public List<string> TargetList { get; set; }
        [Key(3)]
        public List<string> IgnoreSPAList { get; set; }
        [Key(4)]
        public int Fade { get; set; }
        [Key(5)]
        public int Update { get; set; }
        [Key(6)]
        public bool dont_duplicate { get; set; }
        [Key(7)]
        public bool chrome_fix { get; set; }
        [Key(8)]
        public bool exp_only { get; set; }
        [Key(9)]
        public bool notify_active { get; set; }

        // コンストラクタ。New式でこのクラスを呼び出した際、引数の内容が自動で格納されてインスタンスが作成される処理。
        /// <summary>
        /// 設定データを作成します。
        /// </summary>
        /// <param name="songList">再生する曲のリスト。</param>
        /// <param name="ig_list">プログレスバーを保有しているウィンドウのうち、指定したプロセス名を動作させないリスト。</param>
        /// <param name="tg_list">プログレスバーを保有しているウィンドウのうち、指定したプロセス名のみ動作させるリスト。</param>
        /// <param name="ig_spa_list">音を鳴らすアプリケーションのうち、動作抑制の対象外とするプロセス名のリスト。</param>
        /// <param name="fade">再生する曲のフェード時間。（ms）</param>
        /// <param name="update">プログレスバーをチェックする間隔。（ms）</param>
        /// <param name="dup">音を鳴らすアプリケーションがある場合、動作を抑制する設定。</param>
        /// <param name="chrome">Google Chromeに関する問題を修正する設定。</param>
        /// <param name="exp">Explorer.exeにのみ動作させる設定。</param>
        /// <param name="notify">状態の変更を通知する設定。</param>
        public SettingsData (List<string> songList, List<string> ig_list, List<string> tg_list, List<string> ig_spa_list, int fade, int update,bool dup,bool chrome,bool exp,bool notify)
        {
            SongList = songList;
            IgnoreList = ig_list;
            TargetList = tg_list;
            IgnoreSPAList = ig_spa_list;
            Fade = fade;
            Update = update;
            dont_duplicate = dup;
            chrome_fix = chrome;
            exp_only = exp;
            notify_active = notify;
        }   
    }
}
