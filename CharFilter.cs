using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleAssignments
{
    public static class CharFilterEx
    {
        public static CharFilter Union(this CharFilter filter, params CharFilter[] filters) => CharFilter.Union(filters.Prepend(filter));
        public static CharFilter Exclude(this CharFilter filter, CharFilter exclude) => CharFilter.ExcludeFrom(exclude, filter);
    }

    // Why not just use a Set / HashSet and be done with it? Or even just a RegEx?
    // Well.... no speical reason. Just felt like this was a more fun challenge.
    public abstract record CharFilter
    {
        public abstract bool IsValid(char value);

        public static CharFilter All { get; } = new AnyFilter();
        public static CharFilter None { get; } = new NoneFilter();
        public static CharFilter Digits_09 { get; } = Range('0', '9');
        public static CharFilter Letters_AZ { get; } = Range('A', 'Z');
        public static CharFilter Letters_az { get; } = Range('a', 'z');
        public static CharFilter FileName { get; } = ExcludeFrom(Set(Path.GetInvalidFileNameChars()));
        public static CharFilter SimpleText { get; } = Union(Digits_09, Letters_AZ, Letters_az,
            Set(' ', '.', ',', '?', '!', '-', '+', '*', '/', ':', ';', '"', '\'', '(', ')'));

        public static CharFilter ExcludeFrom(CharFilter exclude, CharFilter? from = null) => new ExcludeFilter(exclude, from ?? All);
        public static CharFilter Range(char fromInclusive, char toInclusive) => new RangeFilter(fromInclusive, toInclusive);
        public static CharFilter Set(params char[] characters) => new SetFilter(characters);
        public static CharFilter Union(IEnumerable<CharFilter> filters) => DecomposeAndMerge(filters);
        public static CharFilter Union(params CharFilter[] filters) => DecomposeAndMerge(filters);

        private struct DecomposedFilters
        {
            private List<RangeFilter>? ranges;
            private List<SetFilter>? sets;

            public bool IsAll { get; private set; }
            public bool IsNone => !IsAll && ranges == null && sets == null;
            public IReadOnlyCollection<RangeFilter> Ranges => AsEmptyIfNull(ranges);
            public IReadOnlyCollection<SetFilter> Sets => AsEmptyIfNull(sets);

            public void AddAll()
            {
                IsAll = true;
                ranges = null;
                sets = null;
            }
            public void Add(RangeFilter range)
            {
                if (!IsAll)
                    (ranges ??= new()).Add(range);
            }
            public void Add(SetFilter set)
            {
                if (!IsAll)
                    (sets ??= new()).Add(set);
            }

            private static IReadOnlyCollection<T> AsEmptyIfNull<T>(IReadOnlyCollection<T>? collection) => collection ?? Array.Empty<T>();
        }

        private static CharFilter DecomposeAndMerge(IEnumerable<CharFilter> filters) // todo: in dire need of some testing...
        {
            DecomposedFilters includes = new();
            DecomposedFilters excludes = new();

            DecomposeMany(filters, ref includes, ref excludes);
            return Merge(excludes, includes);

            static void DecomposeMany(IEnumerable<CharFilter> filters, ref DecomposedFilters includes, ref DecomposedFilters excludes)
            {
                foreach(var filter in filters)
                    Decompose(filter, ref includes, ref excludes);
            }

            static void Decompose(CharFilter filter, ref DecomposedFilters includes, ref DecomposedFilters excludes)
            {
                switch (filter)
                {
                    case AnyFilter:
                        includes.AddAll();
                        break;
                    case NoneFilter:
                        break;
                    case RangeFilter range:
                        includes.Add(range);
                        break;
                    case SetFilter set:
                        includes.Add(set);
                        break;
                    case ExcludeFilter ef:
                        Decompose(ef.FromSource, ref includes, ref excludes);
                        Decompose(ef.Exclude, ref excludes, ref includes);
                        break;
                    case UnionFilter union:
                        DecomposeMany(union.Filters, ref includes, ref excludes);
                        break;
                }
            }

            static CharFilter Merge(DecomposedFilters excludes, DecomposedFilters includes)
            {
                if (excludes.IsAll || includes.IsNone)
                    return None;
                else if (excludes.IsNone)
                    return Union(includes);
                else
                    return Exclusion(excludes, includes);

                static CharFilter Exclusion(DecomposedFilters excludes, DecomposedFilters includes)
                {
                    var exclude = Union(excludes);
                    var include = Union(includes); // <-- TODO... we can optimize this "later" (or never...)
                    return new ExcludeFilter(exclude, include);
                }

                static CharFilter Union(DecomposedFilters filters)
                    => filters.Sets != null
                        ? filters.Ranges != null
                            ? MergeUnion(filters.Ranges, filters.Sets)
                            : MergeSets(filters.Sets)
                        : filters.Ranges != null
                            ? new UnionFilter(MergeOverlappingRanges(filters.Ranges))
                            : filters.IsAll ? All : None;

                static IEnumerable<char> OptimizeSets(IEnumerable<SetFilter> sets) => sets.SelectMany(set => set.Characters).Distinct();

                static SetFilter MergeSets(IEnumerable<SetFilter> sets) => new SetFilter(OptimizeSets(sets).ToArray());

                static IEnumerable<RangeFilter> MergeOverlappingRanges(IEnumerable<RangeFilter> ranges)
                {
                    List<RangeFilter> seed = new();
                    return ranges
                        .OrderBy(range => range.FromInclusive)
                        .Aggregate(seed, (list, next) =>
                        {
                            if (list.Count == 0)
                            {
                                list.Add(next);
                            }
                            else
                            {
                                var prev = list[^1];
                                if ((prev.ToInclusive + 1) >= next.FromInclusive) // +1 covers the case where they are adjacent
                                {
                                    char max = next.ToInclusive >= prev.ToInclusive ? next.ToInclusive : prev.ToInclusive;
                                    list[^1] = prev with { ToInclusive = max };
                                }
                                else
                                {
                                    list.Add(next); // no intersection between prev and next
                                }
                            }
                            return list;
                        });
                }

                static UnionFilter MergeUnion(IEnumerable<RangeFilter> ranges, IEnumerable<SetFilter> sets)
                {
                    IEnumerable<CharFilter> optimizedRanges = MergeOverlappingRanges(ranges);
                    ICollection<char> optimizedSet = OptimizeSets(sets).Where(NotFoundInOptimizedRanges).ToArray();
                    var mergedSet = new SetFilter(optimizedSet);
                    return new UnionFilter(optimizedRanges.Append(mergedSet));

                    bool NotFoundInOptimizedRanges(char c) => !optimizedRanges.Any(filter => filter.IsValid(c));
                }
            }
        }

        private record AnyFilter : CharFilter
        {
            public override bool IsValid(char value) => true;
        }

        private record NoneFilter : CharFilter
        {
            public override bool IsValid(char value) => false;
        }

        private record RangeFilter(char FromInclusive, char ToInclusive) : CharFilter
        {
            public override bool IsValid(char value) => value >= FromInclusive & value <= ToInclusive; // intentional bit-op
        }

        private record SetFilter(ICollection<char> Characters) : CharFilter // small sets, thus Array more performant than HashSet
        {
            public override bool IsValid(char value) => Characters.Contains(value);
        }

        private record UnionFilter(IEnumerable<CharFilter> Filters) : CharFilter
        {
            public override bool IsValid(char value) => Filters.Any(f => f.IsValid(value));
        }

        private record ExcludeFilter(CharFilter Exclude, CharFilter FromSource) : CharFilter // Exclusion takes precedence over inclusion.
        {
            public override bool IsValid(char value) => !Exclude.IsValid(value) && FromSource.IsValid(value);
        }
    }
}
