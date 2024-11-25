- refactor: forecasted this, forecasted that...
- test: throughput strategy selection theory "ForecastTakesLongerThanPeriod_ThroughputIsNotExhausted"
- feat: validate source CSV meets preconditions. What happens if the DateTime culture is different? Or its format?
- feat: allow for different CSV delimiters. These are culture dependent.
- feat: what if there is any issue reading/writing to the disk? Permissions, free space, etc.
- test: a bunch of tasks cannot be completed on single day if there isn't a day with, at least, that throughput
- feat: allow specifying the header names to prevent users from changing their spreadsheets headers just for this to
  work
- feat: ensure Completions are written into the result CSV with the same culture the read Period had
