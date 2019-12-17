using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class World
    {
        private readonly List<Location> _locations = new List<Location>();

        internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string imageSource)
        {
            var location = new Location(xCoordinate,yCoordinate,name,description,imageSource);
            _locations.Add(location);
        }

        /// <summary>
        /// Returns an instance of Location that exists at the given coordinates.
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <returns></returns>
        public Location LocationAt(int xCoordinate, int yCoordinate) 
            => _locations.FirstOrDefault(l => l.XCoordinate == xCoordinate && l.YCoordinate == yCoordinate);
    }
}
