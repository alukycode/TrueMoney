using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Common
{
    public static class RoleNames
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public static class ErrorMessages
    {
        public const string Required = "Поле '{0}' не может быть пустым";
        public const string Invalid = "Поле \"{0}\" содержит некорректное значение"; 
    }

    public static class BankConstants
    {
        public const string TrueMoneyAccountNumber = "123123";
    }

    public static class StringFormats
    {
        public const string DateWithTime = "dd.MM.yyyy hh:mm";
        public const string DateOnly = "dd.MM.yyyy";
    }
}
