using System;
using System.Collections.Generic;
using System.Text;

namespace Alliander.Connectivity.Registry
{
    public interface IContainerRecipe
    {
        /// <summary>
        /// Densify the topology of the nodes.
        /// </summary>
        /// <param name="node">Instance of <see cref="INodes"/></param>
        /// <returns></returns>
        public string Densify(INodes node);
    }
}
