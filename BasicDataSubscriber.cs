using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	/// <summary>
	/// This is a simple implementation of the IData subscriber interface. 
	/// It contains a dictionary of callbacks, and calls them when data comes in matching the
	/// required callback id.
	/// </summary>
	public class BasicDataSubscriber : IDataSubscriber
	{
		private Dictionary<string, CallResult> _callbacks = new Dictionary<string, CallResult>();
		private Dictionary<string, CallResult> _subscriptions = new Dictionary<string, CallResult>();

		public event MessageReceivedDelegate MessageReceived;

		/// <summary>
		/// Await the result of a particular message id.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="callback">Callback.</param>
		public void AwaitResult(string id, CallResult callback)
		{
			_callbacks.Add(id, callback);
		}

		public void AwaitSubscription(string id, CallResult callback)
		{
			_subscriptions.Add(id, callback);
		}

		/// <summary>
		/// Called when data is received.
		/// </summary>
		/// <param name="data">The JSON data</param>
		public virtual void DataReceived(JObject data)
		{
			if (null != data["msg"] && "result".Equals(data["msg"].ToString()) &&
				null != data["id"] && _callbacks.ContainsKey(data["id"].ToString()))
			{
				_callbacks[data["id"].ToString()](data);
				_callbacks.Remove(data["id"].ToString());
			}
			else if (null != data["msg"] && "ready".Equals(data["msg"].ToString()) &&
					 null != data["subs"] && data["subs"] is JArray)
			{
				foreach (var id in data["subs"] as JArray)
				{
					var idString = id.Value<string>();
					if (_subscriptions.ContainsKey(idString))
					{
						_subscriptions[idString](data);
						_subscriptions.Remove(idString);
					}
				}
			}
			else if (MessageReceived != null)
			{
				MessageReceived(data);
			}
		}
	}
}
