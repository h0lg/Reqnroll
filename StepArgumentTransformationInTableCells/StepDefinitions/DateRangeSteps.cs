using StepArgumentTransformationInTableCells.Support;

namespace StepArgumentTransformationInTableCells.StepDefinitions
{
    [Binding]
    internal sealed class RelativeDateTransformations
    {
        [StepArgumentTransformation(@"(\d+) days? ago")]
        internal DateTime _DaysAgo(int days) => DateTime.Today.AddDays(-days);

        [StepArgumentTransformation(@"in (\d+) days?")]
        internal DateTime In_Days(int days) => DateTime.Today.AddDays(days);
    }

    public class DateRange
    {
        public int ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    [Binding]
    public sealed class DateRangeSteps
    {
        private Dictionary<int, DateRange>? ranges;

        public DateRangeSteps(ScenarioContext scenario)
            => TableCellStepArgumentConverterValueRetriever.Register(scenario.ScenarioContainer);

        [Given("the date ranges")]
        public void GivenTheDateRanges(Table table)
            => ranges = table.CreateSet<DateRange>().ToDictionary(dr => dr.ID);

        [Then(@"date range (\d+) should start (.+) and end (.+)")]
        public void DateRange_ShouldStart_AndEnd_(int id, DateTime start, DateTime end)
        {
            if (ranges == null) Assert.Fail("No date ranges were given.");
            if (!ranges.TryGetValue(id, out var range)) Assert.Fail($"The date range with ID {id} doesn't exist");

            // ensure normal step transformation works
            Assert.AreNotEqual(default, start);
            Assert.AreNotEqual(default, end);

            Assert.AreEqual(start, range.Start);
            Assert.AreEqual(end, range.End);
        }
    }
}
