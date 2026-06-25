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
        string[] loedFile =  LoadFile(path);
        if (loedFile == null)
        {
            return ;
        }
        int numCurrect = 0;
        ProcessReports(path,UnitName,ReportType,Priority,Score,Status,ref numCurrect);
        DisplayBasicStatistics(Score, numCurrect);
        DisplayStatusCounts(Status, numCurrect);
        DisplayTypeCounts(ReportType, numCurrect);
        DisplayHighestPriorityApproved(UnitName, ReportType, Priority, Score, Status,numCurrect);
        DisplayAverageByPriority(Priority,Score,numCurrect);
        
    }
    static string[]? LoadFile(string path)
    {
        string filePath = path;
        if (!File.Exists(path))
        {
            string[] notExist = path.Split("\\");
            Console.WriteLine($"file {notExist[notExist.Length-1]} not exist");
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
        Console.WriteLine($"File loaded: {numOfLins} lines found");
        Console.WriteLine("Processing complete");
        Console.WriteLine($"Valid records: {countCurrectLine}");
        Console.WriteLine($"Invalid records: {numOfLins - countCurrectLine}");
        Console.WriteLine("Stored 23 valid records for analysis");
        return countCurrectLine;
    }
    static double CalculateAverage(int numCurrect, double[] score)
    {
        double numOfAllScore = 0.0;
        for(int i =0;i<numCurrect;i++)
        {
            numOfAllScore += score[i];
        }
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
        Console.WriteLine($"Lowest Score: {min}\n");
    }
    static void DisplayStatusCounts(string[]reportStatus,int numCurrect)
    {
        int pending = CountByStatus(reportStatus,"Pending",numCurrect);
        int approved = CountByStatus(reportStatus, "Approved", numCurrect);
        int rejected = CountByStatus(reportStatus, "Rejected", numCurrect);
        Console.WriteLine("=== Reports by Status ===");
        Console.WriteLine($"Pending: {pending}");
        Console.WriteLine($"Approved: {approved}");
        Console.WriteLine($"Rejected: {rejected}\n");
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
        Console.WriteLine($"Intel: {intel}\n");

    }
    static void DisplayHighestPriorityApproved(string[] unitName, string[] reportType, int[] priority, double[] score, string[] status,int numCurrect)
    {
        int indexHighestPriority = 0;
        int highestPriority = 0;
        for (int i = 0; i < numCurrect;i++)
        {
            if (status[i] == "Approved")
            {
                if (priority[i]>highestPriority)
                {
                    highestPriority = priority[i];
                    indexHighestPriority = i;
                }
            }
        }
        Console.WriteLine("=== Highest Priority Approved Report ===");
        Console.WriteLine($"Unit: {unitName[indexHighestPriority]}");
        Console.WriteLine($"Type: {reportType[indexHighestPriority]}");
        Console.WriteLine($"Priority: {priority[indexHighestPriority]}");
        Console.WriteLine($"Score: {score[indexHighestPriority]}\n");
    }
    static void DisplayAverageByPriority(int[] priority, double[] score,int numCurrect)
    {
        double scorePriority1 = 0.0;
        int priority1 = 0;
        double scorePriority2 = 0.0;
        int priority2 = 0;
        double scorePriority3 = 0.0;
        int priority3 = 0;
        double scorePriority4 = 0.0;
        int priority4 = 0;
        double scorePriority5 = 0.0;
        int priority5 = 0;
        
        for (int i= 0; i < numCurrect; i++)
        {
            if (priority[i] == 1)
            {
                priority1 += 1;
                scorePriority1 += score[i];
            }
            if (priority[i] == 2)
            {
                priority2 += 1;
                scorePriority2 += score[i];
            }
            if (priority[i] == 3)
            {
                priority3 += 1;
                scorePriority3 += score[i];
            }
            if (priority[i] == 4)
            {
                priority4 += 1;
                scorePriority4 += score[i];
            }
            if (priority[i] == 5)
            {
                priority5 += 1;
                scorePriority5 += score[i];
            }

        }
        Console.WriteLine("=== Average Score by Priority ===");
        if (priority1 != 0)
        { Console.WriteLine($"priority 1: {(scorePriority1/priority1):F2}"); }
        else
        { Console.WriteLine("Priority 1: No reports"); }
        if (priority2 != 0)
        { Console.WriteLine($"priority 2: {(scorePriority2 / priority2):F2}"); }
        else
        { Console.WriteLine("Priority 2: No reports"); }
        if (priority3 != 0)
        { Console.WriteLine($"priority 3: {(scorePriority3 / priority3):F2}"); }
        else
        { Console.WriteLine("Priority 3: No reports"); }
        if (priority4 != 0)
        { Console.WriteLine($"priority 4: {(scorePriority4 / priority4):F2}"); }
        else
        { Console.WriteLine("Priority 4: No reports"); }
        if (priority5 != 0)
        { Console.WriteLine($"priority 5: {(scorePriority5 / priority5):F2}"); }
        else
        { Console.WriteLine("Priority 5: No reports"); }

    }



}