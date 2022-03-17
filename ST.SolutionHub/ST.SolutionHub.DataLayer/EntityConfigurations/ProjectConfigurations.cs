
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ST.SolutionHub.DataLayer.Constants;
using ST.SolutionHub.DataLayer.Entities;

namespace ST.SolutionHub.DataLayer.EntityConfigurations
{
    public class ProjectConfigurations : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(TableConstants.NAME_LENGTH);
            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(TableConstants.DESCRIPTION_LENGTH);
            builder.Property(x => x.ClientInformation)
                   .HasMaxLength(TableConstants.DESCRIPTION_LENGTH);
            builder.Property(x => x.DeploymentDetails)
                  .HasMaxLength(TableConstants.DESCRIPTION_LENGTH);
            builder.Property(x => x.Files)
                  .HasMaxLength(TableConstants.DESCRIPTION_LENGTH);
            builder.Property(x => x.Videos)
                  .HasMaxLength(TableConstants.DESCRIPTION_LENGTH);

            builder.HasOne(x => x.Type)
                   .WithMany(x => x.Projects)
                   .HasForeignKey(x => x.TypeId);

        }
    }
}
