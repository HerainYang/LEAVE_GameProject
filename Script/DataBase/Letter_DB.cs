using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;
using System;

public class Letter_DB : MonoBehaviour
{
    private static SQLiteHelper sql;
    
    public static string GetContent(int Day,string Sender)
    {
        SqliteDataReader reader;
        sql = new SQLiteHelper("data source=LetterInfo.db");
        
        string Day_string = "\"" + Day + "\"";
        string Sender_string = "\'" + Sender + "\'";
        
        reader = sql.ReadTable("LetterInfo", new string[] { "Content" }, new string[] { "Day", "Sender", "Readable" }, new string[] { "=", "=", "=" }, new string[] { Day_string, Sender_string, "1" });
        reader.Read();
        return reader.GetString(reader.GetOrdinal("Content"));
    }
    public static int GetAddressableLetter()
    {
        SqliteDataReader reader;
        int Count = 0;
        int currentDay = 0;
        sql = new SQLiteHelper("data source=LetterInfo.db");
        reader = sql.ReadTable("LetterInfo", new string[] { "Day"}, new string[] { "Readable" }, new string[] { "=" }, new string[] { "1" });
        while (reader.Read())
        {
            reader.GetInt32(reader.GetOrdinal("Day"));
            if(reader.GetInt32(reader.GetOrdinal("Day")) !=currentDay)
            {
                Count++;
            }
            currentDay = reader.GetInt32(reader.GetOrdinal("Day"));
        }
        return Count;
    }
    public static void CloseTheDataBase()//所有涉及到数据库的操作全部完成之后都需要调用一次，不然会导致数据库锁死
    {
        sql.CloseConnection();
    }
}
