using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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