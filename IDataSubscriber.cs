using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	/// <summary>
	/// A Delegate for handling result callbacks.
	/// </summary>
	public delegate void CallResult(JObject data);

	/// <summary>
	/// Data subscriber.
	/// </summary>
	public interface IDataSubscriber
	{
		event MessageReceivedDelegate MessageReceived;
		/// <summary>
		/// Called when data is received.
		/// </summary>
		/// <param name="data">The JSON data</param>
		void DataReceived(JObject data);

		/// <summary>
		/// Await the result of a particular message id.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="callback">Callback.</param>
		void AwaitResult(string id, CallResult callback);

		/// <summary>
		/// Awaits the subscription.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="callback">Callback.</param>
		void AwaitSubscription(string id, CallResult callback);
	}
}
