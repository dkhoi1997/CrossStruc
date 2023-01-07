using RobotOM;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace CrossStruc.Extensions
{
    public class RobotInteractive
    {
        public static (HashSet<int>, List<int[]>) ExtractBarResult(string comb)  // Get internal force bar element in Robot
        {
            List<int[]> listQuery = new List<int[]>();
            IRobotApplication myRobot = new RobotApplication();
            IRobotStructure myStructure = myRobot.Project.Structure;

            // Get selected bar
            RobotSelection selectObject = myStructure.Selections.Get(IRobotObjectType.I_OT_BAR);
            RobotSelection SelCas = myStructure.Selections.Create(IRobotObjectType.I_OT_CASE);
            SelCas.AddText(comb);

            // Use query method
            RobotResultRowSet RobResRowSet = new RobotResultRowSet();
            byte point = 6;

            RobotResultQueryParams RobResQueryParams = myRobot.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
            RobResQueryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            RobResQueryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            RobResQueryParams.SetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT, point);
            RobResQueryParams.Selection.Set(IRobotObjectType.I_OT_BAR, selectObject);
            RobResQueryParams.Selection.Set(IRobotObjectType.I_OT_CASE, SelCas);

            RobResQueryParams.ResultIds.SetSize(6);
            RobResQueryParams.ResultIds.Set(1, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
            RobResQueryParams.ResultIds.Set(2, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
            RobResQueryParams.ResultIds.Set(3, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);
            RobResQueryParams.ResultIds.Set(4, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
            RobResQueryParams.ResultIds.Set(5, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
            RobResQueryParams.ResultIds.Set(6, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);

            IRobotResultQueryReturnType Res = myStructure.Results.Query(RobResQueryParams, RobResRowSet);

            HashSet<int> hashBar = new HashSet<int>();

            bool ok = RobResRowSet.MoveFirst();
            while (ok)
            {
                int[] temp = new int[8];
                temp[0] = RobResRowSet.CurrentRow.GetParam(IRobotResultParamType.I_RPT_BAR);
                temp[1] = RobResRowSet.CurrentRow.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                temp[2] = Convert.ToInt32(RobResRowSet.CurrentRow.GetValue(RobResRowSet.ResultIds.Get(1)) / 1000);
                temp[3] = Convert.ToInt32(RobResRowSet.CurrentRow.GetValue(RobResRowSet.ResultIds.Get(2)) / 1000);
                temp[4] = Convert.ToInt32(RobResRowSet.CurrentRow.GetValue(RobResRowSet.ResultIds.Get(3)) / 1000);
                temp[5] = Convert.ToInt32(RobResRowSet.CurrentRow.GetValue(RobResRowSet.ResultIds.Get(4)) / 1000);
                temp[6] = Convert.ToInt32(RobResRowSet.CurrentRow.GetValue(RobResRowSet.ResultIds.Get(5)) / 1000);
                temp[7] = Convert.ToInt32(RobResRowSet.CurrentRow.GetValue(RobResRowSet.ResultIds.Get(6)) / 1000);

                if (hashBar.Contains(temp[0]) == false)
                {
                    hashBar.Add(temp[0]);
                }
                listQuery.Add(temp);

                ok = RobResRowSet.MoveNext();
            }

            return (hashBar, listQuery);
        }

        public static List<(string[], List<int[]>)> GetConcColumnForceRobot(string comb) // Get RC column force from Robot
        {
            List<(string[], List<int[]>)> listCol= new List<(string[], List<int[]>)>();

            List<string[]> listItem = new List<string[]>();
            (HashSet<int> hashBar, List<int[]> listQuery) = ExtractBarResult(comb);

            // Show progress bar
            ProgressBarWindow progressBar = new();
            progressBar.Show();

            // Merge info and force list
            IRobotApplication myRobot = new RobotApplication();
            IRobotStructure myStructure = myRobot.Project.Structure;
            IRobotBarServer barSer = myStructure.Bars;
            foreach (int item in hashBar)
            {
                double curpercent = (double)item / hashBar.Count * 100; // Update progress bar
                progressBar.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { progressBar.UpdateProgress(curpercent); }));

                IRobotBar barObject = (IRobotBar)barSer.Get(item);
                string barLabel = barObject.GetLabelName(IRobotLabelType.I_LT_BAR_SECTION);
                IRobotLabel secLabel = myStructure.Labels.Get(IRobotLabelType.I_LT_BAR_SECTION, barLabel);
                IRobotBarSectionData dataLabel = secLabel.Data;
                if ((dataLabel.ShapeType == IRobotBarSectionShapeType.I_BSST_CONCR_COL_R) ||
                    (dataLabel.ShapeType == IRobotBarSectionShapeType.I_BSST_CONCR_COL_C))
                {

                    string[] colDefine = new string[6];
                    colDefine[0] = Convert.ToString(item); // Bar name
                    colDefine[1] = barLabel; //  Bar section
                    if (dataLabel.ShapeType == IRobotBarSectionShapeType.I_BSST_CONCR_COL_R)
                    {
                        double Cx = dataLabel.GetValue(IRobotBarSectionDataValue.I_BSDV_BF) * 1000; // Cx
                        double Cy = dataLabel.GetValue(IRobotBarSectionDataValue.I_BSDV_D) * 1000; // Cy
                        colDefine[2] = "Rec";
                        colDefine[3] = Convert.ToString(Cx);
                        colDefine[4] = Convert.ToString(Cy);
                    }
                    else
                    {
                        double D = dataLabel.GetValue(IRobotBarSectionDataValue.I_BSDV_VZ) * 2000; // Diameter
                        colDefine[2] = "Cir";
                        colDefine[3] = Convert.ToString(D);
                    }
                    colDefine[5] = Convert.ToString(barObject.Length);
                    List<int[]> listForce = new List<int[]>();
                    foreach (int[] force in listQuery)
                    {
                        if (force[0] == item)
                        {
                            int[] temp = new int[7];
                            temp[0] = force[1];
                            temp[1] = force[2];
                            temp[2] = force[3];
                            temp[3] = force[4];
                            temp[4] = force[5];
                            temp[5] = force[6];
                            temp[6] = force[7];
                            listForce.Add(temp);
                        }
                    }
                    listCol.Add((colDefine, listForce));
                }
            }
            progressBar.Close();
            return listCol;
        }

        public static List<(string[], List<int[]>)> GetConcBeamForceRobot(string comb) // Get RC beam force from Robot
        {
            List<(string[], List<int[]>)> listBeam= new List<(string[], List<int[]>)>();

            List<string[]> listItem = new List<string[]>();
            (HashSet<int> hashBar, List<int[]> listQuery) = ExtractBarResult(comb);

            // Hiện progress bar
            ProgressBarWindow progressBar = new();
            progressBar.Show();

            // Gộp list chứa thông tin và list nội lực
            IRobotApplication myRobot = new RobotApplication();
            IRobotStructure myStructure = myRobot.Project.Structure;
            IRobotBarServer barSer = myStructure.Bars;

            foreach (int item in hashBar)
            {
                double curpercent = (double)item / hashBar.Count * 100; // Update cho progress bar
                progressBar.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { progressBar.UpdateProgress(curpercent); }));

                IRobotBar barObject = (IRobotBar)barSer.Get(item);
                string barLabel = barObject.GetLabelName(IRobotLabelType.I_LT_BAR_SECTION);
                IRobotLabel secLabel = myStructure.Labels.Get(IRobotLabelType.I_LT_BAR_SECTION, barLabel);
                IRobotBarSectionData dataLabel = secLabel.Data;
                if ((dataLabel.ShapeType == IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT) ||
                        (dataLabel.ShapeType == IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT))
                {
                    double Cx = dataLabel.GetValue(IRobotBarSectionDataValue.I_BSDV_BF) * 1000; // Cx
                    double Cy = dataLabel.GetValue(IRobotBarSectionDataValue.I_BSDV_D) * 1000; // Cy

                    string[] beamDefine = new string[6];
                    beamDefine[0] = Convert.ToString(item); // Bar name
                    beamDefine[1] = barLabel; //  Bar section
                    beamDefine[2] = "Rec";
                    beamDefine[3] = Convert.ToString(Cx);
                    beamDefine[4] = Convert.ToString(Cy);
                    beamDefine[5] = Convert.ToString(barObject.Length);
                    List<int[]> listForce = new List<int[]>();
                    foreach (int[] force in listQuery)
                    {
                        if (force[0] == item)
                        {
                            int[] temp = new int[7];
                            temp[0] = force[1];
                            temp[1] = force[2];
                            temp[2] = force[3];
                            temp[3] = force[4];
                            temp[4] = force[5];
                            temp[5] = force[6];
                            temp[6] = force[7];
                            listForce.Add(temp);
                        }
                    }
                    listBeam.Add((beamDefine, listForce));
                }
            }
            progressBar.Close();
            return listBeam;
        }
    }
}
