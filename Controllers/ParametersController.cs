using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iot.Common.Parameter;
using Iot.WebAPI.Dal;
using Iot.WebAPI.Domain;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Iot.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ParametersController : ControllerBase
	{
		#region Data
		#region Fields
		private readonly DatabaseContext _db;
		private readonly IMapper _mapper;
		#endregion
		#endregion

		#region .ctor
		public ParametersController(DatabaseContext context, IMapper mapper)
		{
			_db = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}
		#endregion

		#region Public
		[HttpPost("{deviceId}/analog")]
		public async Task<ActionResult<AnalogParameterDto>> AddParameter(Guid deviceId,
			[FromBody] AnalogParameterDto parameter)
		{
			return await AddParameter<AnalogParameterDto, AnalogParameter>(deviceId, parameter);
		}

		[HttpPost("{deviceId}/discrete")]
		public async Task<ActionResult<DiscreteParameterDto>> AddParameter(Guid deviceId,
			[FromBody] DiscreteParameterDto parameter)
		{
			return await AddParameter<DiscreteParameterDto, DiscreteParameter>(deviceId, parameter);
		}

		[HttpDelete("{parameterId}")]
		public async Task<ActionResult> DeleteParameter(Guid parameterId)
		{
			if (parameterId == Guid.Empty)
			{
				return BadRequest();
			}

			var parameter = _db.Parameters.SingleOrDefault(p => p.Id == parameterId);

			if (parameter == null)
			{
				return NotFound(parameterId);
			}

			_db.Parameters.Remove(parameter);
			await _db.SaveChangesAsync();

			return NoContent();
		}

		[HttpPut("analog/{parameterId}")]
		public async Task<ActionResult<ParameterDto>> UpdateParameter([FromBody] AnalogParameterDto parameter)
		{
			return await Update(parameter, UpdateUnitValue);
		}

		private void UpdateUnitValue(ParameterDto parameter, ParameterBase param)
		{
			if (parameter is AnalogParameterDto analog &&
				param is AnalogParameter analogParameterEntity)
			{
				var unit = _mapper.Map<Unit>(analog.Unit);
				_db.Entry(unit)
				   .State = EntityState.Unchanged;
				analogParameterEntity.Unit = unit;
			}
		}

		private async Task<ActionResult<ParameterDto>> Update(ParameterDto parameter, Action<ParameterDto, ParameterBase> updateUnitAction = null)
		{
			if (parameter == null)
			{
				return BadRequest();
			}

			var param = await _db.Parameters.SingleOrDefaultAsync(p => p.Id == parameter.Id);

			if (param == null)
			{
				return BadRequest();
			}

			param.Name = parameter.Name;

			updateUnitAction?.Invoke(parameter, param);

			_db.Parameters.Update(param);
			var savedEntities = await _db.SaveChangesAsync();

			if (savedEntities <= 0)
			{
				return BadRequest();
			}

			return parameter;
		}

		[HttpPut("discrete/{parameterId}")]
		public async Task<ActionResult<ParameterDto>> UpdateParameter([FromBody] DiscreteParameterDto parameter)
		{
			return await Update(parameter);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ParameterDto>>> GetParameters()
		{
			var parameters = await _db.Parameters.Include(b => (b as AnalogParameter).Unit)
									  .ToListAsync();
			var dtos = _mapper.Map<IEnumerable<ParameterDto>>(parameters);
			return Ok(dtos);
		}
		#endregion

		#region Private
		private async Task<ActionResult<TResult>> AddParameter<TResult, TDomain>(Guid deviceId, TResult parameter)
			where TResult : ParameterDto where TDomain : ParameterBase
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var device = await _db.Devices.Where(d => d.Id == deviceId)
								  .Include(p => p.Parameters)
								  .SingleOrDefaultAsync();
			if (device == null)
			{
				return BadRequest();
			}

			if (device.Parameters.Any(p => p.Name == parameter.Name))
			{
				return BadRequest(parameter.Name);
			}

			var addingParameter = _mapper.Map<TDomain>(parameter);

			_db.Entry(addingParameter)
			   .State = EntityState.Added;
			device.Parameters.Add(addingParameter);

			await _db.SaveChangesAsync();

			return Ok(_mapper.Map<TResult>(addingParameter));
		}
		#endregion
	}
}
