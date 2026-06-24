using System;
using System.IO;
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
        ProcessReports(path,UnitName,ReportType,Priority,Score,Status);
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
    static void ProcessReports(string filePath,string[]unitName, string[] reportType, int[] priority, double[] score, string[] status)
    {
        
        string[] currentdata = LoadFile(filePath);
        int numOfLins = currentdata.Length;


        for (int i = 0; i < numOfLins; i++)
        {

            string[] parts = currentdata[i].Split(",");
            if (parts.Length != 5)
            {continue;}
            if (parts[0].Trim()=="")
            { continue;}
            if (!Enum.TryParse(parts[1], true, out ReportType newData))
            {continue;}
            if (!int.TryParse(parts[2], out int newPriority))
            { continue;}
            if (newPriority<1 | newPriority>5)
            {continue;}
            if (!double.TryParse(parts[3],out double newScore))
            {continue;}
            if (newScore<0.0 | newScore>100.0)
            {continue;}
            if (!Enum.TryParse(parts[4], true, out Status newStatus))
            {continue;}
            else
            {
                unitName[i] = parts[0].Trim();
                reportType[i] = parts[1];
                priority[i] = newPriority;
                score[i] = newScore;
                status[i] = parts[4];

            }


            


        }
    }
    //static void inEnum(Enum enumName,string data)
    //{
        
    //    if (Enum.TryParse(data,true,out enumName newData))
    //    {

    //    }
    //}
    
}