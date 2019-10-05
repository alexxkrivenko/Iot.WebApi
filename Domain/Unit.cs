using System;

namespace Iot.WebAPI.Domain
{
	public class Unit : Entity
	{
		#region Properties
		public string Name
		{
			get;
			set;
		}
		#endregion

		protected Unit()
		{
			
		}
		public Unit(Guid unitId, string name)
			: base(unitId)
		{
			Name = name;
		}
	}
}
