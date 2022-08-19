.\flatc --csharp test.fbs --gen-onefile
IF ERRORLEVEL 1 PAUSE
START ../../../PacketGenerator/bin/PacketGenerator.exe ./test.fbs
XCOPY /Y test.cs "../../../DummyClient/Packet"
XCOPY /Y test.cs "../../../Server/Packet"
XCOPY /Y ServerPacketManager.cs "../../../Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../../DummyClient/Packet"