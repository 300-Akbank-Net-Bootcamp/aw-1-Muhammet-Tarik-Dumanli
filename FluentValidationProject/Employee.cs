using System;

namespace FluentValidationProject
{
     public class Employee
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double HourlySalary { get; set; }
        public double MinJuniorSalary { get; set; }
        public double MinSeniorSalary { get; set; }
    }
}