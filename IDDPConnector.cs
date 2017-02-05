using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	public enum DDPState { None = -1,
		Connecting,
		Open,
		Closing,
		Closed}
	
	public interface IDDPConnector
	{
		/// <summary>
		/// Check if the connector is alive.
		/// </summary>
		/// <value><c>true</c> if is alive; otherwise, <c>false</c>.</value>
		bool IsAlive { get; }

		/// <summary>
		/// Connect the specified DDP Service, optionally enabling keepalive packets and ssl.
		/// </summary>
		/// <returns>The connect.</returns>
		/// <param name="url">The URL to the DDP Service.</param>
		/// <param name="keepAlive">If set to <c>true</c> send keep alive packets.</param>
		/// <param name="useSsl">If set to <c>true</c> use ssl.</param>
		void Connect(string url, bool keepAlive = true, bool useSsl = false);

		/// <summary>
		/// Get the state of the DDP Connection
		/// </summary>
		/// <value>The state.</value>
		DDPState State { get; }

		/// <summary>
		/// Close this instance.
		/// </summary>
		void Close();

		/// <summary>
		/// Send a message to the DDP service
		/// </summary>
		/// <param name="message">The message to send</param>
		void Send(string message);
	}
}
