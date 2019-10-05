using AutoMapper;
using Iot.Common;
using Iot.Common.Parameter;
using Iot.WebAPI.Domain;

namespace Iot.WebAPI.Profiles
{
	public class ParameterProfile : Profile
	{
		#region .ctor
		public ParameterProfile()
		{

			CreateMap<ParameterBase, ParameterDto>()
				.ReverseMap();

			CreateMap<DiscreteParameter, DiscreteParameterDto>()
				.IncludeBase<ParameterBase, ParameterDto>()
				.ReverseMap();
			CreateMap<AnalogParameter, AnalogParameterDto>()
				.IncludeBase<ParameterBase, ParameterDto>()
				.ReverseMap();
		}
		#endregion
	}
}
