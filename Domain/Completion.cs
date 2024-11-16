﻿namespace Domain
{
    public class Completion
    {
        public DateTime When { get; set; }
        public int Occurrences { get; set; }

        public Completion(DateTime when, int occurrences)
        {
            When = when;
            Occurrences = occurrences;
        }

        public override bool Equals(object? obj)
        {
            return obj is Completion completion
                && When == completion.When;
        }

        public static bool operator ==(Completion left, Completion right) =>
            left.Equals(right);
        public static bool operator !=(Completion left, Completion right) =>
            !left.Equals(right);
    };
}