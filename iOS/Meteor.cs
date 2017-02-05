using System;
namespace MeteorPCL
{
	public class Meteor: AbstractMeteor
	{
		public Meteor() : base(new DDPClient())
		{
		}
	}
}
