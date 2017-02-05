using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	/// <summary>
	/// Abstract class which connects to a DDP server.
	/// This abstract class is necessary so that platform specific implementations
	/// can be made in the concrete classes.
	/// </summary>
	public abstract class AbstractDDPClient: IDDPClient
	{
		private int _uniqueId;
		protected IDDPConnector _connector;
		protected IResultQueue _queueHandler;
		protected IDataSubscriber _subscriber;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MeteorPCL.AbstractDDPClient"/> class.
		/// </summary>
		public AbstractDDPClient()
		{
			_uniqueId = 1;
		}

		/// <summary>
		/// Initialize the client with a DDPConnector, subscriber and ResultQueue.
		/// This shouldnt be user facing, but is necessary in the concrete classes.
		/// </summary>
		/// <returns>The init.</returns>
		/// <param name="connector">Connector for sending DDP Messages</param>
		/// <param name="subscriber">Subscriber for subscribing to DDP Messages</param>
		/// <param name="queue">Message queue</param>
		protected void Init(IDDPConnector connector, IDataSubscriber subscriber, IResultQueue queue)
		{
			this._connector = connector;
			this._subscriber = subscriber;
			this._queueHandler = queue;
		}

		/// <summary>
		/// Check if the connector is alive.
		/// </summary>
		/// <value><c>true</c> if is alive; otherwise, <c>false</c>.</value>
		public bool IsAlive
		{
			get
			{
				return _connector != null && _connector.IsAlive;
			}
		}

		/// <summary>
		/// Add a JSON item to the Message queue.
		/// </summary>
		/// <param name="jsonItem">Json item.</param>
		public void AddItem(string jsonItem)
		{
			_queueHandler.AddItem(jsonItem);
		}

		/// <summary>
		/// Connect to the DDP Service.
		/// </summary>
		/// <param name="url">The URL of the DDP service</param>
		/// <param name="useSsl">If set to <c>true</c> use ssl.</param>
		public void Connect(string url, bool useSsl = false)
		{
			_connector.Connect(url, useSsl: useSsl);
		}

		/// <summary>
		/// Call the specified methodName and args.
		/// </summary>
		/// <returns>The call.</returns>
		/// <param name="methodName">Method name.</param>
		/// <param name="args">Arguments.</param>
		public void Call(string methodName, params object[] args)
		{
			_connector.Send(JsonConvert.SerializeObject(new
			{
				msg = "method",
				method = methodName,
				@params = args,
				id = this.NextId().ToString()
			}
			));
		}

		/// <summary>
		/// Call the specified methodName with arguments and return the result as 
		/// a JSON Object task.
		/// </summary>
		/// <returns>Task with the result.</returns>
		/// <param name="methodName">Method name.</param>
		/// <param name="args">Arguments.</param>
		public Task<JObject> CallWithResult(string methodName, object[] args)
		{
			var task = new Task<JObject>(() =>
			{
				var newId = this.NextId().ToString();
				var obj = JsonConvert.SerializeObject(new
				{
					msg = "method",
					method = methodName,
					@params = args,
					id = newId
				});

				JObject result = null;

				_subscriber.AwaitResult(newId, (response) =>
				{
					result = response;
				});
				_connector.Send(obj);

				while (result == null)
				{
					//	Give up the stream for a wee bit.
					Sleep(10);
				}

				return result;
			});

			task.Start();
			return task;
		}

		/// <summary>
		/// Sleep the specified time.
		/// This is needed because thread.sleep is implemented in the concrete platform specific classes.
		/// </summary>
		/// <param name="time">Time to sleep</param>
		protected abstract void Sleep(int time);

		/// <summary>
		/// Subscribe to the specified DDP Stream
		/// </summary>
		/// <returns>The unique subscription ID</returns>
		/// <param name="subscribeTo">The stream to subscribe to</param>
		/// <param name="args">Any arguments required to subscribe</param>
		public int Subscribe(string subscribeTo, params object[] args)
		{
			_connector.Send(JsonConvert.SerializeObject(new
			{
				msg = "sub",
				name = subscribeTo,
				@params = args,
				id = this.NextId().ToString()
			}
			));
			return this.GetCurrentRequestId();
		}

		/// <summary>
		/// Get the current state of the DDP Connection.
		/// </summary>
		/// <value>The state.</value>
		public DDPState State
		{
			get { return this._connector.State; }
		}

		/// <summary>
		/// Next identifier.
		/// </summary>
		/// <returns>The identifier.</returns>
		private int NextId()
		{
			return _uniqueId++;
		}

		/// <summary>
		/// Gets the current request identifier.
		/// </summary>
		/// <returns>The current request identifier.</returns>
		public int GetCurrentRequestId()
		{
			return _uniqueId;
		}

		/// <summary>
		/// Disconnect this instance.
		/// </summary>
		public void Disconnect()
		{
			_connector.Close();
		}

	}
	
}
