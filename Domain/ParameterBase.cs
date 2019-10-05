using System;

namespace Iot.WebAPI.Domain
{
	public class ParameterBase : Entity
	{
		#region .ctor
		protected ParameterBase()
		{
		}

		public ParameterBase(Guid parameterId)
			: base(parameterId)
		{
		}
		#endregion

		#region Properties
		public string Name
		{
			get;
			set;
		}
		#endregion

		#region Overridable
		protected virtual object GetValueOrNull(object value)
		{
			return value;
		}
		#endregion
	}
}
