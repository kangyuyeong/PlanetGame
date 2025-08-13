# Planet Game
🌎🪐👽👩🏻‍🚀🚀🛰️🌑

> **Unity 2D Physics + Merge Puzzle Game**  
> 수박 게임(Suika Game) 규칙을 응용해 만든 행성 합치기 게임입니다.  
> 같은 레벨의 행성이 만나면 더 큰 행성으로 변하고, 행성이 데스 라인을 넘으면 게임이 종료됩니다.

[▶ 게임 플레이하기 (itch.io)]  (https://yuyeongkang.itch.io/planet-game)


## 게임 규칙

- **행성 레벨**: 0 ~ 7 (총 8단계)
- **합치기 규칙**: 같은 레벨의 행성끼리 닿으면 레벨이 1 증가
- **점수 시스템**:  
  - 레벨업 시 `score += 2^level`
  - 더 높은 레벨의 행성을 만들수록 점수가 커짐
- **게임 종료 조건**:  
  - 지정된 데스 라인(Finish)에 행성이 일정 시간(약 3초 이상) 닿으면 행성이 하얗게 변함
  - 5초 이상 닿아 있으면 **모든 행성이 사라지고 게임 종료**


## 행성 이미지
- 아래의 이미지들은 ProCreate를 이용해 직접 제작하였습니다.
<p>
  <img src="GamePlay/동글0.png" alt="동글 0" width="100">
  <img src="GamePlay/동글1.png" alt="동글 1" width="100">
  <img src="GamePlay/동글2.png" alt="동글 2" width="100">
  <img src="GamePlay/동글3.png" alt="동글 3" width="100">
  <img src="GamePlay/동글4.png" alt="동글 4" width="100">
  <img src="GamePlay/동글5.png" alt="동글 5" width="100">
  <img src="GamePlay/동글6.png" alt="동글 6" width="100">
  <img src="GamePlay/동글7.png" alt="동글 7" width="100">
</p>


## 게임 플레이 스크린샷
<p>
  <img src="GamePlay/screen_gamestart.png" alt="동글 0" width="100">
  <img src="GamePlay/screen_gameover.png" alt="동글 1" width="100">
  <img src="GamePlay/screen_play1.png" alt="동글 2" width="100">
  <img src="GamePlay/screen_play2.png" alt="동글 3" width="100">
  <img src="GamePlay/screen_play3.png" alt="동글 4" width="100">
</p>

## 주요 기능

- **Physics 기반 오브젝트 충돌**
- **Object Pooling**으로 효율적인 오브젝트 관리
- **Prefab 시스템**: Dongle(행성) 프리팹으로 레벨별 행성 재사용
- **레벨업 애니메이션**
- **물방울 이펙트**
- **점수 / 최고점 UI 관리**
- **사운드 효과** (레벨업, 다음 행성 생성, 버튼 클릭, 게임 오버)
- **데스 라인 판정** 및 게임 오버 루틴


## 개발 환경

- **Unity**: 2021.x 이상
- **언어**: C#
- **플랫폼**: WebGL (itch.io 업로드), iOS (Xcode 빌드 테스트)
- **참고 강의**: [골드메탈 Unity 강좌](https://www.youtube.com/watch?v=eQPp0QTz4JM&list=PLO-mt5Iu5TeajtA5UQT7_2UjB7_dkGagU)


## 📂 프로젝트 구조
```plaintext
PlanetGame
├── Assets/
│   ├── Scripts/
│   │   ├── Dongle.cs         # 행성 개별 동작 제어
│   │   └── GameManager.cs    # 전체 게임 로직, 점수/오브젝트 관리
│   ├── Sprites/              # 행성, UI, 효과 이미지
│   ├── Prefabs/              # Dongle prefab, Effect prefab
│   ├── Scenes/
│   │   └── Main.unity
│   └── Audio/                # 사운드 효과 파일
└── README.md
```

## 핵심 스크립트

### Dongle.cs
- 드래그 & 드롭 기능
- 같은 레벨끼리 충돌 시 합치기
- 레벨업 이펙트 / 애니메이션 처리
- 데스 라인 충돌 시간 체크 → 게임 오버 호출

### GameManager.cs
- 게임 시작 / 종료 관리
- 오브젝트 풀링 시스템
- 점수 / 최고 점수 관리 (PlayerPrefs 사용)
- 사운드 효과 재생


## 해결했던 문제들

- **행성 이미지 해상도 문제**  
  고해상도 이미지를 적용하면 SpriteRenderer의 Scale과 맞지 않아 오브젝트를 뚫고 나오는 문제 발생  
  → 이미지 해상도/Scale 조정 필요 (초기 화면 해상도 설정 영향 있음)
- **Unity 애니메이션 Preview 버튼 비활성화 문제**  
  → 해당 오브젝트를 Scene에서 선택한 상태로 Animation 창을 열면 해결

