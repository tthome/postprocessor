using System;
using System.Collections.Generic;

namespace PostprocessorWPF.Backend
{
    internal class Line
    {
        private readonly Point3D _point1;
        private readonly Point3D _point2;

        public Line(Point3D point1, Point3D point2)
        {
            _point1 = point1;
            _point2 = point2;
        }

        public Point3D GetPoint(double offset)
        {
            var d = _point1.Distance(_point2);
            var l = offset/(d - offset);
            var x = (_point1.X + l*_point2.X)/(1 + l);
            var y = (_point1.Y + l*_point2.Y)/(1 + l);
            var z = (_point1.Z + l*_point2.Z)/(1 + l);
            return new Point3D(x, y, z);
        }

        public Point3D[] Split(KeyValuePair<int, double> partition)
        {
            var points = new Point3D[partition.Key + 1];
            var coefficient = partition.Value;
            if (coefficient < 0.0)
            {
                coefficient = -1.0/coefficient;
            }
            var length = _point1.Distance(_point2);
            double interval;
            if (Math.Abs(coefficient - 1.0) < double.Epsilon)
            {
                interval = length/partition.Key;
            }
            else
            {
                interval = length*(1 - coefficient)/(1 - Math.Pow(coefficient, partition.Key));
            }
            var segment = interval;
            points[0] = _point1;
            for (var i = 1; i < partition.Key; i++)
            {
                points[i] = new Line(_point1, _point2).GetPoint(interval);
                segment = segment*coefficient;
                interval += segment;
            }
            points[partition.Key] = _point2;
            return points;
        }
    }
}