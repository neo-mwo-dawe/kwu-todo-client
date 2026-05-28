// =============================================================
//  TodoApi.cs — Python FastAPI 서버와의 공통 HTTP 헬퍼
//  · Add/Edit/Detail 폼이 공유하는 PUT/POST 메서드
// =============================================================
// [신규] 원본의 TodoForm.cs 내부에 있던 PUT/POST 헬퍼를 정적 클래스로 분리.
//        Add/Edit/Detail 세 폼이 동일한 통신 코드를 재사용하도록 통합.

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KwuTodoAI
{
    /// <summary>FastAPI 서버 호출을 위한 공통 정적 헬퍼.</summary>
    public static class TodoApi
    {
        public const string BASE_URL = "http://localhost:8000";

        private static readonly JsonSerializerOptions _json =
            new() { PropertyNameCaseInsensitive = true };

        /// <summary>POST 요청 후 응답을 T로 역직렬화. 실패 시 null + onError 콜백.</summary>
        public static async Task<T?> PostAsync<T>(
            HttpClient http, string endpoint, object body, Action<string> onError)
            where T : class
        {
            try
            {
                string json    = JsonSerializer.Serialize(body);
                var    content = new StringContent(json, Encoding.UTF8, "application/json");
                var    res     = await http.PostAsync(BASE_URL + endpoint, content);
                if (!res.IsSuccessStatusCode)
                {
                    onError($"서버 오류 {(int)res.StatusCode}: {res.ReasonPhrase}");
                    return null;
                }
                string resp = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(resp, _json);
            }
            catch (HttpRequestException)  { onError("서버에 연결할 수 없습니다.\nuvicorn main:app --reload 를 먼저 실행하세요."); return null; }
            catch (TaskCanceledException) { onError("요청 시간 초과."); return null; }
            catch (Exception ex)          { onError($"오류: {ex.Message}"); return null; }
        }

        /// <summary>PUT 요청. 성공 시 true.</summary>
        public static async Task<bool> PutAsync(
            HttpClient http, string endpoint, object body, Action<string> onError)
        {
            try
            {
                string json    = JsonSerializer.Serialize(body);
                var    content = new StringContent(json, Encoding.UTF8, "application/json");
                var    res     = await http.PutAsync(BASE_URL + endpoint, content);
                if (!res.IsSuccessStatusCode)
                {
                    onError($"서버 오류 {(int)res.StatusCode}: {res.ReasonPhrase}");
                    return false;
                }
                return true;
            }
            catch (HttpRequestException)  { onError("서버에 연결할 수 없습니다.\nuvicorn main:app --reload 를 먼저 실행하세요."); return false; }
            catch (TaskCanceledException) { onError("요청 시간 초과."); return false; }
            catch (Exception ex)          { onError($"오류: {ex.Message}"); return false; }
        }
    }
}
