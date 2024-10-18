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
- [Managers.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Managers/Managers.cs)  
- [DataManager.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Managers/DataManager.cs)  
- [UIManager.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Managers/UIManager.cs)  
- [SoundManager.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Managers/SoundManager.cs)  
- [EffectManager.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Managers/EffectManager.cs)  
- [ResourceManager.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Managers/ResourceManager.cs)  
- [SceneManagerEx.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Managers/SceneManagerEx.cs)  

### Character  
- [Character.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Scenes/Battle/Character.cs)  
- [Enemy.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Scenes/Battle/Enemy.cs)  
- [Player.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Scenes/Battle/Player.cs)  
- [Skill.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Scenes/Battle/Skill/Skill.cs)  

### Battle  
- [MapManager.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Scenes/Battle/MapManager.cs)  
- [TurnManager.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Scenes/Battle/TurnManager.cs)  
- [Tile.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/Scenes/Battle/Tile.cs)  

### UI  
- [UIBase.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/UI/UIBase.cs)  
- [UIEventHandler.cs](https://github.com/201710783/SRpg/blob/main/Srpg/Assets/Scripts/UI/UIEventHandler.cs)  
- [Popup](https://github.com/201710783/SRpg/tree/main/Srpg/Assets/Scripts/UI/Popup)  
- [Scene](https://github.com/201710783/SRpg/tree/main/Srpg/Assets/Scripts/UI/Scene)  
- [SubItem](https://github.com/201710783/SRpg/tree/main/Srpg/Assets/Scripts/UI/SubItem)  

Etc  

---
## 주요 기능

### 전투
![캐릭터 선택](https://github.com/user-attachments/assets/b4d3d4aa-92da-4424-97eb-fc6f6d321dd0)  
전투 시작 시 캐릭터 선택 UI 등장  
클릭을 통해 캐릭터의 생성 및 제거  

![캐릭터 이동](https://github.com/user-attachments/assets/284b35ec-21d6-4943-b4bb-4fc45fb3453b)  
길 찾기 알고리즘을 이용한 이동 가능 범위을 표시  
이동 가능 타일을 클릭하여 해당 위치로 이동  

![캐릭터 공격](https://github.com/user-attachments/assets/b82eda34-11cc-400f-8d42-8609af744207)  
스킬(공격) UI를 클릭하여 해당 캐릭터의 상태 변화  
길 찾기 알고리즘을 이용하여 공격 가능한 범위 표시  
범위 안의 대상을 선택하여 해당 스킬 사용  

![적 AI](https://github.com/user-attachments/assets/8ff568c8-3a2f-4ed9-a363-b035b87eec10)  
해당 적의 턴중이 오면 자동으로 동작  
가장 가까운 적을 찾아 이동  
이동 후 공격 범위에 적이 있으면 공격  

### 육성
![캐릭터 육성](https://github.com/user-attachments/assets/bcee6e7d-0126-4c8b-9bda-f9d0d1d5b3a8)  
UI를 클릭하여 원하는 능력치를 변화  
능력치를 증가 시키는데 사용되는 코스트는 레벨업을 통해 획득  

![캐릭터 스킬](https://github.com/user-attachments/assets/8a39922e-2191-475b-a51a-5bb17d090328)  
스킬을 배우고 사용할 스킬을 설정하는 것이 가능  
이전 단계의 스킬을 배우지 않으면 다음 스킬을 배우는 것이 불가능  

### 세이브 및 로드
![세이브](https://github.com/user-attachments/assets/c4d81680-9871-4601-b49f-357e1d93a17f)  
UI를 클릭하는 것으로 해당 슬롯에 맞춰 플레이 데이터를 JSON파일로 세이브  

![로드](https://github.com/user-attachments/assets/2bdef09c-299c-4b67-87ae-9130f1938dfb)  
클릭한 UI슬롯에 맞는 JSON파일이 존재할 경우 플레이 데이터를 로드  

### 잔체적인 게임의 흐름
- 시작 씬 흐름도  
![시작 씬 흐름도](https://github.com/user-attachments/assets/44c46e25-cf61-4887-8721-9fc4f66ae915)

- 다이얼로그 씬 흐름도  
![다이얼로그 씬 흐름도](https://github.com/user-attachments/assets/2d32557f-8fcd-46a6-9b20-ac2bbf83efd5)

- 로비 씬 흐름도  
![로비 씬 흐름도](https://github.com/user-attachments/assets/c813d4f2-af87-4e39-875b-534a1d976f79)

- 전투 씬 흐름도  
![전투 씬 흐름도](https://github.com/user-attachments/assets/64fa9caf-5a41-431e-a727-7211043fc8e5)

