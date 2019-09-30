using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp2;
using System.Threading;
using System.IO;

namespace TLS1._5
{
    class Program
    {
        static void Main(string[] args)
        {

            File.Delete("bufferToClient.data");
            File.Delete("bufferToServer.data");
            Thread ServerThread = new Thread(TLS1.RunServer);
            ServerThread.Start();
            Thread ClientThread = new Thread(TLS1.RunClient);
            ClientThread.Start();
            ServerThread.Join();
            ClientThread.Join();
            //byte[] Data = File.ReadAllBytes("C:\\Users\\drudnitskiy\\Desktop");
            //ServerThread = new Thread(() => TLS1.SendClientData(Encoding.UTF8.GetString(Data)));
            ServerThread = new Thread(() => TLS1.SendClientData("text"));
            ClientThread = new Thread(() => TLS1.RecieveServerData());
            ServerThread.Start();
            ClientThread.Start();
            ServerThread.Join();
            ClientThread.Join();

        }
    }
}
