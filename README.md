# SRPG 게임

### 프로젝트 소개
전략시뮬레이션의 유닛들을 RPG처럼 육성하는 기본적인 SRPG요소들을 가진 게임입니다.

### 프로젝트 동기
1) 이전까지 배운 다양한 기능들을 구현
2) Astar를 비롯한 탐색 알고리즘의 구현및활용
3) 처음으로 일정규모 이상의 개인 프로젝트 제작

### 프로젝트 언어 및 환경
  프로그래밍 언어: C#  
  게임엔진 : Unity
  
---
## 실습 코드
### Manager  
- [Managers.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Managers/Managers.cs)
- [DataManager.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Managers/DataManager.cs)  
- [UIManager.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Managers/UIManager.cs)  
- [SoundManager.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Managers/SoundManager.cs)  
- [EffectManager.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Managers/EffectManager.cs)  
- [ResourceManager.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Managers/ResourceManager.cs)  
- [SceneManagerEx.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Managers/SceneManagerEx.cs)  

### Character  
- [Character.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Scenes/Battle/Character.cs)  
- [Enemy.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Scenes/Battle/Enemy.cs)  
- [Player.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Scenes/Battle/Player.cs)  
- [Skill.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Scenes/Battle/Skill/Skill.cs)  

### Battle  
- [MapManager.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Scenes/Battle/MapManager.cs)  
- [TurnManager.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Scenes/Battle/TurnManager.cs)  
- [Tile.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/Scenes/Battle/Tile.cs)  

### UI  
- [UIBase.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/UI/UIBase.cs)  
- [UIEventHandler.cs](https://github.com/Songhosub/SRPG/blob/main/SRPG/Assets/Scripts/UI/UIEventHandler.cs)  
- [Popup](https://github.com/Songhosub/SRPG/tree/main/SRPG/Assets/Scripts/UI/Popup)  
- [Scene](https://github.com/Songhosub/SRPG/tree/main/SRPG/Assets/Scripts/UI/Scene)  
- [SubItem](https://github.com/Songhosub/SRPG/tree/main/SRPG/Assets/Scripts/UI/SubItem)  

Etc  

---
## 주요 기능

### 전투
![캐릭터 선택](https://github.com/user-attachments/assets/5054ac56-9bd1-4cbd-9a58-17282464cc35)
전투 시작 시 캐릭터 선택 UI 등장  
클릭을 통해 캐릭터의 생성 및 제거  

![캐릭터 이동](https://github.com/user-attachments/assets/e67bb97c-8dd6-4310-a164-3dffb1c2d80f)
길 찾기 알고리즘을 이용한 이동 가능 범위을 표시  
이동 가능 타일을 클릭하여 해당 위치로 이동  

![캐릭터 공격](https://github.com/user-attachments/assets/11e10789-0763-4e80-8744-8c129e9e0c7b)
스킬(공격) UI를 클릭하여 해당 캐릭터의 상태 변화  
길 찾기 알고리즘을 이용하여 공격 가능한 범위 표시  
범위 안의 대상을 선택하여 해당 스킬 사용  

![적 AI](https://github.com/user-attachments/assets/1203710a-7896-4462-9192-688afb51576a)
해당 적의 턴중이 오면 자동으로 동작  
가장 가까운 적을 찾아 이동  
이동 후 공격 범위에 적이 있으면 공격  

### 육성
![캐릭터 육성](https://github.com/user-attachments/assets/8beece11-65b3-4bdc-b8f7-913b9bdff79c)
UI를 클릭하여 원하는 능력치를 변화  
능력치를 증가 시키는데 사용되는 코스트는 레벨업을 통해 획득  

![캐릭터 스킬](https://github.com/user-attachments/assets/717228ab-571e-4663-8e7d-187f1662da9a)
스킬을 배우고 사용할 스킬을 설정하는 것이 가능  
이전 단계의 스킬을 배우지 않으면 다음 스킬을 배우는 것이 불가능  

### 세이브 및 로드
![세이브](https://github.com/user-attachments/assets/0179a32b-83d4-4925-af5a-47f2dad4eeb8)
UI를 클릭하는 것으로 해당 슬롯에 맞춰 플레이 데이터를 JSON파일로 세이브  

![로드](https://github.com/user-attachments/assets/de7a38a8-a539-4f1c-906e-345efc4db06c)
클릭한 UI슬롯에 맞는 JSON파일이 존재할 경우 플레이 데이터를 로드  

### 잔체적인 게임의 흐름
- 시작 씬 흐름도  
![시작 씬 흐름도](https://github.com/user-attachments/assets/e5c5fef3-9592-4423-b308-e42d69ce35ca)

- 다이얼로그 씬 흐름도  
![다이얼로그 씬 흐름도](https://github.com/user-attachments/assets/5d2963ae-0265-44c7-b4d3-0f3d6c991e17)

- 로비 씬 흐름도  
![로비 씬 흐름도](https://github.com/user-attachments/assets/07ef2120-ea9c-4a0d-9674-0bc63dd5ad38)

- 전투 씬 흐름도  
![전투 씬 흐름도](https://github.com/user-attachments/assets/e2c517b2-0092-45bf-8541-4210de3a2b43)

