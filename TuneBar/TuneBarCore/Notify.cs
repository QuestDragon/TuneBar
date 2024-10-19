using Microsoft.Toolkit.Uwp.Notifications;
using System.Diagnostics;

namespace TuneBar
{
    internal class Notify
    {
        /// <summary>
        /// トースト通知を作成し、通知を表示します。
        /// </summary>
        /// <param name="title">通知のタイトル</param>
        /// <param name="description">通知の本文をここに記述します。</param>
        public static async void show_notify(string title, string description, string pic_uri = "", string pic_path = "")
        {
            string uri_path = pic_uri;
            if(pic_uri == "")
            {
                string appPath = Application.StartupPath;
                uri_path = "file:///" + appPath + @"Images\info.png";
                // Debug.WriteLine(uri_path);
            }

            // トースト通知のコンテンツを作成
            new ToastContentBuilder()
                .AddAppLogoOverride(new Uri(uri_path), ToastGenericAppLogoCrop.Default,"info") // アイコンを追加
                .AddArgument("action", "viewDetails")
                .AddText(title)
                .AddText(description)
                .Show(); // トースト通知を表示

            ToastNotificationManagerCompat.OnActivated += ToastActivated;

            Debug.WriteLine("トースト通知が表示されました。");

            if(pic_path != "") //一時ファイルの削除
            {
                if (File.Exists(pic_path))
                {
                    await Task.Delay(1000); //1s wait

                    File.Delete(pic_path);
                    Debug.WriteLine("一時ファイルが削除されました: " + pic_path);
                }
                else
                {
                    Debug.WriteLine("指定された一時ファイルは存在しません: " + pic_path);
                }
            }
        }

        /// <summary>
        /// トースト通知がクリックされたときの処理
        /// </summary>
        /// <param name="e"></param>
        private static void ToastActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            // クリックされた際の処理を記述
            // 引数e.Argumentsから送信されたデータを取得できます
            if (e.Argument.Contains("viewDetails"))
            {
                // クリックされた後に行う処理
                Debug.WriteLine("トーストがクリックされました！アクション: " + e.Argument);
                // 必要なアクションを実行 (ウィンドウを開くなど)
                Form1.this_form.form_state(true);
            }
        }
    }
}
