using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	/// <summary>
	/// This is the main interface for an implementation of the Meteor client.
	/// </summary>
	public interface IMeteor
	{
		/// <summary>
		/// Connect the specified host, ssl and token.
		/// </summary>
		/// <returns>The connect.</returns>
		/// <param name="host">Host.</param>
		/// <param name="ssl">If set to <c>true</c> connect using ssl.</param>
		/// <param name="token">Token.</param>
		Task<JObject> Connect(string host, bool ssl, string token);

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
		void Subscribe(string collection, object[] parameters);

		/// <summary>
		/// Subscribe the specified subscriptionName.
		/// </summary>
		/// <returns>The subscription task.</returns>
		/// <param name="collection">Collection name.</param>
		void Subscribe(string collection);

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
