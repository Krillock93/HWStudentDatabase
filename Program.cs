using System;
using System.Collections.Generic;
using System.Linq;

namespace HWStudentDatabase
{
    public enum TypeOfGrade { Deutsch, Englisch, Mathematik }

    class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = new List<Student>();
            ShowMainMenu();
            MainLoop(ref students);
            Exit();
            Console.ReadKey();
        }

        private static void MainLoop(ref List<Student> students)
        {
            int select = GetInputAsInteger("");

            while (select < 6 && select > 0)
            {
                switch (select)
                {
                    case 1:
                        students.Add(CreateNewStudent());
                        break;
                    case 2:
                        if (StudentsAvailable(students))
                            ShowStudentsWithNotes(students);
                        break;
                    case 3:
                        if (StudentsAvailable(students))
                        {
                            Student s ;
                            s = SelectStudent(ref students);
                            ChangeNotes(ref s, ref students);
                        }
                        break;
                    case 4:
                        if (StudentsAvailable(students))
                            DeleteStudent(ref students);
                        break;
                    case 5:
                        if (StudentsAvailable(students))
                            AverageOfWholeSchool(students);
                        break;
                    default:
                        break;
                }

                WaitForKeyPress();
                Console.WriteLine("Nächste Operation?");
                ShowMainMenu();
                select = GetInputAsInteger("");
                Console.Clear();
            }
        }

        /// <summary>
        /// Auswerten des Durchschnitts in den verschiedenen Fächern
        /// über die gesamte Schule
        /// </summary>
        /// <param name="students">Liste der Schüler</param>

        private static void AverageOfWholeSchool(List<Student> students)
        {

            decimal avgDeutsch = 0;
            decimal avgEnglisch = 0;
            decimal avgMathe = 0;
            
            for (int i = 0; i < students.Count; i++)
            {
                students[i].Grades.TryGetValue(TypeOfGrade.Deutsch, out decimal d);
                students[i].Grades.TryGetValue(TypeOfGrade.Englisch, out decimal e);
                students[i].Grades.TryGetValue(TypeOfGrade.Mathematik, out decimal m);

                avgDeutsch += d;
                avgEnglisch += e;
                avgMathe += m;
            }

            avgDeutsch /= students.Count;
            avgEnglisch /= students.Count;
            avgMathe /= students.Count;

            Console.WriteLine("Durschnitt der ganzen Schule in Deutsch: " + avgDeutsch.ToString("N2"));
            Console.WriteLine("Durschnitt der ganzen Schule in Englisch: " + avgEnglisch.ToString("N2"));
            Console.WriteLine("Durschnitt der ganzen Schule in Mathematik: " + avgMathe.ToString("N2"));
        }

        /// <summary>
        /// Programm Ende
        /// </summary>
        private static void Exit()
        {
            Console.WriteLine("Programm wird beendet");
        }

        /// <summary>
        /// Student Löschen
        /// </summary>
        /// <param name="students">Liste der Schüler</param>
        private static void DeleteStudent(ref List<Student> students)
        {
            int i;
            ShowAllStudents(students);
            Console.WriteLine("Student mit Auflistungsnummer auswählen:");
            Int32.TryParse(Console.ReadLine(), out i);
            if (SelectionIsValid(i, students.Count))
            {
                students.RemoveAt(i);
            }
        }

        /// <summary>
        /// Auswählen eines Schülers
        /// </summary>
        /// <param name="students">Liste der Schüler als ref Parameter</param>
        /// <returns>Gibt die Referenz zum ausgewählten Schüler zurück</returns>
        private static Student SelectStudent(ref List<Student> students)
        {
            Student s;
            int i;

            while (true)
            {
                ShowAllStudents(students);
                Console.WriteLine("Student mit Auflistungsnummer auswählen:");
                Int32.TryParse(Console.ReadLine(), out i);
                if (SelectionIsValid(i, students.Count))
                {
                    s = students[i];
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Ungültige Eingabe");
                }
            }

            return s;
        }

        /// <summary>
        /// Auswertung ob Eingabe gültig ist
        /// </summary>
        /// <param name="selection">Eingabe des Benutzers</param>
        /// <param name="maxValue">Maximale mögliche Zahl zur Eingabe</param>
        /// <returns>True wenn Eingabe gültig</returns>
        private static bool SelectionIsValid(int selection, int maxValue)
        {
            return selection >= 0 && selection <= maxValue;
        }

        /// <summary>
        /// Alle Schüler aus der vorhandenen Liste ausgeben
        /// </summary>
        /// <param name="students">Liste der Schüler</param>
        private static void ShowAllStudents(List<Student> students)
        {

            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine(i + ": " + students[i].FullName);
            }

        }

        /// <summary>
        /// Noten eines Schülers neu eingeben
        /// </summary>
        /// <param name="student">Schüler dessen Noten geändert werden sollen</param>
        /// <param name="students">Liste der Schüler</param>
        private static void ChangeNotes(ref Student student, ref List<Student> students)
        {
            int i = students.IndexOf(student);
            student.GetNewGrades(CreateNewStudentGrades());
            students[i] = student;
        }

        /// <summary>
        /// Alle Schüler samt ihren Noten ausgeben
        /// </summary>
        /// <param name="students">Liste der Schüler</param>
        private static void ShowStudentsWithNotes(List<Student> students)
        {
            foreach (Student s in students)
                s.ShowStudentNotesAndName();
        }

        /// <summary>
        /// Abfrage ob überhaupt Schüler vorhanden sind
        /// </summary>
        /// <param name="students">Liste der Schüler</param>
        /// <returns>True wenn students.Count > 0 ist</returns>
        private static bool StudentsAvailable(List<Student> students)
        {
            if (students.Count == 0)
            {
                Console.WriteLine("Sie haben noch keine Schüler angelegt!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Neuen Studenten anlegen
        /// </summary>
        /// <returns>Neuen Studenten damit er mit List.Add() hinzugefügt werden kann</returns>
        private static Student CreateNewStudent()
        {
            Console.Clear();
            string name = GetInputAsString("Vorname ?");
            string lastName = GetInputAsString("Nachname ?");
            DateTime birthday = GetInputAsDateTime("Geburtsdatum ?");
            string visitsClass = GetInputAsString("Klasse ?");
            string sex = GetInputAsString("Geschlecht ?");
            Dictionary<TypeOfGrade, decimal> grades = CreateNewStudentGrades();
            Student placeholder = new Student(grades, name, lastName, birthday, visitsClass, sex);
            Console.WriteLine($"Student {placeholder.FullName} wurde angelegt");
            return placeholder;
        }

        /// <summary>
        /// Neues Dictionary für die Noten erstellen und zurückgeben
        /// </summary>
        /// <returns>Noten Dictionary</returns>
        private static Dictionary<TypeOfGrade, decimal> CreateNewStudentGrades()
        {
            decimal gradeGerman = GetInputAsDecimal("Note in Deutsch ?");
            decimal gradeEnglish = GetInputAsDecimal("Note in Englisch ?");
            decimal gradeMaths = GetInputAsDecimal("Note in Mathematik ?");

            Dictionary<TypeOfGrade, decimal> grades = new Dictionary<TypeOfGrade, decimal>()
            {
                {TypeOfGrade.Deutsch,gradeGerman },
                {TypeOfGrade.Englisch,gradeEnglish },
                {TypeOfGrade.Mathematik,gradeMaths }
            };
            return grades;
        }

        #region Input Handling
        private static DateTime GetInputAsDateTime(string s)
        {
            Console.WriteLine(s);
            DateTime birthday;

            while (!DateTime.TryParse(Console.ReadLine(), out birthday))
            {
                Console.WriteLine("Falsche Eingabe");
            }

            return birthday;
        }

        private static string GetInputAsString(string s)
        {
            Console.WriteLine(s);
            return Console.ReadLine();
        }

        private static int GetInputAsInteger(string s)
        {
            Console.WriteLine(s);

            while (true)
            {
                if (Int32.TryParse(Console.ReadLine(), out int result))
                    return result;
                else Console.WriteLine("Falsche eingabe");
            }
        }

        private static decimal GetInputAsDecimal(string s)
        {
            Console.WriteLine(s);
            while (true)
            {
                if (Decimal.TryParse(Console.ReadLine(), out decimal result))
                    return result;
                else Console.WriteLine("Falsche eingabe");
            }

        }

        #endregion

        /// <summary>
        /// Hauptmenü anzeigen
        /// </summary>
        private static void ShowMainMenu()
        {
            Console.WriteLine("Neue/n Schüler:in anlegen -> 1");
            Console.WriteLine("Alle Schüler: innen inklusive Noten anzeigen -> 2");
            Console.WriteLine("Noten abändern -> 3");
            Console.WriteLine("Schüler:in von der Schule schmeißen -> 4");
            Console.WriteLine("Anzeige der Durchschnittsnoten aller Schüler: innen pro Fach -> 5");
            Console.WriteLine("Programm beenden -> 6");
            Console.Write("Ihre Auswahl: ");
        }

        /// <summary>
        /// Auf beliebige Taste warten
        /// </summary>
        private static void WaitForKeyPress()
        {
            Console.WriteLine("Weiter mit Beliebiger Taste");
            Console.ReadKey();
            Console.Clear();
        }
    }




    #region Struct Student
    public struct Student
    {
        public Dictionary<TypeOfGrade, decimal> Grades { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }
        public DateTime Birthday { get; private set; }
        public string VisitsClass { get; private set; }
        public string Sex { get; private set; }

        public Student(Dictionary<TypeOfGrade, decimal> grades, string name, string lastName, DateTime birthday, string className, string sex)
        {
            Grades = grades;
            Name = name;
            LastName = lastName;
            FullName = name + " " + lastName;
            Birthday = birthday;
            VisitsClass = className;
            Sex = sex;
        }

        private void ShowStudentNotes()
        {
            foreach (var grade in Grades)
            {
                Console.WriteLine(grade);
            }

            ShowAverage();
        }

        public void ShowStudentNotesAndName()
        {
            Console.WriteLine(FullName + ":");
            ShowStudentNotes();
        }

        private void ShowAverage()
        {
            CalculateAverage();
            Console.WriteLine($"[Durchschnitt, " + CalculateAverage().ToString("N2") + "]");
        }

        private decimal CalculateAverage()
        {
            decimal[] notes = Grades.Values.ToArray();
            decimal avrg = 0;

            for (int i = 0; i < notes.Length; i++)
            {
                avrg += notes[i];
            }

            return avrg /= Enum.GetValues<TypeOfGrade>().Length;
        }

        public void GetNewGrades(Dictionary<TypeOfGrade, decimal> grades)
        {
            Grades = grades;
        }
    }

    #endregion
}
