using System.Collections.Generic;
using DefaultNamespace.Core.models;

namespace Core.models
{
    public class UserMap
    {
        public UserMariana UserMariana { get; set; }
        public List<Structure> Structures { get; set; }
        public List<Plot> Plots { get; set; }
    }
}