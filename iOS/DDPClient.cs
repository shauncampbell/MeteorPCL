using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket4Net;

namespace MeteorPCL
{
	public class DDPClient : AbstractDDPClient
	{

		public DDPClient(): base()
		{
			var subscriber = new BasicDataSubscriber();
			var resultQueue = new ResultQueue(subscriber);

			Init(new DDPConnector(this), subscriber, resultQueue);
		}

		protected override void Sleep(int time)
		{
			Thread.Sleep(time);
		}

	}
}
