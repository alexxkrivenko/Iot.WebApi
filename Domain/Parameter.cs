using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iot.WebAPI.Domain
{
	public abstract class Parameter <T> : ParameterBase
	{
		protected Parameter()
		{
		}
		protected Parameter(Guid parameterId)
			: base(parameterId)
		{
		}

		public T Value
		{
			get;
			set;
		}
	}
}
