# KWU Todo Client — Agent Context

## 프로젝트 개요

광운대 AI TODO 관리 앱 — C# WinForms (.NET 10) 클라이언트.
Python FastAPI 서버(kwu-todo-server, localhost:8000)와 HTTP로 통신한다.

- **빌드**: `dotnet build`
- **실행**: `dotnet run` 또는 `bin/Debug/net10.0-windows/KwuTodoAI.exe`
- **GitHub**: `https://github.com/neo-mwo-dawe/kwu-todo-client`
- **작업 브랜치**: `develop`
- **네임스페이스**: `KwuTodoAI`

---

## 아키텍처

```
Program.cs               # 진입점
MainForm.cs              # 메인 화면: 캘린더(좌) + TODO 카드 목록(우)
LoginForm.cs             # 로그인 화면
TodoForm.cs              # TODO 추가/수정 폼
ApiClient.cs             # HTTP 클라이언트 (localhost:8000)
DataManager.cs           # 로컬 JSON 저장소
Models/
  TodoItem.cs            # 데이터 모델 + 표시용 프로퍼티
Components/
  TodoCard.cs            # (참고용, 실제 카드는 MainForm.MakeCard로 인라인 생성)
MainForm_Integration.cs  # 빌드 제외 파일 (참고용만)
```

### MainForm 레이아웃 구조

```
Form (1080×700)
├── pnlHeader   [Dock=Top, H=52]   헤더 (제목, 날짜, 테마버튼)
├── pnlLeft     [Dock=Left, W=310] 캘린더 + 일정 목록
├── sep         [Dock=Left, W=1]   구분선
└── pnlRight    [Dock=Fill]        TODO 영역
    ├── pnlTodoTop  [Dock=Top, H=46]  제목 + AI생성/추가 버튼
    ├── pnlStats    [Dock=Top, H=32]  통계 (전체/긴급/높음/시험/과제)
    └── flpTodos    [Dock=Fill]       FlowLayoutPanel (TopDown)
        └── Panel (card) × N          TODO 카드
```

### TODO 카드 (MakeCard) 내부 구조

```
card [Panel, H=84, Width=flpTodos.ClientWidth-6]
├── barLeft   [Dock=Left,  W=4]    우선순위 색상 바
├── pnlRight  [Dock=Right, W=160]  D-Day 라벨 + 우선순위 배지 + 수정 버튼
├── pnlContent[Dock=Fill]          제목/부제목/카테고리 (Padding=8,4,4,4)
│   ├── lblTitle [Dock=Top, H=26]  제목 (AutoEllipsis=true)
│   ├── lblSub   [Dock=Top, H=20]  마감일 | 이유
│   └── lblCat   [Dock=Top, H=18]  카테고리
└── chk [Location=(6,33)]         완료 체크박스
```

**중요**: WinForms Dock 레이아웃은 Controls 컬렉션 역순으로 처리된다.
카드를 flpTodos에 추가하기 **전에** `card.Width`를 명시적으로 설정해야 한다.
그렇지 않으면 Width=0 상태로 Dock 레이아웃이 계산되어 텍스트가 표시되지 않는다.

```csharp
// RebuildTodoList() 내부 — 이 패턴을 반드시 유지할 것
int cardW = flpTodos.ClientSize.Width > 50 ? flpTodos.ClientSize.Width - 6 : 700;
foreach (var todo in _todos)
{
    var card = MakeCard(todo);
    card.Width = cardW;          // Dock 레이아웃이 올바른 폭으로 계산되도록 선행 설정
    flpTodos.Controls.Add(card);
}
```

---

## 빌드 및 실행

```bash
# Python 서버 먼저 실행 (kwu-todo-server 레포 참고)
python -m uvicorn main:app --reload

# C# 앱 빌드 & 실행
cd kwu-todo-client
dotnet build
dotnet run
```

---

## 주요 버그 수정 이력 (완료)

| 파일 | 문제 | 수정 내용 |
|------|------|-----------|
| `ApiClient.cs` | nullable 경고 | `_instance` 필드를 `ApiClient?` 로 변경 |
| `MainForm.cs` | TODO 카드 텍스트 잘림 (1차) | 우선순위 색 바/배지/수정버튼 위치가 (0,0)에 몰림 → Dock 기반 레이아웃으로 재설계 |
| `MainForm.cs` | TODO 카드 텍스트 잘림 (2차) | `flpTodos.Controls.Add()` 전에 `card.Width` 설정 |
| `KwuTodoAI.csproj` | .csproj 파일 없어 빌드 불가 | 파일 생성, `MainForm_Integration.cs` 빌드에서 제외 |

---

## 코드 작성 규칙

- UI 스레드 외에서 컨트롤 접근 시 반드시 `InvokeRequired` 체크
- `MainForm_Integration.cs`는 빌드에서 제외된 참고 파일 — 수정 금지
- WinForms Dock 레이아웃 시 Fill 컨트롤은 `Controls.Add` 순서에 유의
- 카드에 컨트롤 추가 순서: `pnlContent(Fill)` → `pnlRight(Right)` → `barLeft(Left)` → `chk(None)` (역순으로 레이아웃 처리됨)
- 서버 오프라인 시 `lblSummary`에 경고 메시지 표시 (MessageBox 금지)
- 작업 브랜치: `develop` (main은 최종 제출용)

---

## 색상 팔레트 (다크/라이트 테마)

```csharp
// MainForm 내 프로퍼티
BG       // 배경
SURFACE  // 카드/패널 배경
SURFACE2 // hover 배경
ACCENT   // 파란색 강조 (82, 130, 255)
TEXT     // 기본 텍스트
SUBTEXT  // 보조 텍스트
URGENT   // 긴급 빨강 (220, 60, 60)
HIGH_C   // 높음 주황 (230, 130, 30)
```
