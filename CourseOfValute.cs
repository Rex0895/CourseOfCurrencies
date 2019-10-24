using System;
using System.Collections.Generic;
using System.Text;

namespace Currencies
{
    class CourseOfValute
    {
        string date;
        string valute;
        double valueOfCourse;

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }
        public string Valute
        {
            get
            {
                return valute;
            }
            set
            {
                valute = value;
            }
        }
        public double ValueOfCourse
        {
            get
            {
                return valueOfCourse;
            }
            set
            {
                valueOfCourse = value;
            }
        }
        public CourseOfValute()
        {
            date = "";
            valute = "";
            valueOfCourse = 0.0;
        }
        public CourseOfValute(string dt)
        {
            date = dt;
        }
        public CourseOfValute(string d, string v,double c)
        {
            date = d;
            valute = v;
            valueOfCourse = c;
        }

        public void PrintValute()
        {
            Console.WriteLine("Date: " + date + " Valute: " + valute + " Course: " + valueOfCourse);
        }

    }
}
