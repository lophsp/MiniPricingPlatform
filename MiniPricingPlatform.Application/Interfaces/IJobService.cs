using MiniPricingPlatform.Application.Models;

public interface IJobService
{
    string CreateJob(string type, object items);
    JobDetailResponse? Get(string jobId);
}