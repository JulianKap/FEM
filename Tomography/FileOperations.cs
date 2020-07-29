namespace Tomography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using System.IO;
    using Delaunay;

    ///// <summary>
    ///// Файловые операции
    ///// </summary>
    //public static class FileOperations
    //{
    //    /// <summary>
    //    /// Открыть список точек
    //    /// </summary>
    //    /// <param name="openFileDialog">Диалоговое окно, позволяющее открыть файл</param>
    //    public static List<Vertex> OpenPoints(OpenFileDialog openFileDialog)
    //    {
    //        var vertex = new List<Vertex>();

    //        openFileDialog.Filter = "Текстовый файл|*.txt*";
    //        if (openFileDialog.ShowDialog() == DialogResult.OK)
    //        {
    //            var lineString = new StreamReader(openFileDialog.FileName);
    //            var sLine = "";

    //            // Заполняем список точек
    //            while (sLine != null)
    //            {
    //                sLine = lineString.ReadLine();

    //                if (sLine != null)
    //                {
    //                    var line = sLine.Split();

    //                    if (line.Count() == 2)
    //                        vertex.Add(new Vertex(float.Parse(line[0]), float.Parse(line[1])));
    //                }
    //            }
    //        }

    //        return vertex;
    //    }

    //    /// <summary>
    //    /// Открыть коллекцию треугольников
    //    /// </summary>
    //    /// <param name="openFileDialog">Диалоговое окно, позволяющее открыть файл</param>
    //    public static ICollection<Triangle> OpenCollectionTriangles(OpenFileDialog openFileDialog)
    //    {
    //        var triangle = new List<Triangle>();

    //        openFileDialog.Filter = "Текстовый файл|*.txt*";
    //        if (openFileDialog.ShowDialog() == DialogResult.OK)
    //        {
    //            var lineString = new StreamReader(openFileDialog.FileName);
    //            var sLine = "";

    //            // Заполняем  список треугольников
    //            while(sLine != null)
    //            {
    //                sLine = lineString.ReadLine();
                    
    //                if (sLine != null)
    //                {
    //                    var line = sLine.Split();

    //                    if (line.Count() == 6)
    //                    {
    //                        var t = new Triangle();

    //                        t.Points[0] = new Vertex(float.Parse(line[0]), float.Parse(line[1]));
    //                        t.Points[1] = new Vertex(float.Parse(line[2]), float.Parse(line[3]));
    //                        t.Points[2] = new Vertex(float.Parse(line[4]), float.Parse(line[5]));

    //                        t.Ribs[0] = new Rib(t.Points[0], t.Points[1], null, null);
    //                        t.Ribs[1] = new Rib(t.Points[1], t.Points[2], null, null);
    //                        t.Ribs[2] = new Rib(t.Points[2], t.Points[0], null, null);

    //                        triangle.Add(t);
    //                    }
    //                }
    //            }
    //        }
            
    //        return triangle;
    //    }

    //    /// <summary>
    //    /// Быстрое сохранение (в ...\TrDelone\bin\Release)
    //    /// </summary>
    //    /// <param name="vertex">Список точек</param>
    //    /// <param name="triangles">Коллекция треугольников</param>
    //    public static void FastWriteData(List<Vertex> vertex, ICollection<Triangle> triangles)
    //    {
    //        WritePointsTxt("TrPoints.txt", vertex);
    //        WriteTriangleTxt("TrTriangles.txt", triangles);
    //    }

    //    /// <summary>
    //    /// Сохранить список точек
    //    /// </summary>
    //    /// <param name="saveFileDialog">Диалоговое окно, позволяющее сохранить файл</param>
    //    /// <param name="vertex">Список точек</param>
    //    public static void WriteDataPoints(SaveFileDialog saveFileDialog, List<Vertex> vertex)
    //    {
    //        saveFileDialog.Filter = "Текстовый файл|*.txt";

    //        if (saveFileDialog.ShowDialog() == DialogResult.OK)
    //            WritePointsTxt(saveFileDialog.FileName, vertex);
    //    }

    //    /// <summary>
    //    /// Сохранить коллекцию точек
    //    /// </summary>
    //    /// <param name="saveFileDialog">Диалоговое окно, позволяющее сохранить файл</param>
    //    /// <param name="triangles">Коллекция треугольников</param>
    //    public static void WriteDataTriangles(SaveFileDialog saveFileDialog, ICollection<Triangle> triangles)
    //    {
    //        saveFileDialog.Filter = "Текстовый файл|*.txt";

    //        if (saveFileDialog.ShowDialog() == DialogResult.OK)
    //            WriteTriangleTxt(saveFileDialog.FileName, triangles);
    //    }

    //    /// <summary>
    //    /// Сохранение списка точек в txt файл
    //    /// </summary>
    //    /// <param name="path">Директория сохранения файла</param>
    //    /// <param name="vertex">Список точек</param>
    //    private static void WritePointsTxt(string path, List<Vertex> vertex)
    //    {
    //        using (StreamWriter writer = new StreamWriter(path))
    //        {
    //            writer.WriteLine(vertex.Count());

    //            foreach (var v in vertex)
    //                writer.WriteLine(String.Format("{0} {1}", v.X, v.Y));
    //        }
    //    }

    //    /// <summary>
    //    /// Сохранение списка треугольников в txt файл
    //    /// </summary>
    //    /// <param name="path">Директория сохранения файла</param>
    //    /// <param name="triangles">Коллекция треугольников</param>
    //    private static void WriteTriangleTxt(string path, ICollection<Triangle> triangles)
    //    {
    //        using (StreamWriter writer = new StreamWriter(path))
    //        {
    //            writer.WriteLine(triangles.Count());

    //            foreach (var t in triangles)
    //                writer.WriteLine(string.Format("{0,5:F4} {1,5:F4} {2,5:F4} {3,5:F4} {4,5:F4} {5,5:F4}",
    //                    t.Points[0].X, t.Points[0].Y, t.Points[1].X, t.Points[1].Y, t.Points[2].X, t.Points[2].Y));
    //        }
    //    }
    //}
}