using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using RSA_128;
using System.Numerics;
using AES___;
using ConsoleApp2;

namespace TLS1._5
{
    class TLS1
    {
        //static public Mutex read = new Mutex(false);
        //static public Mutex write = new Mutex(true);

        //static private Mutex mtx = new Mutex(false);

        static public SemaphoreSlim ReadClient = new SemaphoreSlim(0);
        static public SemaphoreSlim WriteClient = new SemaphoreSlim(1);

        static public SemaphoreSlim ReadServer = new SemaphoreSlim(0);
        static public SemaphoreSlim WriteServer = new SemaphoreSlim(1);


        static MultipleAES AES_Server;
        static MultipleAES AES_Client;
        static byte[] AesKeyClient;
        static byte[] AesKeyServer;

        static PublicKey ClientKey;
        static PrivateKey ClientPrivKey;

        static PublicKey ServerKey;
        static PrivateKey ServerPrivKey;


        public static void SendClient(byte[] text)
        {
            WriteClient.Wait();
            //if (File.Exists("bufferToServer.data"))
            //{
            //    File.Delete("bufferToServer.data");
            //    File.Create("bufferToServer.data");
            //}
            File.WriteAllBytes("bufferToServer.data", text);
            Thread.Sleep(100);
            ReadServer.Release();
        }

        public static byte[] ReceiveClient()
        {
            ReadClient.Wait();
            //if (File.Exists("bufferToClient.data"))
            //{
            //    File.Delete("bufferToClient.data");
            //    File.Create("bufferToClient.data");
            //}
            var text = File.ReadAllBytes("bufferToClient.data");
            Thread.Sleep(100);
            WriteServer.Release();
            return text;
        }

        public static void SendServer(byte[] text)
        {
            WriteServer.Wait();
            File.WriteAllBytes("bufferToClient.data", text);
            Thread.Sleep(100);
            ReadClient.Release();
        }

        public static byte[] ReceiveServer()
        {
            ReadServer.Wait();
            var text = File.ReadAllBytes("bufferToServer.data");
            Thread.Sleep(100);
            WriteClient.Release();
            return text;
        }

        public static void RunClient()
        {
            RSA CipherClient = new RSA();
            ClientPrivKey = CipherClient.priv;
            SendClient(Encoding.UTF8.GetBytes("init"));
            Thread.Sleep(100);
            ServerKey = new PublicKey(ReceiveClient());
            SendClient(CipherClient.pub.ToBytes());
            byte[] Encrypted1 = ReceiveClient();
            AesKeyClient = Encrypted1;
            //(CipherClient.priv.Decrypt(new BigInteger(Encrypted1))).ToByteArray(); // ???
            AES_Client = new MultipleAES();
            Thread.Sleep(100);
            //foreach (int i in AesKeyClient)
            //{
            //    Console.Write("{0} ", i);
            //}
            //Console.WriteLine();
            SendClient(AES_Client.EncryptECB(Encoding.UTF8.GetBytes("text"), AesKeyClient));
        }

        public static void RunServer()
        {
            RSA CipherServer = new RSA();
            ServerPrivKey = CipherServer.priv;
            Thread.Sleep(100);
            byte[] buff = ReceiveServer();
            if (Encoding.UTF8.GetString(buff) != "init")
            {
                throw new IOException("Наша ошибка");
            }
            SendServer(CipherServer.pub.ToBytes());
            ClientKey = new PublicKey(ReceiveServer());
            AesKeyServer = new byte[16];                                  //Формирование ключа с помощью ГСПЧ
            Random rand = new Random();
            for (int i = 0; i < AesKeyServer.Length; i++)
            {
                AesKeyServer[i] = (byte)rand.Next(0, 255);
            }
            byte[] Encrypted = (ClientKey.Encrypt(new BigInteger(AesKeyServer))).ToByteArray();     //
            //SendServer(Encrypted);
            SendServer(AesKeyServer);
            //foreach (int i in AesKeyServer)
            //{
            //    Console.Write("{0} ", i);
            //}
            //Console.WriteLine();
            AES_Server = new MultipleAES();
            string text = Encoding.UTF8.GetString(AES_Server.DecryptECB(ReceiveServer(), AesKeyServer));
            //Console.WriteLine(text);
            Thread.Sleep(100);
        }

        public static void SendClientData(string text)
        {
            byte[] Encrypted = AES_Client.EncryptECB(Encoding.UTF8.GetBytes(text), AesKeyClient);
            Md5 md = new Md5();
            SendClient(Encrypted);
            string h_code = md.GetHash(text);
            //string h_code = md.GetHash(Encoding.UTF8.GetString(Encrypted));
            BigInteger c = new BigInteger(Encoding.UTF8.GetBytes(h_code));
            SendClient(ClientPrivKey.Decrypt(c).ToByteArray());
            //SendClient(Encoding.UTF8.GetBytes(h_code));//
            Console.WriteLine("{0} Отправляемые данные", text);
            Console.WriteLine("{0} Отправляемый хэш", h_code);
            Thread.Sleep(100);
            //подписать хэш от длины и отправить 
        }

      

        public static void RecieveServerData()
        {
            string text = Encoding.UTF8.GetString(AES_Server.DecryptECB(ReceiveServer(), AesKeyServer));
            byte[] h_code = ReceiveServer();
            Console.WriteLine("{0} Полученные данные", text);
            h_code = ClientKey.Encrypt(new BigInteger(h_code)).ToByteArray();
            Md5 md = new Md5();
            string hash = Encoding.UTF8.GetString(h_code);
            string hash2 = md.GetHash(text);
            Console.WriteLine("{0} Полученный хэш", hash);
            Console.WriteLine("{0} Посчитанный хэш", hash2);
            if (hash2 == hash)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("!OK");
            }
            Thread.Sleep(100);
            //получить хэш от длины и проверить
        }

        //public static void RecieveClientData()
        //{
        //    byte[] Encrypted = ReceiveClient();
        //    string text = Encoding.UTF8.GetString(AES_Client.DecryptCBC(Encrypted, AesKeyClient));
        //    string h_code = Encoding.UTF8.GetString(ReceiveServer());
        //    Md5 md = new Md5();
        //    h_code = Encoding.UTF8.GetString(ClientKey.Encrypt(new BigInteger(Encoding.UTF8.GetBytes(h_code))).ToByteArray());
        //    Console.WriteLine(h_code);
        //    Console.WriteLine(md.GetHash(Encoding.UTF8.GetString(Encrypted)));
        //    if (md.GetHash(text) == h_code)
        //    {
        //        Console.WriteLine("OK");
        //    }
        //    Thread.Sleep(100);
        //    //получить хэш от длины и проверить
        //}
    }
}
//public static void SendServerData(string text)
//{
//    byte[] Encrypted = AES_Client.EncryptCBC(Encoding.UTF8.GetBytes(text), AesKeyServer);
//    Md5 md = new Md5();
//    SendClient(Encrypted);
//    string h_code = md.GetHash(Encoding.UTF8.GetString(Encrypted));
//    BigInteger c = new BigInteger(Encoding.UTF8.GetBytes(h_code));
//    SendClient(ClientPrivKey.Decrypt(c).ToByteArray());
//    Thread.Sleep(100);
//    //подписать хэш от длины и отправить 
//}