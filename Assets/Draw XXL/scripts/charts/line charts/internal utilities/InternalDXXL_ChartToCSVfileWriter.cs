namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;

    public class InternalDXXL_ChartToCSVfileWriter
    {
        public static string default_csvFileName = "DrawXXL_chart";

        public void ExportToCSVfile(ChartLines lines, string fileName)
        {
            if (fileName == null || fileName == "")
            {
                fileName = default_csvFileName;
            }

            string pathPlusFileNameInclFileTypeEnding = Create_pathPlusFileNameInclFileTypeEnding(fileName);

            int maxFilesWithSameName = 1000000;
            bool hasFoundValidFilename = false;
            int sequentialNumberOfSameFilename = -1;
            for (int i = 0; i < maxFilesWithSameName; i++)
            {
                if (File.Exists(pathPlusFileNameInclFileTypeEnding))
                {
                    sequentialNumberOfSameFilename = i + 1;
                    pathPlusFileNameInclFileTypeEnding = Create_pathPlusFileNameInclFileTypeEnding(fileName + "(" + sequentialNumberOfSameFilename + ")");
                }
                else
                {
                    hasFoundValidFilename = true;
                    break;
                }
            }

            if (hasFoundValidFilename == false)
            {
                Debug.LogError("Generating CSV file failed: Too many files with same name (of '" + fileName + "').");
                return;
            }

            StreamWriter streamWriter = null;
            try
            {
                List<ChartLine> allLinesWithAtLeastOneDataPoint_validOrInvalid = lines.Get_all_hiddenAndUnhiddenLines_withAtLeastOneValidOrInvalidDatapoint(out int numberOfDatapoints_inLongestLine, false);
                streamWriter = new StreamWriter(pathPlusFileNameInclFileTypeEnding);

                //Line names:
                for (int i_line = 0; i_line < allLinesWithAtLeastOneDataPoint_validOrInvalid.Count; i_line++)
                {
                    string lineName = allLinesWithAtLeastOneDataPoint_validOrInvalid[i_line].GetNameCompound(false);
                    if (i_line < (allLinesWithAtLeastOneDataPoint_validOrInvalid.Count - 1))
                    {
                        streamWriter.Write("\"x of " + lineName + "\",\"y of " + lineName + "\",");
                    }
                    else
                    {
                        streamWriter.WriteLine("\"x of " + lineName + "\",\"y of " + lineName + "\"");
                    }
                }

                //data points:
                for (int i_datapoint = 0; i_datapoint < numberOfDatapoints_inLongestLine; i_datapoint++)
                {
                    for (int i_line = 0; i_line < allLinesWithAtLeastOneDataPoint_validOrInvalid.Count; i_line++)
                    {
                        if (i_datapoint < allLinesWithAtLeastOneDataPoint_validOrInvalid[i_line].dataPoints.Count)
                        {
                            if (i_line < (allLinesWithAtLeastOneDataPoint_validOrInvalid.Count - 1))
                            {
                                streamWriter.Write("\"" + allLinesWithAtLeastOneDataPoint_validOrInvalid[i_line].dataPoints[i_datapoint].xValue + "\",\"" + allLinesWithAtLeastOneDataPoint_validOrInvalid[i_line].dataPoints[i_datapoint].yValue + "\",");
                            }
                            else
                            {
                                streamWriter.WriteLine("\"" + allLinesWithAtLeastOneDataPoint_validOrInvalid[i_line].dataPoints[i_datapoint].xValue + "\",\"" + allLinesWithAtLeastOneDataPoint_validOrInvalid[i_line].dataPoints[i_datapoint].yValue + "\"");
                            }
                        }
                        else
                        {
                            if (i_line < (allLinesWithAtLeastOneDataPoint_validOrInvalid.Count - 1))
                            {
                                streamWriter.Write(",,");
                            }
                            else
                            {
                                streamWriter.WriteLine(",");
                            }
                        }
                    }
                }
            }
            catch
            {
                Debug.LogError("Writing CSV file failed.");
            }
            finally
            {
                try
                {
                    streamWriter.Close();
                    if (sequentialNumberOfSameFilename == (-1))
                    {
                        Debug.Log("The file '" + fileName + ".csv' was generated.");
                    }
                    else
                    {
                        Debug.Log("The file '" + fileName + "(" + sequentialNumberOfSameFilename + ").csv' was generated.");
                    }
                }
                catch
                {
                    Debug.LogError("Closing the new CSV file failed.");
                }
            }
        }

        string Create_pathPlusFileNameInclFileTypeEnding(string fileName)
        {
            return (Application.dataPath + "/" + fileName + ".csv");
        }

    }
}
