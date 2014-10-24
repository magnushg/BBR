namespace BouvetCodeCamp.SpillSimulator.Jobs
{
    using System;

    public class Job
    {
        public string SkrivTidsstempel()
        {
            return DateTime.Now.ToLongTimeString();
        }
    }
}