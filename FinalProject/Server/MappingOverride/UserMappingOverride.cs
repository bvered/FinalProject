using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Server.Models;

namespace Server.MappingOverride
{
    public class UserMappingOverride : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Map(x => x.BirthDate).Not.Nullable();
        }
    }
}