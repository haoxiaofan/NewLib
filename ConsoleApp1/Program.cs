﻿using ConsoleApp1.HardwareService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcp = new SocketServer();
            tcp.SocketInit();
            while (true)
            {
                var message = Console.ReadLine();
                Console.ReadKey();
                tcp.SendMessage(message);
            }
        }
    }
}
