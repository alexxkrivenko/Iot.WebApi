using System;

namespace Iot.WebAPI.Domain
{
	public abstract class Entity
	{
		protected Entity()
		{
		}
		public Entity(Guid id)
		{
			Id = id;
		}

		#region Properties
		public Guid Id
		{
			get;
			protected set;
		}
		#endregion
	}
}