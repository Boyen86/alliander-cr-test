using System;
using System.Collections.Generic;
using System.Text;

namespace Alliander.Connectivity.Registry
{
    public interface ISplitContainer
    {
        /// <summary>
        /// Performs a splitContainerRecipe that will densify a collection of nodes to a split container.
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns>Result in JSON format</returns>
        public string SplitContainerRecipe(INodes nodes);
    }
}
