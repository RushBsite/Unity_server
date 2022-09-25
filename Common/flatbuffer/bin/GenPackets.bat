.\flatc --csharp proto.fbs --gen-onefile --gen-object-api  --gen-compare
IF ERRORLEVEL 1 PAUSE
START ../../../PacketGenerator/bin/PacketGenerator.exe ./proto.fbs
XCOPY /Y proto.cs "../../../DummyClient/Packet"
XCOPY /Y proto.cs "../../../Server/Packet"
XCOPY /Y ServerPacketManager.cs "../../../Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../../DummyClient/Packet"