// src/EmployeeRegistrationApp.Domain/Entities/Employee.cs
using EmployeeRegistrationApp.Domain.Enums;
using System;

namespace EmployeeRegistrationApp.Domain.Entities
{
    public partial class Employee
    {
        public void Deactivate(DateTime? terminationDate = null)
        {
            IsActive = false;

            TerminationDate = terminationDate ?? TerminationDate ?? DateTime.UtcNow.Date;

            // mantém consistência do status
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
    }
}
