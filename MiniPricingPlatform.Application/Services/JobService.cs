using System.Text.Json;
using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.Application.Models;
using MiniPricingPlatform.Domain.Models;

public class JobService : IJobService
{
    private readonly IPricingService _pricingService;

    private static Dictionary<string, JobDetailResponse> _jobs = new();

    public JobService(IPricingService pricingService)
    {
        _pricingService = pricingService;
    }

    public string CreateJob(string type, object items)
    {
        List<BulkItemInput> parsedItems;

        if (type.ToLower() == "json")
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var json = JsonSerializer.Serialize(items);
            parsedItems = JsonSerializer.Deserialize<List<BulkItemInput>>(json,options)
                          ?? new List<BulkItemInput>();
        }
        else if (type.ToLower() == "csv")
        {
            parsedItems = ParseCsv(items.ToString()!);
        }
        else
        {
            throw new ArgumentException($"Unsupported type: {type}");
        }

        var jobId = Guid.NewGuid().ToString();
        _jobs[jobId] = new JobDetailResponse { Status = "processing" };

        Task.Run(() => ProcessJob(jobId, parsedItems));

        return jobId;
    }

    private List<BulkItemInput> ParseCsv(string csvText)
    {
        var lines = csvText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var result = new List<BulkItemInput>();

        foreach (var line in lines.Skip(1))
        {
            var cols = line.Split(',');
            result.Add(new BulkItemInput
            {
                Weight = int.Parse(cols[0]),
                Area = cols[1],
                RequestTime = DateTime.Parse(cols[2])
            });
        }

        return result;
    }

    private async Task ProcessJob(string jobId, List<BulkItemInput> requests)
    {
        var results = new List<JobResultItem>();

        foreach (var req in requests)
        {
            var input = new PricingInput
            {
                Weight = req.Weight,
                Area = req.Area,
                RequestTime = req.RequestTime
            };

            var price = await _pricingService.CalculateAsync(input);

            results.Add(new JobResultItem
            {
                Weight = req.Weight,
                Area = req.Area,
                Price = price
            });
        }

        _jobs[jobId] = new JobDetailResponse
        {
            Status = "completed",
            Results = results
        };
    }

    public JobDetailResponse? Get(string jobId)
    {
        return _jobs.ContainsKey(jobId) ? _jobs[jobId] : null;
    }
}