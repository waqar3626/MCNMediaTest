using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    /// <summary>
    /// The base modal class for location.
    /// Can be used for Country and County.
    /// </summary>
    /// <remarks>
    /// Place is a base modal class and can be extended for specialized type of models
    /// </remarks>
    public class Place
    {
        /// <summary>The PlaceId property represents the place identifier.</summary>
        public int PlaceId { get; set; }

        /// <summary>The PlaceName property represents the place name.</summary>
        public string PlaceName { get; set; }

        /// <summary>The PlaceSlug property represents the place slug.</summary>
        public string PlaceSlug { get; set; }
    }
}
