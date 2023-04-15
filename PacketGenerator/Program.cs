using System;
using System.IO;
using System.Xml;

namespace PacketGenerator
{
    class Program
    {
        static string clientRegister;
        static string serverRegister;
        static void Main(string[] args)
        {
            string file = "../../Server/Packet/test.cs";

            if (args.Length >= 1)
                file = args[0];

            bool startParsing = false;

            foreach (string line in File.ReadAllLines(file))
            {
                if(!startParsing && line.Contains("union fbsId"))
                {
                    startParsing = true;
                    continue;
                }

                if (!startParsing)
                    continue;

                if (line.Contains("}"))
                    break;

                string[] names = line.Trim().Split(new string[] { "," }, StringSplitOptions.None);
                if (names.Length == 0)
                    continue;
                string name = names[0];
                if (name.StartsWith("S_"))
                {
                    clientRegister += string.Format(PacketFormat.managerRegisterFormat, name, name);
                }
                else if (name.StartsWith("C_"))
                {
                    serverRegister += string.Format(PacketFormat.managerRegisterFormat, name, name);
                }
            }

            string clientManagerText = string.Format(PacketFormat.managerFormat, clientRegister);
            File.WriteAllText("ClientPacketManager.cs", clientManagerText);
            string serverManagerText = string.Format(PacketFormat.managerFormat, serverRegister);
            File.WriteAllText("ServerPacketManager.cs", serverManagerText);

        }

        public static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return input[0].ToString().ToUpper() + input.Substring(1).ToLower();
        }



    }
}
