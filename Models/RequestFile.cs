using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CoreHttpServer.Models
{
  public class RequestFile
  {
    private byte[] content;

    public string FileName { get; private set; }

    public string Extension { get; private set; }

    public bool IsImage { get; private set; }

    public RequestFile(string fileName)
    {
      FileName = fileName;
      LoadFile();
    }

    public RequestFile(Socket s)
    {
      FileName = GetFileNameRequestFromSocket(s);
      LoadFile();
    }

    public string GetFileNameRequestFromSocket(Socket s)
    {
      try
      {
        var buffer = new byte[1024];
        s.Receive(buffer);
        var sb = new StringBuilder();
        foreach (var b in buffer)
          sb.Append((char)b);
        return sb.ToString().Split("\n")[0].Split(" ")[1].Replace("/", "");
      }
      catch { return null; }
    }

    public byte[] GetBytes() => content;

    public bool Exists => content != null;

    private void LoadFile()
    {
      try
      {
        FileName = string.IsNullOrEmpty(FileName) ? "index.html" : FileName;
        Extension = FileName.Substring(FileName.IndexOf(".") + 1);
        var imageExtension = new[] { "jpeg", "jpg", "png", "gif" };
        IsImage = imageExtension.Contains(Extension);
        content = File.ReadAllBytes("./files/" + FileName);
      }
      catch { content = null; }
    }
  }
}