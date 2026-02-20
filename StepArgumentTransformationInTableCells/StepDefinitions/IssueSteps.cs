using StepArgumentTransformationInTableCells.Support;

namespace StepArgumentTransformationInTableCells.StepDefinitions
{
    [Binding]
    internal sealed class BoolTransformations
    {
        [StepArgumentTransformation("(yes|no)")]
        internal bool YesNoToBool(string input) => input == "yes";
    }

    [Binding]
    internal sealed class GlyphTransformations
    {
        [StepArgumentTransformation("(🐛|🦗|🧩|⚙)")]
        internal Issue.Kinds GlyphToIssueKind(string glyph)
            => glyph == "🧩" ? Issue.Kinds.Feature
                : glyph == "⚙" ? Issue.Kinds.Chore
                : Issue.Kinds.Bug;

        [StepArgumentTransformation("(☐|☑)")]
        internal bool GlyphToBool(string glyph) => glyph == "☑";
    }

    public class Issue
    {
        public int ID { get; set; }
        public Kinds Kind { get; set; }
        public string? Title { get; set; }
        public bool Closed { get; set; }
        public DateTime Created { get; set; }

        public enum Kinds { Bug, Feature, Chore }
    }

    [Binding]
    public sealed class IssueSteps
    {
        private Dictionary<int, Issue>? issues;

        public IssueSteps(ScenarioContext scenario)
            => TableCellStepArgumentConverterValueRetriever.Register(scenario.ScenarioContainer);

        [Given("the issues")]
        public void GivenTheIssues(Table table)
            => issues = table.CreateSet<Issue>().ToDictionary(dr => dr.ID);

        [Then(@"(.+) (\d+) '(.+)' \(created (.+)\) should be (.+)")]
        public void VerifyIssueDetails(Issue.Kinds kind, int id, string title, DateTime created, bool closed)
        {
            if (issues == null) Assert.Fail("No issues were given.");
            if (!issues.TryGetValue(id, out var issue)) Assert.Fail($"The issue with ID {id} doesn't exist");

            Assert.AreEqual(kind, issue.Kind);
            Assert.AreEqual(title, issue.Title);
            Assert.AreEqual(closed, issue.Closed);
            Assert.AreEqual(created.RemoveTicks(), issue.Created.RemoveTicks()); //🐒 nom nom
        }

        [StepArgumentTransformation("(open|closed)")]
        internal bool OpenClosedToBool(string input) => input == "closed";
    }

    internal static class DateTimeComparisonExtensions
    {
        /// <summary>Removes ticks to make <see cref="DateTime"/>s
        /// with differences at the millisecond level or below comparable,
        /// like <see cref="DateTime.Now"/> in the test setup phase vs. during assertion.
        /// From https://stackoverflow.com/a/11558076 .
        /// This is also a good practice in general, because they can carry dangerous diseases.</summary>
        internal static DateTime RemoveTicks(this DateTime dt) => dt.AddTicks(-dt.Ticks % TimeSpan.TicksPerSecond);
    }
}
