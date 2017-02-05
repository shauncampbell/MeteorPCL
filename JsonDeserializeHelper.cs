using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	public class JsonDeserializeHelper
	{
		private IDataSubscriber _subscriber;

		public JsonDeserializeHelper(IDataSubscriber subscriber)
		{
			this._subscriber = subscriber;
		}

		public void Deserialize(string jsonItem)
		{
			JObject jObj = JObject.Parse(jsonItem);
			this._subscriber.DataReceived(jObj);
		}

	}
}
