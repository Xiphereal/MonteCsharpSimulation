- test: throughput strategy selection theory "ForecastTakesLongerThanPeriod_ThroughputIsNotExhausted"
- feat: validate source CSV meets preconditions. What happens if the DateTime culture is different? Or its format?
- feat: allow for different CSV delimiters. These are culture dependent.
- feat: what if there is any issue reading/writing to the disk? Permissions, free space, etc.
- test: a bunch of tasks cannot be completed on single day if there isn't a day with, at least, that throughput
- feat: ensure Completions are written into the result CSV with the same culture the read Period had
- test: e2e where the entrypoint is referenced as package. Can simulation be run with what is packaged? How to automate
  updating the package version? Should it be from nuget or can it be used just from a package, created locally?
- feat: make this compatible with the lowest .NET version possible. May it be on .NET Standard?