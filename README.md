# Unity_server
본 repo는 2022 AWS 메가 게임잼에 참가한 **<Hide &  socks>** 게임의 서버 구현 부분 입니다.
게임 클라이언트의 Unity 코드는 *Plastic scm* 으로 관리되어, 서버 사이드만 따로 repo를 구성하였습니다.
## 주요 파일구조

### **ServerCore**
![제목 없는 다이어그램 drawio](https://user-images.githubusercontent.com/28249906/232250268-5a956151-844c-4936-8522-2c0999324046.png)  
📦ServerCore  
 ┣ 📂bin  
 ┣ 📜Connector.cs  
 ┣ 📜Listener.cs  
 ┣ 📜PriorityQueue.cs  &nbsp;&nbsp;&nbsp;  ...// 스레드 큐잉 작업에 사용되는 우선순위 큐  
 ┣ 📜RecvBuffer.cs  
 ┣ 📜ServerCore.csproj  
 ┗ 📜Session.cs  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  ...// 세션 추상 클래스 명세

### **Server**  
![제목 없는 다이어그램 drawio (1)](https://user-images.githubusercontent.com/28249906/232250273-58b1fe3a-1203-456d-8080-3ea6aa1abc85.png)

 📦Server  
 ┣ 📂bin  
 ┃ ┗ 📂Release  
 ┣ 📂Game  
 ┃ ┣ 📂Job  
 ┃ ┃ ┣ 📜Job.cs  
 ┃ ┃ ┣ 📜JobSerializer.cs  
 ┃ ┃ ┗ 📜JobTimer.cs  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; .............//jobFlush를 위한 타이머  
 ┃ ┣ 📂Object  
 ┃ ┃ ┣ 📜Player.cs  
 ┃ ┃ ┗ 📜PlayerManager.cs  
 ┃ ┗ 📂Room  
 ┃ ┃ ┣ 📜GameRoom.cs  
 ┃ ┃ ┗ 📜RoomManager.cs  
 ┣ 📂obj  
 ┃ ┣ 📂Release  
 ┣ 📂Packet  
 ┃ ┣ 📜PacketHandler.cs&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;.............// 받은 패킷에 대한 관리, 수동 수정 부분  
 ┃ ┣ 📜proto.cs&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;.....................// flatbuffer를 사용한 패킷 명세서  
 ┃ ┗ 📜ServerPacketManager.cs&nbsp;&nbsp;&nbsp;...// PacketGenerator로 자동 생성한 매니저 코드   
 ┣ 📂Session  
 ┃ ┣ 📜ClientSession.cs  
 ┃ ┗ 📜SessionManager.cs  
 ┣ 📜install.bat  
 ┣ 📜JobTimer.cs  
 ┣ 📜Program.cs  
 ┗ 📜Server.csproj 

 ### **PacketGenerator** 

 📦PacketGenerator  
 ┣ 📂bin  
 ┃ ┣ 📜FlatBuffers.dll&nbsp;&nbsp;&nbsp;// 플랫 버퍼 사용을 위한 dll 파일  
 ┃ ┣ 📜GenPackets.cs &nbsp;// PacketFormet.cs 의 형식대로 서버 패킷 매니저 생성을 자동화 하는 코드  
 ┃ ┣ 📜PacketGenerator.exe  // 실행파일  
 ┣ 📜PacketFormat.cs  
 ┗ 📜Program.cs  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;//Packetformat의 텍스트를 파싱 처리  
