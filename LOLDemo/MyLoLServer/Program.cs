using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFramework;
using NetFramework.auto;

namespace MyLoLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Server Init
            ServerStart ss = new ServerStart(9000);      
            ss.Ecode = MessageEncoding.encode;
            ss.center = new HandlerCenter();
            ss.Dcode = MessageEncoding.decode;
            ss.LD = LengthEncoding.decode;
            ss.LE = LengthEncoding.encode;
            ss.Start(6660);
            Console.WriteLine("サーバー起動開始");
            while(true){ }
        }
    }
}
