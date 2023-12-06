using Newtonsoft.Json;
using System;

namespace PermissionManagement.Models
{
  /// <summary>
  /// ResponseStatus
  /// </summary>
  public class ResponseStatus
  {
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public ResponseStatus(string code, string message)
    {
      Code = code;
      Message = message;
    }

    /// <summary>
    /// code
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; }

    /// <summary>
    /// message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; }
  }

  /// <summary>
  /// HotelResponseMessage
  /// </summary>
  public class HotelResponseMessage<T> : IDisposable
  {
    /// <summary>
    /// _disposedValue
    /// </summary>
    private bool _disposedValue;

    /// <summary>
    /// response status
    /// </summary>
    [JsonProperty("status")]
    public ResponseStatus Status { get; set; }

    /// <summary>
    /// response data
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; }

    /// <summary>
    /// response success
    /// </summary>
    /// <param name="data"></param>
    public HotelResponseMessage(T data) : this("success", String.Empty, data)
    {
    }

    /// <summary>
    /// response with code and message
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public HotelResponseMessage(string code, string message) : this(code, message, default(T))
    {
    }

    /// <summary>
    /// response with code, message and object data
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <param name="data"></param>
    public HotelResponseMessage(string code, string message, T data)
    {
      Status = new ResponseStatus(code, message);
      Data = data;
    }

    /// <summary>
    /// dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
      if (!_disposedValue)
      {
        if (disposing)
        {
          Status = null;
          Data = default(T);
        }

        _disposedValue = true;
      }
    }

    /// <summary>
    /// ~HotelResponseMessage
    /// </summary>
    ~HotelResponseMessage()
    {
      Dispose(disposing: false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }

  public class HotelResponseMessage : HotelResponseMessage<object>
  {
    public HotelResponseMessage(object data) : base(data)
    {
    }

    public HotelResponseMessage(string code, string message) : base(code, message)
    {
    }

    public HotelResponseMessage(string code, string message, object data) : base(code, message, data)
    {
    }
  }
}
