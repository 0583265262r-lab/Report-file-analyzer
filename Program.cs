using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace reportAnalyze;
enum ReportType {Collect,Analyze,Recon,Intel}
enum Status {Pending,Approved,Rejected}
class Analyze
{
    static void Main()
    {
        const int MAX_REPORTS = 50;
        string path = @"C:\Users\user1\OneDrive\שולחן העבודה\Report file analyzer\reports.txt";
        string[] UnitName = new string [MAX_REPORTS];
        string[] ReportType = new string[MAX_REPORTS];
        int[] Priority = new int[MAX_REPORTS];
        double[] Score = new double[MAX_REPORTS];
        string[] Status = new string[MAX_REPORTS];
        LoadFile(path);
        int numCurrect = ProcessReports(path,UnitName,ReportType,Priority,Score,Status);
        CalculateAverage(numCurrect, Score);
        FindMaxScore(Score);
        FindMinScore(numCurrect, Score);
        
    }
    static string[]? LoadFile(string path)
    {
        string filePath = path;
        if (!File.Exists(path))
        {
            Console.WriteLine("file not exist");
            return null;
        }

        string[] readTetx = File.ReadAllLines(path);
        if (readTetx.Length == 0)
        {
            Console.WriteLine("file is empty");
            return null;
        }
        return readTetx;
    }
    static int ProcessReports(string filePath,string[]unitName, string[] reportType, int[] priority, double[] score, string[] status)
    {
        int countCurrectLine = 0;
        string[] currentdata = LoadFile(filePath);
        int numOfLins = currentdata.Length;


        for (int i = 0; i < numOfLins; i++)
        {

            string[] parts = currentdata[i].Split(",");
            if (parts.Length != 5)
            {
                i -= 1;
                continue;}
            if (parts[0].Trim()=="")
            {
                i -= 1; continue;}
            if (!Enum.TryParse(parts[1], true, out ReportType newData))
            { i -= 1; continue;}
            if (!int.TryParse(parts[2], out int newPriority))
            { i -= 1; continue;}
            if (newPriority<1 | newPriority>5)
            { i -= 1; continue;}
            if (!double.TryParse(parts[3],out double newScore))
            { i -= 1; continue;}
            if (newScore<0.0 | newScore>100.0)
            { i -= 1; continue;}
            if (!Enum.TryParse(parts[4], true, out Status newStatus))
            { i -= 1; continue; }
            else
            {
                unitName[i] = parts[0].Trim();
                reportType[i] = parts[1];
                priority[i] = newPriority;
                score[i] = newScore;
                status[i] = parts[4];
                countCurrectLine++;
            }
            
        }
        return countCurrectLine;
    }
    static double CalculateAverage(int numCurrect, double[] score)
    {

        double numOfAllScore = 0.0;
        for(int i =0;i<numCurrect;i++)
        {
            numOfAllScore += score[i];
            
        }
        Console.WriteLine(numOfAllScore / numCurrect);
        return numOfAllScore / numCurrect;
    }
    static double FindMaxScore(double[]score)
    {
        double maxNum = 0.0;
        for(int i = 0;i<score.Length;i++)
        {
            
            if (score[i]>maxNum)
            {
                maxNum = score[i];
            }
        }
        Console.WriteLine(maxNum);
        return maxNum;
    }
    static double FindMinScore(int numCurrect, double[] score)
    {
        double minScore = 0.0;
        for (int i = 0; i < numCurrect; i++)
        {
            if (i == 0)
            {
                minScore = score[i];
            }
            else
            {
                if (minScore > score[i])
                    minScore = score[i];
            }  
        }
        Console.WriteLine(numCurrect);
        Console.WriteLine(minScore);
        return minScore;
    }



}