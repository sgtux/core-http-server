using System.Text;

namespace CoreHttpServer.Models
{
  public class Response
  {
    private RequestFile file;

    public Response(RequestFile file)
    {
      this.file = file;
    }

    public byte[] getBytes()
    {
      StringBuilder sbHeader = new StringBuilder();
      if (file.Exists)
      {
        sbHeader.Append("HTTP/1.1 200 OK\n");
        if (file.IsImage)
        {
          sbHeader.Append($"content-type:image/{file.Extension};\n");
        }
        else
        {
          sbHeader.Append("content-type:text/html; charset=utf-8\n");
        }
      }
      else
      {
        sbHeader.Append("HTTP/1.1 404 Not Found\n");
        sbHeader.Append("content-type:text/html; charset=utf-8\n");
        file = new RequestFile("not-found.html");
      }

      byte[] bodyBytes = file.GetBytes();

      sbHeader.Append("connection:close\n");
      sbHeader.Append($"content-length:{bodyBytes.Length}");
      sbHeader.Append("\nserver:CoreHttpServer");
      sbHeader.Append("\n\n");

      var headerBytes = Encoding.ASCII.GetBytes(sbHeader.ToString());
      byte[] bytesResponse = new byte[headerBytes.Length + bodyBytes.Length];

      for (int i = 0; i < headerBytes.Length; i++)
        bytesResponse[i] = headerBytes[i];

      for (int i = 0; i < bodyBytes.Length; i++)
        bytesResponse[i + headerBytes.Length - 1] = bodyBytes[i];

      StringBuilder sb = new StringBuilder();

      foreach (var b in bytesResponse)
        sb.Append((char)b);
      System.Console.WriteLine("-------------- START RESPONSE ---------------");
      System.Console.WriteLine(sb);
      System.Console.WriteLine("-------------- END RESPONSE ---------------");
      return bytesResponse;
    }
  }
}