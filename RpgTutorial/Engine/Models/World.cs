using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class World
    {
        private List<Location> _locations = new List<Location>();

        internal void AddLocation(int xCoord, int yCoord, string name, string description, string imageSource)
        {
            Location location = new Location()
            {
                XCoordinate = xCoord,
                YCoordinate = yCoord,
                Name = name,
                Description = description,
                ImageName = imageSource
            };

            _locations.Add(location);
        }

        /// <summary>
        /// Returns an instance of Location that exists at the given coordinates.
        /// </summary>
        /// <param name="xCoord"></param>
        /// <param name="yCoord"></param>
        /// <returns></returns>
        public Location LocationAt(int xCoord, int yCoord)
        {
            Location loc = _locations.FirstOrDefault(l => l.XCoordinate == xCoord && l.YCoordinate == yCoord);
            return loc;
        }
    }
}
