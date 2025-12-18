using NepaliCalendarBS;
using NepaliDateConverter.Net;
using System;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;

namespace TN.Shared.Infrastructure.Repository
{
    public class DateConvertHelper : IDateConvertHelper
    {
        public Task<DateTime> ConvertToEnglish(string nepaliDate)
        {
            try
            {

                string[] nepaliNumbers = { "०", "१", "२", "३", "४", "५", "६", "७", "८", "९" };
                for (int i = 0; i < nepaliNumbers.Length; i++)
                {
                    nepaliDate = nepaliDate.Replace(nepaliNumbers[i], i.ToString());
                }

                DateTime englishDate = NepaliCalendarBS.NepaliCalendar.Convert_BS2AD(nepaliDate);
                return Task.FromResult(englishDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error converting Nepali date to English", ex);
            }
        }

        public Task<string> ConvertToNepali(DateTime englishDate)
        {
            try
            {
                // Convert English (AD) date to Nepali (BS)
                var nepaliDate = NepaliCalendarBS.NepaliCalendar.Convert_AD2BS(englishDate).ToString();

                // Convert digits to Nepali numerals
                //string[] nepaliNumbers = { "०", "१", "२", "३", "४", "५", "६", "७", "८", "९" };
                nepaliDate = nepaliDate.Replace("/", "-");

                //for (int i = 0; i < nepaliNumbers.Length; i++)
                //{
                //    nepaliDate = nepaliDate.Replace(i.ToString(), nepaliNumbers[i]);
                //}

                return Task.FromResult(nepaliDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error converting English date to Nepali", ex);
            }
        }

        public async Task<(DateTime StartUtc, DateTime EndUtc)> GetDateRangeUtc(string? startDate, string? endDate)
        {
            TimeZoneInfo nepalTz =
        TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time");

            // --- START DATE ---
            DateTime startLocal = string.IsNullOrWhiteSpace(startDate)
                ? DateTime.Today
                : await ConvertFullBsDateToEnglish(startDate);

            startLocal = DateTime.SpecifyKind(startLocal, DateTimeKind.Unspecified);

            DateTime startUtc =
                TimeZoneInfo.ConvertTimeToUtc(startLocal, nepalTz);

            // --- END DATE ---
            DateTime endLocal = string.IsNullOrWhiteSpace(endDate)
                ? DateTime.Today
                : await ConvertFullBsDateToEnglish(endDate);

            endLocal = endLocal.Date
                .AddDays(1)
                .AddTicks(-1); // end of same day

            endLocal = DateTime.SpecifyKind(endLocal, DateTimeKind.Unspecified);

            DateTime endUtc =
                TimeZoneInfo.ConvertTimeToUtc(endLocal, nepalTz);

            return (startUtc, endUtc);
        }

        private async Task<DateTime> ConvertFullBsDateToEnglish(string bsDateTime)
        {
            string[] parts = bsDateTime.Split(' ');

            string bsDatePart = parts[0];
            string bsTimePart = parts.Length > 1
                ? string.Join(" ", parts.Skip(1))
                : "12:00:00 AM";

            DateTime englishDate = await ConvertToEnglish(bsDatePart);

            if (DateTime.TryParse(bsTimePart, out DateTime timePart))
            {
                englishDate = englishDate.Date
                    .AddHours(timePart.Hour)
                    .AddMinutes(timePart.Minute)
                    .AddSeconds(timePart.Second);
            }

            return englishDate;
        }
    }
}
