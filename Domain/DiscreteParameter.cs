using System;

namespace Iot.WebAPI.Domain
{
	public class DiscreteParameter : Parameter<bool>
	{
		protected DiscreteParameter()
		{
		}
		protected override object GetValueOrNull(object value)
		{
			if (bool.TryParse(value?.ToString(), out var v))
			{
				return v;
			}

			return false;
		}

		public DiscreteParameter(Guid parameterId)
			: base(parameterId)
		{
		}
	}
}