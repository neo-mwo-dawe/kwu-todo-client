# KWU Todo Client

광운대 학생을 위한 AI 기반 TODO 관리 앱 — C# WinForms 클라이언트

## 실행 전 필수 조건

**Python 백엔드 서버를 먼저 실행해야 합니다.**
→ [kwu-todo-server](https://github.com/your-org/kwu-todo-server) 참고

서버 없이도 기존 로컬 데이터는 조회할 수 있습니다.

## 실행 방법

1. Visual Studio에서 `KWUStudentManager.sln` 열기
2. NuGet 패키지 복원 (자동 or `도구 > NuGet 패키지 관리자 > 솔루션 복원`)
3. `F5` 실행

## 프로젝트 구조

```
KWUStudentManager/
├── MainForm.cs          # 메인 화면 (캘린더 + TODO)
├── TodoForm.cs          # TODO 상세/추가 폼
├── DataManager.cs       # JSON 로컬 저장 (AppData)
├── ApiClient.cs         # Python 서버 HTTP 통신
├── Models/
│   └── TodoItem.cs      # 데이터 모델
└── Components/
    └── TodoCard.cs      # 우선순위 카드 커스텀 컨트롤
```

## 로컬 데이터 저장 경로

```
C:\Users\{사용자명}\AppData\Roaming\KWUStudentManager\
├── todos.json
├── schedules.json
└── courses.json
```

## API 연결 정보

| 항목 | 값 |
|------|-----|
| 서버 주소 | `http://localhost:8000` |
| 타임아웃 | 30초 (AI 생성 고려) |

## 우선순위 색상

| 우선순위 | 색상 코드 |
|---------|----------|
| 높음 | `#EF4444` |
| 보통 | `#F97316` |
| 낮음 | `#22C55E` |

## 역할 분담

| 팀원 | 담당 파일 |
|------|----------|
| 지원 | `DataManager.cs`, `ApiClient.cs`, `TodoItem.cs` |
| 유빈 | `MainForm.cs`, `TodoForm.cs`, `TodoCard.cs` |

## 브랜치 전략

```
main      ← 최종 제출본
develop   ← 통합 브랜치 (PR 대상)
feature/{이름}/{기능}
fix/{이름}/{내용}
```
