using AutoMapper;
using Iot.Common;
using Iot.WebAPI.Domain;

namespace Iot.WebAPI.Profiles
{
	public class UnitProfile : Profile
	{
		#region .ctor
		public UnitProfile()
		{
			CreateMap<Unit, UnitDto>()
				.ReverseMap();
		}
		#endregion
	}
}
