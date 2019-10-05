using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Iot.Common;
using Iot.WebAPI.Dal;
using Iot.WebAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Iot.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DevicesController : ControllerBase
	{
		#region Data
		#region Fields
		private readonly DatabaseContext _db;
		private readonly IMapper _mapper;
		#endregion
		#endregion

		#region .ctor
		public DevicesController(DatabaseContext context, IMapper mapper)
		{
			_db = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}
		#endregion

		#region Public
		[HttpPost]
		public async Task<ActionResult<DeviceDto>> AddDevice([FromBody] DeviceDto deviceDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(deviceDto);
			}

			if (await _db.Devices.AnyAsync(d => d.DeviceName == deviceDto.DeviceName))
			{
				return BadRequest(deviceDto.DeviceName);
			}

			var device = _mapper.Map<Device>(deviceDto);
			var result = await _db.Devices.AddAsync(device);
			await _db.SaveChangesAsync();
			return Ok(_mapper.Map<DeviceDto>(result.Entity));
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteDevice(Guid id)
		{
			if (id == Guid.Empty)
			{
				return BadRequest(id);
			}

			var result = await _db.Devices.FindAsync(id);

			if (result == null)
			{
				return NotFound(id);
			}

			_db.Remove(result);
			await _db.SaveChangesAsync();

			return NoContent();
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
		{
			var devices = await _db.Devices
										.Include(d => d.Parameters)
										.ThenInclude(b => (b as AnalogParameter).Unit)
										.ToListAsync();
			var dtos = _mapper.Map<IEnumerable<DeviceDto>>(devices);

			return Ok(dtos);
		}
		#endregion
	}
}
