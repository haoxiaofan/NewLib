/* Copyright 2010-2015 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Collections.ObjectModel;
using NewLib.Data.Mongodb.Bson.Serialization.Attributes;
using NewLib.Data.Mongodb.Driver.GeoJsonObjectModel.Serializers;

namespace NewLib.Data.Mongodb.Driver.GeoJsonObjectModel
{
    /// <summary>
    /// Represents a GeoJson 3D geographic position (longitude, latitude, altitude).
    /// </summary>
    [BsonSerializer(typeof(GeoJson3DGeographicCoordinatesSerializer))]
    public class GeoJson3DGeographicCoordinates : GeoJsonCoordinates
    {
        // private fields
        private ReadOnlyCollection<double> _values;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoJson3DGeographicCoordinates"/> class.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="altitude">The altitude.</param>
        public GeoJson3DGeographicCoordinates(double longitude, double latitude, double altitude)
        {
            _values = new ReadOnlyCollection<double>(new[] { longitude, latitude, altitude });
        }

        // public properties
        /// <summary>
        /// Gets the coordinate values.
        /// </summary>
        public override ReadOnlyCollection<double> Values
        {
            get { return _values; }
        }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        public double Longitude
        {
            get { return _values[0]; }
        }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        public double Latitude
        {
            get { return _values[1]; }
        }

        /// <summary>
        /// Gets the altitude.
        /// </summary>
        public double Altitude
        {
            get { return _values[2]; }
        }
    }
}
