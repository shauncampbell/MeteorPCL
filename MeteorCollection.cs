using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace MeteorPCL
{
	public interface MeteorCollection<T>
	{
		void Add(string id, JObject fields);
		void Change(string id, JObject fields);
		void Delete(string id);

		ObservableCollection<T> Collection { get; }
	}
}
