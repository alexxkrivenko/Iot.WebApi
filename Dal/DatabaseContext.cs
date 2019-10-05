using Iot.WebAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace Iot.WebAPI.Dal
{
	public class DatabaseContext : DbContext
	{
		#region .ctor
		public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{
		}
		#endregion

		#region Properties
		public DbSet<Device> Devices
		{
			get;
			set;
		}

		public DbSet<Unit> Units
		{
			get;
			set;
		}

		public DbSet<ParameterBase> Parameters
		{
			get;
			set;
		}
		#endregion

		#region Overrided
		/// <summary>
		/// Override this method to further configure the model that was discovered by convention from the entity types
		/// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting
		/// model may be cached
		/// and re-used for subsequent instances of your derived context.
		/// </summary>
		/// <remarks>
		/// If a model is explicitly set on the options for this context (via
		/// <see
		///     cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />
		/// )
		/// then this method will not be run.
		/// </remarks>
		/// <param name="modelBuilder">
		/// The builder being used to construct the model for this context. Databases (and other extensions) typically
		/// define extension methods on this object that allow you to configure aspects of the model that are specific
		/// to a given database.
		/// </param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AnalogParameter>()
						.ToTable("Analog")
						.HasOne(parameter => parameter.Unit);
			modelBuilder.Entity<DiscreteParameter>();

			base.OnModelCreating(modelBuilder);
		}
		#endregion
	}
}
