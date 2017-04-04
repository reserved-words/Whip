using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.ViewModels.Singletons.Interfaces;

namespace Whip.ViewModels.Singletons
{
    public class Library : ILibrary
    {
        public ICollection<Artist> Artists { get; set; }
    }
}
