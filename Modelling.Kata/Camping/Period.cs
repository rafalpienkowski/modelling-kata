namespace Modelling.Kata.Camping;

public record Period
{
    public DateTime From { get; }
    public DateTime To { get; }

    private Period(DateTime from, DateTime to)
    {
        From = from.Date;
        To = to.Date;
    }

    public int NumberOfDays => (int)(To - From).TotalDays;

    public static Period BasedOn(DateTime from, int days) => new(from, from.AddDays(days));

    public bool OverlapsWith(Period anotherPeriod) => (From >= anotherPeriod.From && From <= anotherPeriod.To) ||
                                                      (To <= anotherPeriod.To && To >= anotherPeriod.From);
    
    public override string ToString() => $"From: {From}, To: {To} ";
}