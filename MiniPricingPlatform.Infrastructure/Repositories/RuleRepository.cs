using Microsoft.Extensions.Configuration;
using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.Domain.Entities;
using MiniPricingPlatform.Domain.Rules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MiniPricingPlatform.Infrastructure.Repositories;

public class RuleRepository : IRuleRepository
{
    private readonly string _filePath;

    public RuleRepository(IConfiguration config)
    {
        _filePath = config["StoragePath"]
                    ?? throw new ArgumentException("StoragePath is not configured");

        var dir = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(_filePath))
            File.WriteAllText(_filePath, "[]");
    }

    public async Task<List<PricingRule>> GetAllAsync()
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine($"rules.json not found at: {_filePath}");
            return new List<PricingRule>();
        }

        var json = await File.ReadAllTextAsync(_filePath);

        var array = JArray.Parse(json);

        var result = new List<PricingRule>();

        foreach (var item in array)
        {
            var type = item["type"]?.ToString()?.ToLower();

            PricingRule? rule = null;

            switch (type)
            {
                case "weighttier":
                    rule = item.ToObject<WeightTierRule>();
                    break;

                case "remoteareasurcharge":
                    rule = item.ToObject<RemoteAreaRule>();
                    break;

                case "timewindowpromotion":
                    rule = item.ToObject<TimeWindowPromotionRule>();
                    break;

                default:
                    Console.WriteLine($"Unknown rule type: {type}");
                    break;
            }

            if (rule != null)
                result.Add(rule);
        }

        return result;
    }
    public async Task<PricingRule?> GetByIdAsync(Guid id)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(x => x.Id == id);
    }

    public async Task AddAsync(PricingRule rule)
    {
        var all = await GetAllAsync();
        all.Add(rule);
        await SaveAllAsync(all);
    }

    public async Task UpdateAsync(PricingRule rule)
    {
        var all = await GetAllAsync();
        var idx = all.FindIndex(x => x.Id == rule.Id);
        if (idx >= 0)
        {
            all[idx] = rule;
            await SaveAllAsync(all);
        }
        else
        {
            throw new KeyNotFoundException($"Rule with id {rule.Id} not found.");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var all = await GetAllAsync();
        var idx = all.FindIndex(x => x.Id == id);
        if (idx >= 0)
        {
            all.RemoveAt(idx);
            await SaveAllAsync(all);
        }
        else
        {
            throw new KeyNotFoundException($"Rule with id {id} not found.");
        }
    }

    public async Task SaveAllAsync(List<PricingRule> rules)
    {
        var array = new JArray();

        foreach (var rule in rules)
        {
            var obj = JObject.FromObject(rule);
            switch (rule)
            {
                case WeightTierRule:
                    obj["type"] = "weighttier";
                    break;
                case RemoteAreaRule:
                    obj["type"] = "remoteareasurcharge";
                    break;
                case TimeWindowPromotionRule:
                    obj["type"] = "timewindowpromotion";
                    break;
            }
            array.Add(obj);
        }

        await File.WriteAllTextAsync(_filePath, array.ToString(Formatting.Indented));
    }
}