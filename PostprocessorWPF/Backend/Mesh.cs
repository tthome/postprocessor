using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace PostprocessorWPF.Backend
{
    internal class Mesh
    {
        private List<Element> _elements;
        private Point3D[][][] _meshPoints;
        // !!!
        private int _nMeshPoints;
        private KeyValuePair<int, double>[][] _partitions;
        private Point3D[][][] _points;
        private Sector[] _sectors;

        private void ReadPoints(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var regex = new Regex(@"\b\d+(\.\d*)?\b");
                var text = reader.ReadToEnd();
                var matches = regex.Matches(text);

                var xCount = Convert.ToInt32(matches[0].Value);
                var yCount = Convert.ToInt32(matches[1].Value);
                var zCount = Convert.ToInt32(matches[2].Value);

                _points = new Point3D[zCount][][];
                for (var i = 0; i < zCount; i++)
                {
                    _points[i] = new Point3D[yCount][];
                    for (var j = 0; j < yCount; j++)
                    {
                        _points[i][j] = new Point3D[xCount];
                    }
                }

                var m = 3;
                for (var i = 0; i < zCount; i++)
                {
                    for (var j = 0; j < yCount; j++)
                    {
                        for (var k = 0; k < xCount; k++)
                        {
                            var x = Convert.ToDouble(matches[m].Value, CultureInfo.InvariantCulture);
                            var y = Convert.ToDouble(matches[m + 1].Value, CultureInfo.InvariantCulture);
                            var z = Convert.ToDouble(matches[m + 2].Value, CultureInfo.InvariantCulture);

                            _points[i][j][k] = new Point3D(x, y, z);
                            m += 3;
                        }
                    }
                }
            }
        }

        private void ReadSectors(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var regex = new Regex(@"\b\d+(\.\d*)?\b");
                var text = reader.ReadToEnd();
                var matches = regex.Matches(text);
                var sectorCount = Convert.ToInt32(matches[0].Value);
                _sectors = new Sector[sectorCount];

                var m = 1;
                for (var i = 0; i < sectorCount; i++)
                {
                    var id = Convert.ToInt32(matches[m].Value, CultureInfo.InvariantCulture);
                    var x1 = Convert.ToInt32(matches[m + 1].Value, CultureInfo.InvariantCulture);
                    var x2 = Convert.ToInt32(matches[m + 2].Value, CultureInfo.InvariantCulture);
                    var y1 = Convert.ToInt32(matches[m + 3].Value, CultureInfo.InvariantCulture);
                    var y2 = Convert.ToInt32(matches[m + 4].Value, CultureInfo.InvariantCulture);
                    var z1 = Convert.ToInt32(matches[m + 5].Value, CultureInfo.InvariantCulture);
                    var z2 = Convert.ToInt32(matches[m + 6].Value, CultureInfo.InvariantCulture);

                    _sectors[i] = new Sector(id, x1, x2, y1, y2, z1, z2);
                    m += 7;
                }
            }
        }

        private void ReadPartitions(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var regex = new Regex(@"\b\d+(\.\d*)?\b");
                var text = reader.ReadToEnd();
                var matches = regex.Matches(text);

                var xCount = _points[0][0].Length - 1;
                var yCount = _points[0].Length - 1;
                var zCount = _points.Length - 1;

                _partitions = new KeyValuePair<int, double>[3][];
                _partitions[0] = new KeyValuePair<int, double>[xCount];
                _partitions[1] = new KeyValuePair<int, double>[yCount];
                _partitions[2] = new KeyValuePair<int, double>[zCount];

                var m = 0;
                for (var i = 0; i < xCount; i++)
                {
                    var p1 = Convert.ToInt32(matches[m].Value, CultureInfo.InvariantCulture);
                    var p2 = Convert.ToDouble(matches[m + 1].Value, CultureInfo.InvariantCulture);

                    _partitions[0][i] = new KeyValuePair<int, double>(p1, p2);
                    m += 2;
                }

                for (var i = 0; i < yCount; i++)
                {
                    var p1 = Convert.ToInt32(matches[m].Value, CultureInfo.InvariantCulture);
                    var p2 = Convert.ToDouble(matches[m + 1].Value, CultureInfo.InvariantCulture);

                    _partitions[1][i] = new KeyValuePair<int, double>(p1, p2);
                    m += 2;
                }

                for (var i = 0; i < zCount; i++)
                {
                    var p1 = Convert.ToInt32(matches[m].Value, CultureInfo.InvariantCulture);
                    var p2 = Convert.ToDouble(matches[m + 1].Value, CultureInfo.InvariantCulture);

                    _partitions[2][i] = new KeyValuePair<int, double>(p1, p2);
                    m += 2;
                }
            }
        }

        private void SplitPoints()
        {
            var nMeshX = 1;
            for (var i = 0; i < _partitions[0].Length; i++)
            {
                nMeshX += _partitions[0][i].Key;
            }

            var nMeshY = 1;
            for (var i = 0; i < _partitions[1].Length; i++)
            {
                nMeshY += _partitions[1][i].Key;
            }

            var nMeshZ = 1;
            for (var i = 0; i < _partitions[2].Length; i++)
            {
                nMeshZ += _partitions[2][i].Key;
            }

            _meshPoints = new Point3D[nMeshZ][][];
            for (var i = 0; i < nMeshZ; i++)
            {
                _meshPoints[i] = new Point3D[nMeshY][];
                for (var j = 0; j < nMeshY; j++)
                {
                    _meshPoints[i][j] = new Point3D[nMeshX];
                }
            }

            for (var i = 0; i < _points.Length - 1; i++)
            {
                var mj = 0;
                for (var j = 0; j < _points[0].Length; j++)
                {
                    var mk = 0;
                    for (var k = 0; k < _points[0][0].Length; k++)
                    {
                        var localPoints = new Line(_points[i][j][k], _points[i + 1][j][k]).Split(_partitions[2][i]);
                        for (var l = 0; l < localPoints.Length; l++)
                        {
                            _meshPoints[i + l][mj][mk] = localPoints[l];
                        }
                        if (k + 1 < _points[0][0].Length)
                        {
                            mk += _partitions[0][k].Key;
                        }
                    }
                    if (j + 1 < _points[0].Length)
                    {
                        mj += _partitions[1][j].Key;
                    }
                }
            }

            for (var i = 0; i < _meshPoints.Length; i++)
            {
                var mj = 0;
                for (var j = 0; j < _points[0].Length - 1; j++)
                {
                    var mk = 0;
                    for (var k = 0; k < _points[0][0].Length; k++)
                    {
                        var localPoints =
                            new Line(_meshPoints[i][mj][mk], _meshPoints[i][mj + _partitions[1][j].Key][mk]).Split(
                                _partitions[1][j]);
                        for (var l = 0; l < localPoints.Length; l++)
                        {
                            _meshPoints[i][mj + l][mk] = localPoints[l];
                        }
                        if (k + 1 < _points[0][0].Length)
                        {
                            mk += _partitions[0][k].Key;
                        }
                    }
                    if (j + 1 < _points[0].Length - 1)
                    {
                        mj += _partitions[1][j].Key;
                    }
                }
            }

            for (var i = 0; i < _meshPoints.Length; i++)
            {
                for (var j = 0; j < _meshPoints[0].Length; j++)
                {
                    var mk = 0;
                    for (var k = 0; k < _points[0][0].Length - 1; k++)
                    {
                        var localPoints =
                            new Line(_meshPoints[i][j][mk], _meshPoints[i][j][mk + _partitions[0][k].Key]).Split(
                                _partitions[0][k]);
                        for (var l = 0; l < localPoints.Length; l++)
                        {
                            _meshPoints[i][j][k + l] = localPoints[l];
                        }
                        if (k + 1 < _points[0][0].Length - 1)
                        {
                            mk += _partitions[0][k].Key;
                        }
                    }
                }
            }
        }

        private void EnumeratePoints()
        {
            var id = 1;
            for (var i = 0; i < _meshPoints.Length; i++)
            {
                for (var j = 0; j < _meshPoints[i].Length; j++)
                {
                    for (var k = 0; k < _meshPoints[i][j].Length; k++)
                    {
                        var isSectorFound = false;
                        for (var l = 0; l < _sectors.Length; l++)
                        {
                            if (_meshPoints[i][j][k].X <
                                _points[_sectors[l].Z1 - 1][_sectors[l].Y1 - 1][_sectors[l].X1 - 1].X) continue;
                            if (_meshPoints[i][j][k].X >
                                _points[_sectors[l].Z2 - 1][_sectors[l].Y2 - 1][_sectors[l].X2 - 1].X) continue;
                            if (_meshPoints[i][j][k].Y <
                                _points[_sectors[l].Z1 - 1][_sectors[l].Y1 - 1][_sectors[l].X1 - 1].Y) continue;
                            if (_meshPoints[i][j][k].Y >
                                _points[_sectors[l].Z2 - 1][_sectors[l].Y2 - 1][_sectors[l].X2 - 1].Y) continue;
                            if (_meshPoints[i][j][k].Z <
                                _points[_sectors[l].Z1 - 1][_sectors[l].Y1 - 1][_sectors[l].X1 - 1].Z) continue;
                            if (_meshPoints[i][j][k].Z >
                                _points[_sectors[l].Z2 - 1][_sectors[l].Y2 - 1][_sectors[l].X2 - 1].Z) continue;
                            isSectorFound = true;
                        }
                        if (isSectorFound)
                        {
                            _meshPoints[i][j][k].Id = id;
                            id++;
                        }
                    }
                }
            }
            _nMeshPoints = id - 1;
        }

        private void CreateElements()
        {
            _elements = new List<Element>();

            for (var i = 0; i < _meshPoints.Length - 1; i++)
            {
                for (var j = 0; j < _meshPoints[i].Length - 1; j++)
                {
                    for (var k = 0; k < _meshPoints[i][j].Length - 1; k++)
                    {
                        var element = new Element
                        {
                            Points =
                            {
                                [0] = _meshPoints[i][j][k],
                                [1] = _meshPoints[i][j][k + 1],
                                [2] = _meshPoints[i][j + 1][k],
                                [3] = _meshPoints[i][j + 1][k + 1],
                                [4] = _meshPoints[i + 1][j][k],
                                [5] = _meshPoints[i + 1][j][k + 1],
                                [6] = _meshPoints[i + 1][j + 1][k],
                                [7] = _meshPoints[i + 1][j + 1][k + 1]
                            }
                        };
                        AssignSector(ref element);
                        if (element.Sector != null)
                        {
                            _elements.Add(element);
                        }
                    }
                }
            }
        }

        private void AssignSector(ref Element element)
        {
            int x1 = 0, x2 = 0, y1 = 0, y2 = 0, z1 = 0, z2 = 0;

            var isFound = false;
            for (var i = 0; i < _points.Length - 1; i++)
            {
                for (var j = 0; j < _points[i].Length - 1; j++)
                {
                    for (var k = 0; k < _points[i][j].Length - 1; k++)
                    {
                        if (element.Points[0].X < _points[i][j][k].X || element.Points[0].Y < _points[i][j][k].Y ||
                            element.Points[0].Z < _points[i][j][k].Z) continue;
                        if (element.Points[1].X > _points[i][j][k + 1].X || element.Points[1].Y < _points[i][j][k + 1].Y ||
                            element.Points[1].Z < _points[i][j][k + 1].Z) continue;
                        if (element.Points[2].X < _points[i][j + 1][k].X || element.Points[2].Y > _points[i][j + 1][k].Y ||
                            element.Points[2].Z < _points[i][j + 1][k].Z) continue;
                        if (element.Points[3].X > _points[i][j + 1][k + 1].X ||
                            element.Points[3].Y > _points[i][j + 1][k + 1].Y ||
                            element.Points[3].Z < _points[i][j + 1][k + 1].Z) continue;
                        if (element.Points[4].X < _points[i + 1][j][k].X || element.Points[4].Y < _points[i + 1][j][k].Y ||
                            element.Points[4].Z > _points[i + 1][j][k].Z) continue;
                        if (element.Points[5].X > _points[i + 1][j][k + 1].X ||
                            element.Points[5].Y < _points[i + 1][j][k + 1].Y ||
                            element.Points[5].Z > _points[i + 1][j][k + 1].Z) continue;
                        if (element.Points[6].X < _points[i + 1][j + 1][k].X ||
                            element.Points[6].Y > _points[i + 1][j + 1][k].Y ||
                            element.Points[6].Z > _points[i + 1][j + 1][k].Z) continue;
                        if (element.Points[7].X > _points[i + 1][j + 1][k + 1].X ||
                            element.Points[7].Y > _points[i + 1][j + 1][k + 1].Y ||
                            element.Points[7].Z > _points[i + 1][j + 1][k + 1].Z) continue;

                        x1 = k + 1;
                        x2 = k + 2;
                        y1 = j + 1;
                        y2 = j + 2;
                        z1 = i + 1;
                        z2 = i + 2;

                        isFound = true;
                        break;
                    }
                }
                if (isFound) break;
            }

            element.Sector = null;
            for (var i = 0; i < _sectors.Length; i++)
            {
                if (x1 >= _sectors[i].X1 && x2 <= _sectors[i].X2 && y1 >= _sectors[i].Y1 && y2 <= _sectors[i].Y2 &&
                    z1 >= _sectors[i].Z1 && z2 <= _sectors[i].Z2)
                {
                    element.Sector = _sectors[i];
                    break;
                }
            }
        }

        public void InputArea(string pointsPath, string sectorsPath, string partitionsPath)
        {
            ReadPoints(pointsPath);
            ReadSectors(sectorsPath);
            ReadPartitions(partitionsPath);
        }

        public void Generate()
        {
            SplitPoints();
            EnumeratePoints();
            CreateElements();
        }

        public void WritePoints(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("{0} {1} {2}", _meshPoints[0][0].Length, _meshPoints[0].Length, _meshPoints.Length);

                for (var i = 0; i < _meshPoints.Length; i++)
                {
                    for (var j = 0; j < _meshPoints[i].Length; j++)
                    {
                        for (var k = 0; k < _meshPoints[i][j].Length; k++)
                        {
                            writer.Write("{0} {1} {2}", _meshPoints[i][j][k].X.ToString(CultureInfo.InvariantCulture),
                                _meshPoints[i][j][k].Y.ToString(CultureInfo.InvariantCulture),
                                _meshPoints[i][j][k].Z.ToString(CultureInfo.InvariantCulture));
                            writer.Write(k < _meshPoints[i][j].Length - 1 ? "\t" : "\n");
                        }
                    }
                }
            }
        }

        public void WriteElements(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(_elements.Count);
                foreach (var element in _elements)
                {
                    writer.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8}", element.Sector.Id, element.Points[0].Id,
                        element.Points[1].Id, element.Points[2].Id, element.Points[3].Id, element.Points[4].Id,
                        element.Points[5].Id, element.Points[6].Id, element.Points[7].Id);
                }
            }
        }
    }
}