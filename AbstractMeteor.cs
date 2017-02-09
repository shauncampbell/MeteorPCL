using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PCLCrypto;

namespace MeteorPCL
{
	public abstract class AbstractMeteor : IMeteor
	{
		private object[] NO_PARAMS = new object[] { };
		private IDDPClient _client = null;
		private string _host, _token;
		private bool _ssl;

		public event MessageReceivedDelegate MessageReceived;
		public event ItemAddedEventArgs ItemAdded;
		public event ItemUpdatedEventArgs ItemUpdated;
		public event ItemDeletedEventArgs ItemDeleted;

		public AbstractMeteor(IDDPClient client)
		{
			_client = client;
			_client.MessageReceived += OnMessageReceived;
		}

		private void OnMessageReceived(JObject obj)
		{
			if (obj == null)
				return;
			Debug.WriteLine(obj);
			if (obj["msg"] != null && obj["id"] != null && obj["collection"] != null && obj["fields"] != null)
			{
				var msg = obj["msg"].Value<string>();
				var id = obj["id"].Value<string>();
				var collection = obj["collection"].Value<string>();
				var fields = obj["fields"] as JObject;

				if ("added".Equals(msg) && ItemAdded != null)
				{
					ItemAdded(id, collection, fields);
				}

				if ("updated".Equals(msg) && ItemUpdated != null)
				{
					ItemUpdated(id, collection, fields);
				}

				if ("removed".Equals(msg) && ItemDeleted != null)
				{
					ItemDeleted(id, collection);
				}
			}
			else if (MessageReceived != null)
			{
				MessageReceived(obj);
			}
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
		public void Connect(string host, bool ssl)
		{
			_host = host;
			_ssl = ssl;

			_client.Connect(_host, _ssl);
		}

		public Task<JObject> Login(Dictionary<string, object> parameters)
		{
			return CallWithResult("login", new object[] { parameters });
		}

		public Task<JObject> LoginWithUsernameAndPassword(string username, string password)
		{
			byte[] data = Encoding.UTF8.GetBytes(password);
			var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
			byte[] hash = hasher.HashData(data);
			string passwordSha = Convert.ToBase64String(hash);

			Dictionary<string, object> parameters = new Dictionary<string, object>() {
				{ "user", new Dictionary<string, object>() { { "username", username } } },
				{ "password", new Dictionary<string, object>() { { "digest", passwordSha }, {"algorithm", "sha-256" } } } };

			return Login(parameters);
		}

		public Task<JObject> LoginWithToken(string token)
		{
			Dictionary<string, object> parameters = new Dictionary<string, object>() { { "resume", token } };

			return Login(parameters);
		}

		/// <summary>
		/// Subscribe to the specified collection.
		/// </summary>
		/// <returns>The subscription task.</returns>
		/// <param name="subscriptionName">Collection name.</param>
		/// <param name="parameters">Parameters.</param>
		public Task<JObject> Subscribe(string subscriptionName, object[] parameters)
		{
			return _client.Subscribe(subscriptionName, parameters);
		}

		/// <summary>
		/// Subscribe the specified subscriptionName.
		/// </summary>
		/// <returns>The subscription task.</returns>
		/// <param name="subscriptionName">Collection name.</param>
		public Task<JObject> Subscribe(string subscriptionName)
		{
			return Subscribe(subscriptionName, NO_PARAMS);
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
