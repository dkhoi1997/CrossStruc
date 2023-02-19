using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossStruc.ConcreteBeam.Function
{
    public static class ImportExport
    {
        public static void SaveFile(List<(string[], List<int[]>)> listBeam, string[] arrBeam) // Save csv file
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                RestoreDirectory = true,
                Filter = "Text Files (*.csv)|*.csv",
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string title1 = "/// Beam Data & Internal Force ///";
                string space = default;
                string title2 = "/// Design Data ///";
                using (StreamWriter sw = File.CreateText(saveFileDialog.FileName))
                {
                    sw.WriteLine(title2);
                    sw.WriteLine(string.Join(",", arrBeam));
                    sw.WriteLine(space);
                    sw.WriteLine(title1);
                    foreach (var item in listBeam)
                    {
                        sw.WriteLine(string.Join(",", item.Item1));
                        for (int i = 0; i < item.Item2.Count; i++)
                        {
                            sw.WriteLine(string.Join(",", item.Item2[i]));
                        }
                    }
                }
            }
        }
        public static (List<(string[], List<int[]>)>, string[]) LoadFile() // Load csv file
        {
            List<(string[], List<int[]>)> listBeam = new List<(string[], List<int[]>)>();
            string[] arrBeam = default;

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                RestoreDirectory = true,
                Filter = "Text Files (*.csv)|*.csv",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string[] sw = File.ReadAllLines(openFileDialog.FileName);
                bool getData = false;
                List<string[]> listTemp = new List<string[]>();

                arrBeam = sw[1].Split(',');
                for (int i = 4; i < sw.Length; i++)
                {
                    string[] current = sw[i].Split(',');
                    listTemp.Add(current);
                    if (i == sw.Length - 1)
                    {
                        getData = true;
                    }
                    else
                    {
                        string[] next = sw[i + 1].Split(',');
                        if (next[2] == "Rec")
                        {
                            getData = true;
                        }
                    }
                    if (getData == true)
                    {
                        string[] arrdesign = listTemp[0].ToArray();
                        List<int[]> listforce = new List<int[]>();
                        for (int j = 1; j < listTemp.Count; j++)
                        {
                            int[] temp = new int[6];
                            for (int k = 0; k < 6; k++)
                            {
                                temp[k] = Convert.ToInt32(listTemp[j][k]);
                            }
                            listforce.Add(temp);
                        }
                        listBeam.Add((arrdesign, listforce));
                        listTemp.Clear();
                        getData = false;
                    }
                }
            }
            return (listBeam, arrBeam);
        }
    }
}
