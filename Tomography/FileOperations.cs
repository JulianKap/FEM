namespace Tomography
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Microsoft.Office.Interop.Excel;
    using Delaunay;
    using Geometry;


    /// <summary>
    /// Файловые операции.
    /// </summary>
    public static class FileOperations
    {
        /// <summary>
        /// Открыть список фигур.
        /// </summary>
        /// <param name="openFileDialog">Диалоговое окно, позволяющее открыть файл</param>
        public static List<IFigure> OpenFigures(OpenFileDialog openFileDialog)
        {
            List<IFigure> figures = null;
            openFileDialog.Filter = "Текстовый файл|*.dat*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                using (var fStream = File.OpenRead(openFileDialog.FileName))
                {
                    var formatter = new BinaryFormatter();
                    figures = (List<IFigure>)formatter.Deserialize(fStream);
                }

            return figures;
        }

        /// <summary>
        /// Быстрое сохранение (в ...\TrDelone\bin\)
        /// </summary>
        /// <param name="vertex">Список точек</param>
        /// <param name="figures">Список фигур.</param>
        /// <param name="count">Количество точек.</param>
        public static void FastWriteData(List<Vertex> vertex, List<IFigure> figures, int count)
        {
            WriteFiguresDat("Figures.dat", figures);
            WriteDataElectrodeTxt("Potential.txt", vertex, count);
        }
        
        /// <summary>
        /// Сохранить список фигур.
        /// </summary>
        /// <param name="saveFileDialog">Диалоговое окно, позволяющее сохранить файл.</param>
        /// <param name="figures">Список фигур.</param>
        public static string WriteFigures(SaveFileDialog saveFileDialog, List<IFigure> figures)
        {
            var str = "";
            saveFileDialog.Filter = "Текстовый файл|*.dat";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                WriteFiguresDat(saveFileDialog.FileName, figures);
                str = "Сохранение фигур завершено!";
            }
            return str;
        }

        /// <summary>
        /// Сохранить данные с датчиков.
        /// </summary>
        /// <param name="saveFileDialog">Диалоговое окно, позволяющее сохранить файл.</param>
        /// <param name="points">Список точек.</param>
        /// <param name="count">Количество точек.</param>
        public static string WriteDataElectrode(SaveFileDialog saveFileDialog, List<Vertex> points, int count)
        {
            var str = "";
            saveFileDialog.Filter = "Текстовый файл|*.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                WriteDataElectrodeTxt(saveFileDialog.FileName, points, count);
                str = "Сохранение данных расчет в файл txt завершено!";
            }

            return str;
        }

        /// <summary>
        /// Сохранение фигур в бинарном формате.
        /// </summary>
        /// <param name="saveFileDialog">Диалоговое окно, позволяющее сохранить файл.</param>
        /// <param name="points"></param>
        /// <param name="count">Количество точек.</param>
        public static string WriteDataElectrodeExcel(SaveFileDialog saveFileDialog, List<Vertex> points, int count)
        {
            var str = "";
            saveFileDialog.Filter = "Microsoft Excele XLSX|*.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Workbook book = null;
                Worksheet sheet = null;

                if (app == null) return str;

                var missing = Type.Missing;

                book = app.Workbooks.Add(missing);

                if (book != null)
                {
                    sheet = (Worksheet)book.Worksheets.Add(missing, missing, 1, missing);

                    if (sheet != null)
                    {
                        var cell = sheet.Cells[1, 1] = "№";
                        cell = sheet.Cells[1, 2] = count.ToString() + "точек";

                        // Данные в таблице.
                        for (int i = 0; i < points.Count; i++)
                        {
                            cell = sheet.Cells[i + 2, 1] = i + 1;
                            cell = sheet.Cells[i + 2, 2] = points[i].Potential;
                        }
                    }

                    book.Close(true, saveFileDialog.FileName, missing);
                }
                str = "Сохранение данных расчет в файл Excel завершено!";

                app.Quit();
            }

            return str;
        }

        /// <summary>
        /// Сохранение фигур в бинарном формате.
        /// </summary>
        static void WriteFiguresDat(string path, List<IFigure> figures)
        {
            using (var fStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fStream, figures);
            }
        }
        
        /// <summary>
        /// Сохранение результатов расчетов в текстовом файле.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="points"></param>
        /// <param name="count">Количество точек.</param>
        static void WriteDataElectrodeTxt(string path, List<Vertex> points, int count)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(count.ToString());
                for (int i = 0; i < points.Count; i++)
                    writer.WriteLine(i.ToString() + ": " + points[i].Potential.Value.ToString());
            }
        }
    }
}