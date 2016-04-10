namespace Hive.Steps
{
    class StepPatternSegment
    {
        public IStep Step { get; }

        public StepPatternSegment[] Children { get; set; }

        public StepPatternSegment(IStep step)
        {
            Step = step;
        }
    }
}