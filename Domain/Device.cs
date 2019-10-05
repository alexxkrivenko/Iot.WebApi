using System;
using System.Collections.Generic;

namespace Iot.WebAPI.Domain
{
	public class Device : Entity
	{
		#region .ctor
		public Device(Guid deviceId, string name)
			: base(deviceId)
		{
			if (deviceId == Guid.Empty)
			{
				throw new ArgumentException("Идентификатор устройства не может быть пустым.", nameof(deviceId));
			}

			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Имя устройства не задано.", nameof(deviceId));
			}

			Id = deviceId;
			DeviceName = name;
		}

		protected Device()
		{
		}
		#endregion

		#region Properties
		public string DeviceName
		{
			get;
			set;
		}

		public List<ParameterBase> Parameters
		{
			get;
			set;
		} = new List<ParameterBase>();
		#endregion
	}
}
