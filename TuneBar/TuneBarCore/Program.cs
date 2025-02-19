namespace TuneBar
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Form1 mainForm = new();
            SplashForm splashForm = new SplashForm();

            //スプラッシュウィンドウを表示
            splashForm.Show();

            Application.Run(mainForm);
        }
    }
}