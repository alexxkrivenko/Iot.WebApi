using System;
using System.Collections.Generic;
using System.Linq;
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
	public class UnitsController : ControllerBase
	{
		private DatabaseContext _db;
		private IMapper _mapper;

		public UnitsController(DatabaseContext context, IMapper mapper)
		{
			_db = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UnitDto>>> GetUnits()
		{
			_db.Units.Add(new Unit(Guid.NewGuid(), "см."));
			_db.Units.Add(new Unit(Guid.NewGuid(), "мм."));
			await _db.SaveChangesAsync();

			var dtos = Enumerable.Empty<UnitDto>();

			dtos = _mapper.Map<IEnumerable<UnitDto>>(await _db.Units.ToArrayAsync());

			return Ok(dtos);
		}


	}
}
