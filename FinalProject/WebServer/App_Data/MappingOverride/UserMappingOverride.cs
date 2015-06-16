using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using WebServer.App_Data.Models;

namespace WebServer.App_Data.MappingOverride
{
    public class VoteMappingOverride : IAutoMappingOverride<Vote>
    {
        public void Override(AutoMapping<Vote> mapping)
        {
            mapping.Map(x => x.Liked).Not.Nullable();
        }
    }

    public class CourseMappingOverride : IAutoMappingOverride<Course>
    {
        public void Override(AutoMapping<Course> mapping)
        {
            mapping.Map(x => x.Name).Not.Nullable();
        }
    }

    public class CourseCriteriaMappingOverride : IAutoMappingOverride<CourseCriteria>
    {
        public void Override(AutoMapping<CourseCriteria> mapping)
        {
            mapping.Map(x => x.DisplayName).Not.Nullable();
        }
    }

    public class FacultyMappingOverride : IAutoMappingOverride<Faculty>
    {
        public void Override(AutoMapping<Faculty> mapping)
        {
            mapping.Map(x => x.Name).Not.Nullable();
        }
    }

    public class TeacherMappingOverride : IAutoMappingOverride<Teacher>
    {
        public void Override(AutoMapping<Teacher> mapping)
        {
            mapping.Map(x => x.Name).Not.Nullable();
        }
    }

    public class UplodedFileMappingOverride : IAutoMappingOverride<UplodedFile>
    {
        public void Override(AutoMapping<UplodedFile> mapping)
        {
            mapping.Map(x => x.File).Not.Nullable();
            mapping.Map(x => x.FileName).Not.Nullable();
        }
    }
}