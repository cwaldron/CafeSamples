namespace GrpcExample.Server
{
    public interface IStatePopulationProvider
    {
        long Get(string state);
    }
}