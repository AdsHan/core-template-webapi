using System;

namespace EntregaFutura.Api.Services
{
    public class DataService
    {
        public DateTime getDataHoraBrasilia() => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
    }
}
