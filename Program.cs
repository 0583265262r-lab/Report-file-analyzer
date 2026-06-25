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
        int numCurrect = 0;
        //Console.WriteLine(numCurrect);
        ProcessReports(path,UnitName,ReportType,Priority,Score,Status,ref numCurrect);
        //CalculateAverage(numCurrect, Score);
        //FindMaxScore(Score);
        //FindMinScore(numCurrect, Score);
        //CountByStatus(Status, "Approved", numCurrect);
        DisplayBasicStatistics(Score, numCurrect);
        DisplayStatusCounts(Status, numCurrect);
        DisplayTypeCounts(ReportType, numCurrect);
        
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
    static int ProcessReports(string filePath,string[]unitName, string[] reportType, int[] priority, double[] score, string[] status,ref int numCurrect)
    {
        int countCurrectLine = 0;
        numCurrect = 0;
        string[] currentdata = LoadFile(filePath);
        int numOfLins = currentdata.Length;
        


        for (int i = 0; i < numOfLins; i++)
        {

            string[] parts = currentdata[i].Split(",");
            if (parts.Length != 5)
            {
                
                continue;}
            if (parts[0].Trim()=="")
            {
               continue;}
            if (!Enum.TryParse(parts[1], true, out ReportType newData))
            {continue;}
            if (!int.TryParse(parts[2], out int newPriority))
            {continue;}
            if (newPriority<1 | newPriority>5)
            {continue;}
            if (!double.TryParse(parts[3],out double newScore))
            {continue;}
            if (newScore<0.0 | newScore>100.0)
            { continue;}
            if (!Enum.TryParse(parts[4], true, out Status newStatus))
            { continue; }
            else
            {
                
                unitName[countCurrectLine] = parts[0].Trim();
                reportType[countCurrectLine] = parts[1];
                priority[countCurrectLine] = newPriority;
                score[countCurrectLine] = newScore;
                status[countCurrectLine] = parts[4];
                countCurrectLine++;
                numCurrect++;
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
        //Console.WriteLine(numOfAllScore / numCurrect);
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
        //Console.WriteLine(maxNum);
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
        //Console.WriteLine(numCurrect);
        //Console.WriteLine(minScore);
        return minScore;
    }
    static int CountByStatus(string[] arrStatus, string status,int numCurrect)
    {
        int countStarus = 0;
        for (int i = 0; i < numCurrect;i++)
        {
            if (arrStatus[i] == status)
            {
                countStarus+=1;
            }
        }
        Console.WriteLine(countStarus);
        return countStarus;
    }
    static int CountByType(string[] arrType, string type, int numCurrect)
    {
        int countType = 0;
        for (int i = 0; i < numCurrect; i++)
        {
            if (arrType[i] == type)
            {
                countType++;
            }
        }
        Console.WriteLine(countType);
        return countType;
    }
    static void DisplayBasicStatistics(double[] score, int numCurrect)
    {
        double ave = CalculateAverage(numCurrect, score);
        double max = FindMaxScore(score);
        double min = FindMinScore(numCurrect, score);
        Console.WriteLine("=== Report Statistics ===");
        Console.WriteLine($"Total Reports: {numCurrect}");
        Console.WriteLine($"Average Score: {ave:F2}");
        Console.WriteLine($"Highest Score: {max}");
        Console.WriteLine($"Lowest Score: {min}");
    }
    static void DisplayStatusCounts(string[]reportStatus,int numCurrect)
    {
        int pending = CountByStatus(reportStatus,"Pending",numCurrect);
        int approved = CountByStatus(reportStatus, "Approved", numCurrect);
        int rejected = CountByStatus(reportStatus, "Rejected", numCurrect);
        Console.WriteLine("=== Reports by Status ===");
        Console.WriteLine($"Pending: {pending}");
        Console.WriteLine($"Approved: {approved}");
        Console.WriteLine($"Rejected: {rejected}");
    }
    static void DisplayTypeCounts(string[] reportType, int numCurrect)
    {
        int collect = CountByType(reportType, "Collect", numCurrect);
        int analyzer = CountByType(reportType, "Analyze", numCurrect);
        int recon = CountByType(reportType, "Recon", numCurrect);
        int intel = CountByType(reportType, "Intel", numCurrect);
        Console.WriteLine("=== Reports by Type ===");
        Console.WriteLine($"Collect: {collect}");
        Console.WriteLine($"Analyze: {analyzer}");
        Console.WriteLine($"Recon: {recon}");
        Console.WriteLine($"Intel: {intel}");

    }



}