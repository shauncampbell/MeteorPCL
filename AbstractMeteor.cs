using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	public class AbstractMeteor: IMeteor
	{
		private object[] NO_PARAMS = new object[] { };
		private IDDPClient _client = null;
		private string _host, _token;
		private bool _ssl;
		public AbstractMeteor(IDDPClient client)
		{
			_client = client;
		}

		public IDataSubscriber Subscriber
		{
			get; set;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:MeteorPCL.IMeteor"/> is alive.
		/// </summary>
		/// <value><c>true</c> if is alive; otherwise, <c>false</c>.</value>
		public bool IsAlive
		{
			get
			{
				return _client != null && _client.IsAlive;
			}
		}

		/// <summary>
		/// Resume this instance.
		/// </summary>
		public Task<JObject> Resume()
		{
			_client.Connect(_host, _ssl);

			return CallWithResult("login", new object[] { 
				new Dictionary<string, object>() { 
					{ "resume", _token }
				} 
			});
		}

		/// <summary>
		/// Connect the specified host, ssl and token.
		/// </summary>
		/// <returns>The connect.</returns>
		/// <param name="host">Host.</param>
		/// <param name="ssl">If set to <c>true</c> connect using ssl.</param>
		/// <param name="token">Token.</param>
		public Task<JObject> Connect(string host, bool ssl, string token)
		{
			_host = host;
			_token = token;
			_ssl = ssl;

			return Resume();
		}

		/// <summary>
		/// Subscribe to the specified collection.
		/// </summary>
		/// <returns>The subscription task.</returns>
		/// <param name="subscriptionName">Collection name.</param>
		/// <param name="parameters">Parameters.</param>
		public void Subscribe(string subscriptionName, object[] parameters)
		{
			 _client.Subscribe(subscriptionName, parameters);
		}

		/// <summary>
		/// Subscribe the specified subscriptionName.
		/// </summary>
		/// <returns>The subscription task.</returns>
		/// <param name="subscriptionName">Collection name.</param>
		public void Subscribe(string subscriptionName)
		{
			Subscribe(subscriptionName, NO_PARAMS);
		}

		/// <summary>
		/// Call the specified methodName and parameters.
		/// </summary>
		/// <returns>The call.</returns>
		/// <param name="methodName">Method name.</param>
		/// <param name="parameters">Parameters.</param>
		public void Call(string methodName, object[] parameters)
		{
			_client.Call(methodName, parameters);
		}

		/// <summary>
		/// Call the specified methodName.
		/// </summary>
		/// <returns>The call.</returns>
		/// <param name="methodName">Method name.</param>
		public void Call(string methodName)
		{
			Call(methodName, NO_PARAMS);
		}

		/// <summary>
		/// Calls the with result.
		/// </summary>
		/// <returns>The with result.</returns>
		/// <param name="methodName">Method name.</param>
		/// <param name="parameters">Parameters.</param>
		public Task<JObject> CallWithResult(string methodName, object[] parameters)
		{
			return _client.CallWithResult(methodName, parameters);
		}

		/// <summary>
		/// Calls the with result.
		/// </summary>
		/// <returns>The with result.</returns>
		/// <param name="methodName">Method name.</param>
		public Task<JObject> CallWithResult(string methodName)
		{
			return _client.CallWithResult(methodName, NO_PARAMS);
		}

		/// <summary>
		/// Disconnect this instance.
		/// </summary>
		public void Disconnect()
		{
			_client.Disconnect();
		}
	}
}
