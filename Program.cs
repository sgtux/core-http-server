using System;
using System.Net;
using System.Net.Sockets;
using CoreHttpServer.Models;

namespace CoreHttpServer
{
  class Program
  {
    private static TcpListener listener;

    static void Main(string[] args)
    {
      var port = 8081;
      var ip = "127.0.0.1";
      listener = new TcpListener(IPAddress.Parse(ip), 8081);
      listener.Start();
      Console.WriteLine($"Listening in {ip}:{port}");

      while (true)
      {
        Socket s = listener.AcceptSocket();
        var rf = new RequestFile(s);
        Console.WriteLine($"\n\nRequest filename: {rf.FileName}");
        var r = new Response(rf);
        s.Send(r.getBytes());
        s.Close();
      }
    }
  }
}