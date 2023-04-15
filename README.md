# Unity_server

## 주요 파일구조

### **ServerCore**


📦ServerCore  
 ┣ 📂bin  
 ┃ ┗ 📂Release  
 ┃ ┃ ┗ 📂netcoreapp3.1  
 ┃ ┃ ┃ ┗ 📜FlatBuffers.dll  
 ┃ ┣ 📂Release  
 ┃ ┃ ┗ 📂netcoreapp3.1  
 ┃ ┗  📜project.assets.json  
 ┣ 📜Connector.cs  
 ┣ 📜Listener.cs  
 ┣ 📜PriorityQueue.cs  
 ┣ 📜RecvBuffer.cs  
 ┣ 📜ServerCore.csproj  
 ┗ 📜Session.cs  

 📦Server  
 ┣ 📂bin  
 ┃ ┗ 📂Release  
 ┣ 📂Game  
 ┃ ┣ 📂Job  
 ┃ ┃ ┣ 📜Job.cs  
 ┃ ┃ ┣ 📜JobSerializer.cs  
 ┃ ┃ ┗ 📜JobTimer.cs  
 ┃ ┣ 📂Object  
 ┃ ┃ ┣ 📜Player.cs  
 ┃ ┃ ┗ 📜PlayerManager.cs  
 ┃ ┗ 📂Room  
 ┃ ┃ ┣ 📜GameRoom.cs  
 ┃ ┃ ┗ 📜RoomManager.cs  
 ┣ 📂obj  
 ┃ ┣ 📂Release  
 ┣ 📂Packet  
 ┃ ┣ 📜PacketHandler.cs  
 ┃ ┣ 📜proto.cs  
 ┃ ┗ 📜ServerPacketManager.cs  
 ┣ 📂Session  
 ┃ ┣ 📜ClientSession.cs  
 ┃ ┗ 📜SessionManager.cs  
 ┣ 📜install.bat  
 ┣ 📜JobTimer.cs  
 ┣ 📜Program.cs  
 ┗ 📜Server.csproj  

 📦PacketGenerator  
 ┣ 📂bin  
 ┃ ┣ 📜FlatBuffers.dll  
 ┃ ┣ 📜GenPackets.cs    
 ┃ ┣ 📜PacketGenerator.exe  
 ┃ ┗ 📜PacketGenerator.pdb  
 ┣ 📂obj  
 ┣ 📜App.config  
 ┣ 📜PacketFormat.cs  
 ┣ 📜PacketGenerator.csproj  
 ┗ 📜Program.cs  