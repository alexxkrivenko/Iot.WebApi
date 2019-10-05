using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iot.Common;
using Iot.WebAPI.Domain;

namespace Iot.WebAPI.Profiles
{
	public class DeviceProfile : Profile
	{
		public DeviceProfile()
		{
			CreateMap<Device, DeviceDto>().ReverseMap();
		}
	}
}
