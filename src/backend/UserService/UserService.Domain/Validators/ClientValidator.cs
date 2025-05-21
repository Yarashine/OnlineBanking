using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UserService.Domain.Validators;

public static class ClientValidator
{
    public const int MAX_FIRST_NAME_LENGHT = 50;
    public const int MIN_FIRST_NAME_LENGHT = 2;

    public const int MAX_LAST_NAME_LENGHT = 50;
    public const int MIN_LAST_NAME_LENGHT = 2;

    public const int MAX_PATRONYMIC_LENGHT = 50;
    public const int MIN_PATRONYMIC_LENGHT = 2;

    public const int PASSPORT_IDENTIFIER_LENGHT = 10;

    public static bool FirstNameValidatiorRule(string firstName)
    {
        var firstNameRegex = new Regex(@"^[a-zA-Z\s-]+$");

        return firstNameRegex.IsMatch(firstName);
    }

    public static bool LastNameValidatiorRule(string lastName)
    {
        var lastNameRegex = new Regex(@"^[a-zA-Z\s-]+$");

        return lastNameRegex.IsMatch(lastName);
    }

    public static bool PatronymicValidatiorRule(string patronymic)
    {
        var patronymicRegex = new Regex(@"^[a-zA-Z\s-]+$");

        return patronymicRegex.IsMatch(patronymic);
    }

    public static bool PhoneNumberValidatiorRule(string phoneNumber)
    {
        var phoneNumberRegex = new Regex(@"^\+\d{10,15}$");

        return phoneNumberRegex.IsMatch(phoneNumber);
    }

    public static bool PassportValidatiorRule(string passportIdentifier)
    {
        var passportIdentifierRegex = new Regex(@"^[A-Za-z0-9]+$");

        return passportIdentifierRegex.IsMatch(passportIdentifier);
    }
}