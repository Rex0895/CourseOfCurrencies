using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.IO;
using System.Data;

namespace Currencies
{
    class SQLiteWorking
    {
        string connectParams;
        public SQLiteWorking(string dbParam,string conPar)
        {
            try
            {
                if (!File.Exists(dbParam))
                {
                    SQLiteConnection.CreateFile(dbParam); // создать базу данны
                }
                connectParams = conPar;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void MakeSQLCommand(string command)
        {
            try
            {
                using (SQLiteConnection Connect = new SQLiteConnection(connectParams)) // подключение
                {
                    //запрос к бд
                    SQLiteCommand Command = new SQLiteCommand(command, Connect);
                    Connect.Open(); // открыть соединение
                    Command.ExecuteNonQuery(); // выполнить запрос
                    Connect.Close(); // закрыть соединение
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void ShowDataFromTable(string tableName)
        {
            try
            {
                using (SQLiteConnection Connect = new SQLiteConnection(connectParams)) // подключение
                {
                    //Посмотреть данные в таблице
                    Connect.Open(); // открыть соединение
                    SQLiteCommand Command = new SQLiteCommand("Select * from " + tableName, Connect);
                    SQLiteDataReader reader1 = Command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader1);
                    reader1.Close();
                    Connect.Close(); // закрыть соединение
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        foreach (var item in dataRow.ItemArray)
                        {
                            string s = item + " ";
                            Console.Write(s.Replace(" 0:00:00 ", " "));//0:00:00 убираем
                        }
                        Console.WriteLine();
                    }
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
