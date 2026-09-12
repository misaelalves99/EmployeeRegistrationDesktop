using System;
using EmployeeRegistrationApp.Domain.Base;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Domain.Entities
{
    public partial class Employee : AggregateRoot
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;

        public string DisplayName => FullName;
        public string FullName => $"{FirstName} {LastName}".Trim();

        public Cpf Cpf { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public PhoneNumber? Phone { get; private set; }

        public DateTime? DateOfBirth { get; private set; }

        public DateTime HireDate { get; private set; }
        public DateTime? TerminationDate { get; private set; }

        public EmploymentStatus EmploymentStatus { get; private set; }
        public ContractType ContractType { get; private set; }
        public JobRole JobRole { get; private set; }

        public Money Salary { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public Guid? DepartmentId { get; private set; }
        public Department? Department { get; private set; }

        public Guid? PositionId { get; private set; }
        public Position? Position { get; private set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        protected Employee()
        {
        }

        public Employee(
            string firstName,
            string lastName,
            Cpf cpf,
            Email email,
            PhoneNumber? phone,
            DateTime hireDate,
            JobRole jobRole,
            EmploymentStatus employmentStatus,
            ContractType contractType,
            Money salary)
        {
            FirstName = firstName?.Trim() ?? string.Empty;
            LastName = lastName?.Trim() ?? string.Empty;

            Cpf = cpf ?? throw new ArgumentNullException(nameof(cpf));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Phone = phone;

            HireDate = hireDate.Date;
            JobRole = jobRole;
            EmploymentStatus = employmentStatus;
            ContractType = contractType;

            Salary = salary ?? throw new ArgumentNullException(nameof(salary));

            IsActive =
                employmentStatus != Enums.EmploymentStatus.Inactive &&
                employmentStatus != Enums.EmploymentStatus.Terminated;

            TerminationDate = null;

            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateContact(
            Email email,
            PhoneNumber? phone)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Phone = phone;

            Touch();
        }

        public void ChangeContract(
            ContractType contractType,
            JobRole jobRole)
        {
            ContractType = contractType;
            JobRole = jobRole;

            Touch();
        }

        public void ChangeEmploymentStatus(
            EmploymentStatus employmentStatus,
            DateTime? terminationDate = null)
        {
            EmploymentStatus = employmentStatus;

            if (terminationDate.HasValue)
            {
                TerminationDate = terminationDate.Value.Date;
            }

            Touch();
        }

        public void AssignDepartment(Department department)
        {
            if (department is null)
                throw new ArgumentNullException(nameof(department));

            Department = department;
            DepartmentId = department.Id;

            Touch();
        }

        public void AssignPosition(Position position)
        {
            if (position is null)
                throw new ArgumentNullException(nameof(position));

            Position = position;
            PositionId = position.Id;

            Touch();
        }

        public void Deactivate(DateTime? terminationDate = null)
        {
            IsActive = false;

            TerminationDate =
                terminationDate ??
                TerminationDate ??
                DateTime.UtcNow.Date;

            EmploymentStatus = terminationDate.HasValue
                ? Enums.EmploymentStatus.Terminated
                : Enums.EmploymentStatus.Inactive;

            Touch();
        }

        public void Reactivate(DateTime? newHireDate = null)
        {
            IsActive = true;

            TerminationDate = null;
            EmploymentStatus = Enums.EmploymentStatus.Active;

            if (newHireDate.HasValue)
                HireDate = newHireDate.Value.Date;

            Touch();
        }

        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdatePersonalInfo(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Touch();
        }

        public void UpdateDocuments(Cpf cpf)
        {
            Cpf = cpf;
            Touch();
        }

        public void UpdateDocuments(Cpf cpf, Email email)
        {
            Cpf = cpf;
            Email = email;
            Touch();
        }

        public void SetSalary(Money salary)
        {
            Salary = salary;
            Touch();
        }
    }
}