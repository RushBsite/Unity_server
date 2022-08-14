﻿using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler //수동관리
{
    public static void C_PlayerInfoReqHandler(PacketSession session, IPacket packet)
    {
        C_PlayerInfoReq p = packet as C_PlayerInfoReq;

        Console.WriteLine($"PlayerInfoReq: {p.playerId} {p.name}");

        foreach (C_PlayerInfoReq.Skill skill in p.skills)
        {
            Console.WriteLine($"Skill({skill.id})({skill.level})({skill.duration})");
        }
    }
   
}
