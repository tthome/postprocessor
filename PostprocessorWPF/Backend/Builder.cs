using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PostprocessorWPF.Backend
{
    internal class Builder
    {
        public List<Element> Elements;
        public List<Point3D> Points;

        public Point3D CenterPoint3D
        {
            get
            {
                var xMax = Points.Max(p => p.X);
                var xMin = Points.Min(p => p.X);

                var yMax = Points.Max(p => p.Y);
                var yMin = Points.Min(p => p.Y);

                var zMax = Points.Max(p => p.Z);
                var zMin = Points.Min(p => p.Z);

                return new Point3D((xMin + xMax)/2, (yMin + yMax)/2, (zMin + zMax)/2);
            }
        }

        public void ReadPoints(string path)
        {
            Points = new List<Point3D>();
            var regex = new Regex(@"\b\d+(\.\d*)?\b");

            using (var reader = new StreamReader(path))
            {
                var line = reader.ReadLine();
                if (line == null) return;

                var s = regex.Matches(line);

                var xCount = Convert.ToInt32(s[0].Value);
                var yCount = Convert.ToInt32(s[1].Value);
                var zCount = Convert.ToInt32(s[2].Value);

                for (var i = 0; i < zCount*yCount; i++)
                {
                    line = reader.ReadLine();
                    if (line == null) continue;

                    s = regex.Matches(line);
                    for (var j = 0; j < xCount; j++)
                    {
                        var x = Convert.ToDouble(s[3*j].Value, CultureInfo.InvariantCulture);
                        var y = Convert.ToDouble(s[3*j + 1].Value, CultureInfo.InvariantCulture);
                        var z = Convert.ToDouble(s[3*j + 2].Value, CultureInfo.InvariantCulture);

                        var point = new Point3D(x, y, z);
                        Points.Add(point);
                    }
                }
            }
        }

        public void ReadElements(string path)
        {
            Elements = new List<Element>();
            var regex = new Regex(@"\b\d+(\.\d*)?\b");

            using (var reader = new StreamReader(path))
            {
                var line = reader.ReadLine();
                if (line == null) return;

                var elementCount = Convert.ToInt32(line);
                for (var i = 0; i < elementCount; i++)
                {
                    line = reader.ReadLine();
                    if (line == null) continue;

                    var element = new Element();
                    var s = regex.Matches(line);

                    element.Id = Convert.ToInt32(s[0].Value);
                    for (var j = 1; j < 9; j++)
                    {
                        element.Points[j - 1] =
                            Points[Convert.ToInt32(s[j].Value, CultureInfo.InvariantCulture) - 1];
                    }
                    Elements.Add(element);
                }
            }
        }

        public void Draw()
        {
            if (Elements == null) return;
            foreach (var element in Elements)
            {
                element.Draw();
            }
        }

        public void SetMode(Mode mode)
        {
            if (Elements == null) return;
            foreach (var element in Elements)
            {
                element.Mode = mode;
            }
        }
    }
}