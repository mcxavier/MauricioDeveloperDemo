using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Utils.Extensions
{
    public static class StringExtensions
    {
        public static string Left(this string value, int length)
        {
            return value == null || length < 0 ? "" : (value.Length <= length ? value : value.Substring(0, length));
        }

        public static string Left(this string value, string search)
        {
            return value == null || value.IndexOf(search) < 0 ? "" : value.Substring(0, value.IndexOf(search));
        }

        public static string Right(this string value, int length)
        {
            return value == null || value.Length < length ? value : value.Substring(value.Length - length);
        }

        public static string Right(this string value, string search)
        {
            if (value == null)
                return "";

            int length = -1;

            while (value.IndexOf(search, length + 1) >= 0)
                length = value.IndexOf(search, length + 1);

            if ((length < 0) || ((length + search.Length) >= value.Length))
                return "";

            return value.Substring(length + search.Length);
        }

        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var startUnderscores = Regex.Match(input, @"^_+");

            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string[] GetUserFirstAndLastName(this string name)
        {
            var firstName = "";
            var lastName = "";

            if (!name.IsNullOrEmpty())
            {
                var len = name.Split(' ').Length;

                string[] names = name.ToString().Trim().Split(new char[] { ' ' }, len);

                if (names.Length == 1)
                {
                    firstName = names[0];
                    lastName = "";
                }
                else if (names.Length == 2)
                {
                    firstName = names[0];
                    lastName = names[1];
                }
                else
                {
                    firstName = names[0];
                    lastName = names[names.Length - 1];
                }
            }

            string[] firstlastName = { (firstName), (lastName) };
            return firstlastName;
        }

        public static string GetFirstName(this string name)
        {
            var result = name.GetUserFirstAndLastName();
            return result[0];
        }

        public static string GetLastName(this string name)
        {
            var result = name.GetUserFirstAndLastName();
            return result[1];
        }

        public static string ToPascalCase(this string s)
        {
            var x = s.Replace("_", "");

            if (x.Length == 0)
                return "Null";

            x = Regex.Replace(x, "([A-Z])([A-Z]+)($|[A-Z])", m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);

            return char.ToUpper(x[0]) + x.Substring(1);
        }

        public static string ToCamelCase(this string s)
        {
            var x = s.Replace("_", "");

            if (x.Length == 0)
                return "Null";

            x = Regex.Replace(x, "([A-Z])([A-Z]+)($|[A-Z])", m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);

            return char.ToLower(x[0]) + x.Substring(1);
        }

        public static string ToKebabCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var startUnderscores = Regex.Match(input, @"^_+");

            return $"{startUnderscores}{Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1-$2").ToLower()}";
        }

        public static string[] TrySplit(this string input, string therm)
        {
            var splited = new[] { "" };

            try
            {
                if (input != null && input.Contains(therm))
                    splited = input.Split(therm).ToArray();

            }
            catch
            {
                splited = new[]{
                    input
                };
            }

            return splited;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string FormatCPFCNPJ(this string value)
        {
            if (value.IsNullOrEmpty())
                return "";

            value = value.DigitsOnly();

            if (value.Length == 11)
                return Convert.ToUInt64(value).ToString(@"000\.000\.000\-00");

            return Convert.ToUInt64(value).ToString(@"00\.000\.000\/0000-00");
        }

        public static string FormatPhone(this string value, bool withAreaCode = true)
        {
            if (value.Length < 7)
                return value;

            value = value.DigitsOnly();

            if (withAreaCode)
                value = value.Insert(2, ") ").Insert(0, "(");

            value = value.Insert(value.Length - 4, "-");

            return value;
        }

        public static string RemoveFormatPhone(this string value)
        {
            if (value.IsNullOrEmpty()) return "";

            value = value.DigitsOnly();

            if (!value.StartsWith("55"))
                value = $"55{value}";
            else if (value.StartsWith("55") && (value.Length == 11 || value.Length == 10))
                value = $"55{value}";

            return value;
        }

        public static string RemoveDDIPhone(this string value)
        {
            if (value.IsNullOrEmpty()) return "";
            value = value.DigitsOnly();

            if (value.StartsWith("55"))
                value = value.Remove(0, 2);

            return value;
        }

        public static string UnformatPhoneNumber(this string value)
        {
            return value?.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace("+", "") ?? "";
        }

        public static string UnformatCpfCnpjRg(this string value)
        {
            return value?.Replace("/", "").Replace("-", "").Replace(" ", "").Replace(".", "") ?? "";
        }

        public static bool IsInt(this string number)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(number);
        }

        public static bool IsValidNumber(this string number)
        {
            ulong n = 0;
            return ulong.TryParse(number, out n);
        }

        public static string RemoveSpaces(this string text)
        {
            if (text.IsNullOrEmpty())
                return "";

            string[] strings;
            string newString = "";
            if (text.Contains(" "))
            {
                strings = text.Split(' ');
                foreach (string s in strings)
                    if (s.Length > 0 && s != " ")
                        if (newString == "")
                            newString = s;
                        else
                            newString = newString + " " + s;
            }
            else
                newString = text;
            return newString;
        }

        public static string WrapText(this string text, int length)
        {
            string result = "";
            int i = 0;
            foreach (char c in text)
            {
                if (i == length)
                    result += "\n";
                else
                    result += c;
            }
            return result;
        }

        public static Decimal ToDecimal(this string text)
        {
            decimal result = 0;

            if (text.IsNullOrEmpty())
                return result;

            if (decimal.TryParse(text.Replace(".", ","), NumberStyles.Currency, new CultureInfo("pt-BR"), out result))
                return result;
            else
                return 0;
        }

        public static int ToInt(this string number)
        {
            number = number.DigitsOnly();

            return int.Parse(
                number,
                NumberStyles.Integer,
                CultureInfo.CurrentCulture.NumberFormat);
        }

        public static int GetOnlyNumbers(this string number)
        {
            try { return number.ToInt(); } catch (Exception) { return 0; }
        }

        public static string DigitsOnly(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return "";

            return Regex.Replace(s, "[^0-9]", "").Trim();
        }

        public static string GetOnlyPhoneNumber(this string value, byte phoneType = 1)
        {
            if (value?.Length == 8 || value?.Length == 9)
                return value;

            value = UnformatPhoneNumber(value);

            if ((phoneType == 1 && value?.Length == 13) || (phoneType == 2 && value?.Length == 14) || (phoneType == 1 && value?.Length == 14) || (phoneType == 2 && value?.Length == 15))
                value = value.Remove(0, 3);

            if ((value.Left(2) == "55" || value?.Length == 13) && value?.Length >= 10)
                value = value.Remove(0, 2);

            if (value.StartsWith("0"))
                value = value.Remove(0, 1);

            if (value?.Length <= 11)
            {
                if (value.Length == 8 || value.Length == 9)
                    return value;

                if (value.StartsWith("0"))
                    value = value.Remove(0, 1);

                if (value.Length >= 11)
                    return value?.Right(9);
                else
                    return value?.Right(8);
            }
            return value;
        }

        public static bool IsMobilePhoneNumber(this string value, byte phoneType = 1)
        {
            string phone = GetOnlyPhoneNumber(value);
            bool isMobile = phone.StartsWith("9") || phone.StartsWith("8") || phone.StartsWith("7");
            return isMobile;
        }

        public static string RemoveSpecialCharacters(this string text, bool allowSpace = false)
        {
            string ret;

            if (allowSpace)
                ret = Regex.Replace(text, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?", string.Empty);
            else
                ret = Regex.Replace(text, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ]+?", string.Empty);

            return ret;
        }
    }
}