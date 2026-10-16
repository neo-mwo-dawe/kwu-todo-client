// =============================================================
//  ServerLauncher.cs — Python FastAPI 서버 자동 실행/종료
//  · 앱 시작 시 8000 포트가 비어 있으면 uvicorn 을 자동 기동
//  · 이미 서버가 떠 있으면 아무것도 하지 않음 (중복 실행/좀비 소켓 방지)
//  · 우리가 띄운 경우에만 앱 종료 시 함께 종료
//  · python 미설치/폴더 못 찾음 등 실패 시 조용히 폴백
//    (LoginForm 의 "서버 연결 실패" 안내로 수동 실행 가능)
// =============================================================

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace KwuTodoAI
{
    internal static class ServerLauncher
    {
        private const string HOST = "127.0.0.1";
        private const int    PORT = 8000;

        // 우리가 직접 띄운 서버 프로세스 (null = 이미 떠 있었거나 기동 실패)
        private static Process? _serverProcess;

        /// <summary>
        /// 서버가 떠 있지 않으면 uvicorn 을 백그라운드로 기동하고
        /// 준비될 때까지(최대 약 8초) 대기한다. 앱 시작 시 1회 호출.
        /// </summary>
        public static void EnsureStarted()
        {
            // 이미 실행 중이면 중복 기동 금지 (포트 충돌/좀비 소켓 방지)
            if (IsPortOpen())
                return;

            string? pythonDir = FindPythonDir();
            if (pythonDir == null)
                return;   // 서버 폴더(main.py) 못 찾음 → 수동 실행 폴백

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName         = "python",
                    Arguments        = "-m uvicorn main:app --host 127.0.0.1 --port 8000",
                    WorkingDirectory = pythonDir,
                    UseShellExecute  = false,
                    CreateNoWindow   = true,
                };
                _serverProcess = Process.Start(psi);
            }
            catch
            {
                // python 미설치/PATH 문제 등 → 조용히 폴백
                _serverProcess = null;
                return;
            }

            // 서버가 응답할 때까지 잠시 대기 (로그인 첫 시도 실패 방지)
            WaitUntilReady(8000);
        }

        /// <summary>우리가 띄운 서버라면 앱 종료 시 함께 종료한다.</summary>
        public static void Stop()
        {
            try
            {
                if (_serverProcess != null && !_serverProcess.HasExited)
                    _serverProcess.Kill(entireProcessTree: true);
            }
            catch { /* 종료 실패는 무시 */ }
        }

        // ── 내부 헬퍼 ─────────────────────────────────────────────

        // 8000 포트가 열려 있는지(서버 실행 중인지) 빠르게 확인
        private static bool IsPortOpen()
        {
            try
            {
                using var client = new TcpClient();
                var ar = client.BeginConnect(HOST, PORT, null, null);
                bool ok = ar.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(400));
                if (ok)
                {
                    client.EndConnect(ar);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // 포트가 열릴 때까지 폴링 (최대 timeoutMs)
        private static void WaitUntilReady(int timeoutMs)
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                if (IsPortOpen())
                    return;
                Thread.Sleep(300);
            }
        }

        // 실행 파일 위치에서 위로 올라가며 main.py 가 있는 Python 폴더 탐색
        private static string? FindPythonDir()
        {
            DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);
            for (int i = 0; i < 8 && dir != null; i++, dir = dir.Parent)
            {
                string candidate = Path.Combine(dir.FullName, "Python");
                if (File.Exists(Path.Combine(candidate, "main.py")))
                    return candidate;
            }
            return null;
        }
    }
}
