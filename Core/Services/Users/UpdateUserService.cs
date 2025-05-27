using System;
using System.Collections.Generic;
using BusinessEntities;
using Common;

namespace Core.Services.Users
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateUserService : IUpdateUserService
    {
        public void Update(User user, string name, string email, int age, UserTypes type, decimal? annualSalary, IEnumerable<string> tags)
        {
            user.SetEmail(email);
            user.SetName(name);
            user.SetAge(age);
            user.SetType(type);
            user.SetMonthlySalary(annualSalary.HasValue ? Math.Round(annualSalary.Value / 12, 2) : (decimal?)null);
            user.SetTags(tags);
        }
    }
}