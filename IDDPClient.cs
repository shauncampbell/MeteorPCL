using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	/// <summary>
	/// Interface representing a DDP Client.
	/// </summary>
	public interface IDDPClient
	{
		event MessageReceivedDelegate MessageReceived;

		/// <summary>
		/// Connect to the DDP Service.
		/// </summary>
		/// <param name="url">The URL of the DDP service</param>
		/// <param name="useSsl">If set to <c>true</c> use ssl.</param>
		void Connect(string url, bool useSsl = false);

		/// <summary>
		/// Add a JSON item to the Message queue.
		/// </summary>
		/// <param name="jsonItem">Json item.</param>
		void AddItem(string jsonItem);

		/// <summary>
		/// Call the specified methodName and args.
		/// </summary>
		/// <returns>The call.</returns>
		/// <param name="methodName">Method name.</param>
		/// <param name="args">Arguments.</param>
		void Call(string methodName, object[] args);

		/// <summary>
		/// Call the specified methodName with arguments and return the result as 
		/// a JSON Object task.
		/// </summary>
		/// <returns>Task with the result.</returns>
		/// <param name="methodName">Method name.</param>
		/// <param name="args">Arguments.</param>
		Task<JObject> CallWithResult(string methodName, object[] args);

		/// <summary>
		/// Subscribe to the specified DDP Stream
		/// </summary>
		/// <returns>The unique subscription ID</returns>
		/// <param name="methodName">The stream to subscribe to</param>
		/// <param name="args">Any arguments required to subscribe</param>
		Task<JObject> Subscribe(string methodName, object[] args);

		/// <summary>
		/// Gets the current request identifier.
		/// </summary>
		/// <returns>The current request identifier.</returns>
		int GetCurrentRequestId();

		/// <summary>
		/// Check if the connector is alive.
		/// </summary>
		/// <value><c>true</c> if is alive; otherwise, <c>false</c>.</value>
		bool IsAlive { get; }

		/// <summary>
		/// Disconnect this instance.
		/// </summary>
		void Disconnect();
	}
}
