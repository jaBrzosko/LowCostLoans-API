namespace Domain.Examples;

public class Example
{
    public Guid Id { get; private init; }
    public string Name { get; private set;  }
    public DateTime CreationTime { get; private init;  }

    public Example(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreationTime = DateTime.Now;
    }
}
