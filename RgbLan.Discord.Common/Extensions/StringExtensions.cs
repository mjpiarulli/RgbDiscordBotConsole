using System.Collections.Generic;
using System.Text;

namespace RgbLan.Discord.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToMoreBetterString(this List<EventDto> dtos)
        {
            var sb = new StringBuilder();
            foreach (var dto in dtos)
                sb.AppendLine(dto.ToString());

            return sb.ToString();
        }
    }
}
