using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Currencies
{
    class Program
    {
        static void Main(string[] args)
        {
            //загружаем xml файл
            StreamReader reader = new StreamReader(WebRequest.Create("http://www.cbr.ru/scripts/XML_daily.asp").
                GetResponse().GetResponseStream());
            XDocument xdoc = XDocument.Load(reader);

            //Считываем атрибут Date из узла ValCurs
            XAttribute dateAttr = xdoc.Element("ValCurs").Attribute("Date");
            String date = dateAttr.Value;

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
                cov.PrintValute();
            }



            //// Console.WriteLine("Hello World!");
            //Encoding win1251 = Encoding.GetEncoding(1251);
            //XmlDocument xDoc = new XmlDocument();
            //xDoc.Load("http://www.cbr.ru/scripts/XML_daily.asp",win1251);
            //// получим корневой элемент
            //XmlElement xRoot = xDoc.DocumentElement;
            ////Контейнер для хранения данных о курсах валют на текущую дату
            //List<ValCurs> todaysCources = new List<ValCurs>();
            //// обход всех узлов в корневом элементе
            //foreach (XmlNode xnode in xRoot)
            //{
            //    //ValCurs valCurs = new ValCurs();
            //    // получаем атрибут Date
            //    if (xnode.Attributes.Count > 0)
            //    {
            //        XmlNode dateAttr = xnode.Attributes.GetNamedItem("Date");
            //        if (dateAttr != null)
            //            //valCurs.Date = dateAttr.Value;
            //           Console.WriteLine(dateAttr.Value);
            //    }
            //    //    // обходим все дочерние узлы элемента Valute
            //    //    foreach (XmlNode childnode in xnode.ChildNodes)
            //    //    {
            //    //        // если узел - CharCode: EUR, USD
            //    //        if (childnode.Name == "CharCode")
            //    //        {
            //    //            valCurs.Valute = childnode.InnerText;
            //    //            //Console.WriteLine($"Валюта: {childnode.InnerText}");
            //    //        }
            //    //        // если узел Value
            //    //        if (childnode.Name == "Value")
            //    //        {
            //    //            valCurs.ValueOfCourse = Convert.ToDouble(childnode.InnerText);
            //    //        }
            //    //    }
            //    //    todaysCources.Add(valCurs);
            //}
        }
    }
}
