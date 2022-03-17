using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ST.SolutionHub.DataLayer.Constants;
using ST.SolutionHub.DataLayer.Entities;

namespace ST.SolutionHub.DataLayer.EntityConfigurations
{
    public class ProjectTypeConfigurations : IEntityTypeConfiguration<ProjectType>
    {
        public void Configure(EntityTypeBuilder<ProjectType> builder)
        {
            builder.ToTable("ProjectTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(TableConstants.NAME_LENGTH);
            builder.Property(x => x.Description)
                  .HasMaxLength(TableConstants.DESCRIPTION_LENGTH);
            builder.Property(x => x.HTML)
                  .HasMaxLength(TableConstants.HTML_LENGTH);
        }
    }
}
