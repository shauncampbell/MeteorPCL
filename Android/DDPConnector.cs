using System;
using WebSocket4Net;
namespace MeteorPCL
{
	internal class DDPConnector: IDDPConnector
	{
		private WebSocket _socket;
		private string _url = string.Empty;
		private int _isWait = 0;
		private IDDPClient _client;
		private DateTime _lastPing = DateTime.Now;
		private bool _keepAlive;

		public DDPConnector(IDDPClient client)
		{
			this._client = client;
		}

		public bool IsAlive
		{
			get
			{
				return _lastPing.AddSeconds(31) > DateTime.Now;
			}
		}

		public void Connect(string url, bool keepAlive = true, bool useSsl = false)
		{
			_keepAlive = keepAlive;

			if (useSsl)
			{
				_url = "wss://" + url + "/websocket";
			}
			else
			{
				_url = "ws://" + url + "/websocket";
			}
			_socket = new WebSocket(_url);
			_socket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(_socket_MessageReceived);
			_socket.Opened += new EventHandler(_socket_Opened);
			_socket.Open();
			_isWait = 1;
			this._wait();
		}

		public DDPState State
		{
			get
			{
				if (this._socket == null)
					return DDPState.None;

				switch (_socket.State)
				{
					case WebSocketState.Open:
						return DDPState.Open;
					case WebSocketState.Closed:
						return DDPState.Closed;
					case WebSocketState.Closing:
						return DDPState.Closing;
					case WebSocketState.Connecting:
						return DDPState.Connecting;
				}
				return DDPState.None;
			}
		}

		public void Close()
		{
			_socket.Close();
		}

		public void Send(string message)
		{
			_socket.Send(message);
		}

		void _socket_Opened(object sender, EventArgs e)
		{
			_lastPing = DateTime.Now;
			_socket.Send("{\"msg\": \"connect\",\"version\":\"1\",\"support\":[\"1\", \"pre1\"]}");
			_isWait = 0;
		}

		void _socket_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("{0} Socket RX: {1}", DateTime.Now.ToString("hh:mm:ss.fff"), e.Message);
			if (!_handle_Ping(e.Message))
			{
				this._client.AddItem(e.Message);
			}
		}

		bool _handle_Ping(string message)
		{
			if (_keepAlive && message.Equals("{\"msg\":\"ping\"}"))
			{
				_socket.Send("{\"msg\":\"pong\"}");
				_lastPing = DateTime.Now;
				return true;
			}
			return false;
		}

		private void _wait()
		{
			while (_isWait != 0)
			{
				System.Threading.Thread.Sleep(100);
			}
		}

	}
}
