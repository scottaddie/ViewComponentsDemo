namespace ViewComponentsDemo.ViewModels
{
    public class Weather
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        //public string Location { get; set; }
        //public double CurrentTemperature { get; set; }
        //public double HighTemperature { get; set; }
        //public double LowTemperature { get; set; }
        //public UnitOfMeasure Unit { get; set; }

        //public enum UnitOfMeasure
        //{
        //    Celsius = 0,
        //    Fahrenheit = 1,
        //    Kelvin = 2
        //}
    }
}
