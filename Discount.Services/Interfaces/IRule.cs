namespace Discount.Services.Interfaces
{
    public interface IRule<in Tin, out Tout> where Tin : class
    {
        Tout Apply(Tin input);
    }

    public interface IRuleAsync<in Tin, Tout> where Tin : class
    {
        Task<Tout> ApplyAsync(Tin input);
    }
}
