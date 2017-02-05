using System;
namespace MeteorPCL
{
	public interface IResultQueue
	{
		/// <summary>
		/// Add an item to the result queue.
		/// </summary>
		/// <param name="jsonItem">Json item.</param>
		void AddItem(string jsonItem);
	}
}
