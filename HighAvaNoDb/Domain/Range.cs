namespace HighAvaNoDb.Domain
{
    /// <summary>
    /// value object
    /// </summary>
    public class Range
    {
        public long Min;
        public long Max;

        public Range(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        public virtual bool Includes(long hash)
        {
            return hash >= Min && hash <= Max;
        }

        public virtual bool IsSubsetOf(Range superset)
        {
            return superset.Min <= Min && superset.Max >= Max;
        }

        public virtual bool overlaps(Range other)
        {
            return Includes(other.Min) || Includes(other.Max) || IsSubsetOf(other);
        }

        public override string ToString()
        {
            return Min.ToString("x") + '-' + Max.ToString("x");
        }

        public override int GetHashCode()
        {

            return (int)(Max + Min);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            Range other = (Range)obj;
            return this.Min == other.Min && this.Max == other.Max;
        }
    }
}
