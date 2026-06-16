namespace KwuTodoAI
{
    internal static class Program
    {
        [System.STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Python FastAPI 서버 자동 기동 (이미 실행 중이면 건너뜀)
            ServerLauncher.EnsureStarted();

            Application.Run(new LoginForm());

            // 우리가 띄운 서버라면 앱 종료 시 함께 종료
            ServerLauncher.Stop();
        }
    }
}
