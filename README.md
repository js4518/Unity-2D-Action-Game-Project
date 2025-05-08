# 유니티 2D 액션 게임 시스템 개발 프로젝트
![Unity-2D-Action-Game-Project-MainScene-WindowsMacLinux-Unity66000 0 43f1_DX11_2025-05-0818-03-53-ezgif com-crop (3)](https://github.com/user-attachments/assets/7fec2d21-aebe-4a00-a461-9af67a731c87)
프로젝트 진행 기간 : 2025.01 ~ 2025.02 / 약 1개월
## 1. 프로젝트 개요

1-1. 프로젝트 주제

2D 탑뷰 액션 장르의 게임 시스템을 개발하여, 캐릭터의 이동과 전투 시스템, 에너미와 에너미 종류에 따른 무빙 패턴을 구현한다. 또한 맵 생성 알고리즘을 제작하여 랜덤한 맵 생성 시스템과 맵간 이동, 방 보상 시스템을 구현한다.

1-2. 프로젝트 목표

Unity Engine을 이용하여 주제에 맞는 게임 시스템의 구현을 통해, Unity Engine의 기초적인 기능과 해당 게임 장르에 필요한 기능의 제작을 학습한다.

1-3. 프로젝트 성과

프로젝트를 통해 게임의 기능을 구현하며 성공적으로 Unity Engine의 기초적인 기능을 학습할 수 있다.

## 2. 추진 체계

2-1. 프로젝트 주제 지정

‘내가 좋아하는 류의 게임의 기능을 구현해보면 더 재밌지 않을까?’의 아이디어를 가지고 가장 오래 플레이한 게임들의 특성을 분석하였다.

- 아이작 : 2D 탑뷰 로그라이크 / 회피와 공격 중심 / 아이템 파밍으로 성장하는 재미 / 시원한 타격감
- 에이펙스 : 3D FPS / 역동적인 이동 / 빠른 속도감 / 전투의 재미 / 뛰어난 타격감
- 테라리아 : 2D 플랫포머 샌드박스 RPG / 아이템 수집으로 강화 / 보스 사냥 / 점진적 성장 시스템 / 이동 시스템(게임 진행에 따라 점프, 대시, 날기, 달리기 등으로 발전) / 적절한 긴장감
- 스펠렁키 : 2D 플랫포머 로그라이트 / 순간적인 판단이 생사를 결정 / 긴박한 게임성

종합적으로, 2D 선호 / 빠른 판단과 직관적이고 속도감 있는 무빙이 핵심 / 성장 시스템 중요 / 적절한 긴장감 필요 / 즉각적으로 다음 목표가 주어지는 시스템 선호한다는 사실을 알게 되었고, 이에 따라 2D 탑뷰 액션 장르 시스템의 구현을 프로젝트 주제로 지정하였다.

2-2. 프로젝트 팀 구성 및 역할

| 이름 | 역할 |
| --- | --- |
| 송종서 | 게임 시스템 구현 / 그래픽, UI 디자인 |

2-3. 프로젝트 관리 체계

Notion을 사용하여 실시간 개발 상황과 과제를 기록하며, Git을 사용하여 저장한다.

## 3. 개발 내용

3-1. 개발 범위 및 순서

2D 탑뷰 액션 장르의 게임의 기초적인 시스템을 개발하였으며, 아래 순서대로 개발하였다.

1. 캐릭터의 기초적인 이동 구현과 타일맵 배경 제작
2. 캐릭터의 대시 시스템 구현
3. 에너미 구현
4. 캐릭터의 공격과 오브젝트 풀링 시스템 구현
5. 에너미의 피격과 Scriptable Object를 이용한 에너미 데이터 관리 구현
6. 캐릭터의 체력과 피격 시스템 구현
7. 에너미 종류 추가 / 에너미 무빙 패턴 제작
8. 캐릭터의 공격 시스템 개편 및 공격 추가
9. 캐릭터의 무기 획득 시스템 구현
10. 맵 시스템과 UI 구현

3-2. 시스템 및 SW 구조

Unity를 이용하여 Windows 운영체제가 설치된 PC에서 실행 가능한 게임을 제작하였으며, 백업 툴로 Git을 사용하였다.

3-3. 주요 개발 내용

![Unity-2D-Action-Game-Project-MainScene-WindowsMacLinux-Unity66000 0 43f1_DX11_2025-05-0818-03-53-ezgif com-crop (3)](https://github.com/user-attachments/assets/7fec2d21-aebe-4a00-a461-9af67a731c87)

⬆ 기본적인 이동과 전투

![Unity-2D-Action-Game-Project-MainScene-WindowsMacLinux-Unity66000 0 43f1_DX11_2025-05-0818-03-53-ezgif com-crop (4)](https://github.com/user-attachments/assets/6a36278b-d34a-4296-b7b2-d9f2ed6b31b8)

⬆ 캐릭터의 피격 처리 / 에너미의 피격 및 사망

![Unity-2D-Action-Game-Project-MainScene-WindowsMacLinux-Unity66000 0 43f1_DX11_2025-05-0818-03-53-ezgif com-crop (5)](https://github.com/user-attachments/assets/ec0438c9-e401-48c2-9047-0814df538a3d)

⬆ 체력 시스템 동적 관리

![Unity-2D-Action-Game-Project-MainScene-WindowsMacLinux-Unity66000 0 43f1_DX11_2025-05-0818-03-53-ezgif com-crop (7)](https://github.com/user-attachments/assets/ddf21c53-e400-4af2-bfca-12337c70b973)

⬆ 맵의 이동과 랜덤 생성

![Unity-2D-Action-Game-Project-MainScene-WindowsMacLinux-Unity66000 0 43f1_DX11_2025-05-0818-03-53-ezgif com-crop (9)](https://github.com/user-attachments/assets/e58e5c44-b6ac-4cf6-b693-7d402f0a084d)

⬆ 근접 공격을 통한 전투


개발 범위 1. 캐릭터의 기초적인 이동 구현과 타일맵 배경 제작

![image](https://github.com/user-attachments/assets/bf98006c-2054-400c-910c-af9024f544b2)

개발 범위 2. 캐릭터의 대시 시스템 구현

![image (1)](https://github.com/user-attachments/assets/e0046ea5-72c8-4e37-84b9-39c652ee823f)

개발 범위 3. 에너미 구현

![image (2)](https://github.com/user-attachments/assets/0f63ce10-3547-4e3a-a4c1-ae963c40c155)

개발 범위 4. 캐릭터의 공격과 오브젝트 풀링 시스템 구현

![image (3)](https://github.com/user-attachments/assets/9e728a9c-9d46-4123-a65b-0d4240636183)
![image (4)](https://github.com/user-attachments/assets/a1acc697-bc9a-4780-a714-435f9447ed44)

개발 범위 5. 에너미의 피격과 Scriptable Object를 이용한 에너미 데이터 관리 구현

![image (5)](https://github.com/user-attachments/assets/982b28f3-65fd-41fc-8c0c-8016bd3a239d)
![image (6)](https://github.com/user-attachments/assets/576b3271-4af9-49a6-a428-cb02e9f77b9a)
![image](https://github.com/user-attachments/assets/e9ff8660-72fe-43d8-bed0-10b7db841ba5)

개발 범위 6. 캐릭터의 체력과 피격 시스템 구현

![image (7)](https://github.com/user-attachments/assets/a9c8e323-2e0e-4071-a745-cb0af4d49122)
![image (8)](https://github.com/user-attachments/assets/e8f8425f-45be-484f-8571-a7c3f7f4158a)

개발 범위 7. 에너미 종류 추가 / 에너미 무빙 패턴 제작

![image (9)](https://github.com/user-attachments/assets/2f0b2b8f-b5b9-4895-ab71-1dc947b57cd9)
![image (10)](https://github.com/user-attachments/assets/b2fa6385-b035-4ee1-8d14-eddeb8a9ab8b)
![image (11)](https://github.com/user-attachments/assets/bcd381c6-5ca7-4089-a3d7-e3b4a8b3d0ee)

개발 범위 8. 캐릭터의 공격 시스템 개편 및 공격 추가

![image (12)](https://github.com/user-attachments/assets/fde965d7-bf65-4315-87c4-7a8d500b1422)
![image (13)](https://github.com/user-attachments/assets/f2a926ea-2fb5-4a72-b365-3c08a05dc801)

개발 범위 9. 캐릭터의 무기 획득 시스템 구현

![image (14)](https://github.com/user-attachments/assets/2a413065-7bc5-4bc8-838c-fa1f95c9836b)

개발 범위 10. 맵 시스템과 UI 구현

![image (15)](https://github.com/user-attachments/assets/f08ab96e-350a-454e-8e90-6ab5b4c217fd)

