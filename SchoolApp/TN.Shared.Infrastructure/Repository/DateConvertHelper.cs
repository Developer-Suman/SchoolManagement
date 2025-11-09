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

        public async Task<(DateTime StartUtc, DateTime EndUtc)> GetDateRangeUtc(string startDate, string endDate)
        {
            DateTime startDateInDateTime;
            DateTime endDateInDateTime;

            startDateInDateTime = string.IsNullOrWhiteSpace(startDate)
                ? DateTime.Today
                : await ConvertFullBsDateToEnglish(startDate);

            endDateInDateTime = string.IsNullOrWhiteSpace(endDate)
                ? DateTime.Today
                : await ConvertFullBsDateToEnglish(endDate);


            startDateInDateTime = startDateInDateTime.Date.AddHours(0).AddMinutes(0).AddSeconds(0);

            TimeZoneInfo nepalTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time");

            DateTime startNepal = TimeZoneInfo.ConvertTime(startDateInDateTime, nepalTimeZone);
            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(startNepal, nepalTimeZone);

            DateTime endNepal = TimeZoneInfo.ConvertTime(endDateInDateTime, nepalTimeZone);
            endNepal = endNepal.Date.AddDays(2).AddTicks(-1);
            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(endNepal, nepalTimeZone);

            return (startUtc, endUtc);
        }

        private async Task<DateTime> ConvertFullBsDateToEnglish(string bsDateTime)
        {
            string[] parts = bsDateTime.Split(' ');
            string bsDatePart = parts[0];
            string bsTimePart = parts.Length > 1 ? bsDateTime.Substring(bsDatePart.Length).Trim() : "12:00:00 AM";
            DateTime englishDate = await ConvertToEnglish(bsDatePart);

            if (DateTime.TryParse(bsTimePart, out DateTime timePart))
            {
                englishDate = englishDate.Date
                              .AddHours(timePart.Hour)
                              .AddMinutes(timePart.Minute)
                              .AddSeconds(timePart.Second);
            }
            else
            {
                englishDate = englishDate.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
            }

            return englishDate;
        }
    }
}
