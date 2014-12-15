using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Server.Models;

namespace Server.MappingOverride
{
    public class UserMappingOverride : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Map(x => x.LoginEmail).Not.Nullable();
            mapping.Map(x => x.PasswordHash).Not.Nullable();
            mapping.Map(x => x.FullName).Not.Nullable();
        }
    }

    public class VoteMappingOverride : IAutoMappingOverride<Vote>
    {
        public void Override(AutoMapping<Vote> mapping)
        {
            mapping.Map(x => x.User).Not.Nullable();
            mapping.Map(x => x.Liked).Not.Nullable();
            mapping.Map(x => x.Comment).Not.Nullable();
        }
    }

    public class CourseMappingOverride : IAutoMappingOverride<Course>
    {
        public void Override(AutoMapping<Course> mapping)
        {
            mapping.Map(x => x.CourseId).Not.Nullable();
            mapping.Map(x => x.Name).Not.Nullable();
            mapping.Map(x => x.University).Not.Nullable();
        }
    }

    public class CommentMappingOverride : IAutoMappingOverride<Comment>
    {
        public void Override(AutoMapping<Comment> mapping)
        {
            mapping.Map(x => x.User).Not.Nullable();
        }
    }

    public class CourseCommentMappingOverride : IAutoMappingOverride<CourseComment>
    {
        public void Override(AutoMapping<CourseComment> mapping)
        {
            mapping.Map(x => x.Course).Not.Nullable();
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
            mapping.Map(x => x.University).Not.Nullable();
        }
    }

    public class TeacherMappingOverride : IAutoMappingOverride<Teacher>
    {
        public void Override(AutoMapping<Teacher> mapping)
        {
            mapping.Map(x => x.Name).Not.Nullable();
        }
    }

    public class TeacherCommentMappingOverride : IAutoMappingOverride<TeacherComment>
    {
        public void Override(AutoMapping<TeacherComment> mapping)
        {
            mapping.Map(x => x.Teacher).Not.Nullable();
        }
    }

    public class UniversityMappingOverride : IAutoMappingOverride<University>
    {
        public void Override(AutoMapping<University> mapping)
        {
            mapping.Map(x => x.Name).Not.Nullable();
        }
    }
}