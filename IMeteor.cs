using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	public delegate void MessageReceivedDelegate(JObject message);
	public delegate void ItemAddedEventArgs(string id, string collection, JObject message);
	public delegate void ItemUpdatedEventArgs(string id, string collection, JObject message);
	public delegate void ItemDeletedEventArgs(string id, string collection);

	/// <summary>
	/// This is the main interface for an implementation of the Meteor client.
	/// </summary>
	public interface IMeteor
	{
		event MessageReceivedDelegate MessageReceived;

		event ItemAddedEventArgs ItemAdded;
		event ItemUpdatedEventArgs ItemUpdated;
		event ItemDeletedEventArgs ItemDeleted;

		/// <summary>
		/// Connect the specified host, ssl and token.
		/// </summary>
		/// <returns>The connect.</returns>
		/// <param name="host">Host.</param>
		/// <param name="ssl">If set to <c>true</c> connect using ssl.</param>
		void Connect(string host, bool ssl);

		/// <summary>
		/// Log in with the specified parameters.
		/// </summary>
		/// <returns>The login response </returns>
		/// <param name="parameters">Parameters.</param>
		Task<JObject> Login(Dictionary<string, object> parameters);

		/// <summary>
		/// Log in with a username and password.
		/// </summary>
		/// <returns>The login response</returns>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		Task<JObject> LoginWithUsernameAndPassword(string username, string password);

		/// <summary>
		/// Login with an auth token.
		/// </summary>
		/// <returns>The login response</returns>
		/// <param name="token">Token.</param>
		Task<JObject> LoginWithToken(string token);

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:MeteorPCL.IMeteor"/> is alive.
		/// </summary>
		/// <value><c>true</c> if is alive; otherwise, <c>false</c>.</value>
		bool IsAlive { get; }

		/// <summary>
		/// Subscribe to the specified collection.
		/// </summary>
		/// <returns>The subscription task.</returns>
		/// <param name="collection">Collection name.</param>
		/// <param name="parameters">Parameters.</param>
		Task<JObject> Subscribe(string collection, object[] parameters);

		/// <summary>
		/// Subscribe the specified subscriptionName.
		/// </summary>
		/// <returns>The subscription task.</returns>
		/// <param name="collection">Collection name.</param>
		Task<JObject> Subscribe(string collection);

		/// <summary>
		/// Call the specified methodName and parameters.
		/// </summary>
		/// <returns>The call.</returns>
		/// <param name="methodName">Method name.</param>
		/// <param name="parameters">Parameters.</param>
		void Call(string methodName, object[] parameters);

		/// <summary>
		/// Call the specified methodName.
		/// </summary>
		/// <returns>The call.</returns>
		/// <param name="methodName">Method name.</param>
		void Call(string methodName);

		/// <summary>
		/// Calls the with result.
		/// </summary>
		/// <returns>The with result.</returns>
		/// <param name="methodName">Method name.</param>
		/// <param name="parameters">Parameters.</param>
		Task<JObject> CallWithResult(string methodName, object[] parameters);

		/// <summary>
		/// Calls the with result.
		/// </summary>
		/// <returns>The with result.</returns>
		/// <param name="methodName">Method name.</param>
		Task<JObject> CallWithResult(string methodName);

		/// <summary>
		/// Resume this instance.
		/// </summary>
		Task<JObject> Resume();

		/// <summary>
		/// Disconnect this instance.
		/// </summary>
		void Disconnect();

	}
}
