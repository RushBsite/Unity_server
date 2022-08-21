.\flatc --csharp chat_test.fbs --gen-onefile
IF ERRORLEVEL 1 PAUSE
START ../../../PacketGenerator/bin/PacketGenerator.exe ./chat_test.fbs
XCOPY /Y chat_test.cs "../../../DummyClient/Packet"
XCOPY /Y chat_test.cs "../../../Server/Packet"
XCOPY /Y ServerPacketManager.cs "../../../Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../../DummyClient/Packet"