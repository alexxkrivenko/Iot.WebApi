using System;

namespace Iot.WebAPI.Domain
{
	public class AnalogParameter : Parameter<double>
	{
		protected AnalogParameter()
		{
		}
		public AnalogParameter(Guid parameterId) : base(parameterId)
		{	
		}
	

		#region Properties
		public Unit Unit
		{
			get;
			set;
		}
		#endregion

		#region Overrided
		protected override object GetValueOrNull(object value)
		{
			if (float.TryParse(value?.ToString(), out var v))
			{
				return v;
			}

			return 0f;
		}
		#endregion
	}
}
