﻿using System.Collections;

namespace Domain
{
    internal class Forecast : IReadOnlyList<Completion>
    {
        private readonly List<Completion> completions = [];

        public void Add(Completion completion)
        {
            if (completions.Contains(completion))
                completions.Find(x => x == completion)!.Occurrences++;
            else
                completions.Add(completion);
        }

        public Completion this[int index] => completions[index];

        public int Count => completions.Count;

        public IEnumerator<Completion> GetEnumerator()
        {
            return completions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
