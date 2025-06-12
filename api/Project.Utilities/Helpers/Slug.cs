using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project.Utilities.Helpers
{
    public class Slug
    {
        public static string ConvertStringToSlug(string name)
        {
            var slug = "";
            try
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                slug = $"{name}".Normalize(NormalizationForm.FormD).Trim().ToLower();

                slug = regex.Replace(slug, String.Empty)
                  .Replace('\u0111', 'd').Replace('\u0110', 'D')
                  .Replace(",", "-").Replace(".", "-").Replace("!", "")
                  .Replace("(", "").Replace(")", "").Replace(";", "-")
                  .Replace("/", "-").Replace("%", "ptram").Replace("&", "va")
                  .Replace("?", "").Replace('"', '-').Replace(' ', '-');


            }
            catch (Exception e)
            {
                slug = "";
            }
            return slug;
        }

        // white space, em-dash, en-dash, underscore
        static readonly Regex WordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);

        // characters that are not valid
        static readonly Regex InvalidChars = new Regex(@"[^a-z0-9\-]", RegexOptions.Compiled);

        // multiple hyphens
        static readonly Regex MultipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);

        public static string ToUrlSlug(string value)
        {
            // convert to lower case
            value = value.ToLowerInvariant();

            // remove diacritics (accents)
            value = RemoveDiacritics(value);

            // ensure all word delimiters are hyphens
            value = WordDelimiters.Replace(value, "-");

            // strip out invalid characters
            value = InvalidChars.Replace(value, "");

            // replace multiple hyphens (-) with a single hyphen
            value = MultipleHyphens.Replace(value, "-");

            // trim hyphens (-) from ends
            return value.Trim('-');
        }

        private static string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
