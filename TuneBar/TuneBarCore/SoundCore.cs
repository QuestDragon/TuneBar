using System.Diagnostics;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using WinRT.Interop;

namespace TuneBar
{
    public class SoundCore
    {
        public static bool IsPlaySound = false; //再生中
        public static List<string> PlayingSoundApps = new List<string>();
        static bool IsGettingReady = false; //再生準備中
        static string current_file_path = "";

        /// <summary>
        /// 起動時にSoundCoreを使用するために実行する
        /// </summary>
        public static void init()
        {
            IsPlaySound = false;
            IsGettingReady = false;
        }

        /// <summary>
        /// 他のアプリケーションが音を出しているかを検知する
        /// </summary>
        /// <returns>Trueで音が出されています。</returns>
        static AudioSessionReturn AudioSessionChecker()
        {
            PlayingSoundApps.Clear();
            AudioSessionReturn res = new AudioSessionReturn(false);
            SettingsData settings = Serializer.Deserialize();

            // MMDeviceEnumeratorを使用してデフォルトのオーディオエンドポイントを取得
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            // AudioMeterInformation audioMeter = defaultDevice.AudioMeterInformation;

            // Debug.WriteLine($"CurrentAudioMeter (All): {audioMeter.MasterPeakValue}");

            // 現在のセッションのオーディオレベルを監視
            Debug.WriteLine("オーディオセッションを確認しています...");
            for (int i = 0; i < defaultDevice.AudioSessionManager.Sessions.Count; i++)
            {
                var session = defaultDevice.AudioSessionManager.Sessions[i];
                AudioMeterInformation audioMeter = session.AudioMeterInformation;

                // 各セッションのプロセスIDと名前を取得
                int processId = (int)session.GetProcessID;
                try
                {
                    string processName = Process.GetProcessById(processId).ProcessName;

                    // セッションの音量設定を取得 → GPTのトラップやわ
                    float volume = session.SimpleAudioVolume.Volume;
                    bool isMuted = session.SimpleAudioVolume.Mute;
                    // というわけで修正版がこちら
                    float Mastervolume = audioMeter.MasterPeakValue;

                    Debug.WriteLine($"Checking: {processName} ({processId})");
                    Debug.WriteLine($"Vol: {Mastervolume} / Mute: {isMuted}");

                    // 音が出ているか確認（ミュート状態でなく、音量が0より大きい場合）
                    if (!isMuted && Mastervolume > 0)
                    {
                        Debug.WriteLine($"SPA_Added: {processName} ({processId})");
                        PlayingSoundApps.Add(processName + ".exe");

                        if (processName != "TuneBar" && !settings.IgnoreSPAList.Contains(processName.ToLower())) //TuneBarとIgnoreリストのアプリ以外が音を出している場合
                        {
                            Debug.WriteLine($"プロセス: {processName} (ID: {processId}) は音を出しています。");
                            res = new AudioSessionReturn(true,processName,processId); //音が鳴っていることを伝える
                        }
                    }
                }
                catch (ArgumentException)
                {
                    Debug.WriteLine($"ID {processId} のプロセスは実行されていません。");
                }
            }

            return res;
        }

        /// <summary>
        /// 音を出している他アプリケーションの一覧を取得します。
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSPA()
        {
            AudioSessionChecker(); //取得するため、チェッカーを実行
            return PlayingSoundApps;
        }

        /// <summary>
        /// アルバムアートを取得します。
        /// </summary>
        /// <param name="filePath">音楽ファイルのパスを指定</param>
        /// <returns>取得したアルバムアート。取得に失敗した場合はNullです。</returns>
        private Bitmap GetAlbumArt(string filePath)
        {
            // TagLib#を使って音楽ファイルを読み込む
            var file = TagLib.File.Create(filePath);

            // アルバムアートを取得
            var tags = file.Tag.Pictures;

            if (tags.Length > 0)
            {
                // 最初の画像を取得
                var picture = tags[0];
                using (var ms = new MemoryStream(picture.Data.Data))
                {
                    // MemoryStreamからBitmapを作成
                    return new Bitmap(ms);
                }
            }

            // ジャケット写真が存在しない場合はnullを返す
            return null;
        }

        //GPTによる知識
        //フィールドの宣言 (型 変数名;) だけであれば、単にメモリ上にその変数の場所を確保することを意味しています。
        //オブジェクトの生成（new） は、特定の条件やタイミング（例えば、メソッド内）で行い、生成されたインスタンスを使います。
        //この方法により、必要なタイミングでしかオブジェクトを作成しないため、効率的ですし、初期化されていない状態でのアクセスによるエラーも防げます。

        //再生担当
        WaveOutEvent waveOut = new WaveOutEvent();
        //ファイル読み込み担当
         AudioFileReader reader; //null許容
        // フェードイン・フェードアウトを処理するプロバイダ
        FadeInOutSampleProvider fadeInOutProvider;

        /// <summary>
        /// 音楽を再生します。
        /// </summary>
        /// <param name="isForce">他のアプリケーションが音を鳴らしているかの判定をスキップします。</param>
        public async void AudioPlay(bool isForce = false)
        {
            if (IsGettingReady)
            {
                Debug.WriteLine("TuneBarは音楽を再生する準備中です…");
                return;
            }

            if (!isForce) //強制再生でない場合
            {
                AudioSessionReturn ASR = AudioSessionChecker(); //音が鳴っているアプリケーションをチェック

                if (ASR.IsPlayAudio) //鳴っていたら
                {
                    string proc_name = ASR.SPA_name;
                    int proc_id = ASR.SPA_ID;

                    Debug.WriteLine("TuneBar停止 - 他アプリケーションが音を鳴らしています");
                    Form2.Update_StripLabel($"Playback has been stopped by the following application: {proc_name}.exe ({proc_id})");
                    AudioStop(); //鳴っている場合はTuneBar停止
                    return;
                }
            }

            if (IsPlaySound)
            {
                Debug.WriteLine("TuneBarは音楽を再生中です…");
                Form2.Update_StripLabel();
                return;
            }

            IsGettingReady = true; //重複動作のブロック

            SettingsData settings = Serializer.Deserialize();

            // 再生開始
            if (waveOut.PlaybackState == PlaybackState.Paused)
            {
                waveOut.Play();
                Form1.update_current_song("Now playing: " + current_file_path);
                Form2.Update_StripLabel();
                IsPlaySound = true;
                Debug.WriteLine("再生再開");
                // 非同期にフェードイン処理
                fadeInOutProvider.BeginFadeIn(settings.Fade);
                await Task.Delay(settings.Fade);
            }
            else
            {

                if (settings.SongList.Count == 0) //音楽ファイルが指定されていない場合は再生できないのでここで終了
                {
                    Debug.WriteLine("音楽ファイルが指定されていません。フォームから音楽ファイルを指定して下さい。");
                    IsGettingReady = false;
                    return;
                }

                int ind = new Random().Next(0, settings.SongList.Count);

                waveOut.PlaybackStopped -= OnPlaybackStopped; //既存のイベントハンドラを削除
                waveOut.PlaybackStopped += OnPlaybackStopped; //登録し直し

                Debug.WriteLine("Selected song: " + settings.SongList[ind]);
                current_file_path = settings.SongList[ind];

                #region 通知用のアルバムアートを取得
                Bitmap albumArt = GetAlbumArt(current_file_path);
                string fileUri = "";
                string filepath = "";
                if (albumArt != null)
                {
                    // ジャケット写真が取得できた場合の処理
                    // 一時ファイルのパスを作成
                    filepath = Application.StartupPath + @$"Images\{ind}.png";
                    fileUri = "file:///" + filepath;

                    // 画像を一時ファイルとして保存
                    albumArt.Save(filepath, System.Drawing.Imaging.ImageFormat.Png); // 保存例
                }
                else
                {
                    Debug.WriteLine("ジャケット写真が見つかりませんでした。");
                }

                #endregion

                // 音声ファイルとフェードプロバイダを新たに作成
                reader = new AudioFileReader(current_file_path);
                fadeInOutProvider = new FadeInOutSampleProvider(reader.ToSampleProvider(), true);
                // waveOut.DeviceNumber = -1; // デフォルトデバイスを使う
                waveOut.Init(fadeInOutProvider);

                waveOut.Play();
                IsPlaySound = true;
                Form1.update_current_song("Now playing: " + current_file_path);
                Form2.Update_StripLabel();
                Debug.WriteLine("再生開始:" + current_file_path);

                if(settings.notify_active) //通知有効時
                {
                    Notify.show_notify("Now playing", Path.GetFileName(current_file_path), fileUri,filepath);
                }

                // 非同期にフェードイン処理
                fadeInOutProvider.BeginFadeIn(settings.Fade);
                await Task.Delay(settings.Fade);
            }

            IsGettingReady = false; //AudioPlay実行可能
        }

        /// <summary>
        /// 音楽再生を一時停止します。
        /// </summary>
        /// <param name="next_track">Trueにすると現在再生中の音楽を停止します。</param>
        public async void AudioStop(bool next_track = false)
        {
            if (waveOut.PlaybackState == PlaybackState.Playing)
            {

                SettingsData settings = Serializer.Deserialize();
                fadeInOutProvider.BeginFadeOut(settings.Fade);
                await Task.Delay(settings.Fade); //非同期処理：UI（フォーム）のフリーズを防ぐためのもの。Pauseの処理には指定した時間待機しないと進まないのでご安心を。

                waveOut.Pause();
                IsPlaySound = false;
                Form1.update_current_song("It's not currently playing music.");
                Form2.Update_StripLabel();
                Debug.WriteLine("再生一時停止");

            }

            //次のトラックへ強制移行させる場合
            if(next_track)
            {
                Debug.WriteLine("次のトラックを強制的に再生します…。");
                waveOut.Stop(); //トラックを停止し、OnPlaybackStoppedを呼び出す
            }
        }

        /// <summary>
        /// Naudioにて再生している音楽が終了した場合に呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">停止した際にエラーが発生した場合、その理由がここに入ります。</param>
        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            reader.Dispose();
            IsPlaySound = false;
            current_file_path = "";

            if (e.Exception == null)
            {
                Debug.WriteLine("再生が終了しました。");
            }
            else
            {
                SettingsData settings = Serializer.Deserialize();
                Debug.WriteLine($"再生中にエラーが発生しました: {e.Exception.Message}");
                if (settings.notify_active)
                {
                    string file_Path = Application.StartupPath + @"Images\ban.png";
                    string file_uri = "file:///" + file_Path;

                    Notify.show_notify("The following error occurred during playback:", e.Exception.Message, file_uri, file_Path);
                }
                else
                {
                    MessageBox.Show("The following error occurred during playback:\n\n" + e.Exception.Message, "TuneBar - Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                
            }
        }
    }

    /// <summary>
    /// オーディオセッションが有効であるか、有効な場合の該当ウィンドウ情報を格納するクラス
    /// </summary>
    class AudioSessionReturn
    {
        public bool IsPlayAudio { get; set; }
        public string SPA_name { get; set; }
        public int SPA_ID { get; set; }

        public AudioSessionReturn(bool isPlayAudio, string name = "", int id = 0)
        {
            IsPlayAudio = isPlayAudio;
            SPA_name = name;
            SPA_ID = id;
        }   
    }
}
