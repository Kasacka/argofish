using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;

namespace ArgoFish.Controllers;

public class Person
{
    public string Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreateDate { get; init; }
    public DateTime ModifiedDate { get; set; }
}

public class PersonCreateCommand
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
}

public class PersonUpdateCommand
{
    public string Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}

public class PersonCreateResult
{
    public string Id { get; init; }
}

public class PersonUpdateResult
{
    public string Id { get; init; }
}

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private static readonly ConcurrentBag<Person> persons = new();

    [HttpGet]
    public IEnumerable<Person> GetAll()
    {
        return persons.ToArray();
    }

    [HttpPut]
    public PersonCreateResult CreatePerson(PersonCreateCommand command)
    {
        var personId = Guid.NewGuid().ToString();
        var createDate = DateTime.UtcNow;
        var person = new Person {
            Id = personId,
            CreateDate = createDate,
            ModifiedDate = createDate,
            FirstName = command.FirstName,
            LastName = command.LastName
        };
        persons.Add(person);
        return new PersonCreateResult { Id = personId };
    }

    [HttpPost]
    public PersonUpdateResult UpdatePerson(PersonUpdateCommand command)
    {
        var person = persons.SingleOrDefault(x => x.Id == command.Id);
        if (person == null)
            return new PersonUpdateResult();
        person.FirstName = command.FirstName;
        person.LastName = command.LastName;
        person.ModifiedDate = DateTime.UtcNow;
        return new PersonUpdateResult();
    }
}

public class RandomResult
{
    public double Number { get; set; }
    public DateTime DateTime { get; set;}
    public DateOnly DateOnly { get; set;}
    //public TimeOnly TimeOnly { get; set;}
}

[ApiController]
[Route("[controller]")]
public class RandomController : ControllerBase
{
    private static readonly Random r = new();

    [HttpGet]
    public RandomResult Get()
    {
        var result = new RandomResult();
        result.Number = r.Next();
        result.DateTime =  DateTime.UtcNow;
        result.DateOnly = DateOnly.FromDateTime(DateTime.UtcNow);
    
        return result;
    }
}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
