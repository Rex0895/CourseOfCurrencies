using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Data.SQLite;
using System.Data;

namespace Currencies
{
    class Program
    {
        static void Main(string[] args)
        {
            //-------------------------------Работа с XML файлом----------------------------------------------
            //загружаем xml файл
            //Пример http://www.cbr.ru/scripts/XML_daily.asp?date_req=21.08.2019
            //На завтрашнюю дату http://www.cbr.ru/scripts/XML_daily.asp
            //На сегодняшнюю дату
            string today = DateTime.Today.ToString("d").Replace("/", ".");
            StreamReader reader = new StreamReader(WebRequest.Create(String.Format("http://www.cbr.ru/scripts/XML_daily.asp?date_req={0}",today)).
                GetResponse().GetResponseStream());
            XDocument xdoc = XDocument.Load(reader);
            //Считываем атрибут Date из узла ValCurs
            XAttribute dateAttr = xdoc.Element("ValCurs").Attribute("Date");
            String date = dateAttr.Value;
            List<CourseOfValute> courses = new List<CourseOfValute>();
            //Пробегаемся по узлам Valute, заполняем объекты класса ValCurs
            foreach (XElement valElem in xdoc.Element("ValCurs").Elements("Valute"))
            {
                //Курс валюты на текущую дату
                CourseOfValute cov = new CourseOfValute(date);
                //Получаем значения из узла Valute
                XElement charCode = valElem.Element("CharCode");
                XElement value = valElem.Element("Value");
                if (charCode != null && value != null)
                {
                    cov.Valute = charCode.Value;
                    cov.ValueOfCourse = Convert.ToDouble(value.Value);
                }
                courses.Add(cov);
                //cov.PrintValute();
            }
            
            

            //--------------------------------------Работа с БД-------------------------------------
            //Создание БД SQLite
            SQLiteWorking SQLiteDB = new SQLiteWorking(@"\TestDB.db", @"Data Source=\TestDB.db; Version=3;");

            //Создание таблицы с курсом валют
            SQLiteDB.MakeSQLCommand("CREATE TABLE IF NOT EXISTS [CoursesOfValute] ([Date] date, [Valute] VARCHAR(5), [Course] float)");
            
            //Заполнение таблицы
            //string commandText = "INSERT INTO CoursesOfValute (Date,Valute,Course) VALUES('2019-24-10','USD', 65.07); "; //пример запроса
            // string commandText1 = "INSERT INTO CoursesOfValute (ID,Date,Valute,Course) VALUES";
            string commandText1 = "INSERT INTO CoursesOfValute (Date,Valute,Course) VALUES";
            for (int i = 0; i < courses.Count; i++)
            {
                if (i != courses.Count - 1)
                    commandText1 += String.Format("('{0}','{1}',{2})," + "\n", courses[i].DateToSqlFormat(), courses[i].Valute,
                        courses[i].ValueOfCourse.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                else //в конце ;
                    commandText1 += String.Format("('{0}','{1}',{2});" + "\n", courses[i].DateToSqlFormat(), courses[i].Valute,
                    courses[i].ValueOfCourse.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                // lastId++;
            }
            SQLiteDB.MakeSQLCommand(commandText1);

            //Удаление повторов
            SQLiteDB.MakeSQLCommand("delete from CoursesOfValute where rowid not in (select min(rowid) from CoursesOfValute group by Date,Valute,Course);");
            //Очистка таблицы
            //SQLiteDB.MakeSQLCommand("DELETE FROM CoursesOfValute;");

            //Вывести данные из таблицы в консоль
            SQLiteDB.ShowDataFromTable("CoursesOfValute");
        }
    }
}
//if (!File.Exists(@"\TestDB.db"))
//{
//    SQLiteConnection.CreateFile(@"\TestDB.db"); // создать базу данны
//}
////Работа с БД
//using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=\TestDB.db; Version=3;")) // подключение
//{
//    //запрос к бд [ID] INTEGER PRIMARY KEY NOT NULL, 
//    string commandText = "CREATE TABLE IF NOT EXISTS [CoursesOfValute] ([Date] date, [Valute] VARCHAR(5), [Course] float)"; // создать таблицу, если её нет
//    SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
//    Connect.Open(); // открыть соединение
//    Command.ExecuteNonQuery(); // выполнить запрос
//    Connect.Close(); // закрыть соединение

//    ////получение последнего ID
//    //commandText = "Select MAX(ID) from CoursesOfValute;";
//    //Command = new SQLiteCommand(commandText, Connect);
//    //Connect.Open(); // открыть соединение
//    ////Command.ExecuteNonQuery(); // выполнить запрос
//    //int lastId = (Int32)Command.ExecuteScalar()-1;
//    //Connect.Close(); // закрыть соединение

////Заполнение таблицы полученными данными
////string commandText = "INSERT INTO CoursesOfValute (Date,Valute,Course) VALUES('2019-24-10','USD', 65.07); "; //пример запроса
//// string commandText1 = "INSERT INTO CoursesOfValute (ID,Date,Valute,Course) VALUES";
//string commandText1 = "INSERT INTO CoursesOfValute (Date,Valute,Course) VALUES";
//for (int i = 0; i < courses.Count; i++)
//{
//    if (i != courses.Count - 1)
//        commandText1 += String.Format("('{0}','{1}',{2})," + "\n", courses[i].DateToSqlFormat(), courses[i].Valute,
//            courses[i].ValueOfCourse.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
//    else //в конце ;
//        commandText1 += String.Format("('{0}','{1}',{2});" + "\n", courses[i].DateToSqlFormat(), courses[i].Valute,
//        courses[i].ValueOfCourse.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
//    // lastId++;
//}

//    //commandText += "DELETE FROM Courses WHERE rowid not in (SELECT MIN(rowid) FROM Courses GROUP BY Date, Valute, Course);";

//    SQLiteCommand Command1 = new SQLiteCommand(commandText1, Connect);
//    Connect.Open(); // открыть соединение
//    Command1.ExecuteNonQuery(); // выполнить запрос
//    Connect.Close(); // закрыть соединение

//    //Посмотреть данные в таблице
//    commandText = "Select * from CoursesOfValute";
//    Connect.Open(); // открыть соединение
//    Command = new SQLiteCommand(commandText, Connect);
//    SQLiteDataReader reader1 = Command.ExecuteReader();
//    DataTable dt = new DataTable();
//    dt.Load(reader1);
//    reader1.Close();
//    Connect.Close(); // закрыть соединение
//    foreach (DataRow dataRow in dt.Rows)
//    {
//        foreach (var item in dataRow.ItemArray)
//        {
//            Console.WriteLine(item);
//        }
//    }

//}




//using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=\TestDB.db; Version=3;")) // подключение
//            {
//                //запрос к бд
//                Connect.Open();
//                string commandText = "Drop table CoursesOfValute";
//SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
//Command.ExecuteNonQuery();
//                Connect.Close();
//            }